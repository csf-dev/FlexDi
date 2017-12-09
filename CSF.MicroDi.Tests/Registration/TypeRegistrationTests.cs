using System;
using CSF.MicroDi.Registration;
using CSF.MicroDi.Resolution;
using CSF.MicroDi.Tests.Autofixture;
using CSF.MicroDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.MicroDi.Tests.Registration
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class TypeRegistrationTests
  {
    [Test,AutoMoqData]
    public void GetFactoryAdapter_uses_constructor_selector(ISelectsConstructor ctorSelector)
    {
      // Arrange
      var type = typeof(SampleServiceWithConstructorParameters);
      var sut = new TypeRegistration(type, ctorSelector);
      var ctor = type.GetConstructor(new [] { typeof(string), typeof(string)});
      Mock.Get(ctorSelector)
          .Setup(x => x.SelectConstructor(type))
          .Returns(ctor);

      // Act
      var result = sut.GetFactoryAdapter();

      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result, Is.InstanceOf<ConstructorFactory>());
      var ctorFactory = (ConstructorFactory) result;
      Assert.That(ctorFactory.Constructor, Is.SameAs(ctor));
      Mock.Get(ctorSelector)
          .Verify(x => x.SelectConstructor(type), Times.Once);
    }
  }
}
