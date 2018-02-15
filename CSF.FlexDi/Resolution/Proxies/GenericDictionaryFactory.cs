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

namespace CSF.FlexDi.Resolution.Proxies
{
  /// <summary>
  /// Implementation of <see cref="IDictionaryFactory"/> which creates dictionary instances.
  /// </summary>
  public class GenericDictionaryFactory : IDictionaryFactory
  {
    static readonly Type
      DictionaryOpenGenericInterface = typeof(IDictionary<,>),
      DictionaryOpenGenericType = typeof(Dictionary<,>);

    /// <summary>
    /// Creates and returns an empty generic <c>IDictionary&lt;TKey,TValue&gt;</c> instance.
    /// </summary>
    /// <param name="keyType">Key type.</param>
    /// <param name="valueType">Value type.</param>
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

    /// <summary>
    /// Gets the key type: 'TKey' for the given dictionary type.
    /// </summary>
    /// <returns>The key type.</returns>
    /// <param name="genericDictionaryType">Generic dictionary type.</param>
    public Type GetKeyType(Type genericDictionaryType)
    {
      if(!IsGenericDictionaryType(genericDictionaryType))
        return null;

      return genericDictionaryType.GetGenericArguments()[0];
    }

    /// <summary>
    /// Gets the key type: 'TKey' for the given dictionary type.
    /// </summary>
    /// <returns>The value type.</returns>
    /// <param name="genericDictionaryType">Generic dictionary type.</param>
    public Type GetValueType(Type genericDictionaryType)
    {
      if(!IsGenericDictionaryType(genericDictionaryType))
        return null;

      return genericDictionaryType.GetGenericArguments()[1];
    }

    /// <summary>
    /// Gets a value which indicates whether or not the specified type is a generic dictionary type.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if the specied type is an <c>IDictionary&lt;TKey,TValue&gt;</c>, <c>false</c> otherwise.</returns>
    /// <param name="type">The type.</param>
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
