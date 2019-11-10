using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KustomizeConfigMapGenerator.Internals
{
    internal class FileConfigMapGenerator : ConfigMapGeneratorBase
    {
        // configMapGenerator spec:
        //
        // {header}: <- required
        //    - name: {name} <- required
        //      behavior: {behavior} <- optional
        //      {kind}: <- requires
        //        - filepath <- requires
        //    - name: {name} <- required
        //      behavior: {behavior} <- optional
        //      {kind}: <- requires
        //        - filepath <- requires
        //
        // # support
        // configMapGenerator:
        //   - name: the-files
        //     behavior: merge
        //     files:	
        //       - hoge.txt
        //       - foo/bar.txt

        // # not support
        // configMapGenerator:
        //   - name: the-files
        //     behavior: merge
        //     files:	
        //       - configkey=hoge.txt

        public FileConfigMapGenerator(string name, Behavior behavior, bool append)
            => (Name, Behavior, Append) = (name, behavior, append);

        public override string Name { get; }
        public override Behavior Behavior { get; }
        public override bool Append { get; }
        protected override string KindKey => "files:";

        public string Generate(string basePath, string searchPattern)
        {
            // get files from basepath -> FileInfo[]
            var files = Directory.EnumerateFiles(basePath, searchPattern, SearchOption.AllDirectories)
                .Select(x => x.Replace(basePath, ""))
                .Select(x => x.Replace(@"\", "/"));

            if (!files.Any())
                return "";

            // get configmap YAML from template
            var yaml = EmbeddedTemplate(files);

            // output to caller
            return yaml;
        }

        protected override string EmbeddedTemplate(IEnumerable<string> values)
        {
            var builder = new StringBuilder();

            // header
            if (!Append)
            {
                builder.AppendLineLF(Header);
            }

            // name
            builder.AppendLineLFIndent2(NameKey + Name);

            // behavior
            if (Behavior != Behavior.unspecified)
            {
                builder.AppendLineLFIndent4(BehaviorKey + Behavior.ToString());
            }

            // kind
            builder.AppendLineLFIndent4(KindKey);

            // values
            foreach (var value in values)
            {
                builder.AppendLineLFIndent6($"- {value}");
            }
            return builder.ToString();
        }
    }
}
