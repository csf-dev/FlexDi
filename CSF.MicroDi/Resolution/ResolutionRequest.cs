using System;
namespace CSF.MicroDi.Resolution
{
  public class ResolutionRequest
  {
    public Type ServiceType { get; private set; }

    public string Name { get; private set; }

    public ResolutionRequest WithoutName()
    {
      return new ResolutionRequest(ServiceType, null);
    }

    public ResolutionRequest(Type serviceType, string name)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      ServiceType = serviceType;
      Name = name;
    }
  }
}
