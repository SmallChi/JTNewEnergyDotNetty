using JTNE.DotNetty.Core.Codecs;
using JTNE.DotNetty.Tcp.Handlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JTNE.DotNetty.Tcp.Test")]

namespace JTNE.DotNetty.Tcp
{
    public static class JTNETcpDotnettyExtensions
    {
        public static IServiceCollection AddJTNETcpHost(this IServiceCollection  serviceDescriptors)
        {
            serviceDescriptors.TryAddScoped<JTNETcpConnectionHandler>();
            serviceDescriptors.TryAddScoped<JTNETcpDecoder>();
            serviceDescriptors.TryAddScoped<JTNETcpServerHandler>();
            serviceDescriptors.AddHostedService<JTNETcpServerHost>();
            return serviceDescriptors;
        }
    }
}