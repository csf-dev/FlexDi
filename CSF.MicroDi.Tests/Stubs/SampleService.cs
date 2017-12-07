using System;
namespace CSF.MicroDi.Tests.Stubs
{
  public interface ISampleService {}

  public class SampleServiceImplementationOne : ISampleService {}

  public class SampleServiceImplementationTwo : ISampleService {}

  public class SampleServiceWithConstructorParameters : ISampleService
  {
    public string StringOne { get; set; }
    public string StringTwo { get; set; }
    public int IntegerOne { get; set; }

    public SampleServiceWithConstructorParameters(string one, string two)
    {
      StringOne = one;
      StringTwo = two;
    }

    public SampleServiceWithConstructorParameters(string one, string two, int intOne)
    {
      StringOne = one;
      StringTwo = two;
      IntegerOne = intOne;
    }
  }

  public class SampleServiceWithAmbiguousCtors : ISampleService
  {
    public string StringOne { get; set; }
    public string StringTwo { get; set; }
    public int IntegerOne { get; set; }

    public SampleServiceWithAmbiguousCtors(string one, string two)
    {
      StringOne = one;
      StringTwo = two;
    }

    public SampleServiceWithAmbiguousCtors(string one, int intOne)
    {
      StringOne = one;
      IntegerOne = intOne;
    }

    SampleServiceWithAmbiguousCtors(string one, string two, int intOne)
    {
      StringOne = one;
      StringTwo = two;
      IntegerOne = intOne;
    }
  }

  public class SampleServiceWithOnlyPrivateConstructors : ISampleService
  {
    SampleServiceWithOnlyPrivateConstructors() {}
  }
}
