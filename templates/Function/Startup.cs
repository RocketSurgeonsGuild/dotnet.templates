using System;
using JetBrains.Annotations;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rocket.Surgery.Extensions.DependencyInjection;
using Rocket.Surgery.Hosting.Functions;

[assembly: WebJobsStartup(typeof(Rocket.Surgery.Functions.Startup))]

namespace Rocket.Surgery.Functions
{
    [PublicAPI]
    public class Startup : IWebJobsStartup, IServiceConvention
    {
        public void Register(IServiceConventionContext context)
        {
        }

        public void Configure(IWebJobsBuilder builder)
        {
            builder.UseRocketSurgery(
                this,
                hostBuilder =>
                {
                });
        }
    }
}
