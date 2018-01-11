//
//    ServiceRegistrationTestBase.cs
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
using CSF.MicroDi.Tests.Autofixture;
using NUnit.Framework;

namespace CSF.MicroDi.Tests.Registration
{
  public abstract class ServiceRegistrationTestBase
  {
    [Test,AutoMoqData]
    public void AssertIsValid_throws_exception_if_registration_is_to_be_disposed_but_is_not_cacheable()
    {
      // Arrange
      var sut = GetValidServiceRegistration();
      try
      {
        sut.Cacheable = false;
      }
      catch(ArgumentException)
      {
        Assert.Pass($"Registration type {sut.GetType()} does not allow cacheable to be set to false, so this test is irrelevant");
      }

      sut.DisposeWithContainer = true;

      // Act & assert
      Assert.That(() => sut.AssertIsValid(), Throws.InstanceOf<InvalidRegistrationException>());
    }

    [Theory]
    public void AssertIsValid_does_not_throw_exception_if_registration_is_not_to_be_disposed_or_is_cacheable(bool dispose,
                                                                                                             bool cacheable)
    {
      Assume.That(!dispose || cacheable,
                  $"Not applicable when {nameof(dispose)} is {Boolean.TrueString} but {nameof(cacheable)} is {Boolean.FalseString}.");

      // Arrange
      var sut = GetValidServiceRegistration();
      try
      {
        sut.Cacheable = cacheable;
      }
      catch(ArgumentException)
      {
        if(cacheable) throw;
        Assert.Pass($"Registration type {sut.GetType()} does not allow cacheable to be set to false, so this test scenario is irrelevant");
      }

      sut.DisposeWithContainer = dispose;

      // Act & assert
      Assert.That(() => sut.AssertIsValid(), Throws.Nothing);
    }

    protected abstract ServiceRegistration GetValidServiceRegistration();

    protected ISelectsConstructor GetConstructorSelector() => new ConstructorWithMostParametersSelector();
  }
}
