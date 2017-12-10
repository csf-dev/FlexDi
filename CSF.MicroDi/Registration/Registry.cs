using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public class Registry : IRegistersServices
  {
    readonly object syncRoot;
    readonly ConcurrentDictionary<ServiceRegistrationKey,IServiceRegistration> registrations;

    public bool Contains(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));
      
      return registrations.ContainsKey(key);
    }

    public void Add(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      registration.AssertIsValid();

      var key = ServiceRegistrationKey.ForRegistration(registration);
      lock(syncRoot)
      {
        IServiceRegistration removed;
        registrations.TryRemove(key, out removed);
        registrations.TryAdd(key, registration);
      }
    }

    public IServiceRegistration Get(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));

      IServiceRegistration output;
      if(registrations.TryGetValue(key, out output))
        return output;

      return null;
    }

    public IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      return registrations
        .Where(x => x.Key.ServiceType == serviceType)
        .Select(x => x.Value)
        .ToArray();
    }

    public IReadOnlyCollection<IServiceRegistration> GetAll()
    {
      return registrations.Values.ToArray();
    }

    IServiceRegistration IServiceRegistrationProvider.Get(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      var key = ServiceRegistrationKey.FromRequest(request);
      return Get(key);
    }

    bool IServiceRegistrationProvider.CanFulfilRequest(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      var key = ServiceRegistrationKey.FromRequest(request);
      return Contains(key);
    }

    public Registry()
    {
      syncRoot = new object();
      registrations = new ConcurrentDictionary<ServiceRegistrationKey, IServiceRegistration>();
    }
  }
}
