using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JTNE.DotNetty.Abstractions
{
    /// <summary>
    /// 会话通知（在线/离线）
    /// </summary>
    public interface IJTNESessionPublishing
    {
        Task PublishAsync(string topicName, string value);
    }
}
