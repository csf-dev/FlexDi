﻿//
//    NeverResolvesAttribute.cs
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
using CSF.MicroDi.Resolution;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.NUnit3;

namespace CSF.MicroDi.Tests.Autofixture
{
  public class NeverResolvesAttribute : CustomizeAttribute
  {
    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
      return new NeverResolvesAnythingCustomization();
    }

    class NeverResolvesAnythingCustomization : ICustomization
    {
      public void Customize(IFixture fixture)
      {
        fixture.Customize<IResolver>(c => {
          return c
            .FromFactory(() => Mock.Of<IResolver>())
            .Do(r => {
              Mock.Get(r)
                  .Setup(x => x.Resolve(It.IsAny<ResolutionRequest>()))
                  .Returns(ResolutionResult.Failure(new ResolutionPath()));
            });
        });
      }
    }
  }
}
