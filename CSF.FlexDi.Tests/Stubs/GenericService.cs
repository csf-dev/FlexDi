﻿//
//    GenericService.cs
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
namespace CSF.FlexDi.Tests.Stubs
{
  public interface IGenericService<T>
  { void DoSomething(T input); }

  public class GenericService<T> : IGenericService<T>
  { public void DoSomething(T input) => input?.ToString(); }

  public interface IOtherGenericService<T>
  { void DoSomethingElse(T input); }

  public class OtherGenericService<T> : IOtherGenericService<T>
  { public void DoSomethingElse(T input) => input?.ToString(); }
}
