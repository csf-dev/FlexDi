//
//    ResolutionPath.cs
//
//    Copyright 2018  Craig Fowler et al
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
//    For further copyright info, including a complete author/contributor
//    list, please refer to the file NOTICE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class ResolutionPath
  {
    readonly Stack<IServiceRegistration> path;

    public bool IsEmpty => !path.Any();

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

    public ResolutionPath(IReadOnlyList<IServiceRegistration> registrations)
    {
      if(registrations == null)
        throw new ArgumentNullException(nameof(registrations));

      path = new Stack<IServiceRegistration>(registrations);
    }

    public ResolutionPath(IServiceRegistration firstRegistration = null)
    {
      path = new Stack<IServiceRegistration>();

      if(firstRegistration != null)
        path.Push(firstRegistration);
    }
  }
}
