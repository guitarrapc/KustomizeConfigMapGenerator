﻿using KustomizeConfigMapGenerator.Internals;
using MicroBatchFramework;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace KustomizeConfigMapGenerator
{
    class Program
    {
        static void Main(string[] args)
            => BatchHost.CreateDefaultBuilder()
                .RunBatchEngineAsync<GeneratorBatch>(args);
    }

    public class GeneratorBatch : BatchBase
    {
        [Command("files", "Generate ConfigMap from specific path's files.")]
        public async Task ExecuteFile(
            [Option("-i", "path to the base directory to search.")]string inputPath,
            [Option("-o", "path to the output kustomization file.")]string outputPath,
            [Option("-s", "search pattern of files. (ex. *.config)")]string searchPattern,
            [Option("-n", "configmap name of ConfigMapGenerator.")]string name,
            [Option("-b", "ConfigMapGenerator behavior.")]Behavior behavior = Behavior.unspecified,
            [Option("-skip", "skip configMapGenerator: output.")]bool skipHeader = false,
            [Option("-a", "append just an value to existing config.")]bool append = false,
            [Option("-f", "override outputfile without prompt.")]bool forceOutput = false,
            [Option("-d", "dry run.")]bool dryRun = true
        )
        {
            var generator = new FileConfigMapGenerator(name, behavior, skipHeader);
            var contents = generator.Generate(inputPath, searchPattern);
            if (dryRun)
            {
                Context.Logger.LogInformation("dryrun mode detected. see output contents.");
                Context.Logger.LogInformation("");
                Context.Logger.LogInformation(contents);
            }
            else
            {
                Context.Logger.LogInformation($"begin writing following contents to {outputPath}.");
                Context.Logger.LogInformation("");
                Context.Logger.LogInformation(contents);

                await generator.WriteAsync(contents, outputPath, forceOutput, append, Context.CancellationToken);
            }
        }

        [Command("literals", "Generate ConfigMap from specific key=value literals.")]
        public async Task ExecuteLiterals(
            [Option("-i", "comma separated key=value style literals. (ex. foo=bar,hoge=fuga")]string inputs,
            [Option("-o", "path to the output kustomization file.")]string outputPath,
            [Option("-n", "configmap name of ConfigMapGenerator.")]string name,
            [Option("-b", "ConfigMapGenerator behavior.")]Behavior behavior = Behavior.unspecified,
            [Option("-skip", "skip configMapGenerator: output.")]bool skipHeader = false,
            [Option("-a", "append just an value to existing config.")]bool append = false,
            [Option("-f", "override outputfile without prompt.")]bool forceOutput = false,
            [Option("-d", "dry run.")]bool dryRun = true
        )
        {
            var generator = new LiteralConfigMapGenerator(name, behavior, skipHeader);
            var keyvalues = inputs.Split(',');
            var contents = generator.Generate(keyvalues);
            if (dryRun)
            {
                Context.Logger.LogInformation("dryrun mode detected. see output contents.");
                Context.Logger.LogInformation("");
                Context.Logger.LogInformation(contents);
            }
            else
            {
                Context.Logger.LogInformation($"begin writing following contents to {outputPath}.");
                Context.Logger.LogInformation("");
                Context.Logger.LogInformation(contents);

                await generator.WriteAsync(contents, outputPath, forceOutput, append, Context.CancellationToken);
            }
        }
    }

    // spec: https://github.com/kubernetes-sigs/kustomize/blob/master/docs/plugins/builtins.md#field-name-configMapGenerator

    // behavior
    public enum Behavior
    {
        /// <summary>
        /// typically treated as a Create.
        /// </summary>
        unspecified,
        /// <summary>
        /// makes a new resource
        /// </summary>
        create,
        // replaces a resource
        replace,
        // attempts to merge a new resource with an existing resource.
        merge,
    }

    // files: array<string> (string is `filename`)
    // literals: array<string> (string is `key=value`)
    public enum Kind
    {
        File,
        Literals
    }
}
