using System;
namespace CSF.MicroDi.Builders
{
  public interface IContainerBuilder
  {
    IContainerBuilder DoNotUseNonPublicConstructors();

    IContainerBuilder UseNonPublicConstructors(bool useNonPublicConstructors = true);

    IContainerBuilder DoNotResolveUnregisteredTypes();

    IContainerBuilder ResolveUnregisteredTypes(bool resolveUnregisteredTypes = true);

    IContainerBuilder DoNotUseInstanceCache();

    IContainerBuilder UseInstanceCache(bool useInstanceCache = true);

    IContainerBuilder DoNotThrowOnCircularDependencies();

    IContainerBuilder ThrowOnCircularDependencies(bool throwOnCircularDependencies = true);

    IContainerBuilder DoNotSupportResolvingNamedInstanceDictionaries();

    IContainerBuilder SupportResolvingNamedInstanceDictionaries(bool supportResolvingNamedInstanceDictionaries = true);

    Container Build();
  }
}
