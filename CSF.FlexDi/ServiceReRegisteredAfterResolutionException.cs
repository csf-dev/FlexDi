//
//    ServiceReRegisteredAfterResolutionException.cs
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
namespace CSF.FlexDi
{
  /// <summary>
  /// A specialisation of <see cref="InvalidRegistrationException"/> which indicates that an attempt has been made
  /// to re-register a service which has already been resolved (and cached).
  /// </summary>
  /// <remarks>
  /// <para>
  /// This exception can only be raised if <see cref="ContainerOptions.UseInstanceCache"/> is <c>true</c> (by default
  /// it is).  It is acceptable to re-register a component; that is, to add a registration for a service type
  /// and name which has already been registered.  Normally that second registration will override the first; the
  /// second registration is the one which would be used for resolution.
  /// </para>
  /// <para>
  /// However, it is impossible to re-register a service if the component instance has already been resolved from the
  /// same container, and thus added to the cache.  This would break the operation of the cache if it were permitted
  /// (because the existing instance in the cache was created from the registration which is now being overridden).
  /// </para>
  /// </remarks>
#if !NETSTANDARD1_1
    [System.Serializable]
#endif
    public class ServiceReRegisteredAfterResolutionException : InvalidRegistrationException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceReRegisteredAfterResolutionException"/> class
    /// </summary>
    public ServiceReRegisteredAfterResolutionException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceReRegisteredAfterResolutionException"/> class
    /// </summary>
    /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
    public ServiceReRegisteredAfterResolutionException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceReRegisteredAfterResolutionException"/> class
    /// </summary>
    /// <param name="message">A <see cref="System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public ServiceReRegisteredAfterResolutionException(string message, Exception inner) : base(message, inner)
    {
    }

#if !NETSTANDARD1_1
    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceReRegisteredAfterResolutionException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected ServiceReRegisteredAfterResolutionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
    {
    }
#endif
  }
}
