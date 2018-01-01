using System;
namespace CSF.MicroDi.Builders
{
  public interface IRegistrationOptionsBuilderWithMultiplicity
  {
    IRegistrationOptionsBuilderWithMultiplicity WithName(string name);

    IRegistrationOptionsBuilderWithMultiplicity SeparateInstancePerResolution();

    IRegistrationOptionsBuilderWithMultiplicity SingleSharedInstance();
  }
}
