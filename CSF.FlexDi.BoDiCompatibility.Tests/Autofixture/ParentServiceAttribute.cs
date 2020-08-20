//
//    ParentServiceAttribute.cs
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
using System.Reflection;
using CSF.FlexDi.Registration;
using CSF.FlexDi.Resolution;
using CSF.FlexDi.Tests.Stubs;
using Moq;
using AutoFixture;
using AutoFixture.NUnit3;

namespace CSF.FlexDi.Tests.Autofixture
{
  public class ParentServiceAttribute : CustomizeAttribute
  {
    public string Name { get; set; }

    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
      if(parameter.ParameterType != typeof(IServiceRegistration))
        return null;
      
      return new SampleServiceCustomization(Name);
    }

    class SampleServiceCustomization : ICustomization
    {
      readonly string name;

      public void Customize(IFixture fixture)
      {
        fixture.Customize<IServiceRegistration>(c => {
          return c
            .FromFactory(() => Mock.Of<IServiceRegistration>())
            .Do(reg => {
              Mock.Get(reg).SetupGet(x => x.ServiceType).Returns(typeof(ParentService));
              Mock.Get(reg).SetupGet(x => x.Name).Returns(name);
            });
        });
      }

      public SampleServiceCustomization(string name)
      {
        this.name = name;
      }
    }
  }
}
