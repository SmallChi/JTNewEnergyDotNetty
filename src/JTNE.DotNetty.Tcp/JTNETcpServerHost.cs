using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Libuv;
using JTNE.DotNetty.Core.Codecs;
using JTNE.DotNetty.Core.Configurations;
using JTNE.DotNetty.Tcp.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace JTNE.DotNetty.Tcp
{
    /// <summary>
    /// JTNE Tcp网关服务
    /// </summary>
    internal class JTNETcpServerHost : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<JTNETcpServerHost> logger;
        private DispatcherEventLoopGroup bossGroup;
        private WorkerEventLoopGroup workerGroup;
        private IChannel bootstrapChannel;
        private IByteBufferAllocator serverBufferAllocator;
        private readonly JTNEConfiguration configuration;

        public JTNETcpServerHost(
            IServiceProvider provider,
            ILoggerFactory loggerFactory,
            IOptions<JTNEConfiguration> jTNEConfigurationAccessor)
        {
            serviceProvider = provider;
            configuration = jTNEConfigurationAccessor.Value;
            logger = loggerFactory.CreateLogger<JTNETcpServerHost>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            bossGroup = new DispatcherEventLoopGroup();
            workerGroup = new WorkerEventLoopGroup(bossGroup, configuration.EventLoopCount);
            serverBufferAllocator = new PooledByteBufferAllocator();
            ServerBootstrap bootstrap = new ServerBootstrap();
            bootstrap.Group(bossGroup, workerGroup);
            bootstrap.Channel<TcpServerChannel>();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                bootstrap
                    .Option(ChannelOption.SoReuseport, true)
                    .ChildOption(ChannelOption.SoReuseaddr, true);
            }
            bootstrap
               .Option(ChannelOption.SoBacklog, configuration.SoBacklog)
               .ChildOption(ChannelOption.Allocator, serverBufferAllocator)
               .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
               {
                   IChannelPipeline pipeline = channel.Pipeline;
                   using (var scope = serviceProvider.CreateScope())
                   {
                       channel.Pipeline.AddLast("systemIdleState", new IdleStateHandler(
                                                configuration.ReaderIdleTimeSeconds,
                                                configuration.WriterIdleTimeSeconds,
                                                configuration.AllIdleTimeSeconds));
                       channel.Pipeline.AddLast("jtneTcpConnection", scope.ServiceProvider.GetRequiredService<JTNETcpConnectionHandler>());
                       //LengthFieldBasedFrameDecoder 定长解码器
                       //参数说明:
                       //maxFrameLength：解码的帧的最大长度
                       //lengthFieldOffset：长度字段的偏差(长度属性的起始位（偏移位），包中存放有整个大数据包长度的字节，这段字节的其实位置)
                       //lengthFieldLength：长度字段占的字节数(即存放整个大数据包长度的字节所占的长度)
                       //lengthAdjustmen：添加到长度字段的补偿值(长度调节值，在总长被定义为包含包头长度时，修正信息长度)。
                       //initialBytesToStrip：从解码帧中第一次去除的字节数(跳过的字节数，根据需要我们跳过lengthFieldLength个字节，以便接收端直接接受到不含“长度属性”的内容)
                       //failFast ：为true，当frame长度超过maxFrameLength时立即报TooLongFrameException异常，为false，读取完整个帧再报异常
                       //22 JTNEPackage数据体长度
                       //2  JTNEPackage数据体长度占两个字节
                       //1  JTNEPackage校验位
                       channel.Pipeline.AddLast("jtneTcpDecoder", new LengthFieldBasedFrameDecoder(int.MaxValue, 22, 2, 1, 0));
                       channel.Pipeline.AddLast("jtneTcpBuffer", scope.ServiceProvider.GetRequiredService<JTNETcpDecoder>());
                       channel.Pipeline.AddLast("jtneTcpServerHandler", scope.ServiceProvider.GetRequiredService<JTNETcpServerHandler>());
                   }
               }));
            logger.LogInformation($"JTNE TCP Server start at {IPAddress.Any}:{configuration.TcpPort}.");
            return bootstrap.BindAsync(configuration.TcpPort)
                .ContinueWith(i => bootstrapChannel = i.Result);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await bootstrapChannel.CloseAsync();
            var quietPeriod = configuration.QuietPeriodTimeSpan;
            var shutdownTimeout = configuration.ShutdownTimeoutTimeSpan;
            await workerGroup.ShutdownGracefullyAsync(quietPeriod, shutdownTimeout);
            await bossGroup.ShutdownGracefullyAsync(quietPeriod, shutdownTimeout);
        }
    }
}
