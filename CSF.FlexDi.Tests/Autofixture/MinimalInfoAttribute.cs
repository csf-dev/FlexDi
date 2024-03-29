﻿//
//    MinimalInfo.cs
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
using CSF.FlexDi.Registration;
using CSF.FlexDi.Resolution;
using Moq;
using AutoFixture;
using AutoFixture.NUnit3;

namespace CSF.FlexDi.Tests.Autofixture
{
  public class MinimalInfoAttribute : CustomizeAttribute
  {
    public bool HasParent { get; set; }

    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
      return new MinimalResolutionInfoCustomization(HasParent);
    }

    public MinimalInfoAttribute()
    {
      HasParent = false;
    }

    class MinimalResolutionInfoCustomization : ICustomization
    {
      readonly bool hasParent;

      public void Customize(IFixture fixture)
      {
        fixture.Customize<IProvidesResolutionInfo>(cfg => {
          return cfg
            .FromFactory(CreateResolutionInfo)
            .Do(ConfigureResolutionInfo);
        });
      }

      IProvidesResolutionInfo CreateResolutionInfo()
      {
        var output = new Mock<IProvidesResolutionInfo> { DefaultValue = DefaultValue.Mock };
        output.SetupGet(x => x.Options).Returns(new ContainerOptions());
        return output.Object;
      }

      void ConfigureResolutionInfo(IProvidesResolutionInfo info)
      {
        var parent = CreateResolutionInfo();
        Mock.Get(parent).SetupGet(x => x.Parent).Returns((IProvidesResolutionInfo) null);
        Mock.Get(info).SetupGet(x => x.Parent).Returns(hasParent? parent : null);
      }

      public MinimalResolutionInfoCustomization(bool hasParent)
      {
        this.hasParent = hasParent;
      }
    }
  }
}
