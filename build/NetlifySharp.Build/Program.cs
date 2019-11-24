using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Octokit;
using Statiq.App;
using Statiq.Common;
using Statiq.Core;

namespace NetlifySharp.Build
{
    public class Program
    {
        private const string BuildVersion = nameof(BuildVersion);
        private const string BuildProperties = nameof(BuildProperties);
        private const string ReleaseNotes = nameof(ReleaseNotes);

        public static async Task<int> Main(string[] args) => await Bootstrapper
            .CreateDefaultWithout(args, DefaultFeatures.HostingCommands)
            .ConfigureSettings(x =>
            {
                List<string> releaseLines = File.ReadAllLines("../../ReleaseNotes.md").ToList();
                int versionIndex = releaseLines.FindIndex(x => x.StartsWith("#"));
                string version = releaseLines[versionIndex].TrimStart('#').Trim();
                string releaseNotes = string.Join(
                    Environment.NewLine,
                    releaseLines.Skip(versionIndex + 1).TakeWhile(x => !x.StartsWith("#")).Where(x => !string.IsNullOrWhiteSpace(x)));
                if (x.ContainsKey("BUILD_BUILDNUMBER"))
                {
                    version += "-build-" + x["BUILD_BUILDNUMBER"];
                }
                x[BuildVersion] = version;
                x[BuildProperties] = $"-p:Version={version} -p:AssemblyVersion={version} -p:FileVersion={version}";
                x[ReleaseNotes] = releaseNotes;
            })
            .AddPipelines<Program>()
            .RunAsync();

        private static DirectoryPath GetBuildPath(IDocument doc, IExecutionContext ctx) =>
            ctx.FileSystem.GetOutputPath((DirectoryPath)("build/" + doc.Source.Directory.Name));

        public class Build : Pipeline
        {
            public Build()
            {
                ExecutionPolicy = ExecutionPolicy.Manual;
                ProcessModules = new ModuleList
                {
                    new ReadFiles("../../../src/*/*.csproj"),
                    new StartProcess("dotnet")
                        .WithArgument("build")
                        .WithArgument(Config.FromDocument(doc => doc.Source.FullPath), true)
                        .WithArgument("-o", Config.FromDocument((doc, ctx) => GetBuildPath(doc, ctx).FullPath), true)
                        .WithArgument(Config.FromSetting<string>(BuildProperties))
                        .WithSequentialExecution()
                        .LogOutput()
                };
            }
        }

        public class Test : Pipeline
        {
            public Test()
            {
                ExecutionPolicy = ExecutionPolicy.Manual;
                Dependencies.Add(nameof(Build));
                ProcessModules = new ModuleList
                {
                    new ReadFiles("../../../tests/*/*.csproj"),
                    new StartProcess("dotnet")
                        .WithArgument("test")
                        .WithArgument("--no-build")
                        .WithArgument("--no-restore")
                        .WithArgument(Config.FromDocument(doc => doc.Source.FullPath))
                        .WithArgument("-o", Config.FromDocument((doc, ctx) => GetBuildPath(doc, ctx).FullPath), true)
                        .WithArgument(Config.FromSetting<string>(BuildProperties))
                        .WithArgument("/p:CollectCoverage=true")
                        .WithArgument(Config.FromSettings(settings => settings.ContainsKey("BUILD_BUILDNUMBER") ? "--test-adapter-path:. --logger:AzurePipelines" : null))
                        .WithSequentialExecution()
                        .LogOutput()
                };
            }
        }

        public class Pack : Pipeline
        {
            public Pack()
            {
                ExecutionPolicy = ExecutionPolicy.Manual;
                Dependencies.Add(nameof(Test));
                ProcessModules = new ModuleList
                {
                    new StartProcess("dotnet")
                        .WithArgument("pack")
                        .WithArgument("../../src/NetlifySharp/NetlifySharp.csproj", true)
                        .WithArgument("--no-build")
                        .WithArgument("--no-restore")
                        .WithArgument("-o", Config.FromContext(ctx => ctx.FileSystem.GetOutputDirectory("packages").Path.FullPath), true)
                        .WithArgument(Config.FromSetting<string>(BuildProperties))
                        .LogOutput(),
                    new ReadFiles(Config.FromContext(ctx => $"{ctx.FileSystem.GetOutputDirectory("packages")}/*.nupkg")),
                    new ExecuteIf(Config.FromContext(ctx => ctx.ContainsKey("DAVIDGLICK_CERTPASS") && ctx.FileSystem.GetRootFile("../../digicert-davidglick.pfx").Exists))
                    {
                        new StartProcess("nuget")
                            .WithArgument("sign")
                            .WithArgument(Config.FromDocument(doc => doc.Source.FullPath), true)
                            .WithArgument("-CertificatePath", Config.FromContext(ctx => ctx.FileSystem.GetRootFile("../../digicert-davidglick.pfx").Path.FullPath), true)
                            .WithArgument("-CertificatePassword", Config.FromSetting<string>("DAVIDGLICK_CERTPASS"), true)
                            .WithArgument("-Timestamper", "http://timestamp.digicert.com", true)
                            .WithArgument("-NonInteractive")
                            .LogOutput()
                    }
                };
            }
        }

        public class Zip : Pipeline
        {
            public Zip()
            {
                ExecutionPolicy = ExecutionPolicy.Manual;
                Dependencies.Add(nameof(Test));
                ProcessModules = new ModuleList
                {
                    new ReadFiles("../../../src/*/*.csproj"),
                    new ExecuteModules(
                        new ExecuteConfig(Config.FromDocument((doc, ctx) =>
                            (object)new CopyFiles("../../../README.md", "../../../ReleaseNotes.md", "../../../LICENSE")
                                .To(x => Task.FromResult(GetBuildPath(doc, ctx).CombineFile(x.Path.FileName)))))),
                    new ZipDirectory(Config.FromDocument(GetBuildPath)),
                    new SetDestination(Config.FromDocument((doc, ctx) => (FilePath)$"zip/{doc.Source.FileNameWithoutExtension}-{ctx.GetString(BuildVersion)}.zip")),
                    new WriteFiles()
                };
            }
        }

        public class Publish : Pipeline
        {
            public Publish()
            {
                ExecutionPolicy = ExecutionPolicy.Manual;
                Deployment = true;
                Dependencies.AddRange(nameof(Pack), nameof(Zip));
                OutputModules = new ModuleList
                {
                    new ThrowException(Config.FromSettings(settings => settings.ContainsKey("NUGET_KEY") ? null : "The setting NUGET_KEY is required")),
                    new ThrowException(Config.FromSettings(settings => settings.ContainsKey("GITHUB_TOKEN") ? null : "The setting GITHUB_TOKEN is required")),
                    new ExecuteModules
                    {
                        new ReadFiles(Config.FromContext(ctx => $"{ctx.FileSystem.GetOutputDirectory("packages")}/*.nupkg")),
                        new StartProcess("dotnet")
                            .WithArgument("nuget")
                            .WithArgument("push")
                            .WithArgument("--api-key", Config.FromSetting<string>("NUGET_KEY"))
                            .WithArgument("--source", "https://api.nuget.org/v3/index.json", true)
                            .WithArgument(Config.FromDocument(doc => doc.Source.FullPath), true)
                            .WithSequentialExecution()
                            .LogOutput()
                    },
                    new ExecuteModules
                    {
                        new ReadFiles(Config.FromContext(ctx => $"{ctx.FileSystem.GetOutputDirectory("zip")}/*.zip")),
                        new ExecuteConfig(Config.FromContext(async ctx =>
                        {
                            GitHubClient github = new GitHubClient(new ProductHeaderValue(nameof(NetlifySharp)));
                            github.Credentials = new Credentials(ctx.GetString("GITHUB_TOKEN"));
                            Release release = github.Repository.Release.Create("daveaglick", "NetlifySharp", new NewRelease(ctx.GetString(BuildVersion))
                            {
                                Name = ctx.GetString(BuildVersion),
                                Body = string.Join(Environment.NewLine, ctx.GetString(ReleaseNotes)),
                                TargetCommitish = "master"
                            }).Result;

                            foreach (IDocument input in ctx.Inputs)
                            {
                                using (Stream stream = input.GetContentStream())
                                {
                                    await github.Repository.Release.UploadAsset(
                                        release,
                                        new ReleaseAssetUpload(input.Source.FileName.FullPath, "application/zip", stream, null));
                                }
                            }
                        }))
                    }
                };
            }
        }
    }
}
