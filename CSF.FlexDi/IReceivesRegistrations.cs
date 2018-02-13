//
//    IReceivesRegistrations.cs
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
using CSF.FlexDi.Builders;
using CSF.FlexDi.Registration;

namespace CSF.FlexDi
{
  /// <summary>
  /// A service which can receive new component registrations.
  /// </summary>
  public interface IReceivesRegistrations
  {
    /// <summary>
    /// Adds new component registrations by use of a helper type.  Registrations are added within a callback which
    /// uses functionality from the helper.
    /// </summary>
    /// <seealso cref="IRegistrationHelper"/>
    /// <param name="registrations">A callback which may use the functionality of the helper type.</param>
    void AddRegistrations(Action<IRegistrationHelper> registrations);

    /// <summary>
    /// Adds a collection of registration instances directly.
    /// </summary>
    /// <param name="registrations">A collection of registrations.</param>
    void AddRegistrations(IEnumerable<IServiceRegistration> registrations);
  }
}
