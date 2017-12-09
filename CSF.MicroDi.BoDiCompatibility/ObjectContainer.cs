using System;
using System.Collections.Generic;
using CSF.MicroDi;

namespace BoDi
{
  public class ObjectContainer : IObjectContainer
  {
    const string REGISTERED_NAME_PARAMETER_NAME = "registeredName";
    readonly Container container;
    bool isDisposed;

    public event Action<object> ObjectCreated;

    public void RegisterTypeAs<TInterface>(Type implementationType, string name = null) where TInterface : class
    {
      container.AddRegistrations(x => {
        x.RegisterType(implementationType)
         .As<TInterface>()
         .WithName(name);
      });
    }

    public void RegisterTypeAs<TType, TInterface>(string name = null) where TType : class, TInterface
    {
      container.AddRegistrations(x => {
        x.RegisterType(typeof(TType))
         .As(typeof(TInterface))
         .WithName(name);
      });
    }

    public void RegisterTypeAs(Type implementationType, Type interfaceType)
    {
      container.AddRegistrations(x => {
        x.RegisterType(implementationType)
         .As(interfaceType);
      });
    }

    public void RegisterInstanceAs(object instance, Type interfaceType, string name = null, bool dispose = false)
    {
      container.AddRegistrations(x => {
        x.RegisterInstance(instance)
         .As(interfaceType)
         .WithName(name);
      });


      //if (instance == null)
      //  throw new ArgumentNullException("instance");
      //var registrationKey = new RegistrationKey(interfaceType, name);
      //AssertNotResolved(registrationKey);

      //ClearRegistrations(registrationKey);
      //AddRegistration(registrationKey, new InstanceRegistration(instance));
      //objectPool[new RegistrationKey(instance.GetType(), name)] = GetPoolableInstance(instance, dispose);
    }

    public void RegisterInstanceAs<TInterface>(TInterface instance, string name = null, bool dispose = false) where TInterface : class
    {
      RegisterInstanceAs(instance, typeof(TInterface), name, dispose);
    }

    public void RegisterFactoryAs<TInterface>(Func<TInterface> factoryDelegate, string name = null)
    {
      RegisterFactoryAs(factoryDelegate, typeof(TInterface), name);
    }

    public void RegisterFactoryAs<TInterface>(Func<IObjectContainer, TInterface> factoryDelegate, string name = null)
    {
      RegisterFactoryAs(factoryDelegate, typeof(TInterface), name);
    }

    public void RegisterFactoryAs<TInterface>(Delegate factoryDelegate, string name = null)
    {
      RegisterFactoryAs(factoryDelegate, typeof(TInterface), name);
    }

    public void RegisterFactoryAs(Delegate factoryDelegate, Type interfaceType, string name = null)
    {
      container.AddRegistrations(x => {
        x.RegisterFactory(factoryDelegate, interfaceType)
         .WithName(name);
      });


      //if (factoryDelegate == null) throw new ArgumentNullException("factoryDelegate");
      //if (interfaceType == null) throw new ArgumentNullException("interfaceType");

      //var registrationKey = new RegistrationKey(interfaceType, name);
      //AssertNotResolved(registrationKey);

      //ClearRegistrations(registrationKey);

      //AddRegistration(registrationKey, new FactoryRegistration(factoryDelegate));
    }

    public bool IsRegistered<T>()
    {
      return IsRegistered<T>(null);
    }

    public bool IsRegistered<T>(string name)
    {
      return container.HasRegistration<T>(name);
    }

    #if !BODI_LIMITEDRUNTIME && !BODI_DISABLECONFIGFILESUPPORT
    //public void RegisterFromConfiguration()
    //{
    //  var section = (BoDiConfigurationSection)ConfigurationManager.GetSection("boDi");
    //  if (section == null)
    //    return;

    //  RegisterFromConfiguration(section.Registrations);
    //}

    //public void RegisterFromConfiguration(ContainerRegistrationCollection containerRegistrationCollection)
    //{
    //  if (containerRegistrationCollection == null)
    //    return;

    //  foreach (ContainerRegistrationConfigElement registrationConfigElement in containerRegistrationCollection)
    //  {
    //    RegisterFromConfiguration(registrationConfigElement);
    //  }
    //}

    #endif

    public T Resolve<T>()
    {
      return Resolve<T>(null);
    }

    public T Resolve<T>(string name)
    {
      return container.Resolve<T>(name);
    }

    public object Resolve(Type typeToResolve, string name = null)
    {
      return container.Resolve(typeToResolve, name);
    }

    public IEnumerable<T> ResolveAll<T>() where T : class
    {
      return container.ResolveAll<T>();
    }

    IEnumerable<T> IObjectContainer.ResolveAll<T>()
    {
      return ResolveAll<T>();
    }


    protected virtual void OnObjectCreated(object obj)
    {
      var eventHandler = ObjectCreated;
      if (eventHandler != null)
        eventHandler(obj);
    }

    public override string ToString()
    {
      var formatter = new RegistrationFormatter();
      return string.Join(Environment.NewLine, formatter.Format(container.GetRegistrations()));
    }

    public void Dispose()
    {
      if(isDisposed)
        return;

      container.Dispose();
      isDisposed = true;
    }

    ObjectContainer GetParentObjectContainer(IObjectContainer baseContainer)
    {
      if(baseContainer == null)
        return null;
      
      try
      {
        return (ObjectContainer) baseContainer;
      }
      catch(InvalidCastException ex)
      {
        throw new ArgumentException($"Base container must be an {nameof(ObjectContainer)}", nameof(baseContainer), ex);
      }
    }

    Container CreateBoDiContainer(ObjectContainer parent)
    {
      if(parent == null)
        return new Container();

      return new Container(parent.container);
    }

    public ObjectContainer(IObjectContainer baseContainer = null) 
    {
      var parent = GetParentObjectContainer(baseContainer);
      container = CreateBoDiContainer(parent);
      RegisterInstanceAs<IObjectContainer>(this);
    }
  }
}
