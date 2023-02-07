namespace MultitargetUserInterface.Modules;

public class PrismNavigationModule : ServiceCollectionModule
{
    /// <inheritdoc />
    protected override IServiceCollection Load(IServiceCollection serviceCollection) => serviceCollection
       .RegisterForNavigation<NavigationPage>(nameof(NavigationPage))
       .RegisterForNavigation<MainPage, MainViewModel>(nameof(MainPage));
}