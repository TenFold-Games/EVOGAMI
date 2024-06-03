using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace EVOGAMI.UI
{
    public abstract class SubMenuBase : MonoBehaviour
    {
        [Tooltip("The sub-menu to be displayed")] 
        [SerializeField] private GameObject panel;

        [Tooltip("The default element to be selected when the sub-menu is opened.")] 
        [SerializeField] private Selectable defaultElement;

        protected Selectable PreviousElement;

        public bool IsActive { get; private set; }

        /// <summary>
        ///     Enable the sub-menu
        /// </summary>
        /// <param name="previousElement">The element used to open the menu</param>
        public virtual void Enable(Selectable previousElement)
        {
            // Activate the panel
            IsActive = true;
            panel.SetActive(IsActive);
            // Set selectables
            PreviousElement = previousElement;
            defaultElement.Select();
        }

        /// <summary>
        ///     Disable the sub-menu
        /// </summary>
        public virtual void Disable()
        {
            // Deactivate the panel
            IsActive = false;
            panel.SetActive(IsActive);
            // Set selectables
            if (PreviousElement) PreviousElement.Select();
            PreviousElement = null;
        }
    }
}