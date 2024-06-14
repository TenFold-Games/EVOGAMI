namespace EVOGAMI.Movement.CheckProvider
{
    /// <summary>
    ///     Interface for all check providers.
    /// </summary>
    public interface ICheckProvider
    {
        /// <summary>
        ///     Check if the condition is met.
        /// </summary>
        /// <returns>True if the condition is met, false otherwise.</returns>
        public bool Check();
    }
}