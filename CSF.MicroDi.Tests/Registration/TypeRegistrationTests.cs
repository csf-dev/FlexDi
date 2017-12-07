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
    public void CreateInstance_uses_constructor_selector(ISelectsConstructor ctorSelector,
                                                         IResolutionContext ctx)
    {
      // Arrange
      IFactoryAdapter adapter = null;
      var type = typeof(SampleServiceWithConstructorParameters);
      var sut = new TypeRegistration(type, ctorSelector);
      var ctor = type.GetConstructor(new [] { typeof(string), typeof(string)});
      Mock.Get(ctorSelector)
          .Setup(x => x.SelectConstructor(type))
          .Returns(ctor);
      Mock.Get(ctx)
          .Setup(x => x.Resolve(It.IsAny<IFactoryAdapter>()))
          .Callback((IFactoryAdapter ad) => adapter = ad);

      // Act
      var result = sut.CreateInstance(ctx);

      // Assert
      Assert.That(adapter, Is.Not.Null);
      Assert.That(adapter, Is.InstanceOf<ConstructorFactory>());
      var ctorFactory = (ConstructorFactory) adapter;
      Assert.That(ctorFactory.Constructor, Is.SameAs(ctor));
      Mock.Get(ctorSelector)
          .Verify(x => x.SelectConstructor(type), Times.Once);
    }
  }
}
