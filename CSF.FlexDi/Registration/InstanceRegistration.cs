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
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi.Registration
{
  /// <summary>
  /// Implementation of an instance registration, which always provides a provided object instance as the result
  /// of resolution.
  /// </summary>
  public class InstanceRegistration : TypedRegistration
  {
    readonly object implementation;

    /// <summary>
    /// Gets the implementation/instance of the component which will be used to fulfil resolution.
    /// </summary>
    /// <value>The implementation.</value>
    public virtual object Implementation => implementation;

    /// <summary>
    /// Gets the type of the concrete component implementation.  This should be either the same as the
    /// <see cref="P:CSF.FlexDi.Registration.TypedRegistration.ServiceType" /> or a derived type.
    /// </summary>
    /// <value>The implementation type.</value>
    public override Type ImplementationType => Implementation.GetType();

    /// <summary>
    /// Gets a value indicating whether this <see cref="IServiceRegistration" /> is cacheable.  This must always be set to
    /// <c>true</c> for instance registrations.
    /// </summary>
    /// <value>
    /// <c>true</c> if the registration is cacheable; otherwise, <c>false</c>.</value>
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

    /// <summary>
    /// Gets a numeric priority for the current registration instance.  Higher numeric priorities take precedence
    /// over lower ones.
    /// </summary>
    /// <value>The priority of this registration.</value>
    public override int Priority => 2;

    /// <summary>
    /// Gets a factory adapter instance, for the current registration, from a specified resolution request.
    /// </summary>
    /// <returns>The factory adapter.</returns>
    /// <param name="request">A resolution request.</param>
    public override IFactoryAdapter GetFactoryAdapter(ResolutionRequest request) => new InstanceFactory(implementation);

    /// <summary>
    /// Returns a <see cref="T:System.String"/> that represents the current <see cref="InstanceRegistration"/>.
    /// </summary>
    /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="InstanceRegistration"/>.</returns>
    public override string ToString()
    {
      return $"[Instance registration for `{ServiceType.FullName}', using an instance of `{ImplementationType.FullName}']";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InstanceRegistration"/> class.
    /// </summary>
    /// <param name="implementation">The component instance/implementation.</param>
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
