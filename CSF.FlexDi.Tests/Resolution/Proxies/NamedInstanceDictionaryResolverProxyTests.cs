//
//    NamedInstanceDictionaryResolverProxyTests.cs
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
using NUnit.Framework;
using CSF.FlexDi.Resolution.Proxies;
using CSF.FlexDi.Tests.Stubs;
using System.Collections.Generic;
using CSF.FlexDi.Tests.Autofixture;
using CSF.FlexDi.Resolution;
using Ploeh.AutoFixture.NUnit3;
using Moq;
using CSF.FlexDi.Registration;
using System.Linq;

namespace CSF.FlexDi.Tests.Resolution.Proxies
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class NamedInstanceDictionaryResolverProxyTests
  {
    [Test,AutoMoqData]
    public void Resolve_uses_proxied_resolver_for_non_dictionary_types([Frozen,ResolvesToFailure] IResolver proxiedResolver,
                                                                       [Frozen] IDictionaryFactory dictionaryFactory,
                                                                       NamedInstanceDictionaryResolverProxy sut,
                                                                       ResolutionRequest request)
    {
      // Arrange
      Mock.Get(dictionaryFactory)
          .Setup(x => x.IsGenericDictionaryType(request.ServiceType))
          .Returns(false);

      // Act
      var result = sut.Resolve(request);

      // Assert
      Mock.Get(proxiedResolver).Verify(x => x.Resolve(request), Times.Once);
      Mock.Get(proxiedResolver).Verify(x => x.Resolve(It.Is<ResolutionRequest>(r => r != request)), Times.Never);
    }

    [Test,AutoMoqData]
    public void Resolve_can_create_dictionary_of_instances([Frozen,ResolvesToFailure] IResolver proxiedResolver,
                                                           [Frozen] IDictionaryFactory dictionaryFactory,
                                                           [Frozen] IServiceRegistrationProvider registrationProvider,
                                                           NamedInstanceDictionaryResolverProxy sut,
                                                           ResolutionRequest request,
                                                           string[] names,
                                                           ResolutionPath path)
    {
      // Arrange
      var serviceType = request.ServiceType;
      var keyType = typeof(string);
      var valueType = typeof(ISampleService);
      Mock.Get(dictionaryFactory)
          .Setup(x => x.IsGenericDictionaryType(serviceType))
          .Returns(true);
      Mock.Get(dictionaryFactory)
          .Setup(x => x.GetKeyType(serviceType))
          .Returns(keyType);
      Mock.Get(dictionaryFactory)
          .Setup(x => x.GetValueType(serviceType))
          .Returns(valueType);
      Mock.Get(dictionaryFactory)
          .Setup(x => x.Create(keyType, valueType))
          .Returns(() => new Dictionary<string,ISampleService>());
      var registrations = names
        .Select(x => Mock.Of<IServiceRegistration>(r => r.Name == x && r.ServiceType == valueType))
        .ToArray();
      Mock.Get(registrationProvider)
          .Setup(x => x.GetAll(valueType))
          .Returns(registrations);
      Mock.Get(proxiedResolver)
          .Setup(x => x.Resolve(It.Is<ResolutionRequest>(r => r.ServiceType == valueType)))
          .Returns((ResolutionRequest r) => ResolutionResult.Success(path, new SampleServiceImplementationOne()));

      // Act
      var result = sut.Resolve(request);

      // Assert
      Mock.Get(proxiedResolver)
          .Verify(x => x.Resolve(It.Is<ResolutionRequest>(r => r.ServiceType == valueType)), Times.Exactly(names.Length));
      foreach(var name in names)
      {
        Mock.Get(proxiedResolver)
          .Verify(x => x.Resolve(It.Is<ResolutionRequest>(r => r.Name == name)), Times.Once);
      }
      Assert.That(result.IsSuccess, Is.True);
      Assert.That(result.ResolvedObject, Is.InstanceOf<IDictionary<string,ISampleService>>());
      var resolvedObject = (IDictionary<string,ISampleService>) result.ResolvedObject;
      Assert.That(resolvedObject, Has.Count.EqualTo(names.Length));
    }

    [Test,AutoMoqData]
    public void Resolve_can_create_dictionary_of_instances_by_enum_values([Frozen,ResolvesToFailure] IResolver proxiedResolver,
                                                                          [Frozen] IDictionaryFactory dictionaryFactory,
                                                                          [Frozen] IServiceRegistrationProvider registrationProvider,
                                                                          NamedInstanceDictionaryResolverProxy sut,
                                                                          ResolutionRequest request,
                                                                          SampleEnum[] names,
                                                                          ResolutionPath path)
    {
      // Arrange
      var serviceType = request.ServiceType;
      var keyType = typeof(SampleEnum);
      var valueType = typeof(ISampleService);
      Mock.Get(dictionaryFactory)
          .Setup(x => x.IsGenericDictionaryType(serviceType))
          .Returns(true);
      Mock.Get(dictionaryFactory)
          .Setup(x => x.GetKeyType(serviceType))
          .Returns(keyType);
      Mock.Get(dictionaryFactory)
          .Setup(x => x.GetValueType(serviceType))
          .Returns(valueType);
      Mock.Get(dictionaryFactory)
          .Setup(x => x.Create(keyType, valueType))
          .Returns(() => new Dictionary<SampleEnum,ISampleService>());
      var registrations = names
        .Select(x => x.ToString())
        .Select(x => Mock.Of<IServiceRegistration>(r => r.Name == x && r.ServiceType == valueType))
        .ToArray();
      Mock.Get(registrationProvider)
          .Setup(x => x.GetAll(valueType))
          .Returns(registrations);
      Mock.Get(proxiedResolver)
          .Setup(x => x.Resolve(It.Is<ResolutionRequest>(r => r.ServiceType == valueType)))
          .Returns((ResolutionRequest r) => ResolutionResult.Success(path, new SampleServiceImplementationOne()));

      // Act
      var result = sut.Resolve(request);

      // Assert
      Mock.Get(proxiedResolver)
          .Verify(x => x.Resolve(It.Is<ResolutionRequest>(r => r.ServiceType == valueType)), Times.Exactly(names.Length));
      foreach(var name in names.Select(x => x.ToString()).ToArray())
      {
        Mock.Get(proxiedResolver)
            .Verify(x => x.Resolve(It.Is<ResolutionRequest>(r => r.Name == name)), Times.Once);
      }
      Assert.That(result.IsSuccess, Is.True);
      Assert.That(result.ResolvedObject, Is.InstanceOf<IDictionary<SampleEnum,ISampleService>>());
      var resolvedObject = (IDictionary<SampleEnum,ISampleService>) result.ResolvedObject;
      Assert.That(resolvedObject, Has.Count.EqualTo(names.Length));
    }

    public enum SampleEnum
    {
      One,
      Two,
      Three
    }
  }
}
