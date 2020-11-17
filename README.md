![dotnet-build](https://github.com/guitarrapc/KustomizeConfigMapGenerator/workflows/dotnet-build/badge.svg) ![release](https://github.com/guitarrapc/KustomizeConfigMapGenerator/workflows/release/badge.svg) [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE) [![codecov](https://codecov.io/gh/guitarrapc/KustomizeConfigMapGenerator/branch/master/graph/badge.svg)](https://codecov.io/gh/guitarrapc/KustomizeConfigMapGenerator)

## KustomizeConfigMapGenerator

dotnet global/local tool & dotnet project tool to generate Kustomization's ConfigMapGenerator section.

* Want run as cli, use dotnet global tool. 
* Want run as build, use dotnet local tool.

> .NET Core 2.2 only: If you want run as .csproj pre|post event, dotnet project tool may help you not need explict installation.

## Concept

* **Simple**: generate kustomization yaml for `configMapGenerator` section only.
* **MultiPlatform**: you can run tool on .NET Core 3.1 and .NET 5 runtime

## Install

nuget | version | description | run
---- | ---- | ---- | ----
KustomizeConfigMapGenerator | [![NuGet](https://img.shields.io/nuget/v/KustomizeConfigMapGenerator.svg)](https://www.nuget.org/packages/KustomizeConfigMapGenerator) | dotnet global tool | `dotnet-kustomizeconfigmapgenerator subcommand args`
dotnet-kustomizationconfigmapgenerator-project-tool | [![NuGet](https://img.shields.io/nuget/v/dotnet-kustomizationconfigmapgenerator-project-tool.svg)](https://www.nuget.org/packages/dotnet-kustomizationconfigmapgenerator-project-tool) | dotnet project tool | `dotnet kustomizeconfigmapgenerator subcommand args`

### dotnet global tool

You can install `KustomizeConfigMapGenerator` with dotnet cli and run as dotnet global tool.

```
dotnet tool install --global KustomizeConfigMapGenerator
```

### dotnet local tools

> This feature is for .NET 3.1 and above.

You can install `KustomizeConfigMapGenerator` for your local tools.

```shell
dotnet new tool-manifest
dotnet tool install KustomizeConfigMapGenerator
```

Generated .config/dotnet-tools.json contains `dotnet-kustomizeconfigmapgenerator` item.

```json
{
  "version": 1,
  "isRoot": true,
  "tools": {
    "kustomizeconfigmapgenerator": {
      "version": "0.4.0",
      "commands": [
        "dotnet-kustomizeconfigmapgenerator"
      ]
    }
  }
}
```

This enable you to restore package via `dotnet restore` then reference within project. (you don't need install global tool)

```xml
  <Target Name="PreBuild" BeforeTargets="PreBuildEvent">
    <Exec Command="dotnet kustomizationconfigmapgenerator literals -i "foo=bar" -o kustomization.yaml -n the-map -d false -f true" />
  </Target>
```

### dotnet project tool

> This feature is for .NET Core 2.2 only.

You can install `dotnet-kustomizationconfigmapgenerator-project-tool` by adding `DotNetCliToolReference` to your csproj.

```xml
  <ItemGroup>
    <DotNetCliToolReference Include="dotnet-kustomizationconfigmapgenerator-project-tool" Version="0.4.0" />
  </ItemGroup>
```

This enable you to restore package via `dotnet restore` then reference within project. (you don't need install global tool)

```xml
  <Target Name="PreBuild" BeforeTargets="PreBuildEvent">
    <Exec Command="dotnet kustomizationconfigmapgenerator literals -i "foo=bar" -o kustomization.yaml -n the-map -d false -f true" />
  </Target>
```

## Help

see samples [below](Samples).

```
literals: Generate ConfigMap from specific key=value literals.
-i, -inputs: comma separated key=value style literals. (ex. foo=bar,hoge=fuga
-o, -outputPath: path to the output kustomization file.
-n, -name: configmap name of ConfigMapGenerator.
-b, -behavior: [default=unspecified]ConfigMapGenerator behavior.
-skip, -skipHeader: [default=False]skip configMapGenerator: output.
-a, -append: [default=False]append just an value to existing config.
-f, -forceOutput: [default=False]override outputfile without prompt.
-d, -dryRun: [default=True]dry run.

files: Generate ConfigMap from specific path's files.
-i, -inputPath: path to the base directory to search.
-o, -outputPath: path to the output kustomization file.
-s, -searchPattern: search pattern of files. (ex. *.config)
-n, -name: configmap name of ConfigMapGenerator.
-b, -behavior: [default=unspecified]ConfigMapGenerator behavior.
-skip, -skipHeader: [default=False]skip configMapGenerator: output.
-a, -append: [default=False]append just an value to existing config.
-f, -forceOutput: [default=False]override outputfile without prompt.
-d, -dryRun: [default=True]dry run.
```

## Motivation

CofigMapGenerator is dynamic configuration source mechanism for Kubernetes.
With this functionality, you can load ConfigurationMap to the deployment everytime ConfigMapGenerator is change.

> see: [kustomize/configGeneration\.md at master · kubernetes\-sigs/kustomize](https://github.com/kubernetes-sigs/kustomize/blob/master/examples/configGeneration.md)

However `kubectl kustomize` not allow directories as input.

> see: [configMapGenerator should allow directories as input, like kubectl · Issue \#189 · kubernetes\-sigs/kustomize](https://github.com/kubernetes-sigs/kustomize/issues/189)

KustomizationConfigMapGenerator is tool to cover this case.


## Samples

As ConfigMapGenerator support `files:` and `literals:`, KustomizeConfigMapGenerator also support this 2 cases.

Let see how to generate each configMapGenerator.

* dry run with simple `files:`. 
```yaml
configMapGenerator:
  - name: the-files
    behavior: merge
    files:	
      - foo/bar.txt
      - hoge.txt
```

`-d true` or omit parameter will dry-run.


```shell
$ dotnet-kustomizeconfigmapgenerator files -i ./samples/ -o ./samples/kustomization.yaml -s *.txt -name the-files -b merge -f true
dryrun mode detected. see output contents.
configMapGenerator:
  - name: the-files
    behavior: merge
    files:
      - foo/bar.txt
      - hoge.txt
```

* working with simple `files:`. 

```yaml
configMapGenerator:
  - name: the-files
    behavior: merge
    files:	
      - foo/bar.txt
      - hoge.txt
```

`-d false` will run without dry-run.

```shell
$ dotnet-kustomizeconfigmapgenerator files -i ./samples/ -o ./samples/kustomization.yaml -s *.txt -name the-files -b merge -d false -f true
$ cat ./samples/kustomization.yaml
configMapGenerator:
  - name: the-files
    behavior: merge
    files:
      - foo/bar.txt
      - hoge.txt
```

* working with `file:` & additional file config.

```yaml
configMapGenerator:
  - name: the-files
    behavior: merge
    files:	
      - foo/bar.txt
      - hoge.txt
  - name: the-files2
    files:	
      - foo/bar.txt
      - hoge.txt
```

```
$ dotnet-kustomizeconfigmapgenerator files -i ./samples/ -o ./samples/kustomization.yaml -s *.txt -name the-files -b merge -d false -f true
$ dotnet-kustomizeconfigmapgenerator files -i ./samples/ -o ./samples/kustomization.yaml -s *.txt -name the-files2 -b merge -d false -f true -skipHeader true -append true
$ cat ./samples/kustomization.yaml
configMapGenerator:
  - name: the-files
    behavior: merge
    files:
      - foo/bar.txt
      - hoge.txt
  - name: the-files2
    behavior: merge
    files:
      - foo/bar.txt
      - hoge.txt
```

* working with simple `literals:`.

```yaml
configMapGenerator:
  - name: the-map
    behavior: merge
    literals:
      - altGreeting=Good Morning!
      - enableRisky=false
```

```shell
$ dotnet-kustomizeconfigmapgenerator literals -i 'altGreeting=Good Morning!,enableRisky=false' -o ./samples/kustomization.yaml -s *.txt -name the-files -b merge -d false -f true
$ cat ./samples/kustomization.yaml
configMapGenerator:
  - name: the-map
    behavior: merge
    literals:
      - altGreeting=Good Morning!
      - enableRisky=false
```

## Q&A

Q. Why not ShellScript?
A. Because ShellScript can not run on Windows, especially set to .csproj build event and run on local Windows and CI Linux env.

Q. Will support other kustomization header like pathes, resources, namespace, commonAnnotations, etc.?
A. No, just an configMapGeneartor section. this output should refer as `base:` kustomization yaml.

## REF

> [\.NET Core CLI extensibility model \- \.NET Core CLI \| Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/core/tools/extensibility)
> [Package Type \[Packing\] · NuGet/Home Wiki](https://github.com/NuGet/Home/wiki/Package-Type-%5BPacking%5D)