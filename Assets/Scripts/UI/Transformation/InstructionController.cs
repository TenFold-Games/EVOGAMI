using System.Collections.Generic;
using EVOGAMI.Core;
using UnityEngine;
using EVOGAMI.Origami;

namespace EVOGAMI.UI.Transformation
{
    public class InstructionController : MonoBehaviour
    {
        [SerializeField] private GameObject transformationUI; // Assign in the inspector
        [SerializeField] private int unlockedFormsCount;
        
        private void Start()
        {
            // Initialize the transformation UI based on the current state of gained forms.
            UpdateUI();

            // Subscribe to the OnGainForm event to update the UI when new forms are gained.
            PlayerManager.Instance.OnGainForm += HandleFormGained;
        }

        private void OnDestroy()
        {
            // Unsubscribe to avoid memory leaks or errors after this component is destroyed.
            if (PlayerManager.Instance != null)
            {
                PlayerManager.Instance.OnGainForm -= HandleFormGained;
            }
        }

        private void HandleFormGained(OrigamiContainer.OrigamiForm form)
        {
            // Update the UI when a new form is gained.
            UpdateUI();
        }

        private void UpdateUI()
        {
            // Count the forms that have been gained.
            unlockedFormsCount = PlayerManager.Instance.CountUnlockedForms();

            
            // Update the visibility of the transformation UI based on the number of unlocked forms.
            bool shouldShowUI = unlockedFormsCount > 2;
            transformationUI.SetActive(shouldShowUI);
        }

        // Utility method to count the number of forms that have been gained.
        
    }
}