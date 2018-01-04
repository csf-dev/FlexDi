//
//    DelegateFactory.cs
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
using System.Reflection;

namespace CSF.MicroDi.Resolution
{
  public class DelegateFactory : IFactoryAdapter
  {
    readonly Delegate factory;

    public Delegate Delegate => factory;

    public bool RequiresParameterResolution => true;

    public object Execute(object[] parameters)
    {
      try
      {
        return factory.DynamicInvoke(parameters);
      }
      catch(TargetInvocationException ex)
      {
        var resolutionException = ex.GetBaseException();
        if(resolutionException == null)
          throw;

        if(resolutionException is CircularDependencyException)
          throw new CircularDependencyException(resolutionException.Message, ex);

        throw new ResolutionException(resolutionException.Message, ex);
      }
    }

    void RethrowCorrectException(ResolutionException ex)
    {
    }

    public IReadOnlyList<ParameterInfo> GetParameters()
    {
      return factory.Method.GetParameters();
    }

    public DelegateFactory(Delegate factory)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));
      this.factory = factory;
    }
  }
}
