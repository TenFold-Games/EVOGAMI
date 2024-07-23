using EVOGAMI.UI.Common;
using UnityEngine;

namespace EVOGAMI.UI.Options
{
    public class SidePanel : MonoBehaviour
    {
        [Header("Reference")]
        // The options menu
        [SerializeField] [Tooltip("The options menu")]
        private OptionsMenu optionsMenu;

        [Header("Buttons")]
        // The graphics button
        [SerializeField] [Tooltip("The graphics button")]
        private MenuButton graphicsButton;
        // The audio button
        [SerializeField] [Tooltip("The audio button")]
        private MenuButton audioButton;
        // The controls button
        [SerializeField] [Tooltip("The controls button")]
        private MenuButton controlsButton;
        // The credits button
        [SerializeField] [Tooltip("The credits button")]
        private MenuButton creditsButton;

        private void Start()
        {
            graphicsButton.onClick.AddListener(OnGraphicsClicked);
            audioButton.onClick.AddListener(OnAudioClicked);
            controlsButton.onClick.AddListener(OnControlsClicked);
            creditsButton.onClick.AddListener(OnCreditsClicked);
        }

        private void OnGraphicsClicked()
        {
            optionsMenu.isInTab = true;
            optionsMenu.lastSelectedButton = graphicsButton;
        }

        private void OnAudioClicked()
        {
            optionsMenu.isInTab = true;
            optionsMenu.lastSelectedButton = audioButton;
        }

        private void OnControlsClicked()
        {
            optionsMenu.isInTab = true;
            optionsMenu.lastSelectedButton = controlsButton;
        }

        private void OnCreditsClicked()
        {
            optionsMenu.isInTab = false;
            optionsMenu.lastSelectedButton = creditsButton;
        }
    }
}