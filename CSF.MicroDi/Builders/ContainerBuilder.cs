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
  public class ContainerBuilder
  {
    bool
      useNonPublicConstructors,
      resolveUnregisteredTypes,
      useInstanceCache,
      throwOnCircularDependencies,
      supportResolvingNamedInstanceDictionaries,
      selfRegisterAResolver,
      selfRegisterTheRegistry;
    ICreatesResolvers resolverFactory;

    public ContainerBuilder DoNotUseNonPublicConstructors()
    {
      useNonPublicConstructors = false;
      return this;
    }

    public ContainerBuilder UseNonPublicConstructors(bool useNonPublicConstructors = true)
    {
      this.useNonPublicConstructors = useNonPublicConstructors;
      return this;
    }

    public ContainerBuilder DoNotResolveUnregisteredTypes()
    {
      resolveUnregisteredTypes = false;
      return this;
    }

    public ContainerBuilder ResolveUnregisteredTypes(bool resolveUnregisteredTypes = true)
    {
      this.resolveUnregisteredTypes = resolveUnregisteredTypes;
      return this;
    }

    public ContainerBuilder DoNotUseInstanceCache()
    {
      useInstanceCache = false;
      return this;
    }

    public ContainerBuilder UseInstanceCache(bool useInstanceCache = true)
    {
      this.useInstanceCache = useInstanceCache;
      return this;
    }

    public ContainerBuilder DoNotSelfRegisterAResolver()
    {
      return SelfRegisterAResolver(false);
    }

    public ContainerBuilder SelfRegisterAResolver(bool selfRegisterTheRegistry = true)
    {
      this.selfRegisterTheRegistry = selfRegisterTheRegistry;
      return this;
    }

    public ContainerBuilder DoNotSelfRegisterTheRegistry()
    {
      return SelfRegisterTheRegistry(false);
    }

    public ContainerBuilder SelfRegisterTheRegistry(bool selfRegisterTheRegistry = true)
    {
      this.selfRegisterTheRegistry = selfRegisterTheRegistry;
      return this;
    }

    public ContainerBuilder DoNotThrowOnCircularDependencies()
    {
      throwOnCircularDependencies = false;
      return this;
    }

    public ContainerBuilder ThrowOnCircularDependencies(bool throwOnCircularDependencies = true)
    {
      this.throwOnCircularDependencies = throwOnCircularDependencies;
      return this;
    }

    public ContainerBuilder DoNotSupportResolvingNamedInstanceDictionaries()
    {
      supportResolvingNamedInstanceDictionaries = false;
      return this;
    }

    public ContainerBuilder SupportResolvingNamedInstanceDictionaries(bool supportResolvingNamedInstanceDictionaries = true)
    {
      this.supportResolvingNamedInstanceDictionaries = supportResolvingNamedInstanceDictionaries;
      return this;
    }

    public ContainerBuilder UseCustomResolverFactory(ICreatesResolvers resolverFactory)
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
      return new ContainerOptions(useNonPublicConstructors,
                                  resolveUnregisteredTypes,
                                  useInstanceCache,
                                  throwOnCircularDependencies,
                                  supportResolvingNamedInstanceDictionaries,
                                  selfRegisterAResolver);
    }

    public ContainerBuilder()
    {
      useNonPublicConstructors = ContainerOptions.Default.UseNonPublicConstructors;
      resolveUnregisteredTypes = ContainerOptions.Default.ResolveUnregisteredTypes;
      useInstanceCache = ContainerOptions.Default.UseInstanceCache;
      throwOnCircularDependencies = ContainerOptions.Default.ThrowOnCircularDependencies;
      supportResolvingNamedInstanceDictionaries = ContainerOptions.Default.SupportResolvingNamedInstanceDictionaries;
      selfRegisterAResolver = ContainerOptions.Default.SelfRegisterAResolver;
      selfRegisterTheRegistry = ContainerOptions.Default.SelfRegisterTheRegistry;
    }
  }
}
