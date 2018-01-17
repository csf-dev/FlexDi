//
//    GenericDictionaryFactoryTests.cs
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

namespace CSF.FlexDi.Tests.Resolution.Proxies
{
  [TestFixture,Parallelizable(ParallelScope.All)]
  public class GenericDictionaryFactoryTests
  {
    [Test,AutoMoqData]
    public void Create_creates_generic_dictionary_of_right_kind(GenericDictionaryFactory sut)
    {
      // Act
      var result = sut.Create(typeof(string), typeof(ISampleService));

      // Assert
      Assert.That(result, Is.InstanceOf<IDictionary<string,ISampleService>>());
    }

    [Test,AutoMoqData]
    public void GetKeyType_returns_correct_key_type_for_generic_dictionary(GenericDictionaryFactory sut)
    {
      // Act
      var result = sut.GetKeyType(typeof(IDictionary<int,ISampleService>));

      // Assert
      Assert.That(result, Is.EqualTo(typeof(int)));
    }

    [Test,AutoMoqData]
    public void GetValueType_returns_correct_key_type_for_generic_dictionary(GenericDictionaryFactory sut)
    {
      // Act
      var result = sut.GetValueType(typeof(IDictionary<string,ISampleService>));

      // Assert
      Assert.That(result, Is.EqualTo(typeof(ISampleService)));
    }

    [Test,AutoMoqData]
    public void GetKeyType_returns_null_for_non_dictionary_type(GenericDictionaryFactory sut)
    {
      // Act
      var result = sut.GetKeyType(typeof(ISampleService));

      // Assert
      Assert.That(result, Is.Null);
    }

    [Test,AutoMoqData]
    public void GetValueType_returns_null_for_non_dictionary_type(GenericDictionaryFactory sut)
    {
      // Act
      var result = sut.GetValueType(typeof(ISampleService));

      // Assert
      Assert.That(result, Is.Null);
    }

    [Test,AutoMoqData]
    public void IsGenericDictionaryType_returns_true_for_dictionary_types(GenericDictionaryFactory sut)
    {
      // Act
      var result = sut.IsGenericDictionaryType(typeof(IDictionary<string,ISampleService>));

      // Assert
      Assert.That(result, Is.True);
    }

    [Test,AutoMoqData]
    public void IsGenericDictionaryType_returns_false_for_non_dictionary_types(GenericDictionaryFactory sut)
    {
      // Act
      var result = sut.IsGenericDictionaryType(typeof(IList<ISampleService>));

      // Assert
      Assert.That(result, Is.False);
    }
  }
}
