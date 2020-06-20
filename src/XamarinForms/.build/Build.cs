using System;
using System.Linq;
using System.Linq.Expressions;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Rocket.Surgery.Nuke;
using Rocket.Surgery.Nuke.Xamarin;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
internal class Practice : XamariniOSBuild, IXamariniOSBuild
{
    /// <summary>
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode
    /// </summary>

    public static int Main() => Execute<Practice>(x => x.Default);

    public override string BaseBundleIdentifier { get; } = "com.mobile";

    private Target Default => _ => _
       .DependsOn(XamariniOS);

    public Target Restore => _ => _
       .With(this, XamariniOSBuild.Restore);

    public Target Build => _ => _
       .With(this, XamariniOSBuild.Build)
       .DependsOn(ModifyPlist);

    public Target Test => _ => _
       .With(this, XamariniOSBuild.Test);

    public Target Package => _ => _
       .With(this, XamariniOSBuild.Package);

    public Target XamariniOS => _ => _
       .DependsOn(Restore)
       .DependsOn(ModifyPlist)
       .DependsOn(Build)
       .DependsOn(Test)
       .DependsOn(Package);
}