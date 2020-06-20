using Rocket.Surgery.Conventions;
//#if autofac
using Rocket.Surgery.Extensions.Autofac;
//#endif
//#if commandline
using Rocket.Surgery.Extensions.CommandLine;
//#endif
//#if (!no-configuration)
using Rocket.Surgery.Extensions.Configuration;
//#endif
//#if (!no-services)
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
using Library;

[assembly: Convention(typeof(LibraryConvention))]

namespace Library
{
    public partial class LibraryConvention
    {
        //#if autofac
        public void Register(IAutofacConventionContext context)
        {

        }
        //#endif
        //#if (!no-configuration)
        public void Register(IConfigurationConventionContext context)
        {

        }
        //#endif
        //#if (!no-services)
        public void Register(IServiceConventionContext context)
        {

        }
        //#endif
        //#if logging
        public void Register(ILoggingConventionContext context)
        {

        }
        //#endif
        //#if serilog
        public void Register(ISerilogConventionContext context)
        {

        }
        //#endif
        //#if commandline
        public void Register(ICommandLineConventionContext context)
        {

        }
        //#endif
        //#if webjobs
        public void Register(IWebJobsConventionContext context)
        {

        }
        //#endif
    }

    #region Partial
    //#if autofac
    public partial class LibraryConvention : IAutofacConvention { }
    //#endif
    //#if (!no-configuration)
    public partial class LibraryConvention : IConfigurationConvention { }
    //#endif
    //#if (!no-services)
    public partial class LibraryConvention : IServiceConvention { }
    //#endif
    //#if logging
    public partial class LibraryConvention : ILoggingConvention { }
    //#endif
    //#if serilog
    public partial class LibraryConvention : ISerilogConvention { }
    //#endif
    //#if commandline
    public partial class LibraryConvention : ICommandLineConvention { }
    //#endif
    //#if webjobs
    public partial class LibraryConvention : IWebJobsConvention { }
    //#endif
    #endregion
}
