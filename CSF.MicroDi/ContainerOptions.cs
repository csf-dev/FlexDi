using System;
namespace CSF.MicroDi
{
  public class ContainerOptions
  {
    static readonly ContainerOptions defaultOptions;

    public bool UseNonPublicConstructors { get; private set; }

    public ContainerOptions(bool useNonPublicConstructors = false)
    {
      UseNonPublicConstructors = useNonPublicConstructors;
    }

    static ContainerOptions()
    {
      defaultOptions = new ContainerOptions();
    }

    public static ContainerOptions Default => defaultOptions;
  }
}
