namespace CSF.FlexDi.Resolution
{
    /// <summary>
    /// An object which can create instances of <see cref="ICachesResolvedServiceInstances"/>.
    /// </summary>
    public interface ICreatesInstanceCache
    {
        /// <summary>
        /// Gets an instance cache.
        /// </summary>
        /// <returns>The instance cache.</returns>
        ICachesResolvedServiceInstances GetCache();
    }
}