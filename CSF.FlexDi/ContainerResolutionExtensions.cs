using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CSF.FlexDi.Resolution;

namespace CSF.FlexDi
{
    /// <summary>
    /// Extension methods for <see cref="IContainer"/> relating to the resolution of services.
    /// </summary>
    public static class ContainerResolutionExtensions
    {

        /// <summary>
        /// Resolves an instance of the specified type.
        /// </summary>
        /// <param name="container">The <see cref="IResolvesServices"/> instance from which to resolve the service.</param>
        /// <typeparam name="T">The component type to be resolved.</typeparam>
        public static T Resolve<T>(this IResolvesServices container) => container.Resolve<T>(null);

        /// <summary>
        /// Resolves an instance of the specified type, using the given named registration.
        /// </summary>
        /// <param name="container">The <see cref="IResolvesServices"/> instance from which to resolve the service.</param>
        /// <param name="name">The registration name.</param>
        /// <typeparam name="T">The component type to be resolved.</typeparam>
        public static T Resolve<T>(this IResolvesServices container, string name)
        {
            if (!container.TryResolve(typeof(T), name, out var output))
                ThrowResolutionFailureException(typeof(T));
            return (T) output;
        }

        /// <summary>
        /// Attempts to resolve an instance of the specified type, but does not raise an exception if resolution fails.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
        /// <param name="container">The <see cref="IResolvesServices"/> instance from which to resolve the service.</param>
        /// <param name="output">The resolved component instance.</param>
        /// <typeparam name="T">The component type to be resolved.</typeparam>
        public static bool TryResolve<T>(this IResolvesServices container, out T output) => container.TryResolve(null, out output);

        /// <summary>
        /// Attempts to resolve an instance of the specified type and using the given named registration, but
        /// does not raise an exception if resolution fails.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
        /// <param name="container">The <see cref="IResolvesServices"/> instance from which to resolve the service.</param>
        /// <param name="output">The resolved component instance.</param>
        /// <param name="name">The registration name.</param>
        /// <typeparam name="T">The component type to be resolved.</typeparam>
        public static bool TryResolve<T>(this IResolvesServices container, string name, out T output)
        {
            var result = container.TryResolve(typeof(T), name, out var resolved);
            output = result ? (T)resolved : default(T);
            return result;
        }

        /// <summary>
        /// Attempts to resolve an instance of the specified type, but returns a <c>null</c> reference if resolution fails.
        /// </summary>
        /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
        /// <param name="container">The <see cref="IResolvesServices"/> instance from which to resolve the service.</param>
        /// <typeparam name="T">The component type to be resolved.</typeparam>
        public static T TryResolve<T>(this IResolvesServices container) where T : class => container.TryResolve<T>(null);

        /// <summary>
        /// Attempts to resolve an instance of the specified type and using the given named registration, but
        /// returns a <c>null</c> reference if resolution fails.
        /// </summary>
        /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
        /// <param name="container">The <see cref="IResolvesServices"/> instance from which to resolve the service.</param>
        /// <param name="name">The registration name.</param>
        /// <typeparam name="T">The component type to be resolved.</typeparam>
        public static T TryResolve<T>(this IResolvesServices container, string name) where T : class
        {
            if (!container.TryResolve<T>(name, out var output))
                return default(T);

            return output;
        }

        /// <summary>
        /// Attempts to resolve an instance of the specified type, but returns a <c>null</c> reference if resolution fails.
        /// </summary>
        /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
        /// <param name="container">The <see cref="IResolvesServices"/> instance from which to resolve the service.</param>
        /// <param name="serviceType">The component type to be resolved.</param>
        public static object TryResolve(this IResolvesServices container, Type serviceType) => container.TryResolve(serviceType, null);

        /// <summary>
        /// Attempts to resolve an instance of the specified type and using the given named registration, but
        /// returns a <c>null</c> reference if resolution fails.
        /// </summary>
        /// <returns>The resolved component instance, or a <c>null</c> reference if resolution fauls.</returns>
        /// <param name="container">The <see cref="IResolvesServices"/> instance from which to resolve the service.</param>
        /// <param name="name">The registration name.</param>
        /// <param name="serviceType">The component type to be resolved.</param>
        public static object TryResolve(this IResolvesServices container, Type serviceType, string name)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            if (serviceType.GetTypeInfo().IsValueType)
                throw new ArgumentException(Resources.ExceptionFormats.TypeToResolveMustBeNullableReferenceType, nameof(serviceType));

            if (!container.TryResolve(serviceType, name, out var output))
                return null;

            return output;
        }

        /// <summary>
        /// Resolves an instance of the specified type.
        /// </summary>
        /// <param name="container">The <see cref="IResolvesServices"/> instance from which to resolve the service.</param>
        /// <param name="serviceType">The component type to be resolved.</param>
        public static object Resolve(this IResolvesServices container, Type serviceType) => container.Resolve(serviceType, null);

        /// <summary>
        /// Resolves an instance of the specified type, using the given named registration.
        /// </summary>
        /// <param name="container">The <see cref="IResolvesServices"/> instance from which to resolve the service.</param>
        /// <param name="serviceType">The component type to be resolved.</param>
        /// <param name="name">The registration name.</param>
        public static object Resolve(this IResolvesServices container, Type serviceType, string name)
        {
            if (!container.TryResolve(serviceType, name, out var output))
                ThrowResolutionFailureException(serviceType);
            return output;
        }

        /// <summary>
        /// Attempts to resolve an instance of the specified type, but does not raise an exception if resolution fails.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
        /// <param name="container">The <see cref="IResolvesServices"/> instance from which to resolve the service.</param>
        /// <param name="output">The resolved component instance.</param>
        /// <param name="serviceType">The component type to be resolved.</param>
        public static bool TryResolve(this IResolvesServices container, Type serviceType, out object output) => container.TryResolve(serviceType, null, out output);

        /// <summary>
        /// Attempts to resolve an instance of the specified type and using the given named registration, but
        /// does not raise an exception if resolution fails.
        /// </summary>
        /// <returns>
        /// <c>true</c>, if resolution was successful, <c>false</c> otherwise.</returns>
        /// <param name="container">The <see cref="IResolvesServices"/> instance from which to resolve the service.</param>
        /// <param name="output">The resolved component instance.</param>
        /// <param name="name">The registration name.</param>
        /// <param name="serviceType">The component type to be resolved.</param>
        public static bool TryResolve(this IResolvesServices container, Type serviceType, string name, out object output)
        {
            var result = container.TryResolve(new ResolutionRequest(serviceType, name));
            output = result.IsSuccess ? result.ResolvedObject : null;
            return result.IsSuccess;
        }

        /// <summary>
        /// Resolves a component, as specified by a <see cref="T:CSF.FlexDi.Resolution.ResolutionRequest" /> instance.
        /// </summary>
        /// <param name="container">The <see cref="IResolvesServices"/> instance from which to resolve the service.</param>
        /// <param name="request">The resolved component instance.</param>
        public static object Resolve(this IResolvesServices container, ResolutionRequest request)
        {
            var result = container.TryResolve(request);
            if (!result.IsSuccess)
                ThrowResolutionFailureException(request.ServiceType);

            return result.ResolvedObject;
        }

        /// <summary>
        /// Creates a collection which contains resolved instances of all of the components registered for a given type.
        /// </summary>
        /// <returns>A collection of resolved components.</returns>
        /// <param name="container">The <see cref="IContainer"/> instance from which to resolve the services.</param>
        /// <typeparam name="T">The type of the components to be resolved.</typeparam>
        public static IReadOnlyCollection<T> ResolveAll<T>(this IContainer container)
            => container.ResolveAll(typeof(T)).Cast<T>().ToArray();

        /// <summary>
        /// Creates a collection which contains resolved instances of all of the components registered for a given type.
        /// </summary>
        /// <returns>A collection of resolved components.</returns>
        /// <param name="container">The <see cref="IContainer"/> instance from which to resolve the services.</param>
        /// <param name="serviceType">The type of the components to be resolved.</param>
        public static IReadOnlyCollection<object> ResolveAll(this IContainer container, Type serviceType)
        {
            return container.GetRegistrations(serviceType)
              .Select(x => container.Resolve(x.ServiceType, x.Name))
              .ToArray();
        }

        static void ThrowResolutionFailureException(Type componentType)
        {
            var message = String.Format(Resources.ExceptionFormats.CannotResolveComponentType, componentType.FullName);
            throw new ResolutionException(message);
        }
    }
}