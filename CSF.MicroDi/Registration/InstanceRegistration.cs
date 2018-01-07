//
//    InstanceRegistration.cs
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
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public class InstanceRegistration : TypedRegistration
  {
    readonly object implementation;

    public virtual object Implementation => implementation;

    public override Type ImplementationType => Implementation.GetType();

    public override bool Cacheable
    {
      get {
        return base.Cacheable;
      }
      set {
        if(!value)
          throw new ArgumentException($"{nameof(InstanceRegistration)} must always be cacheable.");
        
        base.Cacheable = value;
      }
    }

    public override int Priority => 2;

    public override IFactoryAdapter GetFactoryAdapter(ResolutionRequest request) => new InstanceFactory(implementation);

    public override string ToString()
    {
      return $"[Instance registration for `{ServiceType.FullName}', using an instance of `{ImplementationType.FullName}']";
    }

    public InstanceRegistration(object implementation)
    {
      if(implementation == null)
        throw new ArgumentNullException(nameof(implementation));

      this.implementation = implementation;
      SetCacheable(true);
      SetDispose(false);
    }
  }
}
