using System;
using System.Collections.Generic;

namespace CSF.MicroDi
{
  public interface IResolvesServices
  {
    T Resolve<T>();
    T Resolve<T>(string name);
    object Resolve(Type serviceType);
    object Resolve(Type serviceType, string name);

    bool TryResolve<T>(out T output);
    bool TryResolve<T>(string name, out T output);
    bool TryResolve(Type serviceType, out object output);
    bool TryResolve(Type serviceType, string name, out object output);

    IReadOnlyCollection<T> ResolveAll<T>();
    IReadOnlyCollection<object> ResolveAll(Type serviceType);
  }
}
