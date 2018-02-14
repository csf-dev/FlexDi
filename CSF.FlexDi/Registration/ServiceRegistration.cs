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
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi.Registration
{
  /// <summary>
  /// Base class for implementations of <see cref="IServiceRegistration"/>.
  /// </summary>
  public abstract class ServiceRegistration : IServiceRegistration
  {
    bool disposeWithContainer, cacheable;

    /// <summary>
    /// Gets a value indicating whether this <see cref="T:CSF.FlexDi.Registration.IServiceRegistration" /> is cacheable.
    /// </summary>
    /// <value>
    /// <c>true</c> if the registration is cacheable; otherwise, <c>false</c>.</value>
    public virtual bool Cacheable
    {
      get { return cacheable; }
      set { cacheable = value; }
    }

    /// <summary>
    /// Gets an optional registration name.
    /// </summary>
    /// <value>The name.</value>
    public virtual string Name { get; set; }

    /// <summary>
    /// Gets the <c>System.Type</c> which will be fulfilled by this registration.
    /// </summary>
    /// <value>The service/component type.</value>
    public virtual Type ServiceType { get; set; }

    /// <summary>
    /// Gets a value indicating whether component instances created from this <see cref="T:CSF.FlexDi.Registration.IServiceRegistration" />
    /// should be disposed with the container which created them.
    /// </summary>
    /// <seealso cref="M:CSF.FlexDi.Builders.IRegistrationOptionsBuilder.DoNotDisposeWithContainer" />
    /// <value>
    /// <c>true</c> if the component should be disposed with the container; otherwise, <c>false</c>.</value>
    public virtual bool DisposeWithContainer
    {
      get { return disposeWithContainer; }
      set { disposeWithContainer = value; }
    }

    /// <summary>
    /// Gets a numeric priority for the current registration instance.  Higher numeric priorities take precedence
    /// over lower ones.
    /// </summary>
    /// <value>The priority of this registration.</value>
    public virtual int Priority => 1;

    /// <summary>
    /// Gets a factory adapter instance, for the current registration, from a specified resolution request.
    /// </summary>
    /// <returns>The factory adapter.</returns>
    /// <param name="request">A resolution request.</param>
    public abstract  IFactoryAdapter GetFactoryAdapter(ResolutionRequest request);

    /// <summary>
    /// Asserts that the current registration is valid (fulfils its invariants).  An exception is raised if it does not.
    /// </summary>
    public virtual void AssertIsValid()
    {
      AssertCachabilityAndDisposalAreValid();
    }

    /// <summary>
    /// Asserts that the values of <see cref="Cacheable"/> and <see cref="DisposeWithContainer"/> are valid.
    /// The disposal setting may not be <c>true</c> if the registration is not cachable.
    /// </summary>
    protected void AssertCachabilityAndDisposalAreValid()
    {
      if(!Cacheable && DisposeWithContainer)
        throw new InvalidRegistrationException($"A registration may not have {nameof(DisposeWithContainer)} set to {Boolean.TrueString} if {nameof(Cacheable)} is {Boolean.FalseString}.");
    }

    /// <summary>
    /// Gets a value that indicates whether or not the current registration matches the specified registration key or not.
    /// </summary>
    /// <returns>
    /// <c>true</c>, if the current instance matches the specified key, <c>false</c> otherwise.</returns>
    /// <param name="key">The registration key against which to test.</param>
    public virtual bool MatchesKey(ServiceRegistrationKey key)
    {
      if(key == null)
        return false;

      return ServiceType == key.ServiceType && Name == key.Name;
    }

    /// <summary>
    /// Sets the value of <see cref="Cacheable"/>,
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is provided so that it may be called by constructors.  It is bad practice to make use of
    /// <c>virtual</c> members from constructors, so they should not set the property directly.
    /// </para>
    /// </remarks>
    /// <param name="cacheable">If set to <c>true</c> cacheable.</param>
    protected void SetCacheable(bool cacheable) => this.cacheable = cacheable;

    /// <summary>
    /// Sets the value of <see cref="DisposeWithContainer"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method is provided so that it may be called by constructors.  It is bad practice to make use of
    /// <c>virtual</c> members from constructors, so they should not set the property directly.
    /// </para>
    /// </remarks>
    /// <param name="dispose">If set to <c>true</c> dispose.</param>
    protected void SetDispose(bool dispose) => this.disposeWithContainer = dispose;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceRegistration"/> class.
    /// </summary>
    protected ServiceRegistration()
    {
      cacheable = true;
      disposeWithContainer = true;
    }
  }
}
