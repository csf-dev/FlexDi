using System;
namespace CSF.MicroDi.Builders
{
  public class ContainerBuilder : IContainerBuilder
  {
    bool useNonPublicConstructors;
    bool resolveUnregisteredTypes;
    bool useInstanceCache;
    bool throwOnCircularDependencies;
    bool supportResolvingNamedInstanceDictionaries;

    public IContainerBuilder DoNotUseNonPublicConstructors()
    {
      useNonPublicConstructors = false;
      return this;
    }

    public IContainerBuilder UseNonPublicConstructors(bool useNonPublicConstructors = true)
    {
      this.useNonPublicConstructors = useNonPublicConstructors;
      return this;
    }

    public IContainerBuilder DoNotResolveUnregisteredTypes()
    {
      resolveUnregisteredTypes = false;
      return this;
    }

    public IContainerBuilder ResolveUnregisteredTypes(bool resolveUnregisteredTypes = true)
    {
      this.resolveUnregisteredTypes = resolveUnregisteredTypes;
      return this;
    }

    public IContainerBuilder DoNotUseInstanceCache()
    {
      useInstanceCache = false;
      return this;
    }

    public IContainerBuilder UseInstanceCache(bool useInstanceCache = true)
    {
      this.useInstanceCache = useInstanceCache;
      return this;
    }

    public IContainerBuilder DoNotThrowOnCircularDependencies()
    {
      throwOnCircularDependencies = false;
      return this;
    }

    public IContainerBuilder ThrowOnCircularDependencies(bool throwOnCircularDependencies = true)
    {
      this.throwOnCircularDependencies = throwOnCircularDependencies;
      return this;
    }

    public IContainerBuilder DoNotSupportResolvingNamedInstanceDictionaries()
    {
      supportResolvingNamedInstanceDictionaries = false;
      return this;
    }

    public IContainerBuilder SupportResolvingNamedInstanceDictionaries(bool supportResolvingNamedInstanceDictionaries = true)
    {
      this.supportResolvingNamedInstanceDictionaries = supportResolvingNamedInstanceDictionaries;
      return this;
    }

    public Container Build()
    {
      return new Container(options: GetContainerOptions());
    }

    ContainerOptions GetContainerOptions()
    {
      return new ContainerOptions(useNonPublicConstructors: useNonPublicConstructors,
                                  resolveUnregisteredTypes: resolveUnregisteredTypes,
                                  useInstanceCache: useInstanceCache,
                                  throwOnCircularDependencies: throwOnCircularDependencies,
                                  supportResolvingNamedInstanceDictionaries: supportResolvingNamedInstanceDictionaries);
    }

    public ContainerBuilder()
    {
      useNonPublicConstructors = ContainerOptions.Default.UseNonPublicConstructors;
      resolveUnregisteredTypes = ContainerOptions.Default.ResolveUnregisteredTypes;
      useInstanceCache = ContainerOptions.Default.UseInstanceCache;
      throwOnCircularDependencies = ContainerOptions.Default.ThrowOnCircularDependencies;
      supportResolvingNamedInstanceDictionaries = ContainerOptions.Default.SupportResolvingNamedInstanceDictionaries;
    }
  }
}
