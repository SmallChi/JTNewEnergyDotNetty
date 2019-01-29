using JTNE.DotNetty.Core.Codecs;
using JTNE.DotNetty.Core.Configurations;
using JTNE.DotNetty.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("JTNE.DotNetty.Core.Test")]
[assembly: InternalsVisibleTo("JTNE.DotNetty.Tcp.Test")]
[assembly: InternalsVisibleTo("JTNE.DotNetty.Udp.Test")]
[assembly: InternalsVisibleTo("JTNE.DotNetty.WebApi.Test")]
namespace JTNE.DotNetty.Core
{
    public static class JTNECoreDotnettyExtensions
    {
        static JTNECoreDotnettyExtensions()
        {
            JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
            {
                Newtonsoft.Json.JsonSerializerSettings settings = new Newtonsoft.Json.JsonSerializerSettings();
                //日期类型默认格式化处理
                settings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                settings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                //空值处理
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                return settings;
            });
        }

        public static IServiceCollection AddJTNECore(this IServiceCollection  serviceDescriptors, IConfiguration configuration, Newtonsoft.Json.JsonSerializerSettings settings=null)
        {
            serviceDescriptors.Configure<JTNEClientConfiguration>(configuration.GetSection("JTNEConfiguration"));
            serviceDescriptors.AddSingleton<JTNETcpAtomicCounterService>();
            serviceDescriptors.AddScoped<JTNETcpDecoder>();
            return serviceDescriptors;
        }
    }
}