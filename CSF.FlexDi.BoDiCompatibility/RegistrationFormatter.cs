//
//    RegistrationFormatter.cs
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
using System.Collections.Generic;
using System.Linq;
using CSF.FlexDi.Registration;

namespace BoDi
{
  /// <summary>
  /// A service which formats the names of <see cref="IServiceRegistration"/> instances into human-readable strings.
  /// </summary>
  public class RegistrationFormatter
  {
    /// <summary>
    /// Gets a collection of the formatted strings representing the specified registrations.
    /// </summary>
    /// <param name="registrations">The registrations to format.</param>
    public IEnumerable<string> Format(IEnumerable<IServiceRegistration> registrations)
    {
      if(registrations == null)
        throw new ArgumentNullException(nameof(registrations));

      return registrations.Select(x => Format(x));
    }

    /// <summary>
    /// Gets a formatted string representing the specified registration.
    /// </summary>
    /// <param name="registration">The registration to format.</param>
    public string Format(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      var serviceKey = FormatServiceKey(registration);
      var formattedRegistration = FormatRegistration(registration);

      return $"{serviceKey} -> {formattedRegistration}";
    }

    string FormatServiceKey(IServiceRegistration registration)
    {
      var output = registration.ServiceType.FullName;

      if(registration.Name != null)
        output = output + $"('{registration.Name}')";

      return output;
    }

    string FormatRegistration(IServiceRegistration registration)
    {
      if(registration is TypeRegistration)
      {
        return FormatRegistration((TypeRegistration) registration);
      }

      if(registration is FactoryRegistration)
      {
        return FormatRegistration((FactoryRegistration) registration);
      }

      if(registration is InstanceRegistration)
      {
        return FormatRegistration((InstanceRegistration) registration);
      }

      return "[Unsupported registration]";
    }

    string FormatRegistration(TypeRegistration registration)
    {
      return $"Type: {registration.ImplementationType.FullName}";
    }

    string FormatRegistration(FactoryRegistration registration)
    {
      return $"Factory delegate";
    }

    string FormatRegistration(InstanceRegistration registration)
    {
      // Special case for the container itself
      if(registration.ServiceType == typeof(IObjectContainer) && registration.Name == null)
        return "<self>";

      string implementationString;

      try
      {
        implementationString = registration.Implementation.ToString();
      }
      catch(Exception ex)
      {
        implementationString = ex.Message;
      }

      return $"Instance: {implementationString}";
    }
  }
}
