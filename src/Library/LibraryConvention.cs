using Rocket.Surgery.Conventions;
//#if UseAutofac
using Rocket.Surgery.Extensions.Autofac;
//#endif
//#if UseDryIoc
using Rocket.Surgery.Extensions.DryIoc;
//#endif
//#if UseCommandLine
using Rocket.Surgery.Extensions.CommandLine;
//#endif
//#if (!SkipConfiguration)
using Rocket.Surgery.Extensions.Configuration;
//#endif
//#if (!SkipDependencyInjection)
using Rocket.Surgery.Extensions.DependencyInjection;
//#endif
//#if (!SkipLogging)
using Rocket.Surgery.Extensions.Logging;
//#endif
//#if UseSerilog
using Rocket.Surgery.Extensions.Serilog;
//#endif
//#if UseWebJobs
using Rocket.Surgery.Extensions.WebJobs;
//#endif
using Library;

[assembly: Convention(typeof(LibraryConvention))]

namespace Library
{
    public partial class LibraryConvention
    {
        //#if UseAutofac
        public void Register(IAutofacConventionContext context)
        {

        }
        //#endif
        //#if UseDryIoc
        public void Register(IDryIocConventionContext context)
        {

        }
        //#endif
        //#if (!SkipConfiguration)
        public void Register(IConfigurationConventionContext context)
        {

        }
        //#endif
        //#if (!SkipDependencyInjection)
        public void Register(IServiceConventionContext context)
        {

        }
        //#endif
        //#if (!SkipLogging)
        public void Register(ILoggingConventionContext context)
        {

        }
        //#endif
        //#if UseSerilog
        public void Register(ISerilogConventionContext context)
        {

        }
        //#endif
        //#if UseCommandLine
        public void Register(ICommandLineConventionContext context)
        {

        }
        //#endif
        //#if UseWebJobs
        public void Register(IWebJobsConventionContext context)
        {

        }
        //#endif
    }

    #region Partial
    //#if UseAutofac
    public partial class LibraryConvention : IAutofacConvention { }
    //#endif
    //#if UseDryIoc
    public partial class LibraryConvention : IDryIocConvention { }
    //#endif
    //#if (!SkipConfiguration)
    public partial class LibraryConvention : IConfigurationConvention { }
    //#endif
    //#if (!SkipDependencyInjection)
    public partial class LibraryConvention : IServiceConvention { }
    //#endif
    //#if (!SkipLogging)
    public partial class LibraryConvention : ILoggingConvention { }
    //#endif
    //#if UseSerilog
    public partial class LibraryConvention : ISerilogConvention { }
    //#endif
    //#if UseCommandLine
    public partial class LibraryConvention : ICommandLineConvention { }
    //#endif
    //#if UseWebJobs
    public partial class LibraryConvention : IWebJobsConvention { }
    //#endif
    #endregion
}
