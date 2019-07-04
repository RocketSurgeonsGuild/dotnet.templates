using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Rocket.Surgery.Conventions;
//#if (!no-services)
using Rocket.Surgery.Extensions.Autofac;
//#endif
//#if commandline
using Rocket.Surgery.Extensions.CommandLine;
//#endif
//#if (!no-configuration)
using Rocket.Surgery.Extensions.Configuration;
//#endif
//#if autofac
using Rocket.Surgery.Extensions.DependencyInjection;
//#endif
//#if logging
using Rocket.Surgery.Extensions.Logging;
//#endif
//#if serilog
using Rocket.Surgery.Extensions.Serilog;
//#endif
//#if webjobs
using Rocket.Surgery.Extensions.WebJobs;
//#endif
using Rocket.Surgery.Library;

[assembly: Convention(typeof(LibraryConvention))]

namespace Rocket.Surgery.Library
{
    public partial class LibraryConvention { }
    //#if (!no-configuration)
    public partial class LibraryConvention : IConfigurationConvention
    {
        void IConvention<IConfigurationConventionContext>.Register(IConfigurationConventionContext context)
        {
            // Add stuff here
        }
    }
    //#endif
    //#if (!no-services)
    public partial class LibraryConvention : IServiceConvention
    {
        void IConvention<IServiceConventionContext>.Register(IServiceConventionContext context)
        {
            // Add stuff here
        }
    }
    //#endif
    //#if autofac
    public partial class LibraryConvention : IAutofacConvention
    {
        void IConvention<IAutofacConventionContext>.Register(IAutofacConventionContext context)
        {
            // Add stuff here
        }
    }
    //#endif
    //#if logging
    public partial class LibraryConvention : ILoggingConvention
    {
        void IConvention<ILoggingConventionContext>.Register(ILoggingConventionContext context)
        {
            // Add stuff here
        }
    }
    //#endif
    //#if serilog
    public partial class LibraryConvention : ISerilogConvention
    {
        void IConvention<ISerilogConventionContext>.Register(ISerilogConventionContext context)
        {
            // Add stuff here
        }
    }
    //#endif
    //#if commandline
    public partial class LibraryConvention : ICommandLineConvention
    {
        void IConvention<ICommandLineConventionContext>.Register(ICommandLineConventionContext context)
        {
            // Add stuff here
        }
    }
    //#endif
    //#if webjobs
    public partial class LibraryConvention : IWebJobsConvention
    {
        void IConvention<IWebJobsConventionContext>.Register(IWebJobsConventionContext context)
        {
            // Add stuff here
        }
    }
    //#endif
}
