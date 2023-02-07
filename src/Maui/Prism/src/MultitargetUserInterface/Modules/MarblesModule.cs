using ReactiveMarbles.Locator;
using System.Reactive.Concurrency;

namespace MultitargetUserInterface.Modules;

public class MarblesModule : ServiceCollectionModule
{
    protected override IServiceCollection Load(IServiceCollection serviceCollection)
    {
        var coreRegistration = CoreRegistrationBuilder
            .Create()
            .WithMainThreadScheduler(RxApp.MainThreadScheduler)
            .WithTaskPoolScheduler(TaskPoolScheduler.Default)
            .WithExceptionHandler(new DebugExceptionHandler())
            .Build();

        ServiceLocator
            .Current()
            .AddCoreRegistrations(() => coreRegistration);

        return serviceCollection.AddSingleton<ICoreRegistration>(_ => coreRegistration);
    }
}