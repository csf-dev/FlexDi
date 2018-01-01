using System;
using CSF.MicroDi.Resolution;

namespace CSF.MicroDi.Registration
{
  public class OpenGenericTypeRegistration : TypeRegistration
  {
    public override void AssertIsValid()
    {
      if(!IsAssignableToGenericType(ImplementationType, ServiceType))
        throw new InvalidTypeRegistrationException($"Invalid {nameof(OpenGenericTypeRegistration)}; the implementation type: `{ImplementationType.FullName}' must derive from the service type: `{ServiceType.FullName}'.");
    }

    // From https://stackoverflow.com/questions/74616/how-to-detect-if-type-is-another-generic-type/1075059#1075059
    bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
      var interfaceTypes = givenType.GetInterfaces();

      foreach (var it in interfaceTypes)
      {
        if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
          return true;
      }

      if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        return true;

      Type baseType = givenType.BaseType;
      if (baseType == null) return false;

      return IsAssignableToGenericType(baseType, genericType);
    }

    public override IFactoryAdapter GetFactoryAdapter(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      if(!request.ServiceType.IsGenericType)
        throw new ArgumentException($"The request must be for a generic type; request type: {request.ServiceType.FullName}", nameof(request));

      var requestedGenericTypeArgs = request.ServiceType.GetGenericArguments();
      var implementationTypeWithGenericParams = ImplementationType.MakeGenericType(requestedGenericTypeArgs);

      return GetFactoryAdapter(implementationTypeWithGenericParams);
    }

    public override bool MatchesKey(ServiceRegistrationKey key)
    {
      if(key == null)
        return false;

      if(!key.ServiceType.IsGenericType)
        return false;

      var openGenericKeyType = key.ServiceType.GetGenericTypeDefinition();
      return openGenericKeyType == ServiceType;
    }

    public OpenGenericTypeRegistration(Type implementationType)
      : this(implementationType, null) {}

    public OpenGenericTypeRegistration(Type implementationType, ISelectsConstructor constructorSelector)
      : base( implementationType, constructorSelector){}
  }
}
