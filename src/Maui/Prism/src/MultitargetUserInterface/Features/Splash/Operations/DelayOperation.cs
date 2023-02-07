namespace MultitargetUserInterface.Features.Splash.Operations;

public sealed class DelayOperation : LoggableStartupOperation
{
    public DelayOperation(ICoreRegistration coreRegistration, ILoggerFactory factory) : base(factory) => _coreRegistration = coreRegistration;

    protected override IObservable<Unit> Start() => Observable
        .Return(Unit.Default);

    private readonly ICoreRegistration _coreRegistration;
}