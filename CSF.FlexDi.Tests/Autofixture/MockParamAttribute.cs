//
//    MockParamAttribute.cs
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
using System.Reflection;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.NUnit3;

namespace CSF.FlexDi.Tests.Autofixture
{
  public class MockParamAttribute : CustomizeAttribute
  {
    readonly Type type;
    public virtual Type Type => type;
    public string Name { get; set; }

    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
      return new MockParamCustomization(type, Name);
    }

    public MockParamAttribute(Type type)
    {
      if(type == null)
        throw new ArgumentNullException(nameof(type));
      this.type = type;
    }

    class MockParamCustomization : ICustomization
    {
      readonly Type type;
      readonly string name;

      public void Customize(IFixture fixture)
      {
        fixture.Customize<ParameterInfo>(c => c.FromFactory(GetParameterFactory()));
      }

      Func<string,ParameterInfo> GetParameterFactory() => CreateParameterInfo;

      ParameterInfo CreateParameterInfo(string randomName)
      {
        var mockParam = new Mock<ParameterInfo>();
        mockParam.SetupGet(x => x.ParameterType).Returns(type);
        mockParam.SetupGet(x => x.Name).Returns(() => name ?? randomName);
        return mockParam.Object;
      }

      public MockParamCustomization(Type type, string name)
      {
        if(type == null)
          throw new ArgumentNullException(nameof(type));
        
        this.type = type;
        this.name = name;
      }
    }
  }
}
