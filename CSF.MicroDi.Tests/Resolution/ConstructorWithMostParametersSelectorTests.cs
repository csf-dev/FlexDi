using System;
using System.Linq;
using CSF.MicroDi.Resolution;
using CSF.MicroDi.Tests.Autofixture;
using CSF.MicroDi.Tests.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.MicroDi.Tests.Resolution
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class ConstructorWithMostParametersSelectorTests
  {
    [Test]
    public void SelectConstructor_gets_correct_public_constructor()
    {
      // Arrange
      var sut = new ConstructorWithMostParametersSelector();
      var expectedParamNames = new [] { "one", "two", "intOne" };

      // Act
      var result = sut.SelectConstructor(typeof(SampleServiceWithConstructorParameters));

      // Assert
      Assert.That(result, Is.Not.Null);
      var actualParamNames = result.GetParameters().Select(x => x.Name).ToArray();
      Assert.That(actualParamNames, Is.EqualTo(expectedParamNames));
    }

    [Test]
    public void SelectConstructor_raises_exception_when_only_ambiguous_constructors_found()
    {
      // Arrange
      var sut = new ConstructorWithMostParametersSelector();

      // Act & assert
      Assert.That(() => sut.SelectConstructor(typeof(SampleServiceWithAmbiguousCtors)),
                  Throws.TypeOf<AmbiguousConstructorException>());
    }

    [Test]
    public void SelectConstructor_raises_exception_when_no_public_constructors_found()
    {
      // Arrange
      var sut = new ConstructorWithMostParametersSelector();

      // Act & assert
      Assert.That(() => sut.SelectConstructor(typeof(SampleServiceWithOnlyPrivateConstructors)),
                  Throws.TypeOf<CannotInstantiateTypeWithoutAnyConstructorsException>());
    }

    [Test]
    public void SelectConstructor_can_use_private_constructor_when_configured_for_it()
    {
      // Arrange
      var sut = new ConstructorWithMostParametersSelector(true);
      var expectedParamNames = new [] { "one", "two", "intOne" };

      // Act
      var result = sut.SelectConstructor(typeof(SampleServiceWithAmbiguousCtors));

      // Assert
      Assert.That(result, Is.Not.Null);
      var actualParamNames = result.GetParameters().Select(x => x.Name).ToArray();
      Assert.That(actualParamNames, Is.EqualTo(expectedParamNames));
    }
  }
}
