//
//    ObjectContainer.cs
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
using BoDi.Internal;
using CSF.MicroDi;
using CSF.MicroDi.Registration;

namespace BoDi
{
  public class ObjectContainer : IObjectContainer
  {
    static readonly ExceptionTransformer exceptionTransformer;
    static readonly BoDiMicroDiContainerFactory containerFactory;

    protected IContainer container;
    bool isDisposed;

    public event Action<object> ObjectCreated;

    internal IContainer GetMicroDiContainer() => container;

    public void RegisterTypeAs<TInterface>(Type implementationType, string name = null) where TInterface : class
    {
      RegisterTypeAs(implementationType, typeof(TInterface), name);
    }

    public void RegisterTypeAs<TType, TInterface>(string name = null) where TType : class, TInterface
    {
      RegisterTypeAs(typeof(TType), typeof(TInterface), name);
    }

    public void RegisterTypeAs(Type implementationType, Type interfaceType)
    {
      RegisterTypeAs(implementationType, interfaceType, null);
    }

    void RegisterTypeAs(Type implementationType, Type interfaceType, string name)
    {
      exceptionTransformer.TransformExceptions(() => {
        container.AddRegistrations(x => {
          x.RegisterType(implementationType)
           .As(interfaceType)
           .WithName(name);
        });
      });
    }

    public void RegisterInstanceAs(object instance, Type interfaceType, string name = null, bool dispose = false)
    {
      exceptionTransformer.TransformExceptions(() => {
        container.AddRegistrations(x => {
          x.RegisterInstance(instance)
           .As(interfaceType)
           .WithName(name)
           .DisposeWithContainer(dispose);
        });
      });
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
      exceptionTransformer.TransformExceptions(() => {
        container.AddRegistrations(x => {
          x.RegisterFactory(factoryDelegate, interfaceType)
           .WithName(name);
        });
      });
    }

    public bool IsRegistered<T>()
    {
      return IsRegistered<T>(null);
    }

    public bool IsRegistered<T>(string name)
    {
      return exceptionTransformer.TransformExceptions(() => {
        return container.HasRegistration<T>(name);
      });
    }

    #if !BODI_LIMITEDRUNTIME && !BODI_DISABLECONFIGFILESUPPORT

    public void RegisterFromConfiguration()
    {
      var section = (BoDiConfigurationSection) System.Configuration.ConfigurationManager.GetSection("boDi");
      if (section == null)
        return;

      RegisterFromConfiguration(section.Registrations);
    }

    public void RegisterFromConfiguration(ContainerRegistrationCollection containerRegistrationCollection)
    {
      if (containerRegistrationCollection == null)
        return;

      foreach (ContainerRegistrationConfigElement registrationConfigElement in containerRegistrationCollection)
      {
        RegisterFromConfiguration(registrationConfigElement);
      }
    }

    void RegisterFromConfiguration(ContainerRegistrationConfigElement registrationConfigElement)
    {
      var interfaceType = Type.GetType(registrationConfigElement.Interface, true);
      var implementationType = Type.GetType(registrationConfigElement.Implementation, true);
      var name = string.IsNullOrEmpty(registrationConfigElement.Name) ? null : registrationConfigElement.Name;

      RegisterTypeAs(implementationType, interfaceType, name);
    }

    #endif

    public T Resolve<T>()
    {
      return Resolve<T>(null);
    }

    public T Resolve<T>(string name)
    {
      return exceptionTransformer.TransformExceptions(() => {
        return container.Resolve<T>(name);
      });
    }

    public object Resolve(Type typeToResolve, string name = null)
    {
      return exceptionTransformer.TransformExceptions(() => {
        return container.Resolve(typeToResolve, name);
      });
    }

    public IEnumerable<T> ResolveAll<T>() where T : class
    {
      return exceptionTransformer.TransformExceptions(() => {
        return container.ResolveAll<T>();
      });
    }

    IEnumerable<T> IObjectContainer.ResolveAll<T>()
    {
      return exceptionTransformer.TransformExceptions(() => {
        return ResolveAll<T>();
      });
    }

    void OnServiceResolved(object sender, ServiceResolutionEventArgs args)
    {
      if(!(args.Registration is TypeRegistration))
        return;
      
      OnObjectCreated(args.Instance);
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

    IContainer CreateMicroDiContainer(ObjectContainer parent)
    {
      if(parent != null)
        return new Container(parentContainer: parent.container);

      return containerFactory.GetContainer();
    }

    IContainer GetMicroDiContainer(ObjectContainer parent)
    {
      var output = CreateMicroDiContainer(parent);
      output.ServiceResolved += OnServiceResolved;
      return output;
    }

    public ObjectContainer(IObjectContainer baseContainer = null) 
    {
      var parent = GetParentObjectContainer(baseContainer);
      container = GetMicroDiContainer(parent);
      RegisterInstanceAs<IObjectContainer>(this);
    }

    static ObjectContainer()
    {
      exceptionTransformer = new ExceptionTransformer();
      containerFactory = new BoDiMicroDiContainerFactory();
    }
  }
}
