//
//    IGenericDictionaryBuilderFactory.cs
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

namespace CSF.FlexDi.Resolution.Proxies
{
  /// <summary>
  /// A service which can create generic <c>IDictionary&lt;TKey,TValue&gt;</c> instances.
  /// </summary>
  public interface IDictionaryFactory
  {
    /// <summary>
    /// Gets a value which indicates whether or not the specified type is a generic dictionary type.
    /// </summary>
    /// <returns><c>true</c>, if the specied type is an <c>IDictionary&lt;TKey,TValue&gt;</c>, <c>false</c> otherwise.</returns>
    /// <param name="type">The type.</param>
    bool IsGenericDictionaryType(Type type);

    /// <summary>
    /// Gets the key type: 'TKey' for the given dictionary type.
    /// </summary>
    /// <returns>The key type.</returns>
    /// <param name="genericDictionaryType">Generic dictionary type.</param>
    Type GetKeyType(Type genericDictionaryType);

    /// <summary>
    /// Gets the key type: 'TKey' for the given dictionary type.
    /// </summary>
    /// <returns>The value type.</returns>
    /// <param name="genericDictionaryType">Generic dictionary type.</param>
    Type GetValueType(Type genericDictionaryType);

    /// <summary>
    /// Creates and returns an empty generic <c>IDictionary&lt;TKey,TValue&gt;</c> instance.
    /// </summary>
    /// <param name="keyType">Key type.</param>
    /// <param name="valueType">Value type.</param>
    IDictionary Create(Type keyType, Type valueType);
  }
}
