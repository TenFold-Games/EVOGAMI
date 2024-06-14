namespace EVOGAMI.Animations
{
    /// <summary>
    ///     Interface for handling animations
    /// </summary>
    public interface IAnimationHandler
    {
        /// <summary>
        ///     Set the parameters for the animator
        /// </summary>
        /// <param name="delta">The delta time</param>
        public void SetAnimationParams(float delta);
    }
}