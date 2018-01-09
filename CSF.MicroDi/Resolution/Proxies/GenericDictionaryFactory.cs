//
//    GenericDictionaryFactory.cs
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
using System.Collections;
using System.Collections.Generic;

namespace CSF.MicroDi.Resolution.Proxies
{
  public class GenericDictionaryFactory : IDictionaryFactory
  {
    static readonly Type
      DictionaryOpenGenericInterface = typeof(IDictionary<,>),
      DictionaryOpenGenericType = typeof(Dictionary<,>);

    public IDictionary Create(Type keyType, Type valueType)
    {
      if(keyType == null)
        throw new ArgumentNullException(nameof(keyType));
      if(valueType == null)
        throw new ArgumentNullException(nameof(valueType));
      
      var dictionaryConcreteType = DictionaryOpenGenericType.MakeGenericType(keyType, valueType);
      var dictionaryInstance = Activator.CreateInstance(dictionaryConcreteType);
      return (IDictionary) dictionaryInstance;
    }

    public Type GetKeyType(Type genericDictionaryType)
    {
      if(!IsGenericDictionaryType(genericDictionaryType))
        return null;

      return genericDictionaryType.GetGenericArguments()[0];
    }

    public Type GetValueType(Type genericDictionaryType)
    {
      if(!IsGenericDictionaryType(genericDictionaryType))
        return null;

      return genericDictionaryType.GetGenericArguments()[1];
    }

    public bool IsGenericDictionaryType(Type type)
    {
      if(type == null)
        return false;
      
      if(!type.IsGenericType)
        return false;

      var requestGenericTypeDefinition = type.GetGenericTypeDefinition();
      return (requestGenericTypeDefinition == DictionaryOpenGenericInterface);
    }
  }
}
