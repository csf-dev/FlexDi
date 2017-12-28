using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class CacheAndRegistrationProviderPair
  {
    static readonly object syncRoot;
    static int nextId;

    readonly int id;

    public IServiceRegistrationProvider RegistrationProvider { get; private set; }
    public ICachesResolvedServiceInstances Cache { get; private set; }

    public override string ToString() => $"[CacheAndRegistrationProviderPair#{id}]";

    int GetId()
    {
      lock(syncRoot)
      {
        return nextId++;
      }
    }

    public CacheAndRegistrationProviderPair(IServiceRegistrationProvider registrationProvider,
                                            ICachesResolvedServiceInstances cache)
    {
      if(cache == null)
        throw new ArgumentNullException(nameof(cache));
      if(registrationProvider == null)
        throw new ArgumentNullException(nameof(registrationProvider));

      RegistrationProvider = registrationProvider;
      Cache = cache;

      id = GetId();
    }

    static CacheAndRegistrationProviderPair()
    {
      syncRoot = new object();
      nextId = 1;
    }
  }
}
