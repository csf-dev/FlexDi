using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class RegisteredNameInjectingResolverProxy : ProxyingResolver
  {
    const string RegisteredName = "registeredName";

    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      if(request.ServiceType != typeof(string) || request.Name != RegisteredName)
        return ProxiedResolver.Resolve(request);

      var currentResolutionPath = request.ResolutionPath;
      var resolvedName = currentResolutionPath.CurrentRegistration.Name;

      var childResolutionPath = CreateRegisteredNameResolutionPath(currentResolutionPath, resolvedName);
      return ResolutionResult.Success(childResolutionPath, resolvedName);
    }

    ResolutionPath CreateRegisteredNameResolutionPath(ResolutionPath parentPath, string name)
    {
      var registration = new InstanceRegistration(name) {
        Name = RegisteredName,
        ServiceType = typeof(string),
      };
      return parentPath.CreateChild(registration);
    }

    public RegisteredNameInjectingResolverProxy(IResolver proxiedResolver) : base(proxiedResolver) {}
  }
}
