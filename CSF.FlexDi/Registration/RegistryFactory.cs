namespace CSF.FlexDi.Registration
{
    /// <summary>
    /// Default implementation of <see cref="ICreatesRegistry"/>.
    /// </summary>
    public class RegistryFactory : ICreatesRegistry
    {
        /// <inheritdoc/>
        public IRegistersServices GetRegistry() => new Registry();
    }
}