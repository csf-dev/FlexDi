﻿//
//    ICachesResolvedServiceInstances.cs
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
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public interface ICachesResolvedServiceInstances
  {
    void Add(IServiceRegistration registration, object instance);
    bool Has(IServiceRegistration registration);
    bool Has(ServiceRegistrationKey key);
    bool TryGet(IServiceRegistration registration, out object instance);
    bool TryGet(Type serviceType, string name, out object instance);
  }
}
