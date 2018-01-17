//
//    ResolverFactoryTests.cs
//
//    Copyright 2018  Craig Fowler
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
using System.Collections.Generic;
using System.Linq;
using CSF.FlexDi.Registration;
using CSF.FlexDi.Resolution;
using CSF.FlexDi.Resolution.Proxies;
using CSF.FlexDi.Tests.Autofixture;
using CSF.FlexDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Resolution
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class ResolverFactoryTests
  {
    [Test,AutoMoqData]
    public void Created_resolver_contains_regular_resolver([MinimalInfo] IProvidesResolutionInfo resInfo,
                                                           ResolverFactory sut)
    {
      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Contains.Item(typeof(Resolver)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_contains_caching_resolver_when_caching_is_enabled([MinimalInfo] IProvidesResolutionInfo resInfo,
                                                                                   ResolverFactory sut)
    {
      // Arrange
      SetOptions(resInfo, new ContainerOptions(useInstanceCache: true));

      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Contains.Item(typeof(CachingResolverProxy)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_does_not_contain_caching_resolver_when_caching_is_disabled([MinimalInfo] IProvidesResolutionInfo resInfo,
                                                                                            ResolverFactory sut)
    {
      // Arrange
      SetOptions(resInfo, new ContainerOptions(useInstanceCache: false));

      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Does.Not.Contain(typeof(CachingResolverProxy)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_contains_fallback_resolver_when_there_is_a_parent([MinimalInfo(HasParent = true)] IProvidesResolutionInfo resInfo,
                                                                                   ResolverFactory sut)
    {
      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Contains.Item(typeof(FallbackResolverProxy)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_does_not_contain_fallback_resolver_when_there_is_no_parent([MinimalInfo(HasParent = false)] IProvidesResolutionInfo resInfo,
                                                                                            ResolverFactory sut)
    {
      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Does.Not.Contain(typeof(FallbackResolverProxy)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_contains_unregistered_service_resolver_when_the_option_is_enabled([MinimalInfo] IProvidesResolutionInfo resInfo,
                                                                                                   ResolverFactory sut)
    {
      // Arrange
      SetOptions(resInfo, new ContainerOptions(resolveUnregisteredTypes: true));

      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Contains.Item(typeof(UnregisteredServiceResolverProxy)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_does_not_contain_unregistered_service_resolver_when_the_option_is_not_enabled([MinimalInfo(HasParent = false)] IProvidesResolutionInfo resInfo,
                                                                                                               ResolverFactory sut)
    {
      // Arrange
      SetOptions(resInfo, new ContainerOptions(resolveUnregisteredTypes: false));

      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Does.Not.Contain(typeof(UnregisteredServiceResolverProxy)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_does_not_contain_unregistered_service_resolver_in_parent_resolver([MinimalInfo] IProvidesResolutionInfo resInfo,
                                                                                                   [MinimalInfo] IProvidesResolutionInfo parentResInfo,
                                                                                                   ResolverFactory sut)
    {
      // Arrange
      SetOptions(resInfo, new ContainerOptions(resolveUnregisteredTypes: true));
      SetOptions(parentResInfo, new ContainerOptions(resolveUnregisteredTypes: true));
      Mock.Get(resInfo).SetupGet(x => x.Parent).Returns(parentResInfo);

      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var fallbackResolver = GetListOfResolversUsed(resolver).OfType<FallbackResolverProxy>().FirstOrDefault();
      Assert.That(fallbackResolver, Is.Not.Null);
      var parentResolver = fallbackResolver.FallbackResolver;
      var parentResolverTypes = GetListOfResolverTypesUsed(parentResolver);
      Assert.That(parentResolverTypes, Does.Not.Contain(typeof(UnregisteredServiceResolverProxy)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_contains_circlar_dependency_resolver_when_the_option_is_enabled([MinimalInfo] IProvidesResolutionInfo resInfo,
                                                                                                   ResolverFactory sut)
    {
      // Arrange
      SetOptions(resInfo, new ContainerOptions(throwOnCircularDependencies: true));

      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Contains.Item(typeof(CircularDependencyPreventingResolverProxy)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_does_not_contain_circlar_dependency_resolver_when_the_option_is_not_enabled([MinimalInfo(HasParent = false)] IProvidesResolutionInfo resInfo,
                                                                                                               ResolverFactory sut)
    {
      // Arrange
      SetOptions(resInfo, new ContainerOptions(throwOnCircularDependencies: false));

      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Does.Not.Contain(typeof(CircularDependencyPreventingResolverProxy)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_contains_name_injecting_resolver([MinimalInfo] IProvidesResolutionInfo resInfo,
                                                                                                 ResolverFactory sut)
    {
      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Contains.Item(typeof(RegisteredNameInjectingResolverProxy)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_contains_instance_dictionary_resolver_when_the_option_is_enabled([MinimalInfo] IProvidesResolutionInfo resInfo,
                                                                                                 ResolverFactory sut)
    {
      // Arrange
      SetOptions(resInfo, new ContainerOptions(supportResolvingNamedInstanceDictionaries: true));

      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Contains.Item(typeof(NamedInstanceDictionaryResolverProxy)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_does_not_contain_instance_dictionary_resolver_when_the_option_is_not_enabled([MinimalInfo(HasParent = false)] IProvidesResolutionInfo resInfo,
                                                                                                             ResolverFactory sut)
    {
      // Arrange
      SetOptions(resInfo, new ContainerOptions(supportResolvingNamedInstanceDictionaries: false));

      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Does.Not.Contain(typeof(NamedInstanceDictionaryResolverProxy)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_contains_dynamic_resolution_resolver_when_the_option_is_enabled([MinimalInfo] IProvidesResolutionInfo resInfo,
                                                                                                  ResolverFactory sut)
    {
      // Arrange
      SetOptions(resInfo, new ContainerOptions(selfRegisterAResolver: true));

      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Contains.Item(typeof(DynamicRecursionResolverProxy)));
    }

    [Test,AutoMoqData]
    public void Created_resolver_does_not_contain_dynamic_resolution_resolver_when_the_option_is_not_enabled([MinimalInfo(HasParent = false)] IProvidesResolutionInfo resInfo,
                                                                                                              ResolverFactory sut)
    {
      // Arrange
      SetOptions(resInfo, new ContainerOptions(selfRegisterAResolver: false));

      // Act
      var resolver = sut.CreateResolver(resInfo);

      // Assert
      var types = GetListOfResolverTypesUsed(resolver);
      Assert.That(types, Does.Not.Contain(typeof(DynamicRecursionResolverProxy)));
    }

    IReadOnlyList<IResolver> GetListOfResolversUsed(IResolver resolver)
    {
      var output = new List<IResolver>();
      var currentResolver = resolver;

      while(currentResolver != null && (currentResolver is IProxiesToAnotherResolver))
      {
        var proxy = (IProxiesToAnotherResolver) currentResolver;
        currentResolver = proxy.ProxiedResolver;
        output.Add(proxy);
      }

      if(currentResolver != null)
        output.Add(currentResolver);

      return output.ToArray();
    }

    IReadOnlyList<Type> GetListOfResolverTypesUsed(IResolver resolver)
    {
      return GetListOfResolversUsed(resolver)
        .Select(x => x.GetType())
        .ToArray();
    }

    void SetOptions(IProvidesResolutionInfo resInfo, ContainerOptions opts)
    {
      if(resInfo == null)
        throw new ArgumentNullException(nameof(resInfo));
      if(opts == null)
        throw new ArgumentNullException(nameof(opts));

      Mock.Get(resInfo).SetupGet(x => x.Options).Returns(opts);
    }
  }
}
