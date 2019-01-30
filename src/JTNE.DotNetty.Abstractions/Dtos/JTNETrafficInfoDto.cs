using System;
using System.Collections.Generic;
using System.Text;

namespace JTNE.DotNetty.Abstractions.Dtos
{
    public class JTNETrafficInfoDto
    {
        /// <summary>
        /// 总接收大小 
        /// 单位KB
        /// </summary>
        public double TotalReceiveSize { get; set; }
        /// <summary>
        /// 总发送大小 
        /// 单位KB
        /// </summary>
        public double TotalSendSize { get; set; }
    }
}
