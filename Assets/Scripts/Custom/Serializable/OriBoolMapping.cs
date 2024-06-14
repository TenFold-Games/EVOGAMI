using System;
using EVOGAMI.Origami;

namespace EVOGAMI.Custom.Serializable
{
    /// <summary>
    ///     Serializable class that maps an Origami form to a boolean.
    /// </summary>
    [Serializable]
    public class OriBoolMapping
    {
        public OrigamiContainer.OrigamiForm form;
        public bool isUnlocked;

        /// <summary>
        ///     Deconstructs the OriBoolMapping into its components.
        /// </summary>
        /// <param name="origamiForm">The form of the origami.</param>
        /// <param name="unlocked">The unlock state of the origami.</param>
        public void Deconstruct(out OrigamiContainer.OrigamiForm origamiForm, out bool unlocked)
        {
            origamiForm = form;
            unlocked = isUnlocked;
        }
    }
}