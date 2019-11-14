[![CircleCI](https://circleci.com/gh/guitarrapc/KustomizeConfigMapGenerator.svg?style=svg)](https://circleci.com/gh/guitarrapc/KustomizeConfigMapGenerator) [![NuGet](https://img.shields.io/nuget/v/KustomizeConfigMapGenerator.svg)](https://www.nuget.org/packages/KustomizeConfigMapGenerator) [![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE) [![codecov](https://codecov.io/gh/guitarrapc/KustomizeConfigMapGenerator/branch/master/graph/badge.svg)](https://codecov.io/gh/guitarrapc/KustomizeConfigMapGenerator)

## KustomizeConfigMapGenerator

dotnet global tool to generate Kustomization's ConfigMapGenerator section.

## Concept

* **Simple**, generate `configMapGenerator` section only YAML file.
* **MultiPlatform**, you can run tool on .NET Core 3.0 support environment.

## Install

```
dotnet tool install --global KustomizeConfigMapGenerator
```

you can run with `dotnet-kustomizeconfigmapgenerator`.
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
```

## Q&A

Q. Why not ShellScript?
A. Because ShellScript can not run on Windows, especially set to .csproj build event and run on local Windows and CI Linux env.

Q. Will support other kustomization header like pathes, resources, namespace, commonAnnotations, etc.?
A. No, just an configMapGeneartor section. this output should refer as `base:` kustomization yaml.
