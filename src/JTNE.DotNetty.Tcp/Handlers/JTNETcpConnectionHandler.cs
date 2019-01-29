using DotNetty.Handlers.Timeout;
using DotNetty.Transport.Channels;
using JTNE.DotNetty.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace JTNE.DotNetty.Tcp.Handlers
{
    /// <summary>
    /// JTNE服务通道处理程序
    /// </summary>
    internal class JTNETcpConnectionHandler : ChannelHandlerAdapter
    {
        private readonly ILogger<JTNETcpConnectionHandler> logger;


        public JTNETcpConnectionHandler(
            ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<JTNETcpConnectionHandler>();
        }

        /// <summary>
        /// 通道激活
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelActive(IChannelHandlerContext context)
        {
            string channelId = context.Channel.Id.AsShortText();
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug($"<<<{ channelId } Successful client connection to server.");
            base.ChannelActive(context);
        }

        /// <summary>
        /// 设备主动断开
        /// </summary>
        /// <param name="context"></param>
        public override void ChannelInactive(IChannelHandlerContext context)
        {
            string channelId = context.Channel.Id.AsShortText();
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug($">>>{ channelId } The client disconnects from the server.");
            
            base.ChannelInactive(context);
        }

        /// <summary>
        /// 服务器主动断开
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task CloseAsync(IChannelHandlerContext context)
        {
            string channelId = context.Channel.Id.AsShortText();
            if (logger.IsEnabled(LogLevel.Debug))
                logger.LogDebug($"<<<{ channelId } The server disconnects from the client.");
            
            return base.CloseAsync(context);
        }

        public override void ChannelReadComplete(IChannelHandlerContext context)=> context.Flush();

        /// <summary>
        /// 超时策略
        /// </summary>
        /// <param name="context"></param>
        /// <param name="evt"></param>
        public override void UserEventTriggered(IChannelHandlerContext context, object evt)
        {
            IdleStateEvent idleStateEvent = evt as IdleStateEvent;
            if (idleStateEvent != null)
            {
                if(idleStateEvent.State== IdleState.ReaderIdle)
                {
                    string channelId = context.Channel.Id.AsShortText();
                    logger.LogInformation($"{idleStateEvent.State.ToString()}>>>{channelId}");
                    context.CloseAsync();
                }
            }
            base.UserEventTriggered(context, evt);
        }

        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            string channelId = context.Channel.Id.AsShortText();
            logger.LogError(exception,$"{channelId} {exception.Message}" );
            context.CloseAsync();
        }
    }
}

