using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Statiq.App;
using Statiq.Common;
using Statiq.Core;

namespace NetlifySharp.Build
{
    public class Program
    {
        // Pipeline names
        public const string Build = nameof(Build);
        public const string Test = nameof(Test);
        public const string Docs = nameof(Docs);
        public const string Pack = nameof(Pack);
        public const string Publish = nameof(Publish);

        public static async Task<int> Main(string[] args) => await Bootstrapper
            .CreateDefault(args, DefaultsToAdd.All & ~DefaultsToAdd.Commands)

            // Configure build settings for the correct version
            .ConfigureSettings(x =>
            {
                string version = File.ReadAllLines("../../ReleaseNotes.md")[0].TrimStart('#').Trim();
                if (x.ContainsKey("BUILD_BUILDNUMBER"))
                {
                    version += "-build-" + x["BUILD_BUILDNUMBER"];
                }
                x["BuildVersion"] = version;
                x["BuildProperties"] = $"-p:Version={version} -p:AssemblyVersion={version} -p:FileVersion={version}";
            })

            // Add build commands to the CLI
            .AddBuildCommand("build", "Builds all projects.", nameof(Build))
            .AddBuildCommand("test", "Builds and tests all projects.", nameof(Test))
            .AddBuildCommand("docs", "Previews the documentation.", nameof(Docs))
            .AddBuildCommand("pack", "Packs the packages.", nameof(Pack))
            .AddBuildCommand("publish", "Publishes the packages and documentation site.", nameof(Publish))

            // Add pipelines
            .BuildPipeline(Build, pipeline => pipeline
                .ManuallyExecute()
                .WithProcessModules(
                    new ReadFiles("../../../src/*/*.csproj"),
                    new StartProcess(
                        "dotnet",
                        Config.FromDocument((doc, ctx) => $"build \"{doc.Source.FullPath}\" {ctx.Settings.GetString("BuildProperties")}"))
                        .WithSequentialExecution()
                        .LogOutput()))

            .BuildPipeline(Test, pipeline => pipeline
                .ManuallyExecute()
                .WithDependencies(Build)
                .WithProcessModules(
                    new ReadFiles("../../../tests/*/*.csproj"),
                    new StartProcess(
                        "dotnet",
                        Config.FromDocument((doc, ctx) =>
                        {
                            StringBuilder builder = new StringBuilder($"test --no-build --no-restore \"{doc.Source.FullPath}\" {ctx.Settings.GetString("BuildProperties")} /p:CollectCoverage=true");
                            if (ctx.Settings.ContainsKey("BUILD_BUILDNUMBER"))
                            {
                                // We're in Azure Pipelines so add the test logger
                                builder.Append(" --test-adapter-path:. --logger:AzurePipelines");
                            }
                            return builder.ToString();
                        }))
                        .WithSequentialExecution()
                        .LogOutput()))

            .BuildPipeline(Pack, pipeline => pipeline
                .ManuallyExecute()
                .WithDependencies(Test)
                .WithProcessModules(
                    new StartProcess(
                        "dotnet",
                        Config.FromContext(ctx => $"pack ../../src/NetlifySharp/NetlifySharp.csproj --no-build --no-restore -o \"{ctx.FileSystem.GetOutputDirectory("packages").Path}\" {ctx.Settings.GetString("BuildProperties")}"))
                        .LogOutput(),
                    new ReadFiles(Config.FromContext(ctx => $"{ctx.FileSystem.GetOutputDirectory("packages").Path}/*.nupkg")),
                    new ExecuteIf(Config.FromContext(ctx => ctx.Settings.ContainsKey("DAVIDGLICK_CERTPASS") && ctx.FileSystem.GetRootFile("../../digicert-davidglick.pfx").Exists))
                    {
                        new StartProcess(
                            "nuget",
                            Config.FromDocument((doc, ctx) =>
                            {
                                string certPath = ctx.FileSystem.GetRootFile("../../digicert-davidglick.pfx").Path.FullPath;
                                string password = ctx.Settings.GetString("DAVIDGLICK_CERTPASS");
                                return $"sign \"{doc.Source.FullPath}\" -CertificatePath \"{certPath}\" -CertificatePassword \"{password}\" -Timestamper \"http://timestamp.digicert.com\" -NonInteractive";
                            }))
                            .LogOutput()
                    }))

            // TODO: Generate zip
            // TODO: Generate GitHub release
            // TODO: Run docs generator with deployment
            .BuildPipeline(Publish, pipeline => pipeline
                .ManuallyExecute()
                .WithDependencies(Pack)
                .WithProcessModules(
                    new ReadFiles(Config.FromContext(ctx => $"{ctx.FileSystem.GetOutputDirectory("packages").Path}/*.nupkg")),
                    new StartProcess(
                        "dotnet",
                        Config.FromDocument((doc, ctx) => $"nuget push --api-key {ctx.Settings.GetString("NUGET_KEY")} --source \"https://api.nuget.org/v3/index.json\" \"{doc.Source.FullPath}\" "))
                        .WithSequentialExecution()
                        .LogOutput()))

            // Run the app
            .RunAsync();
    }
}
