namespace MultitargetUserInterface.Features.Splash;

public class SplashModule : ServiceCollectionModule
{
    protected override IServiceCollection Load(IServiceCollection serviceCollection) =>
        serviceCollection
            .RegisterForNavigation<SplashScreen, SplashViewModel>(nameof(SplashScreen));
}