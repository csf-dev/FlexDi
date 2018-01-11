//
//    ServiceRegistration.cs
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
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public abstract class ServiceRegistration : IServiceRegistration
  {
    bool disposeWithContainer, cacheable;

    public virtual bool Cacheable
    {
      get { return cacheable; }
      set { cacheable = value; }
    }

    public virtual string Name { get; set; }

    public virtual Type ServiceType { get; set; }

    public virtual bool DisposeWithContainer
    {
      get { return disposeWithContainer; }
      set { disposeWithContainer = value; }
    }

    public virtual int Priority => 1;

    public abstract  IFactoryAdapter GetFactoryAdapter(ResolutionRequest request);

    public virtual void AssertIsValid()
    {
      AssertCachabilityAndDisposalAreValid();
    }

    protected void AssertCachabilityAndDisposalAreValid()
    {
      if(!Cacheable && DisposeWithContainer)
        throw new InvalidRegistrationException($"A registration may not have {nameof(DisposeWithContainer)} set to {Boolean.TrueString} if {nameof(Cacheable)} is {Boolean.FalseString}.");
    }

    public virtual bool MatchesKey(ServiceRegistrationKey key)
    {
      if(key == null)
        return false;

      return ServiceType == key.ServiceType && Name == key.Name;
    }

    protected void SetCacheable(bool cacheable) => this.cacheable = cacheable;
    protected void SetDispose(bool dispose) => this.disposeWithContainer = dispose;

    public ServiceRegistration()
    {
      cacheable = true;
      disposeWithContainer = true;
    }
  }
}
