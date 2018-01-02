//
//    ConstructorWithMostParametersSelectorTests.cs
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
