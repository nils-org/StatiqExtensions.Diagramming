#load nuget:?package=Cake.Recipe&version=4.0.0

Environment.SetVariableNames();

BuildParameters.SetParameters(
    context: Context,
    buildSystem: BuildSystem,
    sourceDirectoryPath: "./src",
    title: "StatiqExtensions.Diagramming",
    solutionFilePath: "./src/Diagramming.sln",
    masterBranchName: "main",
    repositoryOwner: "nils-org",
    shouldRunDotNetCorePack: true,
    shouldUseDeterministicBuilds: true,
    shouldRunCodecov: false,
    preferredBuildProviderType: BuildProviderType.GitHubActions,
    preferredBuildAgentOperatingSystem: PlatformFamily.Linux);

BuildParameters.PrintParameters(Context);

ToolSettings.SetToolSettings(context: Context);

Build.RunDotNetCore();
