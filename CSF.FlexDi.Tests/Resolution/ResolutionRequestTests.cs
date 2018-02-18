//
//    ResolutionRequestTests.cs
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
using CSF.FlexDi.Resolution;
using CSF.FlexDi.Tests.Autofixture;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Resolution
{
  [TestFixture]
  public class ResolutionRequestTests
  {
    [Test,AutoMoqData]
    public void GetCopyWithoutName_copies_service_type(Type type, string name, ResolutionPath path)
    {
      // Arrange
      var sut = new ResolutionRequest(type, name, path);

      // Act
      var result = sut.GetCopyWithoutName();

      // Assert
      Assert.That(result.ServiceType, Is.SameAs(type));
    }

    [Test,AutoMoqData]
    public void GetCopyWithoutName_copies_resolution_path(Type type, string name, ResolutionPath path)
    {
      // Arrange
      var sut = new ResolutionRequest(type, name, path);

      // Act
      var result = sut.GetCopyWithoutName();

      // Assert
      Assert.That(result.ResolutionPath, Is.SameAs(path));
    }

    [Test,AutoMoqData]
    public void GetCopyWithoutName_does_not_copy_the_name(Type type, string name, ResolutionPath path)
    {
      // Arrange
      var sut = new ResolutionRequest(type, name, path);

      // Act
      var result = sut.GetCopyWithoutName();

      // Assert
      Assert.That(result.Name, Is.Null);
    }
  }
}
