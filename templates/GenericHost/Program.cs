﻿using System;
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
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Conventions.Reflection;
using Rocket.Surgery.Conventions.Scanners;
using Rocket.Surgery.Extensions.CommandLine;
using Rocket.Surgery.Extensions.Configuration;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Hosting;

namespace Rocket.Surgery.GenericHost
{
    public class Program
    {
        public static Task Main(string[] args)
        {
            try
            {
                var diagnosticSource = new DiagnosticListener("GenericHost");
                DiagnosticListenerExtensions.SubscribeWithAdapter(
                    diagnosticSource,
                    new DiagnosticListenerLoggingAdapter(
                        new ServiceCollection()
                            .AddLogging(x => x.AddConsole().SetMinimumLevel(LogLevel.Trace))
                            .BuildServiceProvider()
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("GenericHost")
                        )
                    );
                return Host.CreateDefaultBuilder(args)
                    .LaunchWith(RocketBooster.For(DependencyContext.Default, diagnosticSource))
                    .ConfigureRocketSurgey(x => {})
                    .RunConsoleAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return Task.CompletedTask;
            }
        }
    }
}
