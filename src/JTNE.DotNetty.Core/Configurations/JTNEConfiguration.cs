using System;
using System.Collections.Generic;
using System.Text;

namespace JTNE.DotNetty.Core.Configurations
{
    public class JTNEConfiguration
    {
        public int TcpPort { get; set; } = 909;

        public int UdpPort { get; set; } = 919;

        public int QuietPeriodSeconds { get; set; } = 1;

        public TimeSpan QuietPeriodTimeSpan => TimeSpan.FromSeconds(QuietPeriodSeconds);

        public int ShutdownTimeoutSeconds { get; set; } = 3;

        public TimeSpan ShutdownTimeoutTimeSpan => TimeSpan.FromSeconds(ShutdownTimeoutSeconds);

        public int SoBacklog { get; set; } = 8192;

        public int EventLoopCount { get; set; } = Environment.ProcessorCount;

        public int ReaderIdleTimeSeconds { get; set; } = 3600;

        public int WriterIdleTimeSeconds { get; set; } = 3600;

        public int AllIdleTimeSeconds { get; set; } = 3600;

        public int UdpSlidingExpirationTimeSeconds { get; set; } = 5*60;

        /// <summary>
        /// WebApi服务
        /// 默认929端口
        /// </summary>
        public int WebApiPort { get; set; } = 929;

        /// <summary>
        /// 转发远程地址 (可选项)知道转发的地址有利于提升性能
        /// </summary>
        public List<JTNEClientConfiguration> ForwardingRemoteAddress { get; set; }
    }
}
