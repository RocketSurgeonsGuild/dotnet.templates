using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rocket.Surgery.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Hosting;
using Microsoft.Extensions.DependencyModel;

namespace Rocket.Surgery.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .LaunchWith(RocketBooster.For(DependencyContext.Default))
                //#if autofac
                .ConfigureRocketSurgery(builder => builder.UseAutofac())
                //#endif
                ;
    }
}
