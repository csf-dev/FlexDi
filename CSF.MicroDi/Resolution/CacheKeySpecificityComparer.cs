using System;
using System.Collections.Generic;

namespace CSF.MicroDi.Resolution
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
