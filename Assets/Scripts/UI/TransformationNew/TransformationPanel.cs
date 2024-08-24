using System;
using System.Collections.Generic;
using EVOGAMI.Core;
using EVOGAMI.Custom.Enums;
using EVOGAMI.Custom.Serializable;
using EVOGAMI.Origami;
using EVOGAMI.UI.Common;
using EVOGAMI.UI.Transformation;
using EVOGAMI.Utils;
using UnityEngine;

namespace EVOGAMI.UI.TransformationNew
{
    public class TransformationPanel : MonoBehaviour
    {
        private static readonly Dictionary<char, Directions> ArrowMapping = new()
        {
            { 'U', Directions.U },
            { 'D', Directions.D },
            { 'L', Directions.L },
            { 'R', Directions.R }
        };

        [Header("Recipe")]
        // The recipes, matched with the forms.
        [SerializeField] [Tooltip("The recipes, matched with the forms.")]
        private OriObjMapping[] recipes;

        [Header("Folding Animation")]
        // The transformation controller.
        [SerializeField] [Tooltip("The transformation controller.")]
        private TransformationController controller;
        // The arrow groups
        [SerializeField] [Tooltip("The arrow groups.")]
        private ArrowGroup[] arrowGroups;
        // The folding animation component.
        [SerializeField] [Tooltip("The folding animation component.")]
        private FoldingAnimation foldingAnimation;
        // The delay after the sequence is completed before closing the panel.
        [SerializeField] [Tooltip("The delay after the sequence is completed before closing the panel.")]
        private float closeDelay = 0.5f;
        
        [Header("HUD")]
        // The HUD instruction for transformation
        [SerializeField] [Tooltip("The HUD to hide during transformation")]
        private GameObject[] hudToHide;

        public delegate void PanelEvent(bool isClosed);
        public PanelEvent OnPanelStateChange = delegate { };

        private Camera _mainCamera;

        private bool[] _hudActiveStates;

        public void Reset()
        {
            // Reset the transformation controller
            controller.Reset();

            // Reset the folding animation
            SetArrows(null);
            foldingAnimation.Reset();

            // Deactivate the panel
            gameObject.SetActive(false);
        }

        #region Unity Functions

        protected void Start()
        {
            Reset();

            // Register callbacks
            Debug.Assert(controller != null, "controller != null");
            controller.SequenceBreakCallback.AddListener(OnSequenceBreak);
            controller.SequenceReadCallback.AddListener(OnSequenceRead);
            controller.SequenceCompleteCallback.AddListener(OnSequenceComplete);

            PlayerManager.Instance.OnGainForm += form =>
            {
                // Check if the form is in the recipes
                var recipe = Array.Find(recipes, obj => obj.form == form);
                if (recipe is null) return; // Should not happen

                // Set game object active
                recipe.gameObject.SetActive(true);
            };

            foreach (var oriObj in recipes)
                oriObj.gameObject.SetActive(PlayerManager.Instance.GainedForms[oriObj.form]);

            _mainCamera = Camera.main;

            _hudActiveStates = new bool[hudToHide.Length];
        }

        #endregion

        #region Callbacks

        private void OnSequenceBreak(string buffer)
        {
            SetArrows(null);
            foldingAnimation.OnSequenceBreak(buffer);
        }

        private void OnSequenceRead(string buffer)
        {
            SetArrows(buffer);
            StopAllCoroutines();
            foldingAnimation.OnSequenceRead(buffer, controller.sequenceLength);
        }

        private void OnSequenceComplete(OrigamiContainer.OrigamiForm form)
        {
            StartCoroutine(CoroutineUtils.DelayActionRealtime(closeDelay, Toggle));
            foldingAnimation.OnSequenceComplete(form);
        }

        #endregion

        #region Input Events

        public void Toggle()
        {
            if (PlayerManager.Instance.FormsGained <= 2) return; // None, Default, and one more form

            if (!gameObject.activeSelf) // Off-screen -> On-Screen
            {
                Time.timeScale = Mathf.Epsilon;

                InputManager.Instance.DisableCameraControls();
                InputManager.Instance.DisablePlayerControls();

                // Let the folding animation cover the player
                var playerScreenPosition = _mainCamera.WorldToScreenPoint(
                    PlayerManager.Instance.Player.transform.position + Vector3.up * 0.5f
                );
                foldingAnimation.transform.position = playerScreenPosition;

                OnPanelStateChange(false);

                for (var i = 0; i < hudToHide.Length; i++)
                {
                    _hudActiveStates[i] = hudToHide[i].activeSelf;
                    hudToHide[i].SetActive(false);
                }

                gameObject.SetActive(true);
            }
            else // On-screen -> Off-screen
            {
                InputManager.Instance.EnableCameraControls();
                InputManager.Instance.EnablePlayerControls();

                Time.timeScale = 1.0f;

                OnPanelStateChange(true);

                for (var i = 0; i < hudToHide.Length; i++)
                    hudToHide[i].SetActive(_hudActiveStates[i]);

                Reset();
            }
        }

        private void SetArrows(string buffer)
        {
            if (buffer == null)
            {
                foreach (var arrowGroup in arrowGroups)
                    arrowGroup.DisplayArrow(Directions.None);
                return;
            }

            int i;
            for (i = 0; i < buffer.Length; i++)
            {
                if (i >= arrowGroups.Length) break; // Should not happen

                // Try to get the direction
                if (!ArrowMapping.TryGetValue(buffer[i], out var direction)) continue;

                arrowGroups[i].DisplayArrow(direction);
            }

            for (; i < arrowGroups.Length; i++) 
                arrowGroups[i].DisplayArrow(Directions.None);
        }

        #endregion
    }
}