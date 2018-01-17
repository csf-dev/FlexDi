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

namespace CSF.FlexDi.Resolution
{
  public class CacheKeySpecificityComparer : Comparer<ServiceCacheKey>
  {
    public override int Compare(ServiceCacheKey x, ServiceCacheKey y)
    {
      if(x == null)
        throw new ArgumentNullException(nameof(x));
      if(y == null)
        throw new ArgumentNullException(nameof(y));

      if(x.ImplementationType == y.ImplementationType) return 0;
      if(x.ImplementationType.IsAssignableFrom(y.ImplementationType)) return -1;
      return 1;
    }
  }
}
