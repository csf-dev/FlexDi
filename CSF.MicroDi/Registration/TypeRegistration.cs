using System;
using System.Reflection;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public class TypeRegistration : TypedRegistration
  {
    readonly Type implementationType;
    readonly ISelectsConstructor constructorSelector;

    public override Type ImplementationType => implementationType;

    public override object CreateInstance(IResolutionContext context)
    {
      if(context == null)
        throw new ArgumentNullException(nameof(context));
      
      var adapter = GetFactoryAdapter();
      return context.Resolve(adapter);
    }

    IFactoryAdapter GetFactoryAdapter()
      => new ConstructorFactory(constructorSelector.SelectConstructor(implementationType));

    public TypeRegistration(Type implementationType) : this(implementationType, null) {}

    public TypeRegistration(Type implementationType, ISelectsConstructor constructorSelector)
    {
      if(implementationType == null)
        throw new ArgumentNullException(nameof(implementationType));

      this.implementationType = implementationType;
      this.constructorSelector = constructorSelector ?? new ConstructorWithMostParametersSelector();
    }
  }
}
