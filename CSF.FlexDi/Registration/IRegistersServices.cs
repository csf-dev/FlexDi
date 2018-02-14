//
//    IRegistersServices.cs
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
  /// A service type which is able to add new registrations to a registry of some kind.
  /// </summary>
  public interface IRegistersServices : IServiceRegistrationProvider
  {
    /// <summary>
    /// Add the specified component registration.
    /// </summary>
    /// <param name="registration">Registration.</param>
    void Add(IServiceRegistration registration);
  }
}
