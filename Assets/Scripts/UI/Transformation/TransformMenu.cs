using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.UI.Transformation
{
    public class TransformMenu : MonoBehaviour
    {
        // The transform panel.
        [SerializeField] [Tooltip("The transform panel.")]
        private TransformPanel panel;

        #region Unity Functions

        private void Start()
        {
            InputManager.Instance.OnTransformPerformed += OnTransformPerformed;
        }

        #endregion

        #region Input Events

        private void OnTransformPerformed()
        {
            panel.Toggle();
        }

        #endregion
    }
}