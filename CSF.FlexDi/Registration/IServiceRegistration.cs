//
//    IServiceRegistration.cs
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
  /// A registration for a single service/component type.  This is used to fulfil the resolution requests by providing
  /// sufficient information to get an instance of a component being resolved, or a dependency thereof.
  /// </summary>
  public interface IServiceRegistration
  {
    /// <summary>
    /// Gets a value indicating whether this <see cref="IServiceRegistration"/> is cacheable.
    /// </summary>
    /// <value><c>true</c> if the registration is cacheable; otherwise, <c>false</c>.</value>
    bool Cacheable { get; }

    /// <summary>
    /// Gets the <c>System.Type</c> which will be fulfilled by this registration.
    /// </summary>
    /// <value>The service/component type.</value>
    Type ServiceType { get; }

    /// <summary>
    /// Gets an optional registration name.
    /// </summary>
    /// <value>The name.</value>
    string Name { get; }

    /// <summary>
    /// Gets a value indicating whether component instances created from this <see cref="IServiceRegistration"/>
    /// should be disposed with the container which created them.
    /// </summary>
    /// <seealso cref="Builders.IRegistrationOptionsBuilder.DoNotDisposeWithContainer"/>
    /// <value><c>true</c> if the component should be disposed with the container; otherwise, <c>false</c>.</value>
    bool DisposeWithContainer { get; }

    /// <summary>
    /// Gets a factory adapter instance, for the current registration, from a specified resolution request.
    /// </summary>
    /// <returns>The factory adapter.</returns>
    /// <param name="request">A resolution request.</param>
    IFactoryAdapter GetFactoryAdapter(ResolutionRequest request);

    /// <summary>
    /// Asserts that the current registration is valid (fulfils its invariants).  An exception is raised if it does not.
    /// </summary>
    void AssertIsValid();

    /// <summary>
    /// Gets a value that indicates whether or not the current registration matches the specified registration key or not.
    /// </summary>
    /// <returns><c>true</c>, if the current instance matches the specified key, <c>false</c> otherwise.</returns>
    /// <param name="key">The registration key against which to test.</param>
    bool MatchesKey(ServiceRegistrationKey key);

    /// <summary>
    /// Gets a numeric priority for the current registration instance.  Higher numeric priorities take precedence
    /// over lower ones.
    /// </summary>
    /// <value>The priority of this registration.</value>
    int Priority { get; }
  }
}
