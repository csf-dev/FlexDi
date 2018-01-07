# MicroDi
_A small dependency injection container for .NET, inspired by [BoDi]._

[BoDi]: https://github.com/gasparnagy/BoDi

## Quick start
Comprehensive documentation is available in [the MicroDi wiki]. This is some minimal sample code to create a container, register a component and resolve it right away. Firstly though, you will want **[the MicroDi NuGet package]**.

```csharp
using CSF.MicroDi;

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

[the MicroDi wiki]: https://github.com/csf-dev/MicroDi/wiki
[the MicroDi NuGet package]: https://www.nuget.org/packages/CSF.MicroDi

## BoDi compatibility
MicroDi may be used as a drop-in replacement for BoDi if desired. A [BoDi compatibility NuGet package] is provided specifically for this purpose.

The compatibility assembly provides versions of the BoDi types which make up it's public API.

* `IObjectContainer`
* `ObjectContainer`
* `ObjectContainerException`

This implementation of ObjectContainer is a wrapper around a MicroDi *Container* instance. The container is preconfigured with options to mimic BoDi's functionality and behaviour.

Further information about using MicroDi as a replacement for BoDi is available in [the MicroDi wiki].

[BoDi compatibility NuGet package]: https://www.nuget.org/packages/CSF.MicroDi.BoDiCompatibility

## Copyright and license
MicroDi is the copyright of [various authors]. It is released under the terms of [the Apache License v2].

[various authors]: NOTICE.txt
[the Apache License v2]: LICENSE.txt

### Continuous integration status
CI builds are configured via both Travis (for build & test on Linux/Mono) and AppVeyor (Windows/.NET).
Below are links to the most recent build statuses for these two CI platforms.

Platform | Status
-------- | ------
Linux/Mono (Travis) | [![Travis Status](https://travis-ci.org/csf-dev/MicroDi.svg?branch=feature%2F10-continuous-integration)](https://travis-ci.org/csf-dev/MicroDi)
Windows/.NET (AppVeyor) | [![AppVeyor status](https://ci.appveyor.com/api/projects/status/nahafeweohn4sy1n?svg=true)](https://ci.appveyor.com/project/craigfowler/microdi)
