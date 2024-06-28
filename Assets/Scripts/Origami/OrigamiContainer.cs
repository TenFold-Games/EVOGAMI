using System;
using System.Collections.Generic;
using EVOGAMI.Custom.Scriptable;
using EVOGAMI.Custom.Serializable;
using EVOGAMI.Core;
using EVOGAMI.Origami.States;
using UnityEngine;

namespace EVOGAMI.Origami
{
    public class OrigamiContainer : MonoBehaviour
    {
        /// <summary>
        ///     The form of the origami. <em>Always leave None as the last element</em>
        /// </summary>
        public enum OrigamiForm
        {
            Crab,
            Frog,
            NinjaStar,
            None
        }
        
        // Managers
        private PlayerManager _playerManager;

        // Forms
        [SerializeField] private OriObjMapping[] formsMeshMapping;
        public Dictionary<OrigamiForm, GameObject> Forms;
        
        // Initial form
        [SerializeField] private OrigamiSettings origamiSettings;

        // States
        private OrigamiCrabState _crabState;
        private OrigamiFrogState _frogState;
        private OrigamiNinjaStarState _ninjaStarState;

        // State machine
        public OrigamiStateMachine StateMachine;
        
        // Callbacks
        public delegate void FormChangeCallback(OrigamiForm form);
        public event FormChangeCallback OnFormChange = delegate { };

        #region Unity Functions

        public void Awake()
        {
            // State machine
            StateMachine = new OrigamiStateMachine(this);

            // States
            _frogState = new OrigamiFrogState(this, OrigamiForm.Frog);
            _crabState = new OrigamiCrabState(this, OrigamiForm.Crab);
            _ninjaStarState = new OrigamiNinjaStarState(this, OrigamiForm.NinjaStar);
            
            // Initialize forms
            Forms = new Dictionary<OrigamiForm, GameObject>();
            foreach (var mapping in formsMeshMapping)
                Forms.Add(mapping.form, mapping.gameObject);
        }

        public void Start()
        {
            // Managers
            _playerManager = PlayerManager.Instance;
            
            // Set up initial form
            switch (origamiSettings.initialForm)
            {
                case OrigamiForm.Crab:
                    StateMachine.Initialize(_crabState);
                    break;
                case OrigamiForm.Frog:
                    StateMachine.Initialize(_frogState);
                    break;
                case OrigamiForm.NinjaStar:
                    StateMachine.Initialize(_ninjaStarState);
                    break;
                case OrigamiForm.None:
                default:
                    Debug.LogError("Initial form not recognized");
                    StateMachine.Initialize(_frogState);
                    break;
            }

            /* TBD: Start ----------------------------------------------------------------------------------------------
            // Set up input handlers
            InputManager.Instance.OnFormChange += ChangeForm;
            ----------------------------------------------------------------------------------------------- TBD: End */
        }

        public void Update()
        {
            StateMachine.Update(Time.deltaTime);
        }

        public void FixedUpdate()
        {
            StateMachine.FixedUpdate(Time.fixedDeltaTime);
        }

        #endregion

        /* TBD: Start --------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Change the form of the origami
        /// </summary>
        /// <param name="form">The form to change to</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the form is not recognized</exception>
        private void ChangeForm(OrigamiForm form)
        {
            switch (form)
            {
                case OrigamiForm.Crab:
                    if (!_playerManager.GainedForms[OrigamiForm.Crab]) return; // Form not gained
                    OnFormChange.Invoke(OrigamiForm.Crab);
                    StateMachine.ChangeState(_crabState);
                    break;
                case OrigamiForm.Frog:
                    if (!_playerManager.GainedForms[OrigamiForm.Frog]) return; // Form not gained
                    OnFormChange.Invoke(OrigamiForm.Frog);
                    StateMachine.ChangeState(_frogState);
                    break;
                case OrigamiForm.NinjaStar:
                    if (!_playerManager.GainedForms[OrigamiForm.NinjaStar]) return; // Form not gained
                    OnFormChange.Invoke(OrigamiForm.NinjaStar);
                    StateMachine.ChangeState(_ninjaStarState);
                    break;
                case OrigamiForm.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(form), form, null);
            }
        }
        ---------------------------------------------------------------------------------------------------- TBD: End */

        #region Form

        public void ChangeForm(OrigamiForm form)
        {
            // Form not gained
            if (!_playerManager.GainedForms[form]) return;
            
            OnFormChange.Invoke(form);

            switch (form)
            {
                case OrigamiForm.Crab:
                    StateMachine.ChangeState(_crabState);
                    break;
                case OrigamiForm.Frog:
                    StateMachine.ChangeState(_frogState);
                    break;
                case OrigamiForm.NinjaStar:
                    StateMachine.ChangeState(_ninjaStarState);
                    break;
                case OrigamiForm.None:
                    break; // Should not happen
                default:
                    throw new ArgumentOutOfRangeException(nameof(form), form, null);
            }
        }

        /// <summary>
        ///     Activate the selected form
        /// </summary>
        /// <param name="form">The form to activate</param>
        public void SetForm(OrigamiForm form)
        {
            if (form == OrigamiForm.None) return;
            Forms[form].SetActive(true);
        }

        /// <summary>
        ///     Deactivate the selected form
        /// </summary>
        /// <param name="form">The form to deactivate</param>
        public void UnsetForm(OrigamiForm form)
        {
            if (form == OrigamiForm.None) return;
            Forms[form].SetActive(false);
        }

        /// <summary>
        ///     Deactivate all forms
        /// </summary>
        public void UnsetAllForms()
        {
            foreach (var form in Forms)
                form.Value.SetActive(false);
        }

        #endregion
    }
}