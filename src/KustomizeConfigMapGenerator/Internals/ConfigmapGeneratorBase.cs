﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KustomizeConfigMapGenerator.Internals
{
    internal abstract class ConfigMapGeneratorBase : IConfigMapGenerator
    {
        protected static readonly Encoding encoding = new UTF8Encoding(false);

        // spec
        protected const string Header = "configMapGenerator:";
        protected const string NameKey = "- name: ";
        protected const string BehaviorKey = "behavior: ";
        protected abstract string KindKey { get; }
        protected abstract string EmbeddedTemplate(IEnumerable<string> values);

        // interface
        public abstract string Name { get; }
        public abstract Behavior Behavior { get; }
        public abstract bool SkipHeader { get; }
        public async Task WriteAsync(string contents, string outputPath, bool force, bool append, CancellationToken cancellationToken = default)
        {
            var directory = Path.GetDirectoryName(outputPath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (File.Exists(outputPath) && !force)
                throw new InvalidOperationException($"Operation cancelled. File already exists. Set `-f true` to force overwrite existing. {outputPath}");

            if (append)
            {
                await File.AppendAllTextAsync(outputPath, contents, encoding, cancellationToken);
            }
            else
            {
                await File.WriteAllTextAsync(outputPath, contents, encoding, cancellationToken);
            }
        }
    }
}
