using System;
using System.Collections;
using System.Collections.Generic;
using CSF.MicroDi.Registration;

namespace CSF.MicroDi.Resolution
{
  public class NamedInstanceDictionaryResolverProxy : ProxyingResolver
  {
    static readonly Type
      DictionaryOpenGenericInterface = typeof(IDictionary<,>),
      DictionaryOpenGenericType = typeof(Dictionary<,>);
    readonly IServiceRegistrationProvider registrationAccessor;

    public override ResolutionResult Resolve(ResolutionRequest request)
    {
      if(request == null)
        throw new ArgumentNullException(nameof(request));

      if(!IsRequestForNamedInstanceDictionary(request))
        return ProxiedResolver.Resolve(request);
      
      return ResolveNamedInstanceDictionary(request);
    }

    bool IsRequestForNamedInstanceDictionary(ResolutionRequest request)
    {
      if(!request.ServiceType.IsGenericType)
        return false;
      
      var requestGenericTypeDefinition = request.ServiceType.GetGenericTypeDefinition();
      if(requestGenericTypeDefinition != DictionaryOpenGenericInterface)
        return false;

      var keyType = request.ServiceType.GetGenericArguments()[0];
      return (keyType == typeof(string) || keyType.IsEnum);
    }

    ResolutionResult ResolveNamedInstanceDictionary(ResolutionRequest request)
    {
      var genericArgs = request.ServiceType.GetGenericArguments();
      var nameType = genericArgs[0];
      var serviceType = genericArgs[1];

      var allServiceTypeRegistrations = registrationAccessor.GetAll(serviceType);
      var dictionary = CreateDictionary(nameType, serviceType);

      foreach(var registration in allServiceTypeRegistrations)
      {
        var serviceResult = ResolveSingleInstance(registration, request);
        if(!serviceResult.IsSuccess)
        {
          // TODO: Throw an exception? Return a failed resolution result? Currently I'm silently skipping the registration.
          continue;
        }

        var dictionaryKey = ConvertToNameType(registration.Name, nameType);
        dictionary.Add(dictionaryKey, serviceResult.ResolvedObject);
      }

      var dictionaryRegistration = CreateNamedInstanceDictionaryRegistration(request.ServiceType, dictionary);
      var resolutionPath = request.ResolutionPath.CreateChild(dictionaryRegistration);
      return ResolutionResult.Success(resolutionPath, dictionary);
    }

    ResolutionResult ResolveSingleInstance(IServiceRegistration registration, ResolutionRequest request)
    {
      var subRequest = new ResolutionRequest(registration.ServiceType, registration.Name, request.ResolutionPath);
      return ProxiedResolver.Resolve(subRequest);
    }

    IDictionary CreateDictionary(Type nameType, Type serviceType)
    {
      var dictionaryConcreteType = DictionaryOpenGenericType.MakeGenericType(nameType, serviceType);
      var dictionaryInstance = Activator.CreateInstance(dictionaryConcreteType);
      return (IDictionary) dictionaryInstance;
    }

    object ConvertToNameType(string registeredName, Type nameType)
    {
      if(nameType == typeof(string))
        return registeredName;

      try
      {
        return Enum.Parse(nameType, registeredName, true);
      }
      catch(ArgumentException ex)
      {
        var message = $"There must be a value in the enumeration `{nameType.FullName}' which matches the name '{registeredName}'.";
        throw new NoMatchingEnumerationConstantException(message, ex);
      }
    }

    IServiceRegistration CreateNamedInstanceDictionaryRegistration(Type serviceType, IDictionary dictionary)
      => new InstanceRegistration(dictionary) { ServiceType = serviceType };

    public NamedInstanceDictionaryResolverProxy(IResolver proxiedResolver,
                                                IServiceRegistrationProvider registrationAccessor) : base(proxiedResolver)
    {
      if(registrationAccessor == null)
        throw new ArgumentNullException(nameof(registrationAccessor));

      this.registrationAccessor = registrationAccessor;
    }
  }
}
