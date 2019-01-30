using System;
using System.Collections.Generic;
using System.Text;

namespace JTNE.DotNetty.Abstractions.Dtos
{
    /// <summary>
    /// 统一下发请求参数
    /// </summary>
    public class JTNEUnificationSendRequestDto
    {
        public string Vin { get; set; }
        public byte[] Data { get; set; }
    }
}

