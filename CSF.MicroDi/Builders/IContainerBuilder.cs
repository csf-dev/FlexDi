using System;
namespace CSF.MicroDi.Builders
{
  public interface IContainerBuilder
  {
    IContainerBuilder DoNotUseNonPublicConstructors();

    IContainerBuilder UseNonPublicConstructors(bool useNonPublicConstructors = true);

    Container Build();
  }
}
