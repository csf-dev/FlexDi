using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CSF.MicroDi.Kernel
{
  public class Registry : IServiceRegistrationProvider
  {
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

      var key = ServiceRegistrationKey.ForRegistration(registration);
      var result = registrations.TryAdd(key, registration);
      if(!result)
        throw new InvalidOperationException($"The registry must not already contain a duplicate registration: {registration.ToString()}");
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
      registrations = new ConcurrentDictionary<ServiceRegistrationKey, IServiceRegistration>();
    }
  }
}
