using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Cli;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.CommandLine;

namespace Rocket.Surgery.Cli
{
    [PublicAPI]
    public class Default : IDefaultCommand
    {
        private readonly IApplicationState _state;
        private readonly ILogger<Default> _logger;

        public Default(IApplicationState state, ILogger<Default> logger)
        {
            _state = state;
            _logger = logger;
        }

        public int Run(IApplicationState state)
        {
            Console.WriteLine("Hello World!");
            return 1;
        }
    }
}
