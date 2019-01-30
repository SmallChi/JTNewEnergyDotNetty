using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JTNE.DotNetty.Abstractions
{
    public interface IJTNESourcePackageDispatcher
    {
        /// <summary>
        /// 源包分发器
        /// 自定义源包分发器业务
        /// ConfigureServices:
        /// services.Replace(new ServiceDescriptor(typeof(IJTNESourcePackageDispatcher),typeof(JTNESourcePackageDispatcherDefaultImpl),ServiceLifetime.Singleton));
        /// </summary>
        Task SendAsync(byte[] data);
    }
}
