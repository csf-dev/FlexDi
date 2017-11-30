using System;
namespace CSF.MicroDi.Registration
{
  public interface IRegistrationOptionsBuilderWithMultiplicity
  {
    IRegistrationOptionsBuilderWithMultiplicity WithName(string name);

    IRegistrationOptionsBuilderWithMultiplicity SeparateInstancePerResolution();

    IRegistrationOptionsBuilderWithMultiplicity SingleSharedInstance();
  }
}
