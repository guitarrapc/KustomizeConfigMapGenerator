using KustomizeConfigMapGenerator;
using KustomizeConfigMapGenerator.Internals;
using System;
using System.IO;
using Xunit;

namespace KustomizeConfigMapGenerator.Tests
{
    public class FileConfigMapGeneratorTests
    {
        private static readonly string basePath = "assets";

        [Theory]
        [InlineData("test", Behavior.unspecified, false)]
        public void UnspecifiedContentsTest(string name, Behavior behavior, bool skipHeader)
        {
            var generator = new FileConfigMapGenerator(name, behavior, skipHeader);
            var actual = generator.Generate(basePath, "*.json");
            // result is always LF
            var expected = @$"configMapGenerator:
  - name: {name}
    files:
      - /bar.json
      - /foo.json
      - /hoge.json
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.create, false)]
        public void CreateContentsTest(string name, Behavior behavior, bool skipHeader)
        {
            var generator = new FileConfigMapGenerator(name, behavior, skipHeader);
            var actual = generator.Generate(basePath, "*.json");
            // result is always LF
            var expected = @$"configMapGenerator:
  - name: {name}
    behavior: create
    files:
      - /bar.json
      - /foo.json
      - /hoge.json
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.merge, false)]
        public void MergeContentsTest(string name, Behavior behavior, bool skipHeader)
        {
            var generator = new FileConfigMapGenerator(name, behavior, skipHeader);
            var actual = generator.Generate(basePath, "*.json");
            // result is always LF
            var expected = @$"configMapGenerator:
  - name: {name}
    behavior: merge
    files:
      - /bar.json
      - /foo.json
      - /hoge.json
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.replace, false)]
        public void ReplaceContentsTest(string name, Behavior behavior, bool skipHeader)
        {
            var generator = new FileConfigMapGenerator(name, behavior, skipHeader);
            var actual = generator.Generate(basePath, "*.json");
            // result is always LF
            var expected = @$"configMapGenerator:
  - name: {name}
    behavior: replace
    files:
      - /bar.json
      - /foo.json
      - /hoge.json
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.unspecified, true)]
        public void UnspecifiedAppendContentsTest(string name, Behavior behavior, bool skipHeader)
        {
            var generator = new FileConfigMapGenerator(name, behavior, skipHeader);
            var actual = generator.Generate(basePath, "*.json");
            // result is always LF
            var expected = @$"  - name: {name}
    files:
      - /bar.json
      - /foo.json
      - /hoge.json
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.create, true)]
        public void CreateAppendContentsTest(string name, Behavior behavior, bool skipHeader)
        {
            var generator = new FileConfigMapGenerator(name, behavior, skipHeader);
            var actual = generator.Generate(basePath, "*.json");
            // result is always LF
            var expected = @$"  - name: {name}
    behavior: create
    files:
      - /bar.json
      - /foo.json
      - /hoge.json
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.merge, true)]
        public void MergeAppendContentsTest(string name, Behavior behavior, bool skipHeader)
        {
            var generator = new FileConfigMapGenerator(name, behavior, skipHeader);
            var actual = generator.Generate(basePath, "*.json");
            // result is always LF
            var expected = @$"  - name: {name}
    behavior: merge
    files:
      - /bar.json
      - /foo.json
      - /hoge.json
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("test", Behavior.replace, true)]
        public void ReplaceAppendContentsTest(string name, Behavior behavior, bool skipHeader)
        {
            var generator = new FileConfigMapGenerator(name, behavior, skipHeader);
            var actual = generator.Generate(basePath, "*.json");
            // result is always LF
            var expected = @$"  - name: {name}
    behavior: replace
    files:
      - /bar.json
      - /foo.json
      - /hoge.json
".Replace("\r\n", "\n");
            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData("test", Behavior.unspecified, false)]
        public void PathNotExistsTest(string name, Behavior behavior, bool skipHeader)
        {
            var generator = new FileConfigMapGenerator(name, behavior, skipHeader);
            Assert.Throws<DirectoryNotFoundException>(() => generator.Generate("FooBar", "*.json"));
        }
    }
}
