﻿using System;
using CSF.MicroDi.Registration;
using CSF.MicroDi.Tests.Autofixture;
using CSF.MicroDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.MicroDi.Tests.Registration
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class RegistryTests
  {
    [Test,AutoMoqData]
    public void Add_can_add_a_registration(Registry sut,
                                           [SampleService] IServiceRegistration registration)
    {
      // Act & assert
      Assert.DoesNotThrow(() => sut.Add(registration));
    }

    [Test,AutoMoqData]
    public void Add_can_add_two_differently_named_registrations(Registry sut,
                                                                [SampleService(Name = "one")] IServiceRegistration registration1,
                                                                [SampleService(Name = "two")] IServiceRegistration registration2)
    {
      // Arrange
      sut.Add(registration1);

      // Act & assert
      Assert.DoesNotThrow(() => sut.Add(registration2));
    }


    [Test,AutoMoqData]
    public void Add_does_not_raise_an_exception_when_adding_the_same_registration_twice(Registry sut,
                                                                                        [SampleService] IServiceRegistration registration)
    {
      // Act
      sut.Add(registration);

      // Assert
      Assert.DoesNotThrow(() => sut.Add(registration));
    }

    [Test,AutoMoqData]
    public void Contains_returns_true_when_a_registration_has_been_added(Registry sut,
                                                                         [SampleService] IServiceRegistration registration)
    {
      // Arrange
      sut.Add(registration);

      // Act
      var result = sut.Contains(ServiceRegistrationKey.ForRegistration(registration));

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void Contains_returns_false_when_a_registration_has_not_been_added(Registry sut,
                                                                              [SampleService] IServiceRegistration registration)
    {
      // Act
      var result = sut.Contains(ServiceRegistrationKey.ForRegistration(registration));

      // Assert
      Assert.That(result, Is.False);
    }

    [Test,AutoMoqData]
    public void Get_returns_a_registration_which_has_been_added(Registry sut,
                                                                [SampleService] IServiceRegistration registration)
    {
      // Arrange
      sut.Add(registration);

      // Act
      var result = sut.Get(ServiceRegistrationKey.ForRegistration(registration));

      // Assert
      Assert.That(result, Is.SameAs(registration));
    }

    [Test,AutoMoqData]
    public void Get_returns_null_for_a_registration_which_has_not_been_added(Registry sut,
                                                                             [SampleService] IServiceRegistration registration)

    {
      // Act
      var result = sut.Get(ServiceRegistrationKey.ForRegistration(registration));

      // Assert
      Assert.That(result, Is.Null);
    }

    [Test,AutoMoqData]
    public void GetAll_can_return_multiple_matching_registrations(Registry sut,
                                                                  [SampleService(Name = "one")] IServiceRegistration registration1,
                                                                  [SampleService(Name = "two")] IServiceRegistration registration2,
                                                                  [SampleService(Name = "three")] IServiceRegistration registration3)
    {
      // Arrange
      sut.Add(registration1);
      sut.Add(registration2);
      sut.Add(registration3);

      var expected = new [] { registration1, registration2, registration3 };

      // Act
      var result = sut.GetAll(typeof(ISampleService));

      // Assert
      Assert.That(result, Is.EquivalentTo(expected));
    }

  }
}