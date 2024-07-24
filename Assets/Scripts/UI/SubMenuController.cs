using System.Collections;
using EVOGAMI.Core;
using EVOGAMI.UI.Options;
using EVOGAMI.UI.PanelMenu;
using EVOGAMI.UI.Transformation;
using UnityEngine;

namespace EVOGAMI.UI
{
    public class SubMenuController : MonoBehaviour
    {
        [Header("Menus")]
        // The pause menu
        [SerializeField] [Tooltip("The pause menu")]
        private PauseMenu pauseMenu;
        // The options menu
        [SerializeField] [Tooltip("The options menu")]
        private OptionsMenu optionsMenu;
        // The transformation menu
        [SerializeField] [Tooltip("The transformation menu")]
        private TransformMenu transformMenu;

        [HideInInspector]
        public SubMenuBase currentMenu;
        [HideInInspector]
        public bool isCancelPerformedThisFrame;
        [HideInInspector]
        public bool isPanelClosed;

        private GameObject _headsUpDisplay;
        
        public void SetCancelPerformedFlag()
        {
            isCancelPerformedThisFrame = true;
            
            StartCoroutine(ResetCancelPerformedFlag());
        }
        
        private IEnumerator ResetCancelPerformedFlag()
        {
            // yield return new WaitForEndOfFrame();
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            isCancelPerformedThisFrame = false;
        }

        public void OpenOptionsMenu(SubMenuBase cameFrom)
        {
            // Options menu was opened from the pause menu
            optionsMenu.cameFrom = cameFrom;
            if (optionsMenu.cameFrom)
                optionsMenu.cameFrom.gameObject.SetActive(false);

            currentMenu = optionsMenu;
            optionsMenu.gameObject.SetActive(true);
        }

        public void CloseOptionsMenu()
        {
            optionsMenu.gameObject.SetActive(false);

            // Options menu was opened from some other menu
            if (optionsMenu.cameFrom)
                optionsMenu.cameFrom.gameObject.SetActive(true);
            currentMenu = optionsMenu.cameFrom;
        }

        public void ToggleHUD(bool isActive)
        {
            _headsUpDisplay?.SetActive(isActive);
        }

        #region Callbacks

        private void OnPausePerformed()
        {
            if (!pauseMenu) return;

            // Ignore if a sub-menu is active and not the pause menu
            if (currentMenu && currentMenu != pauseMenu) return;

            currentMenu = pauseMenu;
            pauseMenu.OnPausePerformed();
        }

        private void OnTransformPerformed()
        {
            if (!transformMenu) return;

            currentMenu = transformMenu;
            transformMenu.OnTransformPerformed();
        }

        private void OnCancelPerformed()
        {
            if (!currentMenu) return;

            currentMenu.OnCancelPerformed(out isPanelClosed);
            if (isPanelClosed) currentMenu = null;
        }

        #endregion

        #region Unity Functions

        public void Start()
        {
            // Get the heads-up display
            _headsUpDisplay = UiManager.Instance ? UiManager.Instance.headsUpDisplay : null;

            // Subscribe to events
            InputManager.Instance.OnPausePerformed += OnPausePerformed;
            InputManager.Instance.OnTransformPerformed += OnTransformPerformed;
            InputManager.Instance.OnCancelPerformed += OnCancelPerformed;
        }

        public void OnDestroy()
        {
            // Unsubscribe from events
            InputManager.Instance.OnPausePerformed -= OnPausePerformed;
            InputManager.Instance.OnTransformPerformed -= OnTransformPerformed;
            InputManager.Instance.OnCancelPerformed -= OnCancelPerformed;
        }
        
        #endregion
    }
}