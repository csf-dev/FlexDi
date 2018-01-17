//
//    ServiceRegistrationKey.cs
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
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi.Registration
{
  public class ServiceRegistrationKey : IEquatable<ServiceRegistrationKey>
  {
    public Type ServiceType { get; private set; }

    public string Name { get; private set; }

    public override bool Equals(object obj)
    {
      return Equals(obj as ServiceRegistrationKey);
    }

    public bool Equals(ServiceRegistrationKey other)
    {
      if(ReferenceEquals(other, null))
        return false;
      if(ReferenceEquals(other, this))
        return true;

      return (other.ServiceType == ServiceType
              && other.Name == Name);
    }

    public override int GetHashCode()
    {
      var typeHash = ServiceType.GetHashCode();
      var nameHash = Name?.GetHashCode() ?? 0;

      return typeHash ^ nameHash;
    }

    public ServiceRegistrationKey(Type serviceType, string name)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      ServiceType = serviceType;
      Name = name;
    }

    public static ServiceRegistrationKey ForRegistration(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      return new ServiceRegistrationKey(registration.ServiceType, registration.Name);
    }

    public static ServiceRegistrationKey FromRequest(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      return new ServiceRegistrationKey(request.ServiceType, request.Name);
    }
  }
}
