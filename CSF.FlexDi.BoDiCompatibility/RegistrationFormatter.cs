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
    /// A class which formats the names of <see cref="IServiceRegistration"/> instances into human-readable strings.
    /// </summary>
    public static class RegistrationFormatter
    {
        /// <summary>
        /// Gets a collection of the formatted strings representing the specified registrations.
        /// </summary>
        /// <param name="registrations">The registrations to format.</param>
        public static IEnumerable<string> Format(IEnumerable<IServiceRegistration> registrations)
        {
            if(registrations == null)
                throw new ArgumentNullException(nameof(registrations));

            return registrations.Select(x => Format(x));
        }

        /// <summary>
        /// Gets a formatted string representing the specified registration.
        /// </summary>
        /// <param name="registration">The registration to format.</param>
        static string Format(IServiceRegistration registration)
        {
            if(registration == null)
                throw new ArgumentNullException(nameof(registration));

            return $"{FormatServiceKey(registration)} -> {FormatRegistration(registration)}";
        }

        static string FormatServiceKey(IServiceRegistration registration)
        {
            var output = registration.ServiceType.FullName;

            if(registration.Name != null)
                output = output + $"('{registration.Name}')";

            return output;
        }

        static string FormatRegistration(IServiceRegistration registration)
        {
            if(registration is TypeRegistration typeReg)
                return $"Type: {typeReg.ImplementationType.FullName}";

            if(registration is FactoryRegistration)
                return $"Factory delegate";

            if(registration is InstanceRegistration instanceReg)
                return FormatInstanceRegistration(instanceReg);

            return "[Unknown registration]";
        }

        static string FormatInstanceRegistration(InstanceRegistration registration)
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
