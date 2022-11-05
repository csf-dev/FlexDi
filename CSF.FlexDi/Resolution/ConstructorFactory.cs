//
//    ConstructorFactory.cs
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
  /// Implementation of a <see cref="IFactoryAdapter"/> which executes a <c>System.Reflection.ConstructorInfo</c>.
  /// </summary>
  public class ConstructorFactory : IFactoryAdapter
  {
    readonly ConstructorInfo ctor;

    /// <summary>
    /// Gets the constructor to be executed.
    /// </summary>
    /// <value>The constructor.</value>
    public ConstructorInfo Constructor => ctor;

    /// <summary>
    /// Gets a value indicating whether this <see cref="CSF.FlexDi.Resolution.IFactoryAdapter" /> requires the resolution/provision
    /// of any parameters.
    /// </summary>
    /// <remarks>
    /// <para>
    /// If this value returns <c>false</c>, then the <see cref="M:CSF.FlexDi.Resolution.IFactoryAdapter.Execute(System.Object[])" /> method may always be executed with
    /// an empty array.
    /// </para>
    /// <para>
    /// If it returns <c>true</c> then the <see cref="M:CSF.FlexDi.Resolution.IFactoryAdapter.GetParameters" /> method should be used in order to determine
    /// which parameters are required by the execute method.  The execute method should then be used with those
    /// parameters.
    /// </para>
    /// </remarks>
    /// <value>
    /// <c>true</c> if this adapter requires parameter resolution; otherwise, <c>false</c>.</value>
    public bool RequiresParameterResolution => true;

    /// <summary>
    /// Executes the logic contained within the factory adapter and gets the component.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The array of parameters passed to this method should correspond to those returned by the
    /// <see cref="M:CSF.FlexDi.Resolution.IFactoryAdapter.GetParameters" /> method.
    /// </para>
    /// </remarks>
    /// <param name="parameters">Parameters.</param>
    public object Execute(object[] parameters)
    {
      return ctor.Invoke(parameters);
    }

    /// <summary>
    /// Exposes a collection of the parameters which are required by the <see cref="M:CSF.FlexDi.Resolution.IFactoryAdapter.Execute(System.Object[])" /> method.
    /// </summary>
    /// <returns>The required parameters.</returns>
    public IReadOnlyList<ParameterInfo> GetParameters()
    {
      return ctor.GetParameters();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.FlexDi.Resolution.ConstructorFactory"/> class.
    /// </summary>
    /// <param name="ctor">The constructor.</param>
    public ConstructorFactory(ConstructorInfo ctor)
    {
      if(ctor == null)
        throw new ArgumentNullException(nameof(ctor));

      this.ctor = ctor;
    }
  }
}
