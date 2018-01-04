//
//    IResolvesServices.cs
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
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi
{
  public interface IResolvesServices
  {
    T Resolve<T>();
    T Resolve<T>(string name);
    object Resolve(Type serviceType);
    object Resolve(Type serviceType, string name);

    bool TryResolve<T>(out T output);
    bool TryResolve<T>(string name, out T output);
    bool TryResolve(Type serviceType, out object output);
    bool TryResolve(Type serviceType, string name, out object output);

    ResolutionResult TryResolve(ResolutionRequest request);
    object Resolve(ResolutionRequest request);

    IReadOnlyCollection<T> ResolveAll<T>();
    IReadOnlyCollection<object> ResolveAll(Type serviceType);
  }
}
