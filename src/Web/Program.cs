using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Hosting;

namespace Rocket.Surgery.Web
{
    public static class Program
    {
        public static Task<int> Main(string[] args) => CreateHostBuilder(args).RunCli();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .LaunchWith(RocketBooster.For(DependencyContext.Default))
                //#if autofac
                .ConfigureRocketSurgery(builder => builder.UseAutofac())
                //#endif
                //#if dryioc
                //.ConfigureRocketSurgery(builder => builder.UseDryIoc())
                //#endif
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
