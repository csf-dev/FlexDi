//
//    ContainerOptions.cs
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
namespace CSF.MicroDi
{
  public class ContainerOptions
  {
    static readonly ContainerOptions defaultOptions;

    public bool UseNonPublicConstructors { get; private set; }
    public bool ResolveUnregisteredTypes { get; private set; }
    public bool UseInstanceCache { get; private set; }
    public bool ThrowOnCircularDependencies { get; private set; }
    public bool SupportResolvingNamedInstanceDictionaries { get; private set; }
    public bool SelfRegisterAResolver { get; private set; }

    public ContainerOptions(bool useNonPublicConstructors = false,
                            bool resolveUnregisteredTypes = false,
                            bool useInstanceCache = true,
                            bool throwOnCircularDependencies = true,
                            bool supportResolvingNamedInstanceDictionaries = false,
                            bool selfRegisterAResolver = true)
    {
      SelfRegisterAResolver = selfRegisterAResolver;
      UseNonPublicConstructors = useNonPublicConstructors;
      ResolveUnregisteredTypes = resolveUnregisteredTypes;
      UseInstanceCache = useInstanceCache;
      ThrowOnCircularDependencies = throwOnCircularDependencies;
      SupportResolvingNamedInstanceDictionaries = supportResolvingNamedInstanceDictionaries;
    }

    static ContainerOptions()
    {
      defaultOptions = new ContainerOptions();
    }

    public static ContainerOptions Default => defaultOptions;
  }
}
