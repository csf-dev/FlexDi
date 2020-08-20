//
//    CacheKeySpecificityComparer.cs
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
using System.Reflection;

namespace CSF.FlexDi.Resolution
{
  /// <summary>
  /// A comparer implementation for <see cref="ServiceCacheKey"/> instances.  It compares then by how 'specific' they are.
  /// </summary>
  /// <remarks>
  /// <para>
  /// If the <see cref="ServiceCacheKey.ImplementationType"/> of two cache keys is identical then this comparer considers
  /// the two implementations to be equal (return value zero).
  /// </para>
  /// <para>
  /// If the implementation type of the first cache key (x) is assignable from the implementation type
  /// of the second cache key (y), then the first key is considered to be 'smaller than' the second (return
  /// value minus one).
  /// </para>
  /// <para>
  /// Otherwise, the first cache key is considered to be greater than the second (return value 1).
  /// </para>
  /// </remarks>
  public class CacheKeySpecificityComparer : Comparer<ServiceCacheKey>
  {
    /// <summary>
    /// Compare the first cache key (<paramref name="x"/>) with the second (<paramref name="y"/>).
    /// </summary>
    /// <param name="x">The first cache key.</param>
    /// <param name="y">The second cache key.</param>
    public override int Compare(ServiceCacheKey x, ServiceCacheKey y)
    {
      if(x == null)
        throw new ArgumentNullException(nameof(x));
      if(y == null)
        throw new ArgumentNullException(nameof(y));

      if(x.ImplementationType == y.ImplementationType) return 0;
      if(x.ImplementationType.GetTypeInfo().IsAssignableFrom(y.ImplementationType.GetTypeInfo())) return -1;
      return 1;
    }
  }
}
