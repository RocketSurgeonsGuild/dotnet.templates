#load "nuget:?package=Rocket.Surgery.Cake.Library&version=0.9.2";

Task("Default")
    .IsDependentOn("dotnetcore");

Task("template pack")
    .WithCriteria(() => Settings.Pack.Enabled)
    .IsDependeeOf("Default")
    .DoesForEach(GetFiles("templates/*.csproj"), (solution) => {
        DotNetCorePack(
            solution.FullPath,
            new DotNetCorePackSettings() {
                Verbosity = Settings.DotNetCoreVerbosity,
                Configuration = Settings.Configuration,
                EnvironmentVariables = Settings.Environment,
                OutputDirectory = ArtifactDirectoryPath("nuget").MakeAbsolute(Context.Environment).FullPath,
                ArgumentCustomization = a => CreateBinLogger(a, "template-pacl"),
                MSBuildSettings = CreateDotNetCoreMsBuildSettings("template-pacl")
                    .WithProperty("RestoreNoCache", BuildSystem.IsLocalBuild.ToString()),
            }
        );
    });

RunTarget(Target);
