//
//    TypeRegistration.cs
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
  public class TypeRegistration : TypedRegistration
  {
    readonly Type implementationType;
    readonly ISelectsConstructor constructorSelector;

    public override Type ImplementationType => implementationType;

    public override IFactoryAdapter GetFactoryAdapter(ResolutionRequest request)
      => GetFactoryAdapter(implementationType);

    protected virtual IFactoryAdapter GetFactoryAdapter(Type type)
      => new ConstructorFactory(constructorSelector.SelectConstructor(type));

    public override string ToString()
    {
      return $"[Type registration for `{ServiceType.FullName}', using type `{ImplementationType.FullName}']";
    }

    public override void AssertIsValid()
    {
      if(!ServiceType.IsAssignableFrom(ImplementationType))
        throw new InvalidTypeRegistrationException($"Invalid {nameof(TypeRegistration)}; the implementation type: `{ImplementationType.FullName}' must derive from the service type: `{ServiceType.FullName}'.");

      base.AssertIsValid();
    }

    public TypeRegistration(Type implementationType) : this(implementationType, null) {}

    public TypeRegistration(Type implementationType, ISelectsConstructor constructorSelector)
    {
      if(implementationType == null)
        throw new ArgumentNullException(nameof(implementationType));

      this.implementationType = implementationType;
      this.constructorSelector = constructorSelector ?? new ConstructorWithMostParametersSelector();
    }
  }
}
