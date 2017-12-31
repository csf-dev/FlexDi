using System;
namespace CSF.MicroDi.Resolution
{
  public class ResolutionResult
  {
    public bool IsSuccess { get; }

    public ResolutionPath ResolutionPath { get; }

    public object ResolvedObject { get; }

    public ResolutionResult(bool success, ResolutionPath resolutionPath, object resolvedObject)
    {
      if(resolutionPath == null)
        throw new ArgumentNullException(nameof(resolutionPath));

      IsSuccess = success;
      ResolvedObject = resolvedObject;
    }

    public static ResolutionResult Failure(ResolutionPath resolutionPath)
      => new ResolutionResult(false, resolutionPath, null);

    public static ResolutionResult Success(ResolutionPath resolutionPath, object resolvedObject)
      => new ResolutionResult(true, resolutionPath, resolvedObject);
  }
}
