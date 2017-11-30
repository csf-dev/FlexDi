using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CSF.MicroDi.Kernel
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

      return providers.Select(x => x.Get(request)).FirstOrDefault(x => x != null);
    }

    public IReadOnlyCollection<IServiceRegistration> GetAll(Type serviceType)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      return providers.SelectMany(x => x.GetAll(serviceType)).ToArray();
    }

    public IRegistersServicesWithScope CreateChildScope(IRegistersServices provider)
    {
      if(provider == null)
        throw new ArgumentNullException(nameof(provider));

      return new RegistryStack(providers, provider);
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
