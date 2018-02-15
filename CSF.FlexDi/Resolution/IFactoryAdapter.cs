//
//    IFactoryAdapter.cs
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
using System.Reflection;

namespace CSF.FlexDi.Resolution
{
  /// <summary>
  /// A factory adapter is a service provided by a registration, which is used to get the instance of the resolved
  /// component.  It exposes functionality to get the component instance using a collection of parameters, as well as
  /// providing a way of querying for what those parameters should be.
  /// </summary>
  public interface IFactoryAdapter
  {
    /// <summary>
    /// Gets a value indicating whether this <see cref="IFactoryAdapter"/> requires the resolution/provision
    /// of any parameters.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this value returns <c>false</c>, then the <see cref="Execute"/> method may always be executed with
    /// an empty array.
    /// </para>
    /// <para>
    /// If it returns <c>true</c> then the <see cref="GetParameters"/> method should be used in order to determine
    /// which parameters are required by the execute method.  The execute method should then be used with those
    /// parameters.
    /// </para>
    /// </remarks>
    /// <value><c>true</c> if this adapter requires parameter resolution; otherwise, <c>false</c>.</value>
    bool RequiresParameterResolution { get; }

    /// <summary>
    /// Exposes a collection of the parameters which are required by the <see cref="Execute"/> method.
    /// </summary>
    /// <returns>The required parameters.</returns>
    IReadOnlyList<ParameterInfo> GetParameters();

    /// <summary>
    /// Executes the logic contained within the factory adapter and gets the component.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The array of parameters passed to this method should correspond to those returned by the
    /// <see cref="GetParameters"/> method.
    /// </para>
    /// </remarks>
    /// <param name="parameters">Parameters.</param>
    object Execute(object[] parameters);
  }
}
