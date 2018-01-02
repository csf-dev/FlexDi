﻿using System;
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class ResolutionPath
  {
    readonly Stack<IServiceRegistration> path;

    public IServiceRegistration CurrentRegistration => path.Peek();

    public bool Contains(Type serviceType) => Contains(serviceType, null);

    public bool Contains(ResolutionRequest request) => Contains(request.ServiceType, request.Name);

    public bool Contains(Type serviceType, string name)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      var candidates = path.Where(x => RegistrationMatches(x, serviceType));

      if(name != null)
        candidates = candidates.Where(x => x.Name == name);

      return candidates.Any();
    }

    public bool Contains(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      var candidates = path.Where(x => RegistrationMatches(x, registration));

      if(registration.Name != null)
        candidates = candidates.Where(x => x.Name == registration.Name);

      return candidates.Any();
    }

    public IList<IServiceRegistration> GetRegistrations() => path.ToList();

    public ResolutionPath CreateChild(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      return new ResolutionPath(path, registration);
    }

    bool RegistrationMatches(IServiceRegistration candidate, Type serviceType)
    {
      return candidate.ServiceType == serviceType;
    }

    bool RegistrationMatches(IServiceRegistration candidate, IServiceRegistration actual)
    {
      var typedCandidate = candidate as TypedRegistration;
      var typedActual = actual as TypedRegistration;
      if(typedCandidate == null || typedActual == null)
        return RegistrationMatches(candidate, actual.ServiceType);

      return (typedCandidate.ServiceType == typedActual.ServiceType
              && typedCandidate.ImplementationType == typedActual.ImplementationType);
    }

    ResolutionPath(IEnumerable<IServiceRegistration> previousPath, IServiceRegistration nextRegistration)
    {
      if(nextRegistration == null)
        throw new ArgumentNullException(nameof(nextRegistration));
      if(previousPath == null)
        throw new ArgumentNullException(nameof(previousPath));

      path = new Stack<IServiceRegistration>(previousPath);
      path.Push(nextRegistration);
    }

    public ResolutionPath(IServiceRegistration firstRegistration = null)
    {
      path = new Stack<IServiceRegistration>();

      if(firstRegistration != null)
        path.Push(firstRegistration);
    }
  }
}