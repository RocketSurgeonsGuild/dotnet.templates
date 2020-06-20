using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.MSBuild;
using Rocket.Surgery.Nuke;
using Rocket.Surgery.Nuke.DotNetCore;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[PublicAPI]
[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
[PackageIcon("https://raw.githubusercontent.com/RocketSurgeonsGuild/graphics/master/png/social-square-thrust-rounded.png")]
[EnsureGitHooks(GitHook.PreCommit)]
[DotNetVerbosityMapping]
[MSBuildVerbosityMapping]
[NuGetVerbosityMapping]
public partial class Solution : NukeBuild,
                        ICanRestoreWithDotNetCore,
                        ICanBuildWithDotNetCore,
                        ICanTestWithDotNetCore,
                        ICanPackWithDotNetCore,
                        IComprehendTemplates,
                        IHaveDataCollector,
                        ICanClean,
                        ICanUpdateReadme,
                        IGenerateCodeCoverageReport,
                        IGenerateCodeCoverageSummary,
                        IGenerateCodeCoverageBadges,
                        IHaveConfiguration<Configuration>,
                        ICanLint,
                        ICanInstallDotNetTemplates
{
    /// <summary>
    /// Support plugins are available for:
    /// - JetBrains ReSharper        https://nuke.build/resharper
    /// - JetBrains Rider            https://nuke.build/rider
    /// - Microsoft VisualStudio     https://nuke.build/visualstudio
    /// - Microsoft VSCode           https://nuke.build/vscode
    /// </summary>
    public static int Main() => Execute<Solution>(x => x.Default);

    [OptionalGitRepository]
    public GitRepository? GitRepository { get; }

    private Target Default => _ => _
       .DependsOn(Restore)
       .DependsOn(Build)
       .DependsOn(Test)
       .DependsOn(Pack)
       .DependsOn(Install)
       .DependsOn(InstallForTest)
       ;

    public Target Build => _ => _.Inherit<ICanBuildWithDotNetCore>(x => x.CoreBuild);
    public Target Install => _ => _.Inherit<ICanInstallDotNetTemplates>(x => x.Install);
    public Target InstallForTest => _ => _.Inherit<ICanInstallDotNetTemplates>(x => x.InstallForTest);

    public Target Pack => _ => _.Inherit<ICanPackWithDotNetCore>(x => x.CorePack)
       .DependsOn(Clean);

    [ComputedGitVersion]
    public GitVersion GitVersion { get; } = null!;

    public Target Clean => _ => _.Inherit<ICanClean>(x => x.Clean);
    public Target Restore => _ => _.Inherit<ICanRestoreWithDotNetCore>(x => x.CoreRestore);
    public Target Test => _ => _.Inherit<ICanTestWithDotNetCore>(x => x.CoreTest);

    public Target BuildVersion => _ => _.Inherit<IHaveBuildVersion>(x => x.BuildVersion)
       .Before(Default)
       .Before(Clean);

    [Parameter("Configuration to build")]
    public Configuration Configuration { get; } = IsLocalBuild ? Configuration.Debug : Configuration.Release;
}

public interface ICanInstallDotNetTemplates : IHaveTestTarget, IHaveBuildTarget, IHavePackTarget, IHaveNuGetPackages, IComprehendSources
{
    public Target InstallForTest => _ => _
       .Before(Test)
       .DependsOn(Build)
       .Executes(() =>
        {
            DotNet($"new --install {SourceDirectory}");
        });

    public Target Install => _ => _
       .After(Pack)
       .DependsOn(Pack)
       .OnlyWhenStatic(() => NukeBuild.IsLocalBuild)
       .Executes(() =>
        {
            DotNet($"new --uninstall {SourceDirectory}");
            foreach (var item in ( (IHaveNuGetPackages)this ).NuGetPackageDirectory.GlobFiles("*.nupkg"))
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
