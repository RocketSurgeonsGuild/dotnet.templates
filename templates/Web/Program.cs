using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Rocket.Surgery.Web
{
    public class Program
    {
        public static Task<int> Main(string[] args) => CreateHostBuilder(args).RunCli();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .LaunchWith(RocketBooster.For(DependencyContext.Default))
                .ConfigureRocketSurgery(builder => { })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
