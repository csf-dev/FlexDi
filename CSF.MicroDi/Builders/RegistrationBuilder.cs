//
//    RegistrationBuilder.cs
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
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Builders
{
  public class RegistrationBuilder
    : IAsBuilder, IAsBuilderWithMultiplicity, IRegistrationOptionsBuilder, IRegistrationOptionsBuilderWithMultiplicity
  {
    ServiceRegistration registration;

    public IRegistrationOptionsBuilder As(Type serviceType)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));
      registration.ServiceType = serviceType;
      return this;
    }

    public IRegistrationOptionsBuilder As<T>()
      where T : class
      => As(typeof(T));

    public IRegistrationOptionsBuilderWithMultiplicity AsOwnType()
    {
      var typedRegistration = registration as TypedRegistration;
      if(typedRegistration == null)
        throw new InvalidOperationException($"This operation is only suitable for {nameof(TypedRegistration)} instances.");

      registration.ServiceType = typedRegistration.ImplementationType;
      return this;
    }

    public IRegistrationOptionsBuilderWithMultiplicity SeparateInstancePerResolution()
    {
      registration.Multiplicity = Multiplicity.InstancePerResolution;
      return this;
    }

    public IRegistrationOptionsBuilderWithMultiplicity SingleSharedInstance()
    {
      registration.Multiplicity = Multiplicity.Shared;
      return this;
    }

    public IRegistrationOptionsBuilder WithName(string name)
    {
      registration.Name = name;
      return this;
    }

    public IRegistrationOptionsBuilder DoNotDisposeWithContainer()
    {
      registration.DisposeWithContainer = false;
      return this;
    }

    public IRegistrationOptionsBuilder DisposeWithContainer(bool disposeWithContainer = true)
    {
      registration.DisposeWithContainer = disposeWithContainer;
      return this;
    }

    IRegistrationOptionsBuilderWithMultiplicity IAsBuilderWithMultiplicity.As(Type serviceType)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));
      registration.ServiceType = serviceType;
      return this;
    }

    IRegistrationOptionsBuilderWithMultiplicity IAsBuilderWithMultiplicity.As<T>()
    {
      registration.ServiceType = typeof(T);
      return this;
    }

    IRegistrationOptionsBuilderWithMultiplicity IRegistrationOptionsBuilderWithMultiplicity.WithName(string name)
    {
      registration.Name = name;
      return this;
    }

    public RegistrationBuilder(ServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      this.registration = registration;
    }
  }
}
