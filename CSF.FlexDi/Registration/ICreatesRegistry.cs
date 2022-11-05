namespace CSF.FlexDi.Registration
{
    /// <summary>
    /// An object which can create instances of <see cref="IRegistersServices"/>.
    /// </summary>
    public interface ICreatesRegistry
    {
        /// <summary>
        /// Creates an instance of <see cref="IRegistersServices"/>.
        /// </summary>
        /// <returns>A registry instance.</returns>
        IRegistersServices GetRegistry();
    }
}