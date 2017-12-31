using System;
namespace CSF.MicroDi.Resolution
{
  public class ResolutionRequest
  {
    public Type ServiceType { get; private set; }

    public string Name { get; private set; }

    public ResolutionPath ResolutionPath { get; private set; }

    public ResolutionRequest GetCopyWithoutName()
    {
      return new ResolutionRequest(ServiceType, null);
    }

    public override string ToString()
    {
      var namePart = (Name != null)? $"('{Name}')" : string.Empty;
      return $"[ResolutionRequest: {ServiceType.FullName}{namePart}]";
    }

    public ResolutionRequest(Type serviceType, ResolutionPath resolutionPath = null)
      : this(serviceType, null, resolutionPath) {}

    public ResolutionRequest(Type serviceType, string name, ResolutionPath resolutionPath = null)
    {
      if(serviceType == null)
        throw new ArgumentNullException(nameof(serviceType));

      ServiceType = serviceType;
      Name = name;
      ResolutionPath = resolutionPath ?? new ResolutionPath();
    }
  }
}
