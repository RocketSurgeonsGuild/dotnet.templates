using Nuke.Common;
using Nuke.Common.Execution;
using Rocket.Surgery.Nuke.DotNetCore;
using Rocket.Surgery.Nuke;
using JetBrains.Annotations;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using Nuke.Common.Tools;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

[PublicAPI]
// [CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[PackageIcon("https://raw.githubusercontent.com/RocketSurgeonsGuild/graphics/master/png/social-square-thrust-rounded.png")]
[EnsurePackageSourceHasCredentials("RocketSurgeonsGuild")]
[EnsureGitHooks(GitHook.PreCommit)]
class Solution : DotNetCoreBuild, IDotNetCoreBuild
{
    /// <summary>
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    /// </summary>

    public static int Main() => Execute<Solution>(x => x.Default);

    Target Default => _ => _
        .DependsOn(Restore)
        .DependsOn(Build)
        .DependsOn(Test)
        .DependsOn(Pack)
        .DependsOn(Install)
        ;

    public new Target Restore => _ => _.With(this, DotNetCoreBuild.Restore);

    public new Target Build => _ => _.With(this, DotNetCoreBuild.Build);

    public new Target Test => _ => _.With(this, DotNetCoreBuild.Test);

    public new Target Pack => _ => _.With(this, DotNetCoreBuild.Pack);

    public Target Install => _ => _
        .After(Pack)
        .DependsOn(Pack)
        .OnlyWhenStatic(() => IsLocalBuild)
        .Executes(() =>
        {

            foreach (var item in NuGetPackageDirectory.GlobFiles("*.nupkg"))
            {
                try
                {
                    DotNet("new --uninstall Rocket.Surgery.Templates");
                }
                catch { }
                DotNet($"new --install {item}");
            }
            // DotNet("new -u ")
            // try
            // {
            //     DotNetToolUninstall(x => x.EnableGlobal().SetPackageName("sync-central-versions")
            //         .ResetVerbosity());
            // }
            // catch { }

            // DotNetToolInstall(x =>
            //     x.EnableGlobal().SetVersion(GitVersion.SemVer).AddSources(NuGetPackageDirectory)
            //         .SetPackageName("sync-central-versions"));
        });


}
