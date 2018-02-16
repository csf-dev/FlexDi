//
//    IAsBuilderWithMultiplicity.cs
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
  /// Helper type which assists in choosing the 'service type' for a component registration.
  /// </summary>
  public interface IAsBuilderWithCacheability
  {
    /// <summary>
    /// Indicates that the component will be registered 'as' the specified generic type parameter.
    /// </summary>
    /// <typeparam name="T">The service type for which to register the component.</typeparam>
    /// <returns>A builder with which to specify registration options.</returns>
    IRegistrationOptionsBuilderWithCacheability As<T>() where T : class;

    /// <summary>
    /// Indicates that the component will be registered 'as' the specified type.
    /// </summary>
    /// <param name="serviceType">The service type for which to register the component.</param>
    /// <returns>A builder with which to specify registration options.</returns>
    IRegistrationOptionsBuilderWithCacheability As(Type serviceType);

    /// <summary>
    /// Indicates that the component will be registered as its own type (and not under a more general type).
    /// </summary>
    /// <returns>A builder with which to specify registration options.</returns>
    IRegistrationOptionsBuilderWithCacheability AsOwnType();
  }
}
