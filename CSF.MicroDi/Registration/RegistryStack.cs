using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public class RegistryStack : IRegistersServicesWithScope
  {
    readonly ConcurrentStack<IRegistersServices> providers;

    protected IRegistersServices CurrentProvider
    {
      get {
        IRegistersServices output;

        if(providers.TryPeek(out output))
          return output;
        
        return null;
      }
    }

    public bool CanFulfilRequest(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      return providers.Any(x => x.CanFulfilRequest(request));
    }

    public void Add(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      CurrentProvider.Add(registration);
    }

    public IServiceRegistration Get(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      var provider = providers.FirstOrDefault(x => x.CanFulfilRequest(request));
      if(provider == null)
        return null;
      
      return provider.Get(request);
    }

    public IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      return providers.SelectMany(x => x.GetAll(serviceType)).ToArray();
    }

    public IReadOnlyCollection<IServiceRegistration> GetAll()
    {
      return providers.SelectMany(x => x.GetAll()).ToArray();
    }

    public IRegistersServicesWithScope CreateChildScope(IRegistersServices provider)
    {
      if(provider == null)
        throw new ArgumentNullException(nameof(provider));

      return new RegistryStack(providers, provider);
    }

    bool IServiceRegistrationProvider.HasRegistration(ServiceRegistrationKey key)
    {
      if(key == null)
        throw new ArgumentNullException(nameof(key));
      return providers.Any(x => x.HasRegistration(key));
    }

    public bool HasRegistration(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));
      return providers.Any(x => x.HasRegistration(registration));
    }

    public RegistryStack(IRegistersServices firstProvider)
      : this(Enumerable.Empty<IRegistersServices>(), firstProvider) {}

    public RegistryStack(IEnumerable<IRegistersServices> anscestorProviders,
                         IRegistersServices provider)
    {
      if(anscestorProviders == null)
        throw new ArgumentNullException(nameof(anscestorProviders));
      if(provider == null)
        throw new ArgumentNullException(nameof(provider));
      
      providers = new ConcurrentStack<IRegistersServices>(anscestorProviders);
      providers.Push(provider);
    }
  }
}
