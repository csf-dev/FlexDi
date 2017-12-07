using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CSF.MicroDi.Resolution
{
  public class ConstructorWithMostParametersSelector : ISelectsConstructor
  {
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
      => type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

    void AssertAConstructorIsFound(Type type, ConstructorInfo ctor)
    {
      if(ctor == null)
        throw new ArgumentException($"The type {type.FullName} must have at least one constructor.", nameof(type));
    }

    void AssertConstructorIsNotAmbiguous(Type type,
                                         ConstructorInfo constructorWithMostParameters,
                                         IEnumerable<ConstructorInfo> allConstructors)
    {
      var paramCount = constructorWithMostParameters.GetParameters().Count();
      if(allConstructors.Count(x => x.GetParameters().Count() == paramCount) > 1)
      {
        throw new ArgumentException($"The type {type.FullName} has multiple constructors with {paramCount} parameter(s).{Environment.NewLine}" +
                                    "The type must have only a single constructor which has the greatest number of parameters.", nameof(type));
      }
    }
  }
}
