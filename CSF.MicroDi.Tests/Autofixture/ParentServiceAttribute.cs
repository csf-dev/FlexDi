using System;
using System.Reflection;
using CSF.MicroDi.Registration;
using CSF.MicroDi.Resolution;
using CSF.MicroDi.Tests.Stubs;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.NUnit3;

namespace CSF.MicroDi.Tests.Autofixture
{
  public class ParentServiceAttribute : CustomizeAttribute
  {
    public string Name { get; set; }

    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
      if(parameter.ParameterType != typeof(IServiceRegistration))
        return null;
      
      return new SampleServiceCustomization(Name);
    }

    class SampleServiceCustomization : ICustomization
    {
      readonly string name;

      public void Customize(IFixture fixture)
      {
        fixture.Customize<IServiceRegistration>(c => {
          return c
            .FromFactory(() => Mock.Of<IServiceRegistration>())
            .Do(reg => {
              Mock.Get(reg).SetupGet(x => x.ServiceType).Returns(typeof(ParentService));
              Mock.Get(reg).SetupGet(x => x.Name).Returns(name);
            });
        });
      }

      public SampleServiceCustomization(string name)
      {
        this.name = name;
      }
    }
  }
}
