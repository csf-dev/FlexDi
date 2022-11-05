using CSF.FlexDi.Builders;
using CSF.FlexDi.Registration;
using CSF.FlexDi.Resolution;
using CSF.FlexDi.Tests.Autofixture;
using NUnit.Framework;

namespace CSF.FlexDi.Tests.Builders
{
    [TestFixture,Parallelizable]
    public class ContainerBuilderTests
    {
        [Test,AutoMoqData]
        public void UseNonPublicConstructorsShouldSetAppropriateValueInConfig(ContainerBuilder sut, bool value)
        {
            sut.UseNonPublicConstructors(value);
            Assert.That(() => sut.BuildContainerOptions()?.UseNonPublicConstructors, Is.EqualTo(value));
        }

        [Test,AutoMoqData]
        public void ResolveUnregisteredTypesShouldSetAppropriateValueInConfig(ContainerBuilder sut, bool value)
        {
            sut.ResolveUnregisteredTypes(value);
            Assert.That(() => sut.BuildContainerOptions()?.ResolveUnregisteredTypes, Is.EqualTo(value));
        }

        [Test,AutoMoqData]
        public void UseInstanceCacheShouldSetAppropriateValueInConfig(ContainerBuilder sut, bool value)
        {
            sut.UseInstanceCache(value);
            Assert.That(() => sut.BuildContainerOptions()?.UseInstanceCache, Is.EqualTo(value));
        }

        [Test,AutoMoqData]
        public void SelfRegisterAResolverShouldSetAppropriateValueInConfig(ContainerBuilder sut, bool value)
        {
            sut.SelfRegisterAResolver(value);
            Assert.That(() => sut.BuildContainerOptions()?.SelfRegisterAResolver, Is.EqualTo(value));
        }

        [Test,AutoMoqData]
        public void SelfRegisterTheRegistryShouldSetAppropriateValueInConfig(ContainerBuilder sut, bool value)
        {
            sut.SelfRegisterTheRegistry(value);
            Assert.That(() => sut.BuildContainerOptions()?.SelfRegisterTheRegistry, Is.EqualTo(value));
        }

        [Test,AutoMoqData]
        public void ThrowOnCircularDependenciesShouldSetAppropriateValueInConfig(ContainerBuilder sut, bool value)
        {
            sut.ThrowOnCircularDependencies(value);
            Assert.That(() => sut.BuildContainerOptions()?.ThrowOnCircularDependencies, Is.EqualTo(value));
        }

        [Test,AutoMoqData]
        public void SupportResolvingNamedInstanceDictionariesShouldSetAppropriateValueInConfig(ContainerBuilder sut, bool value)
        {
            sut.SupportResolvingNamedInstanceDictionaries(value);
            Assert.That(() => sut.BuildContainerOptions()?.SupportResolvingNamedInstanceDictionaries, Is.EqualTo(value));
        }

        [Test,AutoMoqData]
        public void SupportResolvingLazyInstancesShouldSetAppropriateValueInConfig(ContainerBuilder sut, bool value)
        {
            sut.SupportResolvingLazyInstances(value);
            Assert.That(() => sut.BuildContainerOptions()?.SupportResolvingLazyInstances, Is.EqualTo(value));
        }

        [Test,AutoMoqData]
        public void MakeAllResolutionOptionalShouldSetAppropriateValueInConfig(ContainerBuilder sut, bool value)
        {
            sut.MakeAllResolutionOptional(value);
            Assert.That(() => sut.BuildContainerOptions()?.MakeAllResolutionOptional, Is.EqualTo(value));
        }

        [Test,AutoMoqData]
        public void UseCustomResolverFactoryShouldAddThatObjectToTheOptions(ContainerBuilder sut, ICreatesResolvers value)
        {
            sut.UseCustomResolverFactory(value);
            Assert.That(() => sut.BuildContainerOptions()?.ResolverFactory, Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void UseCustomRegistryFactoryShouldAddThatObjectToTheOptions(ContainerBuilder sut, ICreatesRegistry value)
        {
            sut.UseCustomRegistryFactory(value);
            Assert.That(() => sut.BuildContainerOptions()?.RegistryFactory, Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void UseCustomCacheFactoryShouldAddThatObjectToTheOptions(ContainerBuilder sut, ICreatesInstanceCache value)
        {
            sut.UseCustomCacheFactory(value);
            Assert.That(() => sut.BuildContainerOptions()?.CacheFactory, Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void UseCustomResolverShouldAddThatObjectToTheOptions(ContainerBuilder sut, IFulfilsResolutionRequests value)
        {
            sut.UseCustomResolver(value);
            Assert.That(() => sut.BuildContainerOptions()?.Resolver, Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void UseCustomDisposerShouldAddThatObjectToTheOptions(ContainerBuilder sut, IDisposesOfResolvedInstances value)
        {
            sut.UseCustomDisposer(value);
            Assert.That(() => sut.BuildContainerOptions()?.Disposer, Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void UseCustomConstructorSelectorShouldAddThatObjectToTheOptions(ContainerBuilder sut, ISelectsConstructor value)
        {
            sut.UseCustomConstructorSelector(value);
            Assert.That(() => sut.BuildContainerOptions()?.ConstructorSelector, Is.SameAs(value));
        }

        [Test,AutoMoqData]
        public void UseNonPublicConstructorsAfterBuildingOptionsShouldThrow(ContainerBuilder sut, bool value)
        {
            sut.BuildContainerOptions();
            Assert.That(() => sut.UseNonPublicConstructors(value), Throws.InvalidOperationException);
        }

        [Test,AutoMoqData]
        public void BuildContainerOptionsAfterAlreadyBuildingOptionsShouldThrow(ContainerBuilder sut)
        {
            sut.BuildContainerOptions();
            Assert.That(() => sut.BuildContainerOptions(), Throws.InvalidOperationException);
        }

    }
}