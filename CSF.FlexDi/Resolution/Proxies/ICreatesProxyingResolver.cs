﻿//
//    ICreatesSingleResolver.cs
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
namespace CSF.FlexDi.Resolution.Proxies
{
  /// <summary>
  /// A service which creates a <see cref="IResolver"/> which implements <see cref="IProxiesToAnotherResolver"/>.
  /// This interface provides a common point of contact for many such services, which construct resolver proxies
  /// from possibly diverse dependencies.
  /// </summary>
  public interface ICreatesProxyingResolver
  {
    /// <summary>
    /// Creates a resolver which wraps/proxies an inner resolver.
    /// </summary>
    /// <param name="resolutionInfo">Resolution info.</param>
    /// <param name="resolverToProxy">The resolver to proxy.</param>
    IResolver Create(IProvidesResolutionInfo resolutionInfo, IResolver resolverToProxy);
  }
}
