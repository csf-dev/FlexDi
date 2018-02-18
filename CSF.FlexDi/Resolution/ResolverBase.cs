//
//    ResolverBase.cs
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
using CSF.FlexDi.Registration;

namespace CSF.FlexDi.Resolution
{
  /// <summary>
  /// Convenience base class for service implementations which will implement <see cref="IResolver"/>, taking care
  /// of some of the boilerplate code.
  /// </summary>
  public abstract class ResolverBase : IResolver
  {
    /// <summary>
    /// Gets a registration which matches the given resolution request.
    /// </summary>
    /// <returns>The registration.</returns>
    /// <param name="request">Request.</param>
    public abstract IServiceRegistration GetRegistration(ResolutionRequest request);

    /// <summary>
    /// Resolves the given resolution request and returns the result.
    /// </summary>
    /// <param name="request">Request.</param>
    public abstract ResolutionResult Resolve(ResolutionRequest request);

    /// <summary>
    /// An event which occurs when a service is resolved.
    /// </summary>
    public event EventHandler<ServiceResolutionEventArgs> ServiceResolved;

    /// <summary>
    /// Invoker for the <see cref="ServiceResolved"/> event.
    /// </summary>
    /// <param name="registration">Registration.</param>
    /// <param name="instance">Instance.</param>
    protected virtual void InvokeServiceResolved(IServiceRegistration registration, object instance)
    {
      var args = new ServiceResolutionEventArgs(registration, instance);
      InvokeServiceResolved(args);
    }

    /// <summary>
    /// Invoker for the <see cref="ServiceResolved"/> event.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="args">Arguments.</param>
    protected virtual void InvokeServiceResolved(object sender, ServiceResolutionEventArgs args)
    {
      ServiceResolved?.Invoke(sender, args);
    }

    /// <summary>
    /// Invoker for the <see cref="ServiceResolved"/> event.
    /// </summary>
    /// <param name="args">Arguments.</param>
    protected virtual void InvokeServiceResolved(ServiceResolutionEventArgs args)
    {
      ServiceResolved?.Invoke(this, args);
    }
  }
}
