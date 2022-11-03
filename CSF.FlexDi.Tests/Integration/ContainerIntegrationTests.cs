using CSF.FlexDi.Builders;
using CSF.FlexDi.Tests.Autofixture;
using CSF.FlexDi.Tests.Stubs;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Integration
{
    [TestFixture,Parallelizable(ParallelScope.Self)]
    public class ContainerIntegrationTests
    {
        ContainerBuilder builder;
        IContainer container;

        [SetUp]
        public void Setup()
        {
            builder = new ContainerBuilder();
            container = builder.Build();
        }

        [Test]
        public void TryResolveGenericNamedRegistrationShouldReturnTrueWhenItExists()
        {
            container.AddRegistrations(r => r.RegisterType<SampleServiceImplementationOne>().As<ISampleService>().WithName("foo"));
            Assert.Multiple(() =>
            {
                Assert.That(container.TryResolve<ISampleService>("foo", out var service), Is.True, "Correct return value");
                Assert.That(service, Is.InstanceOf<SampleServiceImplementationOne>(), "Service is of expected type");
            });
        }

        [Test]
        public void TryResolveGenericNamedRegistrationShouldReturnFalseWhenItDoesNotExist()
        {
            Assert.That(container.TryResolve<ISampleService>("foo", out _), Is.False);
        }

        [Test]
        public void TryResolveTypedNamedRegistrationShouldReturnTrueWhenItExists()
        {
            container.AddRegistrations(r => r.RegisterType<SampleServiceImplementationOne>().As(typeof(ISampleService)).WithName("foo"));
            Assert.Multiple(() =>
            {
                Assert.That(container.TryResolve(typeof(ISampleService), "foo", out var service), Is.True, "Correct return value");
                Assert.That(service, Is.InstanceOf<SampleServiceImplementationOne>(), "Service is of expected type");
            });
        }

        [Test]
        public void TryResolveTypedNamedRegistrationShouldReturnFalseWhenItDoesNotExist()
        {
            Assert.That(container.TryResolve(typeof(ISampleService), "foo", out _), Is.False);
        }

        [Test]
        public void HasRegistrationShouldReturnTrueIfARegistrationExists()
        {
            container.AddRegistrations(r => r.RegisterType<SampleServiceImplementationOne>().As<ISampleService>().WithName("foo"));
            Assert.That(container.HasRegistration<ISampleService>("foo"), Is.True);
        }

        [Test]
        public void HasRegistrationShouldReturnFalseIfARegistrationDoesNotExist()
        {
            Assert.That(container.HasRegistration<ISampleService>("foo"), Is.False);
        }
    }
}