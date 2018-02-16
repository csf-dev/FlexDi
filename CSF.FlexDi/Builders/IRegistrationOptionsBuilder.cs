//
//    IRegistrationOptionsBuilder.cs
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
namespace CSF.FlexDi.Builders
{
  /// <summary>
  /// A type which assists in the selection of optional aspects of creating a registration.
  /// </summary>
  public interface IRegistrationOptionsBuilder
  {
    /// <summary>
    /// Indicates that the current registration is to be registered under a specified name.
    /// </summary>
    /// <returns>The builder instance.</returns>
    /// <param name="name">The name of the registration.</param>
    IRegistrationOptionsBuilder WithName(string name);

    /// <summary>
    /// Indicates that an instance of the component which is created by a container should not be disposed along with that
    /// container (if the component implements <c>IDisposable</c>).  This overrides the default behaviour.
    /// </summary>
    /// <remarks>
    /// <para>
    /// By default, when a container is disposed, any components which were created by that container and contained
    /// within its cache are disposed along with the container.  This depends upon the option
    /// <see cref="ContainerOptions.UseInstanceCache"/> being enabled (it is by default).
    /// </para>
    /// <para>
    /// Disabling this option means that the automatic disposal does not occur, meaning that the client
    /// developer must take responsibility for performing the disposal of any components which had been resolved
    /// from that container.
    /// </para>
    /// </remarks>
    /// <returns>The builder instance.</returns>
    IRegistrationOptionsBuilder DoNotDisposeWithContainer();

    /// <summary>
    /// Indicates that this component should be disposed along with the container which resolves it.
    /// </summary>
    /// <seealso cref="DoNotDisposeWithContainer"/>
    /// <returns>The builder instance.</returns>
    /// <param name="disposeWithContainer">If set to <c>true</c> then the component will be disposed with the container.</param>
    IRegistrationOptionsBuilder DisposeWithContainer(bool disposeWithContainer = true);
  }
}
