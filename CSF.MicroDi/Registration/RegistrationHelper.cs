using System;
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Kernel;

namespace CSF.MicroDi.Registration
{
  public class RegistrationHelper : IRegistrationHelper, IBulkRegistrationProvider
  {
    readonly ICollection<IServiceRegistration> registrations;

    public IRegistrationOptionsBuilderWithMultiplicity RegisterFactory(Delegate factory, Type serviceType)
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      var registration = new FactoryRegistration(factory) { ServiceType = serviceType };
      registrations.Add(registration);
      return new RegistrationBuilder(registration);
    }

    public IRegistrationOptionsBuilderWithMultiplicity RegisterFactory<TService>(Delegate factory)
      where TService : class
      => RegisterFactory(factory, typeof(TService));

    public IAsBuilderWithMultiplicity RegisterFactory<T>(Func<T> factory) where T : class
    {
      if(factory == null)
        throw new ArgumentNullException(nameof(factory));

      var registration = new FactoryRegistration<T>(factory) { ServiceType = typeof(T) };
      registrations.Add(registration);
      return new RegistrationBuilder(registration);
    }

    public IAsBuilder RegisterInstance(object instance)
    {
      if(instance == null)
        throw new ArgumentNullException(nameof(instance));

      var registration = new InstanceRegistration(instance) { ServiceType = instance.GetType() };
      registrations.Add(registration);
      return new RegistrationBuilder(registration);
    }

    public IAsBuilderWithMultiplicity RegisterType(Type concreteType)
    {
      if(concreteType == null)
        throw new ArgumentNullException(nameof(concreteType));

      var registration = new TypeRegistration(concreteType) { ServiceType = concreteType };
      registrations.Add(registration);
      return new RegistrationBuilder(registration);
    }

    public IAsBuilderWithMultiplicity RegisterType<T>()
      where T : class
      => RegisterType(typeof(T));

    public IReadOnlyCollection<IServiceRegistration> GetRegistrations() => registrations.ToArray();

    public RegistrationHelper()
    {
      this.registrations = new List<IServiceRegistration>();
    }
  }
}
