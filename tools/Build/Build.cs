return BuildRunner.Execute(args, build =>
{
	var codegen = "fsdgencsharp";

	var gitLogin = new GitLoginInfo("FacilityApiBot", Environment.GetEnvironmentVariable("BUILD_BOT_PASSWORD") ?? "");

	var dotNetBuildSettings = new DotNetBuildSettings
	{
		NuGetApiKey = Environment.GetEnvironmentVariable("NUGET_API_KEY"),
		DocsSettings = new DotNetDocsSettings
		{
			GitLogin = gitLogin,
			GitAuthor = new GitAuthorInfo("FacilityApiBot", "facilityapi@gmail.com"),
			SourceCodeUrl = "https://github.com/FacilityApi/FacilityCSharp/tree/master/src",
			ProjectHasDocs = name => !name.StartsWith("fsdgen", StringComparison.Ordinal) && name != "FacilityConformance",
		},
		PackageSettings = new DotNetPackageSettings
		{
			GitLogin = gitLogin,
			PushTagOnPublish = x => $"nuget.{x.Version}",
		},
	};

	build.AddDotNetTargets(dotNetBuildSettings);

	build.Target("codegen")
		.Describe("Generates code from the FSD")
		.Does(() => CodeGen(verify: false));

	build.Target("verify-codegen")
		.Describe("Ensures the generated code is up-to-date")
		.Does(() => CodeGen(verify: true));

	build.Target("test")
		.DependsOn("verify-codegen");

	void CodeGen(bool verify)
	{
		var configuration = dotNetBuildSettings.GetConfiguration();
		RunDotNet("build", "-f", "net6.0", "-c", configuration, $"src/{codegen}");

		RunCodeGen("fsd/FacilityCore.fsd", "src/Facility.Core/", "--nullable");
		RunCodeGen("conformance/ConformanceApi.fsd", "src/Facility.ConformanceApi/", "--nullable", "--clean");
		RunCodeGen("tools/EdgeCases.fsd", "tools/EdgeCases/", "--nullable", "--clean");

		void RunCodeGen(params string?[] args) =>
			RunDotNet(new[] { "run", "--no-build", "--project", $"src/{codegen}", "-f", "net6.0", "-c", configuration, "--", "--newline", "lf", verify ? "--verify" : null }.Concat(args));
	}
});
