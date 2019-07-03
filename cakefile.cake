#load "nuget:?package=Rocket.Surgery.Cake.Library&version=0.10.0";

Task("Default")
    .IsDependentOn("dotnetcore test");

Task("template pack")
    .WithCriteria(() => Settings.Pack.Enabled)
    .IsDependeeOf("Default")
    .DoesForEach(GetFiles("*.sln"), (solution) => {
        DotNetCorePack(
            solution.FullPath,
            new DotNetCorePackSettings() {
                Verbosity = Settings.DotNetCoreVerbosity,
                Configuration = Settings.Configuration,
                EnvironmentVariables = Settings.Environment,
                OutputDirectory = ArtifactDirectoryPath("nuget").MakeAbsolute(Context.Environment).FullPath,
                ArgumentCustomization = a => CreateBinLogger(a, "template-pack"),
                MSBuildSettings = CreateDotNetCoreMsBuildSettings("template-pack")
                    .WithProperty("RestoreNoCache", BuildSystem.IsLocalBuild.ToString()),
            }
        );
    });

RunTarget(Target);
