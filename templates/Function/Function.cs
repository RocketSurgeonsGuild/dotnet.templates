using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Rocket.Surgery.Functions
{
    [PublicAPI]
    public class Function
    {
        private readonly ILogger _logger;

        public Function(ILogger<Function> logger) => _logger = logger;

        [FunctionName("Function")]
        public ActionResult<int> Run([HttpTrigger("get")]HttpRequest context, ILogger logger)
        {
            logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            return 1;
        }
    }
}
