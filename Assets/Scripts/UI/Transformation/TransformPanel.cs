using System.Collections.Generic;
using EVOGAMI.Core;
using EVOGAMI.Custom.Enums;
using EVOGAMI.Custom.Serializable;
using EVOGAMI.Origami;
using EVOGAMI.UI.Common;
using EVOGAMI.Utils;
using UnityEngine;

namespace EVOGAMI.UI.Transformation
{
    public class TransformPanel : MonoBehaviour
    {
        private static readonly Dictionary<char, Directions> ArrowMapping = new()
        {
            { 'U', Directions.U },
            { 'D', Directions.D },
            { 'L', Directions.L },
            { 'R', Directions.R }
        };

        [Header("Moving Animation")]
        // The RectTransform of the canvas.
        [SerializeField] [Tooltip("The RectTransform of the canvas.")]
        private RectTransform canvasRect;
        // The duration of the move animation.
        [SerializeField] [Tooltip("The duration of the move animation.")] [Range(0.1f, 5f)]
        private float moveDuration = 0.1f;
        // The delay after the sequence is completed before closing the panel.
        [SerializeField] [Tooltip("The delay after the sequence is completed before closing the panel.")]
        private float closeDelay = 0.5f;

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

        public delegate void PanelEvent(bool isOffScreen);
        public PanelEvent OnPanelMovementStopped = delegate { };

        // The off-screen and on-screen positions of the canvas.
        private Vector2 _offScreenPos; // Off-screen
        private Vector2 _onScreenPos; // On-screen

        // Move variables
        private float _moveTimer;
        private Vector2 _startPos;
        private Vector2 _endPos;

        public bool isPanelOpen = false;
        
        // Flags
        [HideInInspector] public bool isMoving;
        [HideInInspector] public bool isOffScreen;

        private void Reset()
        {
            // Re-calculate positions
            _offScreenPos = Vector2.zero;
            _onScreenPos = new Vector2(0, -2160);

            // Move back to start position
            canvasRect.anchoredPosition = _offScreenPos;
            isOffScreen = true;
            isMoving = false;

            // Reset the transformation controller
            controller.Reset();

            // Reset the folding animation
            SetArrows(null);
            foldingAnimation.Reset();

            // Deactivate the panel
            gameObject.SetActive(false);
        }

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
            StartCoroutine(CoroutineUtils.DelayAction(closeDelay, Toggle));
            foldingAnimation.OnSequenceComplete(form);
        }

        #endregion

        #region Input Events

        public void Toggle()
        {
            if (PlayerManager.Instance.FormsGained <= 2) return; // None, Default, and one more form
            
            // Should not move
            if (isMoving) return;

            if (isOffScreen) gameObject.SetActive(true); // On-screen

            isMoving = true;

            if (isOffScreen) // Off-screen -> On-screen
            {
                InputManager.Instance.DisablePlayerControls();
                _startPos = _offScreenPos;
                _endPos = _onScreenPos;
            }
            else // On-screen -> Off-screen
            {
                Debug.Log("Closing the panel.");
                _startPos = _onScreenPos;
                _endPos = _offScreenPos;
                InputManager.Instance.EnablePlayerControls();
                
                controller.Reset();
            }
            isPanelOpen = !isPanelOpen;
        }
        
        private void SetArrows(string buffer)
        {
            if (buffer == null)
            {
                foreach (var arrowGroup in arrowGroups) arrowGroup.DisplayArrow(Directions.None);
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

            for (; i < arrowGroups.Length; i++) arrowGroups[i].DisplayArrow(Directions.None);
        }

        #endregion

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
                var recipe = System.Array.Find(recipes, obj => obj.form == form);
                if (recipe is null) return; // Should not happen
                
                // Set game object active
                recipe.gameObject.SetActive(true);
            };
            
            foreach (var oriObj in recipes)
                oriObj.gameObject.SetActive(PlayerManager.Instance.GainedForms[oriObj.form]);
        }

        private void Update()
        {
            // Should not move
            if (!isMoving) return;

            _moveTimer += Time.deltaTime;
            var t = _moveTimer / moveDuration;
            canvasRect.anchoredPosition = Vector2.Lerp(_startPos, _endPos, t);

            if (t >= 1) // Move completed
            {
                isMoving = false;
                _moveTimer = 0;
                isOffScreen = !isOffScreen;
                
                OnPanelMovementStopped(isOffScreen);

                if (isOffScreen) Reset(); // Off-screen
            }
        }

        #endregion
    }
}