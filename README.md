# FlexDi

_A small dependency injection container for .NET, inspired by [BoDi]._

[BoDi]: https://github.com/SpecFlowOSS/BoDi

## Quick start

Comprehensive documentation is available in the [FlexDi wiki]. This is some minimal sample code to create a container, register a component and resolve it right away. Firstly though, you will want **[the FlexDi NuGet package]**.

```csharp
using CSF.FlexDi;

var container = Container
    .CreateBuilder()
    // Call other methods from the builder if desired,
    // in order to configure container options & functionality
    .Build();

container.AddRegistrations(x => {
    x.RegisterType<MyServiceType>()
     .As<IServiceInterface>();
});

var myService = container.Resolve<IServiceInterface>();
```

[FlexDi wiki]: https://github.com/csf-dev/FlexDi/wiki
[the FlexDi NuGet package]: https://www.nuget.org/packages/CSF.FlexDi

## BoDi compatibility

FlexDi may be used as a drop-in replacement for BoDi, via [a compatibility package]. The assembly contained within that NuGet package provides copies of BoDi's public types, exposing the same API. The FlexDi container wrapped by the BoDi compatibility assembly is preconfigured with options to mimic BoDi's functionality and behaviour.

The [FlexDi wiki] has a section dedicated to [using FlexDi as a replacement for BoDi].

[a compatibility package]: https://www.nuget.org/packages/CSF.FlexDi.BoDiCompatibility
[using FlexDi as a replacement for BoDi]: https://github.com/csf-dev/FlexDi/wiki/BoDiReplacement

## Copyright and license

FlexDi is the copyright of [various authors]. It is released under the terms of [the Apache License v2].

[various authors]: NOTICE.txt
[the Apache License v2]: LICENSE.txt

### Continuous integration status

CI builds are configured via AppVeyor for both Linux & Windows.

[![AppVeyor status](https://ci.appveyor.com/api/projects/status/u9u4f99p45jyhd6u?svg=true)](https://ci.appveyor.com/project/craigfowler/flexdi)
