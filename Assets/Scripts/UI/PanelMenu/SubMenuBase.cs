using EVOGAMI.UI.Common;
using UnityEngine;

namespace EVOGAMI.UI.PanelMenu
{
    public abstract class SubMenuBase : MonoBehaviour
    {
        // The sub-menu to be displayed
        [SerializeField] [Tooltip("The sub-menu to be displayed")]
        protected GameObject panel;
        // The default element to be selected when the sub-menu is opened
        [SerializeField] [Tooltip("The default element to be selected when the sub-menu is opened.")]
        protected MenuButton defaultButton;
        
        public delegate void SubMenuEvent();
        public SubMenuEvent EnabledCallback = delegate { };
        public SubMenuEvent DisabledCallback = delegate { };

        public bool IsActive { get; private set; }

        /// <summary>
        ///     Enable the sub-menu
        /// </summary>
        public virtual void OnEnable()
        {
            // Activate the panel
            IsActive = true;
            panel.SetActive(IsActive);
            
            EnabledCallback();

            // Set selectables
            defaultButton.Select(false);
        }

        /// <summary>
        ///     Disable the sub-menu
        /// </summary>
        public virtual void OnDisable()
        {
            // Set selectables
            defaultButton.Select();
            
            DisabledCallback();

            // Deactivate the panel
            IsActive = false;
            panel.SetActive(IsActive);
        }
    }
}