//
//    ContainerBuilder.cs
//
//    Copyright 2018  Craig Fowler et al
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
//    For further copyright info, including a complete author/contributor
//    list, please refer to the file NOTICE.txt

using System;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Builders
{
  public class ContainerBuilder : IContainerBuilder
  {
    bool useNonPublicConstructors;
    bool resolveUnregisteredTypes;
    bool useInstanceCache;
    bool throwOnCircularDependencies;
    bool supportResolvingNamedInstanceDictionaries;
    ICreatesResolvers resolverFactory;

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

    public IContainerBuilder UseCustomResolverFactory(ICreatesResolvers resolverFactory)
    {
      if(resolverFactory == null)
        throw new ArgumentNullException(nameof(resolverFactory));
      
      this.resolverFactory = resolverFactory;
      return this;
    }

    public Container Build()
    {
      return new Container(options: GetContainerOptions(),
                           resolverFactory: resolverFactory);
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
