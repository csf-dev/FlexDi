//
//    FactoryRegistration`1.cs
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
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi.Registration
{
  public class FactoryRegistration<T> : TypedRegistration
  {
    readonly Delegate factory;

    public override Type ImplementationType => typeof(T);

    public override IFactoryAdapter GetFactoryAdapter(ResolutionRequest request) => new DelegateFactory(factory);

    public override string ToString()
    {
      return $"[Factory registration for `{ServiceType.FullName}', creating an instance of `{ImplementationType.FullName}']";
    }

    public FactoryRegistration(Delegate factory)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));

      this.factory = factory;
    }
  }
}
