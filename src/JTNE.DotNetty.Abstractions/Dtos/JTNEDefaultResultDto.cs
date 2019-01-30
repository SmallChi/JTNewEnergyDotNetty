using System;
using System.Collections.Generic;
using System.Text;

namespace JTNE.DotNetty.Abstractions.Dtos
{
    class JTNEDefaultResultDto : JTNEResultDto<string>
    {
        public JTNEDefaultResultDto()
        {
            Data = "Hello,JTNE WebAPI";
            Code = JTNEResultCode.Ok;
        }
    }
}
