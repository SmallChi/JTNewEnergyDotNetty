using JTNE.DotNetty.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace JTNE.DotNetty.Tcp.Test
{
    public class TestBase
    {
        public static IServiceProvider ServiceProvider;

        static TestBase()
        {
            var serverHostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<ILoggerFactory, LoggerFactory>();
                    services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
                    services.AddJTNECore(hostContext.Configuration)
                            .AddJTNETcpHost();
                });
            var build = serverHostBuilder.Build();
            build.Start();
            ServiceProvider = build.Services;
        }
    }
}
