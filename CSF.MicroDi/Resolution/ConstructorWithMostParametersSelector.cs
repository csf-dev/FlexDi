using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF.MicroDi.Resolution
{
  public class ConstructorWithMostParametersSelector : ISelectsConstructor
  {
    readonly bool includeNonPublicConstructors;

    public ConstructorInfo SelectConstructor(Type type)
    {
      if(type == null)
        throw new ArgumentNullException(nameof(type));

      var allConstructors = GetAllConstructors(type);
      var constructorWithMostParameters = allConstructors
        .OrderByDescending(x => x.GetParameters().Count())
        .FirstOrDefault();
      
      AssertAConstructorIsFound(type, constructorWithMostParameters);
      AssertConstructorIsNotAmbiguous(type, constructorWithMostParameters, allConstructors);

      return constructorWithMostParameters;
    }

    IEnumerable<ConstructorInfo> GetAllConstructors(Type type)
      => type.GetConstructors(GetBindingFlags());

    BindingFlags GetBindingFlags()
    {
      var flags = BindingFlags.Public | BindingFlags.Instance;

      if(includeNonPublicConstructors)
        flags = flags | BindingFlags.NonPublic;

      return flags;
    }

    void AssertAConstructorIsFound(Type type, ConstructorInfo ctor)
    {
      if(ctor == null)
        throw new CannotInstantiateTypeWithoutAnyConstructorsException($"The type {type.FullName} must have at least one constructor.{Environment.NewLine}Interfaces must be registered with a concrete implementation.");
    }

    void AssertConstructorIsNotAmbiguous(Type type,
                                         ConstructorInfo constructorWithMostParameters,
                                         IEnumerable<ConstructorInfo> allConstructors)
    {
      var paramCount = constructorWithMostParameters.GetParameters().Count();
      if(allConstructors.Count(x => x.GetParameters().Count() == paramCount) > 1)
      {
        var parametersText = (paramCount == 1)? "1 parameter" : $"{paramCount} parameters";
        throw new AmbiguousConstructorException($"The type {type.FullName} has multiple constructors with {parametersText}.{Environment.NewLine}" +
                                                "If you wish to register this type, you must use a factory registration and manually choose the appropriate constructor.");
      }
    }

    public ConstructorWithMostParametersSelector() : this(false) {}

    public ConstructorWithMostParametersSelector(bool includeNonPublicConstructors)
    {
      this.includeNonPublicConstructors = includeNonPublicConstructors;
    }
  }
}
