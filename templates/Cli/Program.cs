using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Rocket.Surgery.Cli;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Extensions.CommandLine;
using Rocket.Surgery.Extensions.Configuration;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Extensions.Logging;
using Rocket.Surgery.Hosting;

[assembly: Convention(typeof(Program))]

namespace Rocket.Surgery.Cli
{
    public class Program : ICommandLineConvention
    {
        public static Task Main(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .LaunchWith(RocketBooster.For(DependencyContext.Default))
                .ConfigureRocketSurgey(builder => { })
                .RunCli();
        }

        public void Register(ICommandLineConventionContext context)
        {
            context.OnRun<Default>();
        }
    }
}
