using Shiny.Stores;

namespace MultitargetUserInterface.Modules;

public class ShinyModule : ServiceCollectionModule
{
    /// <inheritdoc />
    protected override IServiceCollection Load(IServiceCollection serviceCollection) =>
        serviceCollection
            .AddSingleton<IKeyValueStore, MemoryKeyValueStore>();
}