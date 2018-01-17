//
//    IRegistrationHelper.cs
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
  public interface IRegistrationHelper
  {
    IAsBuilderWithCacheability RegisterType<T>() where T : class;
    IAsBuilderWithCacheability RegisterType(Type concreteType);

    IAsBuilderWithCacheability RegisterFactory<TReg>(Func<TReg> factory) where TReg : class;
    IAsBuilderWithCacheability RegisterFactory<T1,TReg>(Func<T1,TReg> factory) where TReg : class;
    IAsBuilderWithCacheability RegisterFactory<T1,T2,TReg>(Func<T1,T2,TReg> factory) where TReg : class;
    IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,TReg>(Func<T1,T2,T3,TReg> factory) where TReg : class;
    IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,TReg>(Func<T1,T2,T3,T4,TReg> factory) where TReg : class;
    IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,T5,TReg>(Func<T1,T2,T3,T4,T5,TReg> factory) where TReg : class;
    IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,T5,T6,TReg>(Func<T1,T2,T3,T4,T5,T6,TReg> factory) where TReg : class;
    IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,T5,T6,T7,TReg>(Func<T1,T2,T3,T4,T5,T6,T7,TReg> factory) where TReg : class;
    IAsBuilderWithCacheability RegisterFactory<T1,T2,T3,T4,T5,T6,T7,T8,TReg>(Func<T1,T2,T3,T4,T5,T6,T7,T8,TReg> factory) where TReg : class;
    IAsBuilderWithCacheability RegisterDynamicFactory<T>(Func<IResolvesServices,T> factory) where T : class;
    IRegistrationOptionsBuilderWithCacheability RegisterFactory(Delegate factory, Type serviceType);
    IRegistrationOptionsBuilderWithCacheability RegisterFactory<TService>(Delegate factory) where TService : class;

    IAsBuilder RegisterInstance<T>(T instance) where T : class;
  }
}
