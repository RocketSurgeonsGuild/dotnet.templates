using Android.App;
using Android.Content.PM;

namespace MultitargetUserInterface.Platforms.Android;

[Activity(Theme = "@style/MultitargetUserInterface.SplashTheme", MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
}