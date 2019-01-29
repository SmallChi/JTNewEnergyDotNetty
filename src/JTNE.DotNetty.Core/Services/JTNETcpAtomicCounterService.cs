using JTNE.DotNetty.Core.Metadata;

namespace JTNE.DotNetty.Core.Services
{
    /// <summary>
    /// Tcp计数包服务
    /// </summary>
    public class JTNETcpAtomicCounterService
    {
        private readonly JTNEAtomicCounter MsgSuccessCounter = new JTNEAtomicCounter();

        private readonly JTNEAtomicCounter MsgFailCounter = new JTNEAtomicCounter();

        public JTNETcpAtomicCounterService()
        {

        }

        public void Reset()
        {
            MsgSuccessCounter.Reset();
            MsgFailCounter.Reset();
        }

        public long MsgSuccessIncrement()
        {
            return MsgSuccessCounter.Increment();
        }

        public long MsgSuccessCount
        {
            get
            {
                return MsgSuccessCounter.Count;
            }
        }

        public long MsgFailIncrement()
        {
            return MsgFailCounter.Increment();
        }

        public long MsgFailCount
        {
            get
            {
                return MsgFailCounter.Count;
            }
        }
    }
}
