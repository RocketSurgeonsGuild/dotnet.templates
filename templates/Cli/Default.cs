using System;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Cli;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.CommandLine;

namespace Rocket.Surgery.Cli
{
    public class Default : IDefaultCommand
    {
        private readonly IApplicationState state;
        private readonly ILogger<Default> logger;

        public Default(IApplicationState state, ILogger<Default> logger)
        {
            this.state = state;
            this.logger = logger;
        }

        public int Run(IApplicationState state)
        {
            Console.WriteLine("Hello World!");
            return 1;
        }
    }
}
