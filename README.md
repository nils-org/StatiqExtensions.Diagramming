# StatiqExtensions.Diagramming

[![standard-readme compliant][]][standard-readme]
[![Contributor Covenant][contrib-covenantimg]][contrib-covenant]
[![Build][buildimage]][build]
[![Codecov Report][codecovimage]][codecov]
[![NuGet package][nugetimage]][nuget]

Tools for [Statiq](https://www.statiq.dev) to render diagrams using different hosted services.

## Table of Contents

- [StatiqExtensions.Diagramming](#statiqextensionsdiagramming)
  - [Table of Contents](#table-of-contents)
  - [Install](#install)
  - [Usage](#usage)
    - [Kroki Shortcode](#kroki-shortcode)
  - [Maintainer](#maintainer)
  - [Contributing](#contributing)
  - [License](#license)

## Install

```ps
dotnet add package StatiqExtensions.Diagramming
```

## Usage

### Kroki Shortcode

A Shortcode for Kroki is available: A Diagram added in this way will be fully embedded in the document.
The diagram will be embedded as a `<figure>`-element, containing either a `<svg>`-element, or
an `<img>`-element.

Usage requires two steps: Registration of the Shortcode and the actual usage:

**Register the Shortcode:**

```cs
await Bootstrapper
    .Factory
    .CreateWeb(args)
    .AddSetting(SettingKeys.Kroki.ServiceUrl, "https:/my.custom.Kroki.service/")
    .AddShortcode<KrokiShortcode>()
    .RunAsync();
```

**Use the Shortcode:**

```md
<?# Kroki diagramType="graphviz" outputFormat="svg" ?>
digraph G {Hello->World}
<?#/ Kroki ?>
```

Allowed output formats are `jpeg`,`png`,`svg`, simply because embedding a `pdf` makes not much sense.

Using `jpeg` or `png` as the output format, will result in an `<img src="data:image...` tag, where the resulting image is
base64 encoded in the document.
Be aware that not all Kroki diagram types are compatible with the `jpeg` and `png` output. Check the Kroki documentation.

Using the `svg` output format will embed the plain svg in the document.

**Arguments to the Kroki Shortcode**

name | used in svg | used in png/jpeg | description
------|-------------|------------------|-------------
Alt          | ⛔ | ✔️ | add an `alt` attribute to the `<img>` element
Caption      | ✔️ | ✔️ | add a `figcaption` element to the `figure` element
DiagramType  | ✔️ | ✔️ | the type of the diagram. **Required**. Check the Kroki documentation for possible values
OutputFormat | ✔️ | ✔️ | output format. **Required**. Possible values are: `svg`, `png`, `jpeg`
Height       | ✔️ | ✔️ | change the displayed height of the image
Width        | ✔️ | ✔️ | change the displayed width of the image

**Settings used by the Kroki Shortcode**

key | description
  --|--
SettingKeys.WebAdapter       | Inject a custom `IWebAdapter` to replace the default one.
SettingKeys.Kroki.ServiceUrl | The URL at wich your Kroki instance is hosted. It is possible to use `https://kroki.io/`, but check the "Free Service"-Note under https://kroki.io/#install.

## Maintainer

[Nils Andresen @nils-a][maintainer]

## Contributing

StatiqExtensions.Diagramming follows the [Contributor Covenant][contrib-covenant] Code of Conduct.

We accept Pull Requests.

Small note: If editing the Readme, please conform to the [standard-readme][] specification.

This project follows the [all-contributors][] specification. Contributions of any kind welcome!


## License

[Statiq Public License][license]

[build]: https://github.com/nils-org/StatiqExtensions.Diagramming/actions/workflows/build.yml
[buildimage]: https://github.com/nils-org/StatiqExtensions.Diagramming/actions/workflows/build.yml/badge.svg
[codecov]: https://codecov.io/gh/nils-org/StatiqExtensions.Diagramming
[codecovimage]: https://img.shields.io/codecov/c/github/nils-org/StatiqExtensions.Diagramming.svg?logo=codecov&style=flat-square
[contrib-covenant]: https://www.contributor-covenant.org/version/2/0/code_of_conduct/
[contrib-covenantimg]: https://img.shields.io/badge/Contributor%20Covenant-v2.0%20adopted-ff69b4.svg
[maintainer]: https://github.com/nils-a
[nuget]: https://nuget.org/packages/StatiqExtensions.Diagramming
[nugetimage]: https://img.shields.io/nuget/v/StatiqExtensions.Diagramming.svg?logo=nuget&style=flat-square
[license]: LICENSE.md
[standard-readme]: https://github.com/RichardLitt/standard-readme
[standard-readme compliant]: https://img.shields.io/badge/readme%20style-standard-brightgreen.svg?style=flat-square
