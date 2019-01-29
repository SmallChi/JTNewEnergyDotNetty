using DotNetty.Buffers;
using DotNetty.Transport.Channels;
using System;
using Microsoft.Extensions.Logging;
using JTNE.Protocol;
using JTNE.DotNetty.Core.Services;
using Newtonsoft.Json;

namespace JTNE.DotNetty.Tcp.Handlers
{
    /// <summary>
    /// JTNE服务端处理程序
    /// </summary>
    internal class JTNETcpServerHandler : SimpleChannelInboundHandler<byte[]>
    {
        private readonly ILogger<JTNETcpServerHandler> logger;
        private readonly JTNETcpAtomicCounterService jTNETcpAtomicCounterService;

        public JTNETcpServerHandler(
            ILoggerFactory loggerFactory,
            JTNETcpAtomicCounterService jTNETcpAtomicCounterService
            )
        {
            logger = loggerFactory.CreateLogger<JTNETcpServerHandler>();
            this.jTNETcpAtomicCounterService = jTNETcpAtomicCounterService;
        }


        protected override void ChannelRead0(IChannelHandlerContext ctx, byte[] msg)
        {
            try
            {
                JTNEPackage jtNePackage = JTNESerializer.Deserialize(msg);
                jTNETcpAtomicCounterService.MsgSuccessIncrement();
                if (logger.IsEnabled(LogLevel.Debug))
                {
                    //logger.LogDebug(JsonConvert.SerializeObject(jtNePackage));
                    logger.LogDebug("accept package success count<<<" + jTNETcpAtomicCounterService.MsgSuccessCount.ToString());
                }
            }
            catch (JTNE.Protocol.Exceptions.JTNEException ex)
            {
                jTNETcpAtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("accept package fail count<<<" + jTNETcpAtomicCounterService.MsgFailCount.ToString());
                    logger.LogError(ex, "accept msg<<<" + ByteBufferUtil.HexDump(msg));
                }
            }
            catch (Exception ex)
            {
                jTNETcpAtomicCounterService.MsgFailIncrement();
                if (logger.IsEnabled(LogLevel.Error))
                {
                    logger.LogError("accept package fail count<<<" + jTNETcpAtomicCounterService.MsgFailCount.ToString());
                    logger.LogError(ex, "accept msg<<<" + ByteBufferUtil.HexDump(msg));
                }
            }
        }
    }
}
