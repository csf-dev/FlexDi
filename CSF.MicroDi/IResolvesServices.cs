using System;
namespace CSF.MicroDi
{
  public interface IResolvesServices
  {
    T Resolve<T>();
    T Resolve<T>(string name);

    bool TryResolve<T>(out T output);
    bool TryResolve<T>(string name, out T output);

    object Resolve(Type serviceType);
    object Resolve(Type serviceType, string name);

    bool TryResolve(Type serviceType, out object output);
    bool TryResolve(Type serviceType, string name, out object output);
  }
}
