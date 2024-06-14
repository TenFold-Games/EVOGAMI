using UnityEngine;

namespace EVOGAMI.Movement.CheckProvider
{
    /// <summary>
    ///     Base class for all check providers.
    /// </summary>
    public abstract class CheckProviderBase :
        MonoBehaviour,
        ICheckProvider
    {
        // The transform from which the check is performed.
        [SerializeField] [Tooltip("The transform from which the check is performed.")]
        protected Transform checkTransform;
        // The layer mask to check against.
        [SerializeField] [Tooltip("The layer mask to check against.")]
        protected LayerMask checkLayer;
        
        public bool IsCheckTrue { get; protected set; }

        public RaycastHit CheckHit = new();

        protected virtual void FixedUpdate()
        {
            Check();
        }

        public abstract bool Check();
    }
}