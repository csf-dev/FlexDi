//
//    TypedRegistration.cs
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
namespace CSF.FlexDi.Registration
{
  /// <summary>
  /// Base type for registrations in which the implementation type of the component is known in advance.
  /// </summary>
  public abstract class TypedRegistration : ServiceRegistration
  {
    /// <summary>
    /// Gets the type of the concrete component implementation.  This should be either the same as the
    /// <see cref="ServiceType"/> or a derived type.
    /// </summary>
    /// <value>The implementation type.</value>
    public abstract Type ImplementationType { get; }

    /// <summary>
    /// Gets the <c>System.Type</c> which will be fulfilled by this registration.
    /// </summary>
    /// <value>The service/component type.</value>
    public override Type ServiceType
    {
      get {
        var explicitType = base.ServiceType;
        if(explicitType != null) return explicitType;
        return ImplementationType;
      }
      set {
        base.ServiceType = value;
      }
    }

    /// <summary>
    /// Gets a value that indicates whether or not the current registration matches the specified registration key or not.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if the current instance matches the specified key, <c>false</c> otherwise.</returns>
    /// <param name="key">The registration key against which to test.</param>
    public override bool MatchesKey(ServiceRegistrationKey key)
    {
      if(base.MatchesKey(key))
        return true;
      
      if(key == null)
        return false;

      return key.ServiceType.IsAssignableFrom(ImplementationType) && Name == key.Name;
    }
  }
}
