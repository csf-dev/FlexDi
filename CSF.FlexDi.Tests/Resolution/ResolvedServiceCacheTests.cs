//
//    ResolvedServiceCacheTests.cs
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
using CSF.FlexDi.Registration;
using CSF.FlexDi.Resolution;
using CSF.FlexDi.Tests.Autofixture;
using CSF.FlexDi.Tests.Stubs;
using Moq;
using NUnit.Framework;
using AutoFixture.NUnit3;

namespace CSF.FlexDi.Tests.Resolution
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class ResolvedServiceCacheTests
  {
    [Test,AutoMoqData]
    public void Has_returns_true_for_a_service_type_after_it_is_added([Registration(ServiceType = typeof(ISampleService))] IServiceRegistration registration,
                                                                      ResolvedServiceCache sut,
                                                                      SampleServiceImplementationOne instance)
    {
      // Act
      sut.Add(registration, instance);
      var result = sut.Has(new ServiceRegistrationKey(typeof(ISampleService), null));

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void Has_returns_true_for_an_implementation_type_after_it_is_added([Registration(ServiceType = typeof(ISampleService))] IServiceRegistration registration,
                                                                              ResolvedServiceCache sut,
                                                                              SampleServiceImplementationOne instance)
    {
      // Act
      sut.Add(registration, instance);
      var result = sut.Has(new ServiceRegistrationKey(typeof(SampleServiceImplementationOne), null));

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void Has_returns_true_for_a_named_service_type_after_it_is_added([Registration(ServiceType = typeof(ISampleService), Name = "A name")] IServiceRegistration registration,
                                                                            ResolvedServiceCache sut,
                                                                            SampleServiceImplementationOne instance)
    {
      // Act
      sut.Add(registration, instance);
      var result = sut.Has(new ServiceRegistrationKey(typeof(ISampleService), registration.Name));

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void Has_returns_true_for_a_named_implementation_type_after_it_is_added([Registration(ServiceType = typeof(ISampleService), Name = "A name")] IServiceRegistration registration,
                                                                                   ResolvedServiceCache sut,
                                                                                   SampleServiceImplementationOne instance)
    {
      // Act
      sut.Add(registration, instance);
      var result = sut.Has(new ServiceRegistrationKey(typeof(SampleServiceImplementationOne), registration.Name));

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void Has_returns_false_for_a_type_which_was_not_added(ResolvedServiceCache sut)
    {
      // Act
      var result = sut.Has(new ServiceRegistrationKey(typeof(ISampleService), null));

      // Assert
      Assert.That(result, Is.False);
    }

    [Test,AutoMoqData]
    public void Has_returns_false_for_a_service_which_was_added_under_a_different_name([Registration(ServiceType = typeof(ISampleService), Name = "A name")] IServiceRegistration registration,
                                                                                       ResolvedServiceCache sut,
                                                                                       SampleServiceImplementationOne instance)
    {
      // Act
      sut.Add(registration, instance);
      var result = sut.Has(new ServiceRegistrationKey(typeof(SampleServiceImplementationOne), "Other name"));

      // Assert
      Assert.That(result, Is.False);
    }

    [Test,AutoMoqData]
    public void Has_returns_true_for_a_named_service_type_after_it_is_added_when_no_name_is_requested([Registration(ServiceType = typeof(ISampleService), Name = "A name")] IServiceRegistration registration,
                                                                                                      ResolvedServiceCache sut,
                                                                                                      SampleServiceImplementationOne instance)
    {
      // Act
      sut.Add(registration, instance);
      var result = sut.Has(new ServiceRegistrationKey(typeof(ISampleService), null));

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void TryGet_returns_cached_instance_for_an_added_service([Registration(ServiceType = typeof(ISampleService))] IServiceRegistration registration,
                                                                    ResolvedServiceCache sut,
                                                                    SampleServiceImplementationOne instance)
    {
      // Act
      sut.Add(registration, instance);
      object cached;
      var result = sut.TryGet(registration, out cached);

      // Assert
      Assert.That(result, Is.True);
      Assert.That(cached, Is.SameAs(instance));
    }
  }
}
