using System;
using System.Linq;
using System.Reflection;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class Resolver : IResolver
  {
    readonly IServiceRegistrationProvider registrationProvider, unregisteredServiceProvider;
    readonly ResolutionPath resolutionPath;
    readonly CircularDependencyDetector circularDependencyDetector;

    public ResolutionPath ResolutionPath => resolutionPath;

    public virtual bool Resolve(ResolutionRequest request, out object output)
      => Resolve(request, out output, this);

    public virtual bool Resolve(ResolutionRequest request, out object output, IResolver outermostResolver)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));
      if(outermostResolver == null)
        throw new ArgumentNullException(nameof(outermostResolver));

      output = null;
      var registration = GetRegistration(request);
      if(registration == null)
        return false;

      circularDependencyDetector.ThrowOnCircularDependency(registration, outermostResolver.ResolutionPath);

      output = Resolve(registration, outermostResolver);
      return true;
    }

    public virtual object Resolve(IFactoryAdapter factory, IResolver outermostResolver)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));

      if(!factory.RequiresParameterResolution)
        return factory.Execute(Enumerable.Empty<object>().ToArray());

      var parameters = factory.GetParameters();
      var resolvedParameters = parameters
        .Select(ConvertToResolutionRequest)
        .Select(x => Resolve(x, outermostResolver))
        .ToArray();

      return factory.Execute(resolvedParameters);
    }

    public virtual IServiceRegistration GetRegistration(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      if(registrationProvider.CanFulfilRequest(request))
        return registrationProvider.Get(request);

      var requestWithoutName = request.GetCopyWithoutName();
      if(registrationProvider.CanFulfilRequest(requestWithoutName))
        return registrationProvider.Get(requestWithoutName);

      return unregisteredServiceProvider.Get(request);
    }

    public event EventHandler<ServiceResolutionEventArgs> ServiceResolved;

    public IResolver CreateChild(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      var path = resolutionPath.CreateChild(registration);
      var resolver = new Resolver(registrationProvider, unregisteredServiceProvider, path);
      resolver.ServiceResolved += OnServiceResolved;
      return resolver;
    }

    protected virtual void InvokeServiceResolved(IServiceRegistration registration, object instance)
    {
      var args = new ServiceResolutionEventArgs(registration, instance);
      InvokeServiceResolved(args);
    }

    protected virtual void InvokeServiceResolved(ServiceResolutionEventArgs args)
    {
      ServiceResolved?.Invoke(this, args);
    }

    protected virtual void OnServiceResolved(object sender, ServiceResolutionEventArgs args)
    {
      InvokeServiceResolved(args);
    }

    protected virtual object Resolve(ResolutionRequest request, IResolver outermostResolver)
    {
      object output;
      Resolve(request, out output, outermostResolver);
      return output;
    }

    protected virtual object Resolve(IServiceRegistration registration, IResolver outermostResolver)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));
      if(outermostResolver == null)
        throw new ArgumentNullException(nameof(outermostResolver));

      var resolver = outermostResolver.CreateChild(registration);
      var factoryAdapter = registration.GetFactoryAdapter();
      var instance = Resolve(factoryAdapter, resolver);
      InvokeServiceResolved(registration, instance);
      return instance;
    }

    protected virtual ResolutionRequest ConvertToResolutionRequest(ParameterInfo parameter)
    {
      if(parameter == null)
        throw new ArgumentNullException(nameof(parameter));
      
      return new ResolutionRequest(parameter.ParameterType, parameter.Name);
    }

    public Resolver(IServiceRegistrationProvider registrationProvider) : this(registrationProvider, null) {}

    public Resolver(IServiceRegistrationProvider registrationProvider,
                    IServiceRegistrationProvider unregisteredServiceProvider)
    {
      if(registrationProvider == null)
        throw new ArgumentNullException(nameof(registrationProvider));

      this.registrationProvider = registrationProvider;
      this.unregisteredServiceProvider = unregisteredServiceProvider?? new ServiceWithoutRegistrationProvider();
      resolutionPath = new ResolutionPath();
      circularDependencyDetector = new CircularDependencyDetector();
    }

    Resolver(IServiceRegistrationProvider registrationProvider,
             IServiceRegistrationProvider unregisteredServiceProvider,
             ResolutionPath resolutionPath) : this(registrationProvider, unregisteredServiceProvider)
    {
      if(resolutionPath == null)
        throw new ArgumentNullException(nameof(resolutionPath));
      
      this.resolutionPath = resolutionPath;
    }
  }
}
