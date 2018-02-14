//
//    FactoryRegistration.cs
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
  /// Implementation of a factory registration, which creates an instance of the registered type via a delegate.
  /// </summary>
  public class FactoryRegistration : ServiceRegistration
  {
    readonly Delegate factory;

    /// <summary>
    /// Gets a factory adapter instance, for the current registration, from a specified resolution request.
    /// </summary>
    /// <returns>The factory adapter.</returns>
    /// <param name="request">A resolution request.</param>
    public override IFactoryAdapter GetFactoryAdapter(ResolutionRequest request) => new DelegateFactory(factory);

    /// <summary>
    /// Returns a <see cref="T:System.String"/> that represents the current <see cref="FactoryRegistration"/>.
    /// </summary>
    /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="FactoryRegistration"/>.</returns>
    public override string ToString()
    {
      return $"[Factory registration for `{ServiceType.FullName}']";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FactoryRegistration"/> class.
    /// </summary>
    /// <param name="factory">The factory delegate.</param>
    public FactoryRegistration(Delegate factory)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));

      this.factory = factory;
    }
  }
}
