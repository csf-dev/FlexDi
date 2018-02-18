//
//    OpenGenericTypeRegistration.cs
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
  /// Specialisation of <see cref="TypeRegistration"/> which registers open generic types, such as
  /// <c>typeof(MyGenericClass&lt;&gt;)</c> without their generic type parameters specified.
  /// </summary>
  public class OpenGenericTypeRegistration : TypeRegistration
  {
    /// <summary>
    /// Asserts that the current registration is valid (fulfils its invariants).  An exception is raised if it does not.
    /// </summary>
    public override void AssertIsValid()
    {
      if(!IsAssignableToGenericType(ImplementationType, ServiceType))
      {
        var message = String.Format(Resources.ExceptionFormats.InvalidOpenGenericRegistration,
                                    nameof(OpenGenericTypeRegistration),
                                    ImplementationType.FullName,
                                    ServiceType.FullName);
        throw new InvalidTypeRegistrationException(message);
      }

      AssertCachabilityAndDisposalAreValid();
    }

    // From https://stackoverflow.com/questions/74616/how-to-detect-if-type-is-another-generic-type/1075059#1075059
    bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
      var interfaceTypes = givenType.GetInterfaces();

      foreach (var it in interfaceTypes)
      {
        if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
          return true;
      }

      if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        return true;

      Type baseType = givenType.BaseType;
      if (baseType == null) return false;

      return IsAssignableToGenericType(baseType, genericType);
    }

    /// <summary>
    /// Gets a factory adapter instance, for the current registration, from a specified resolution request.
    /// </summary>
    /// <returns>The factory adapter.</returns>
    /// <param name="request">A resolution request.</param>
    public override IFactoryAdapter GetFactoryAdapter(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      if(!request.ServiceType.IsGenericType)
      {
        var message = String.Format(Resources.ExceptionFormats.RequestMustBeForGenericType, request.ServiceType.FullName);
        throw new ArgumentException(message, nameof(request));
      }

      var requestedGenericTypeArgs = request.ServiceType.GetGenericArguments();
      var implementationTypeWithGenericParams = ImplementationType.MakeGenericType(requestedGenericTypeArgs);

      return GetFactoryAdapter(implementationTypeWithGenericParams);
    }

    /// <summary>
    /// Gets a value that indicates whether or not the current registration matches the specified registration key or not.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if the current instance matches the specified key, <c>false</c> otherwise.</returns>
    /// <param name="key">The registration key against which to test.</param>
    public override bool MatchesKey(ServiceRegistrationKey key)
    {
      if(key == null)
        return false;

      if(!key.ServiceType.IsGenericType)
        return false;

      var openGenericKeyType = key.ServiceType.GetGenericTypeDefinition();
      return openGenericKeyType == ServiceType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenGenericTypeRegistration"/> class.
    /// </summary>
    /// <param name="implementationType">Implementation type.</param>
    public OpenGenericTypeRegistration(Type implementationType)
      : this(implementationType, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenGenericTypeRegistration"/> class.
    /// </summary>
    /// <param name="implementationType">Implementation type.</param>
    /// <param name="constructorSelector">Constructor selector.</param>
    public OpenGenericTypeRegistration(Type implementationType, ISelectsConstructor constructorSelector)
      : base( implementationType, constructorSelector){}
  }
}
