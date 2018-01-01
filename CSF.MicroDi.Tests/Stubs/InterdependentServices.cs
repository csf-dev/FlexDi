using System;
namespace CSF.MicroDi.Tests.Stubs
{
  public class ParentService
  {
    public ChildServiceOne ChildOne { get; set; }
    public ChildServiceTwo ChildTwo { get; set; }

    public ParentService(ChildServiceOne one, ChildServiceTwo two)
    {
      ChildOne = one;
      ChildTwo = two;
    }
  }

  public class ChildServiceOne {}

  public class ChildServiceTwo {}
}
