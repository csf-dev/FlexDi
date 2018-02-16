//
//    NamedInstanceDictionaryResolverProxy.cs
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
using System.Collections;
using CSF.FlexDi.Registration;

namespace CSF.FlexDi.Resolution.Proxies
{
  /// <summary>
  /// A proxying resolver which can create named instance dictionaries.
  /// </summary>
  /// <seealso cref="ContainerOptions.SupportResolvingNamedInstanceDictionaries"/>
  public class NamedInstanceDictionaryResolverProxy : ProxyingResolver
  {
    readonly IServiceRegistrationProvider registrationAccessor;
    readonly IDictionaryFactory dictionaryFactory;

    /// <summary>
    /// Resolves the given resolution request and returns the result.
    /// </summary>
    /// <param name="request">Request.</param>
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
      if(!dictionaryFactory.IsGenericDictionaryType(request.ServiceType))
        return false;

      var keyType = dictionaryFactory.GetKeyType(request.ServiceType);
      return (keyType == typeof(string) || keyType.IsEnum);
    }

    ResolutionResult ResolveNamedInstanceDictionary(ResolutionRequest request)
    {
      var keyType = dictionaryFactory.GetKeyType(request.ServiceType);
      var valueType = dictionaryFactory.GetValueType(request.ServiceType);
      var dictionary = dictionaryFactory.Create(keyType, valueType);

      var allServiceTypeRegistrations = registrationAccessor.GetAll(valueType);

      foreach(var registration in allServiceTypeRegistrations)
      {
        var serviceResult = ResolveSingleInstance(registration, request);
        if(!serviceResult.IsSuccess)
        {
          // TODO: Throw an exception? Return a failed resolution result? Currently I'm silently skipping the registration.
          continue;
        }

        var dictionaryKey = ConvertToNameType(registration.Name, keyType);
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

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="T:CSF.FlexDi.Resolution.Proxies.NamedInstanceDictionaryResolverProxy"/> class.
    /// </summary>
    /// <param name="proxiedResolver">Proxied resolver.</param>
    /// <param name="registrationAccessor">Registration accessor.</param>
    /// <param name="dictionaryFactory">Dictionary factory.</param>
    public NamedInstanceDictionaryResolverProxy(IResolver proxiedResolver,
                                                IServiceRegistrationProvider registrationAccessor,
                                                IDictionaryFactory dictionaryFactory) : base(proxiedResolver)
    {
      if(dictionaryFactory == null)
        throw new ArgumentNullException(nameof(dictionaryFactory));
      if(registrationAccessor == null)
        throw new ArgumentNullException(nameof(registrationAccessor));

      this.registrationAccessor = registrationAccessor;
      this.dictionaryFactory = dictionaryFactory;
    }
  }
}
