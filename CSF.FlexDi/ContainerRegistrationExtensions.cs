using System;
using CSF.FlexDi.Builders;
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi
{
    /// <summary>
    /// Extension methods for <see cref="IContainer"/> relating to registration of services.
    /// </summary>
    public static class ContainerRegistrationExtensions
    {
        /// <summary>
        /// Gets a value indicating whether or not the container has a registration for the specified component type.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if the container has a registration for the component type, <c>false</c> otherwise.</returns>
        /// <param name="container">The container within which to look for a registration.</param>
        /// <param name="name">An optional registration name.</param>
        /// <typeparam name="T">The component type for which to check.</typeparam>
        public static bool HasRegistration<T>(this IContainer container, string name = null)
            => container.HasRegistration(typeof(T), name);

        /// <summary>
        /// Adds new component registrations by use of a helper type.  Registrations are added within a callback which
        /// uses functionality from the helper.
        /// </summary>
        /// <seealso cref="IRegistrationHelper"/>
        /// <param name="container">The container to which registrations should be added.</param>
        /// <param name="registrationActions">A callback which may use the functionality of the helper type.</param>
        public static void AddRegistrations(this IReceivesRegistrations container, Action<IRegistrationHelper> registrationActions)
        {
            if (registrationActions == null)
                throw new ArgumentNullException(nameof(registrationActions));

            var constructorSelector = (container is IProvidesResolutionInfo resolutionInfoProvider)
                ? resolutionInfoProvider.ConstructorSelector
                : new ConstructorWithMostParametersSelector();

            var helper = new RegistrationHelper(constructorSelector);
            registrationActions(helper);

            container.AddRegistrations(helper.GetRegistrations());
        }
    }
}