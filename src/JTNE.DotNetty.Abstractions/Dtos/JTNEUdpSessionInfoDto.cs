using System;
using System.Collections.Generic;
using System.Text;

namespace JTNE.DotNetty.Abstractions.Dtos
{
    public class JTNEUdpSessionInfoDto
    {
        /// <summary>
        /// 最后上线时间
        /// </summary>
        public DateTime LastActiveTime { get; set; }
        /// <summary>
        /// 上线时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 终端VIN
        /// </summary>
        public string Vin { get; set; }
        /// <summary>
        /// 远程ip地址
        /// </summary>
        public string RemoteAddressIP { get; set; }
    }
}

