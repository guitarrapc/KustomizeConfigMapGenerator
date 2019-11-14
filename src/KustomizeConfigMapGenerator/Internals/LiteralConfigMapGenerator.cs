using System;
using System.Collections.Generic;
using System.Text;

namespace KustomizeConfigMapGenerator.Internals
{
    internal class LiteralConfigMapGenerator : ConfigMapGeneratorBase
    {
        // configMapGenerator spec:
        //
        // {header}: <- required
        //    - name: {name} <- required
        //      behavior: {behavior} <- optional
        //      {kind}: <- requires
        //        - key=value <- requires
        //    - name: {name} <- required
        //      behavior: {behavior} <- optional
        //      {kind}: <- requires
        //        - key=value <- requires
        //
        // # support
        // configMapGenerator:
        //   - name: the-map
        //     behavior: merge
        //     literals:
        //       - altGreeting=Good Morning!
        //       - enableRisky=false

        public LiteralConfigMapGenerator(string name, Behavior behavior, bool skipHeader)
            => (Name, Behavior, SkipHeader) = (name, behavior, skipHeader);

        public override string Name { get; }
        public override Behavior Behavior { get; }
        public override bool SkipHeader { get; }
        protected override string KindKey => "literals:";

        public string Generate(IEnumerable<string> inputs)
        {
            // get configmap YAML from template
            var yaml = EmbeddedTemplate(inputs);

            // output to caller
            return yaml;
        }

        protected override string EmbeddedTemplate(IEnumerable<string> values)
        {
            var builder = new StringBuilder();

            // header
            if (!SkipHeader)
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
