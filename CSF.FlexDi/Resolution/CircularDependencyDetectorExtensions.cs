using System;
using CSF.FlexDi.Registration;

namespace CSF.FlexDi.Resolution
{
    /// <summary>
    /// Extension methods for <see cref="IDetectsCircularDependencies"/>.
    /// </summary>
    public static class CircularDependencyDetectorExtensions
    {
        /// <summary>
        /// Performs the same check as <see cref="IDetectsCircularDependencies.HasCircularDependency" /> but raises an exception
        /// if a circular dependency is found.
        /// </summary>
        /// <param name="detector">The circular dependency detector.</param>
        /// <param name="registration">The registration to find.</param>
        /// <param name="resolutionPath">A resolution path.</param>
        public static void ThrowOnCircularDependency(this IDetectsCircularDependencies detector, IServiceRegistration registration, ResolutionPath resolutionPath)
        {
            if (detector is null)
                throw new ArgumentNullException(nameof(detector));

            if (!detector.HasCircularDependency(registration, resolutionPath)) return;

            throw new CircularDependencyException(Resources.ExceptionFormats.CircularDependencyDetected)
            {
                ResolutionPath = resolutionPath
            };
        }
    }
}