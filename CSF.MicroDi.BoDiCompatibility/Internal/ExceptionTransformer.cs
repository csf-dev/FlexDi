using System;
using System.Linq;
using CSF.MicroDi;
using CSF.MicroDi.Registration;

namespace BoDi.Internal
{
  class ExceptionTransformer
  {
    internal void TransformExceptions(Action action)
    {
      if(action == null)
        throw new ArgumentNullException(nameof(action));

      try
      {
        action();
      }
      catch(InvalidTypeRegistrationException ex)
      {
        throw new InvalidOperationException(ex.Message, ex);
      }
      catch(ContainerException ex)
      {
        throw new ObjectContainerException(ex.Message, GetResolutionPath(ex), ex);
      }
    }

    internal T TransformExceptions<T>(Func<T> action)
    {
      if(action == null)
        throw new ArgumentNullException(nameof(action));

      try
      {
        return action();
      }
      catch(InvalidTypeRegistrationException ex)
      {
        throw new InvalidOperationException(ex.Message, ex);
      }
      catch(ContainerException ex)
      {
        throw new ObjectContainerException(ex.Message, GetResolutionPath(ex), ex);
      }
    }

    Type[] GetResolutionPath(ContainerException ex)
    {
      if(ex.ResolutionPath != null)
        return ex.ResolutionPath.GetRegistrations().Select(x => x.ServiceType).ToArray();

      return null;
    }
  }
}
