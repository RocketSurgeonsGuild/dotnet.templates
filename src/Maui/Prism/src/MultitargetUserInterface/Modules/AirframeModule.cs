using MultitargetUserInterface.Features.Splash.Operations;

namespace MultitargetUserInterface.Modules;

public class AirframeModule : ServiceCollectionModule
{
    /// <inheritdoc />
    protected override IServiceCollection Load(IServiceCollection serviceCollection) =>
        serviceCollection
            .AddStartup<ApplicationStartup>(startupOption =>
                                                startupOption
                                                    .AddOperation<DelayOperation>());
}