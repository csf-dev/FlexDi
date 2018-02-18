//
//    ServiceResolutionEventArgs.cs
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
using CSF.FlexDi.Registration;

namespace CSF.FlexDi
{
  /// <summary>
  /// Event arguments which relate to the successful resolution of a component/service.
  /// </summary>
  public class ServiceResolutionEventArgs : EventArgs
  {
    readonly object instance;
    readonly IServiceRegistration registration;

    /// <summary>
    /// Gets the component instance which was just resolved.
    /// </summary>
    /// <value>The instance.</value>
    public object Instance => instance;

    /// <summary>
    /// Gets a reference to the registration instance which was used to resolve the <see cref="Instance"/>.
    /// </summary>
    /// <value>The registration.</value>
    public IServiceRegistration Registration => registration;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResolutionEventArgs"/> class.
    /// </summary>
    /// <param name="registration">Registration.</param>
    /// <param name="instance">Instance.</param>
    public ServiceResolutionEventArgs(IServiceRegistration registration, object instance)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      this.registration = registration;
      this.instance = instance;
    }
  }
}
