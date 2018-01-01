using System;
namespace CSF.MicroDi.Builders
{
  public interface IRegistrationOptionsBuilder
  {
    IRegistrationOptionsBuilder WithName(string name);

    IRegistrationOptionsBuilder DoNotDisposeWithContainer();

    IRegistrationOptionsBuilder DisposeWithContainer(bool disposeWithContainer = true);
  }
}
