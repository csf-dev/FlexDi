using System;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class ResolverFactory
  {
    public IResolver CreateResolver(IProvidesResolutionInfo resolutionInfo)
    {
      return CreateResolver(resolutionInfo, isPrimary: true);
    }

    IResolver CreateResolver(IProvidesResolutionInfo resolutionInfo, bool isPrimary)
    {
      AssertResolutionInfoIsValid(resolutionInfo);

      var output = new LateBoundResolverProxy();

      var coreResolver = GetCoreResolver(resolutionInfo, output);
      IResolver currentResolver = coreResolver;

      currentResolver = GetCachingResolver(resolutionInfo, currentResolver) ?? currentResolver;
      currentResolver = GetParentResolver(resolutionInfo, currentResolver) ?? currentResolver;

      if(isPrimary)
        currentResolver = GetUnregisteredServiceResolver(resolutionInfo, currentResolver, coreResolver);

      currentResolver = GetCircularDependencyProtectingResolver(resolutionInfo, currentResolver) ?? currentResolver;
      currentResolver = GetRegisteredNameInjectingResolver(currentResolver) ?? currentResolver;

      output.ProvideProxiedResolver(currentResolver);

      return output;
    }

    Resolver GetCoreResolver(IProvidesResolutionInfo resolutionInfo,
                             IResolver outermostResolver)
    {
      var instanceCreator = new InstanceCreator(outermostResolver);
      return new Resolver(resolutionInfo.Registry, instanceCreator);
    }

    IResolver GetCachingResolver(IProvidesResolutionInfo resolutionInfo,
                                 IResolver resolverToProxy)
    {
      if(!resolutionInfo.Options.UseInstanceCache)
        return null;
      if(resolutionInfo.Cache == null)
        throw new ArgumentException("The cache provided by the resolution info must not be null.", nameof(resolutionInfo));

      return new CachingResolverProxy(resolverToProxy, resolutionInfo.Cache);
    }

    IResolver GetCircularDependencyProtectingResolver(IProvidesResolutionInfo resolutionInfo,
                                                      IResolver resolverToProxy)
    {
      if(!resolutionInfo.Options.ThrowOnCircularDependencies)
        return null;

      var detector = new CircularDependencyDetector();
      return new CircularDependencyPreventingResolverProxy(resolverToProxy, detector);
    }

    IResolver GetParentResolver(IProvidesResolutionInfo resolutionInfo,
                                IResolver resolverToProxy)
    {
      var parentInfo = resolutionInfo.Parent;

      if(parentInfo == null)
        return null;

      var parentResolver = CreateResolver(parentInfo, isPrimary: false);
      return new FallbackResolverProxy(resolverToProxy, parentResolver);
    }

    IResolver GetUnregisteredServiceResolver(IProvidesResolutionInfo resolutionInfo,
                                             IResolver resolverToProxy,
                                             IResolvesRegistrations registrationResolver)
    {
      if(registrationResolver == null)
        throw new ArgumentNullException(nameof(registrationResolver));
      
      if(!resolutionInfo.Options.ResolveUnregisteredTypes)
        return null;
      
      var unregisteredServiceRegistry = new ServiceWithoutRegistrationProvider();
      return new UnregisteredServiceResolverProxy(resolverToProxy,
                                                  registrationResolver,
                                                  unregisteredServiceRegistry);
    }

    IResolver GetRegisteredNameInjectingResolver(IResolver resolverToProxy)
    {
      return new RegisteredNameInjectingResolverProxy(resolverToProxy);
    }

    void AssertResolutionInfoIsValid(IProvidesResolutionInfo resolutionInfo)
    {
      if(resolutionInfo == null)
        throw new ArgumentNullException(nameof(resolutionInfo));
      if(resolutionInfo.Registry == null)
        throw new ArgumentException("The registry provided by the resolution info must not be null.", nameof(resolutionInfo));
      if(resolutionInfo.Options == null)
        throw new ArgumentException("The options provided by the resolution info must not be null.", nameof(resolutionInfo));
    }
  }
}
