﻿//
//    IFulfilsResolutionRequests.cs
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
namespace CSF.FlexDi.Resolution
{
  /// <summary>
  /// A service which fulfils requests to resolve a component.
  /// </summary>
  public interface IFulfilsResolutionRequests
  {
    /// <summary>
    /// Performs component resolution based upon the given request, returning an object which indicates the outcome.
    /// </summary>
    /// <param name="request">The resolution request to fulfil.</param>
    ResolutionResult Resolve(ResolutionRequest request);

    /// <summary>
    /// An event which occurs when a service/component is resolved.
    /// </summary>
    event EventHandler<ServiceResolutionEventArgs> ServiceResolved;
  }
}
