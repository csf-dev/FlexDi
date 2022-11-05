namespace CSF.FlexDi.Resolution
{
    /// <summary>
    /// Default implementation of <see cref="ICreatesInstanceCache"/>.
    /// </summary>
    public class InstanceCacheFactory : ICreatesInstanceCache
    {
        /// <inheritdoc/>
        public ICachesResolvedServiceInstances GetCache() => new ResolvedServiceCache();
    }
}