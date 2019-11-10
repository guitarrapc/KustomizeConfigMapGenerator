using KustomizeConfigMapGenerator;
using KustomizeConfigMapGenerator.Internals;
using System;
using System.IO;
using Xunit;

namespace KustomizeConfigMapGenerator.Tests
{
    public class LiteralConfigMapGeneratorTests
    {
        private static readonly string[] inputs = new[] { "foo=bar", "piyo=poyo", "hoge=fuga" };

        [Theory]
        [InlineData("test", Behavior.unspecified, false)]
        public void UnspecifiedContentsTest(string name, Behavior behavior, bool append)
        {
            var generator = new LiteralConfigMapGenerator(name, behavior, append);
            var actual = generator.Generate(inputs);
            // result is always LF
            var expected = @$"configMapGenerator:
  - name: {name}
    literals:
      - foo=bar
      - piyo=poyo
      - hoge=fuga
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.create, false)]
        public void CreateContentsTest(string name, Behavior behavior, bool append)
        {
            var generator = new LiteralConfigMapGenerator(name, behavior, append);
            var actual = generator.Generate(inputs);
            // result is always LF
            var expected = @$"configMapGenerator:
  - name: {name}
    behavior: create
    literals:
      - foo=bar
      - piyo=poyo
      - hoge=fuga
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.merge, false)]
        public void MergeContentsTest(string name, Behavior behavior, bool append)
        {
            var generator = new LiteralConfigMapGenerator(name, behavior, append);
            var actual = generator.Generate(inputs);
            // result is always LF
            var expected = @$"configMapGenerator:
  - name: {name}
    behavior: merge
    literals:
      - foo=bar
      - piyo=poyo
      - hoge=fuga
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.replace, false)]
        public void ReplaceContentsTest(string name, Behavior behavior, bool append)
        {
            var generator = new LiteralConfigMapGenerator(name, behavior, append);
            var actual = generator.Generate(inputs);
            // result is always LF
            var expected = @$"configMapGenerator:
  - name: {name}
    behavior: replace
    literals:
      - foo=bar
      - piyo=poyo
      - hoge=fuga
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.unspecified, true)]
        public void UnspecifiedAppendContentsTest(string name, Behavior behavior, bool append)
        {
            var generator = new LiteralConfigMapGenerator(name, behavior, append);
            var actual = generator.Generate(inputs);
            // result is always LF
            var expected = @$"  - name: {name}
    literals:
      - foo=bar
      - piyo=poyo
      - hoge=fuga
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.create, true)]
        public void CreateAppendContentsTest(string name, Behavior behavior, bool append)
        {
            var generator = new LiteralConfigMapGenerator(name, behavior, append);
            var actual = generator.Generate(inputs);
            // result is always LF
            var expected = @$"  - name: {name}
    behavior: create
    literals:
      - foo=bar
      - piyo=poyo
      - hoge=fuga
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.merge, true)]
        public void MergeAppendContentsTest(string name, Behavior behavior, bool append)
        {
            var generator = new LiteralConfigMapGenerator(name, behavior, append);
            var actual = generator.Generate(inputs);
            // result is always LF
            var expected = @$"  - name: {name}
    behavior: merge
    literals:
      - foo=bar
      - piyo=poyo
      - hoge=fuga
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.replace, true)]
        public void ReplaceAppendContentsTest(string name, Behavior behavior, bool append)
        {
            var generator = new LiteralConfigMapGenerator(name, behavior, append);
            var actual = generator.Generate(inputs);
            // result is always LF
            var expected = @$"  - name: {name}
    behavior: replace
    literals:
      - foo=bar
      - piyo=poyo
      - hoge=fuga
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
    }
}
