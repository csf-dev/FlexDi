﻿using System;
namespace CSF.MicroDi
{
  public class ContainerOptions
  {
    static readonly ContainerOptions defaultOptions;

    public bool UseNonPublicConstructors { get; private set; }
    public bool ResolveUnregisteredTypes { get; private set; }
    public bool UseInstanceCache { get; private set; }
    public bool ThrowOnCircularDependencies { get; private set; }
    public bool SupportResolvingNamedInstanceDictionaries { get; private set; }

    public ContainerOptions(bool useNonPublicConstructors = false,
                            bool resolveUnregisteredTypes = false,
                            bool useInstanceCache = true,
                            bool throwOnCircularDependencies = true,
                            bool supportResolvingNamedInstanceDictionaries = false)
    {
      UseNonPublicConstructors = useNonPublicConstructors;
      ResolveUnregisteredTypes = resolveUnregisteredTypes;
      UseInstanceCache = useInstanceCache;
      ThrowOnCircularDependencies = throwOnCircularDependencies;
      SupportResolvingNamedInstanceDictionaries = supportResolvingNamedInstanceDictionaries;
    }

    static ContainerOptions()
    {
      defaultOptions = new ContainerOptions();
    }

    public static ContainerOptions Default => defaultOptions;
  }
}