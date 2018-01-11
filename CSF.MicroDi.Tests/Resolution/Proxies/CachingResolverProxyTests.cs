//
//    CachingResolverProxyTests.cs
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
using CSF.MicroDi.Registration;
using CSF.MicroDi.Resolution;
using CSF.MicroDi.Resolution.Proxies;
using CSF.MicroDi.Tests.Autofixture;
using CSF.MicroDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.MicroDi.Tests.Resolution.Proxies
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class CachingResolverProxyTests
  {
    [Test,AutoMoqData]
    public void Resolve_returns_instance_from_cache_if_it_exists_there(IResolver proxiedResolver,
                                                                       [Registration] IServiceRegistration registration,
                                                                       ICachesResolvedServiceInstances cache,
                                                                       ResolutionRequest request,
                                                                       ISampleService cached)
    {
      // Arrange
      object cachedItem = cached;
      Mock.Get(proxiedResolver).Setup(x => x.GetRegistration(request)).Returns(registration);
      Mock.Get(cache).Setup(x => x.TryGet(registration, out cachedItem)).Returns(true);
      var sut = new CachingResolverProxy(proxiedResolver, cache);

      // Act
      var result = sut.Resolve(request);

      // Assert
      Assert.That(result.IsSuccess, Is.True);
      Assert.That(result.ResolvedObject, Is.SameAs(cached));
    }

    [Test,AutoMoqData]
    public void Resolve_does_not_use_proxied_resolver_if_item_was_cached(IResolver proxiedResolver,
                                                                         [Registration] IServiceRegistration registration,
                                                                         ICachesResolvedServiceInstances cache,
                                                                         ResolutionRequest request,
                                                                         ISampleService cached)
    {
      // Arrange
      object cachedItem = cached;
      Mock.Get(proxiedResolver).Setup(x => x.GetRegistration(request)).Returns(registration);
      Mock.Get(cache).Setup(x => x.TryGet(registration, out cachedItem)).Returns(true);
      var sut = new CachingResolverProxy(proxiedResolver, cache);

      // Act
      var result = sut.Resolve(request);

      // Assert
      Mock.Get(proxiedResolver).Verify(x => x.Resolve(It.IsAny<ResolutionRequest>()), Times.Never);
    }

    [Test,AutoMoqData]
    public void Resolve_resolves_from_proxied_resolver_if_item_is_not_cached(IResolver proxiedResolver,
                                                                             [Registration] IServiceRegistration registration,
                                                                             ICachesResolvedServiceInstances cache,
                                                                             ResolutionRequest request,
                                                                             ISampleService resolved,
                                                                             ResolutionPath resolutionPath)
    {
      // Arrange
      object cachedItem = null;
      Mock.Get(proxiedResolver).Setup(x => x.GetRegistration(request)).Returns(registration);
      Mock.Get(proxiedResolver).Setup(x => x.Resolve(request)).Returns(ResolutionResult.Success(resolutionPath, resolved));
      Mock.Get(cache).Setup(x => x.TryGet(registration, out cachedItem)).Returns(false);
      var sut = new CachingResolverProxy(proxiedResolver, cache);

      // Act
      var result = sut.Resolve(request);

      // Assert
      Assert.That(result.IsSuccess, Is.True);
      Assert.That(result.ResolvedObject, Is.SameAs(resolved));
    }

    [Test,AutoMoqData]
    public void Resolve_adds_item_from_proxied_resolver_to_the_cache(IResolver proxiedResolver,
                                                                     [Registration] IServiceRegistration registration,
                                                                     ICachesResolvedServiceInstances cache,
                                                                     ResolutionRequest request,
                                                                     ISampleService resolved,
                                                                     ResolutionPath resolutionPath)
    {
      // Arrange
      object cachedItem = null;
      Mock.Get(proxiedResolver).Setup(x => x.GetRegistration(request)).Returns(registration);
      Mock.Get(proxiedResolver).Setup(x => x.Resolve(request)).Returns(ResolutionResult.Success(resolutionPath, resolved));
      Mock.Get(cache).Setup(x => x.TryGet(registration, out cachedItem)).Returns(false);
      var sut = new CachingResolverProxy(proxiedResolver, cache);

      // Act
      var result = sut.Resolve(request);

      // Assert
      Mock.Get(cache).Verify(x => x.Add(registration, resolved), Times.Once);
    }

    [Test,AutoMoqData]
    public void Resolve_does_not_add_anything_to_the_cache_if_it_is_not_cacheable(IResolver proxiedResolver,
                                                                                  [Registration(Cacheable = false)] IServiceRegistration registration,
                                                                                  ICachesResolvedServiceInstances cache,
                                                                                  ResolutionRequest request,
                                                                                  ISampleService resolved,
                                                                                  ResolutionPath resolutionPath)
    {
      // Arrange
      object cachedItem = null;
      Mock.Get(proxiedResolver).Setup(x => x.GetRegistration(request)).Returns(registration);
      Mock.Get(proxiedResolver).Setup(x => x.Resolve(request)).Returns(ResolutionResult.Success(resolutionPath, resolved));
      Mock.Get(cache).Setup(x => x.TryGet(registration, out cachedItem)).Returns(false);
      var sut = new CachingResolverProxy(proxiedResolver, cache);

      // Act
      var result = sut.Resolve(request);

      // Assert
      Mock.Get(cache).Verify(x => x.Add(registration, resolved), Times.Never);
    }

    [Test,AutoMoqData]
    public void Resolve_does_not_add_anything_to_the_cache_if_resolution_fails(IResolver proxiedResolver,
                                                                               [Registration] IServiceRegistration registration,
                                                                               ICachesResolvedServiceInstances cache,
                                                                               ResolutionRequest request,
                                                                               ResolutionPath resolutionPath)
    {
      // Arrange
      object cachedItem = null;
      Mock.Get(proxiedResolver).Setup(x => x.GetRegistration(request)).Returns(registration);
      Mock.Get(proxiedResolver).Setup(x => x.Resolve(request)).Returns(ResolutionResult.Failure(resolutionPath));
      Mock.Get(cache).Setup(x => x.TryGet(registration, out cachedItem)).Returns(false);
      var sut = new CachingResolverProxy(proxiedResolver, cache);

      // Act
      var result = sut.Resolve(request);

      // Assert
      Mock.Get(cache).Verify(x => x.Add(registration, It.IsAny<object>()), Times.Never);
    }
  }
}
