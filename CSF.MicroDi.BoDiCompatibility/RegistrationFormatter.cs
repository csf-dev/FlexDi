using System;
using System.Collections.Generic;
using System.Linq;
using CSF.MicroDi.Registration;

namespace BoDi
{
  public class RegistrationFormatter
  {
    public IEnumerable<string> Format(IEnumerable<IServiceRegistration> registrations)
    {
      if(registrations == null)
        throw new ArgumentNullException(nameof(registrations));

      return registrations.Select(x => Format(x));
    }

    public string Format(IServiceRegistration registration)
    {
      if(registration == null)
        throw new ArgumentNullException(nameof(registration));

      var serviceKey = FormatServiceKey(registration);
      var formattedRegistration = FormatRegistration(registration);

      return $"{serviceKey} -> {formattedRegistration}";
    }

    string FormatServiceKey(IServiceRegistration registration)
    {
      var output = registration.ServiceType.FullName;

      if(registration.Name != null)
        output = output + $"('{registration.Name}')";

      return output;
    }

    string FormatRegistration(IServiceRegistration registration)
    {
      if(registration is TypeRegistration)
      {
        return FormatRegistration((TypeRegistration) registration);
      }

      if(registration is FactoryRegistration)
      {
        return FormatRegistration((FactoryRegistration) registration);
      }

      if(registration is InstanceRegistration)
      {
        return FormatRegistration((InstanceRegistration) registration);
      }

      return "[Unsupported registration]";
    }

    string FormatRegistration(TypeRegistration registration)
    {
      return $"Type: {registration.ImplementationType.FullName}";
    }

    string FormatRegistration(FactoryRegistration registration)
    {
      return $"Factory delegate";
    }

    string FormatRegistration(InstanceRegistration registration)
    {
      // Special case for the container itself
      if(registration.ServiceType == typeof(IObjectContainer) && registration.Name == null)
        return "<self>";

      string implementationString;

      try
      {
        implementationString = registration.Implementation.ToString();
      }
      catch(Exception ex)
      {
        implementationString = ex.Message;
      }

      return $"Instance: {implementationString}";
    }
  }
}
