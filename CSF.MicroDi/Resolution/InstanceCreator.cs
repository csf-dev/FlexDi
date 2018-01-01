using System;
using System.Linq;
using System.Reflection;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class InstanceCreator : ICreatesObjectInstances
  {
    readonly IFulfilsResolutionRequests resolver;

    public virtual object CreateFromFactory(IFactoryAdapter factory,
                                            ResolutionPath path,
                                            IServiceRegistration registration)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));
      
      if(!factory.RequiresParameterResolution)
        return factory.Execute(Enumerable.Empty<object>().ToArray());

      var parameters = factory.GetParameters();

      var resolvedParameters = parameters
        .Select(param => ResolveParameter(param, path, registration))
        .ToArray();

      return factory.Execute(resolvedParameters);
    }

    protected virtual object ResolveParameter(ParameterInfo parameter,
                                              ResolutionPath path,
                                              IServiceRegistration registration)
    {
      var request = ConvertToResolutionRequest(parameter, path, registration);
      var result = resolver.Resolve(request);

      if(!result.IsSuccess)
      {
        var message = $"Failed to resolve parameter: {parameter.ParameterType.FullName} {parameter.Name}";
        throw new CannotResolveParameterException(message) {
          ResolutionPath = path,
        };
      }

      return result.ResolvedObject;
    }

    ResolutionRequest ConvertToResolutionRequest(ParameterInfo parameter,
                                                 ResolutionPath path,
                                                 IServiceRegistration registration)
    {
      if(parameter == null)
        throw new ArgumentNullException(nameof(parameter));

      var childPath = path.CreateChild(registration);
      return new ResolutionRequest(parameter.ParameterType, parameter.Name, childPath);
    }

    public InstanceCreator(IFulfilsResolutionRequests resolver)
    {
      if(resolver == null)
        throw new ArgumentNullException(nameof(resolver));

      this.resolver = resolver;
    }
  }
}
