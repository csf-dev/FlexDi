//
//    IProxiesToAnotherResolver.cs
//
//    Copyright 2018  Craig Fowler
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
namespace CSF.FlexDi.Resolution
{
  /// <summary>
  /// A specialisation of an <see cref="IResolver"/> which proxies to another 'wrapped' resolver.
  /// </summary>
  /// <remarks>
  /// <para>
  /// In the building of resolvers, this is a common theme; many of the resolvers are in fact proxies using the
  /// Chain of Responsibility pattern.  Each proxy may fulfil the resolution request itself or hand the request down
  /// to the resolver which it wraps.
  /// </para>
  /// <para>
  /// Of course, these proxies may also modify the output of the resolution or perform additional logic as they
  /// see fit.
  /// </para>
  /// </remarks>
  public interface IProxiesToAnotherResolver : IResolver
  {
    /// <summary>
    /// Gets the wrapped/proxied resolver instance.
    /// </summary>
    /// <value>The proxied resolver.</value>
    IResolver ProxiedResolver { get; }
  }
}
