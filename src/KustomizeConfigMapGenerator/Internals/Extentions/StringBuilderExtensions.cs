using System;
using System.Collections.Generic;
using System.Text;

namespace KustomizeConfigMapGenerator.Internals
{
    internal static class StringBuilderExtensions
    {
        private readonly static char lf = '\n';
        private readonly static string indent2 = "  ";
        private readonly static string indent4 = "    ";
        private readonly static string indent6 = "      ";
        public static StringBuilder AppendLineLF(this StringBuilder builder, string value)
        {
            builder.Append(value + lf);
            return builder;
        }
        public static StringBuilder AppendLineLFIndent2(this StringBuilder builder, string value)
            => builder.AppendLineLF(indent2 + value);
        public static StringBuilder AppendLineLFIndent4(this StringBuilder builder, string value)
            => builder.AppendLineLF(indent4 + value);
        public static StringBuilder AppendLineLFIndent6(this StringBuilder builder, string value)
            => builder.AppendLineLF(indent6 + value);
    }
}
