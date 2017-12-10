using System;
namespace CSF.MicroDi.Builders
{
  public class ContainerBuilder : IContainerBuilder
  {
    bool useNonPublicConstructors;

    public IContainerBuilder DoNotUseNonPublicConstructors()
    {
      useNonPublicConstructors = false;
      return this;
    }

    public IContainerBuilder UseNonPublicConstructors(bool useNonPublicConstructors = true)
    {
      this.useNonPublicConstructors = useNonPublicConstructors;
      return this;
    }

    public Container Build()
    {
      return new Container(options: GetContainerOptions());
    }

    ContainerOptions GetContainerOptions()
    {
      return new ContainerOptions(useNonPublicConstructors);
    }

    public ContainerBuilder()
    {
      useNonPublicConstructors = false;
    }
  }
}
