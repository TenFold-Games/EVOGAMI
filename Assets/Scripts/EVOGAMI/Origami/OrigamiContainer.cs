using System;
using System.Collections.Generic;
using Custom.Scriptable;
using Custom.Serializable;
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
            Humanoid,
            Crane,
            Airplane,
            Boat,
            None
        }
        
        // Managers
        private PlayerManager _playerManager;

        // Forms
        [SerializeField] private OriObjMapping[] formsMeshMapping;
        public Dictionary<OrigamiForm, GameObject> forms;

        // States
        private OrigamiBoatState _boatState;
        private OrigamiCraneState _craneState;
        private OrigamiHumanoidState _humanoidState;
        private OrigamiAirPlaneState _airPlaneState;

        // State machine
        private OrigamiStateMachine _stateMachine;
        
        // Initial form
        [SerializeField] private OrigamiSettings settings;
        
        public void Awake()
        {
            // State machine
            _stateMachine = new OrigamiStateMachine(this);

            // States
            _craneState = new OrigamiCraneState(this, OrigamiForm.Crane);
            _airPlaneState = new OrigamiAirPlaneState(this, OrigamiForm.Airplane);
            _boatState = new OrigamiBoatState(this, OrigamiForm.Boat);
            _humanoidState = new OrigamiHumanoidState(this, OrigamiForm.Humanoid);
            
            // Initialize forms
            forms = new Dictionary<OrigamiForm, GameObject>();
            foreach (var mapping in formsMeshMapping)
                forms.Add(mapping.form, mapping.gameObject);
        }

        public void Start()
        {
            switch (settings.initialForm)
            {
                case OrigamiForm.Crane:
                    _stateMachine.Initialize(_craneState);
                    break;
                case OrigamiForm.Airplane:
                    _stateMachine.Initialize(_airPlaneState);
                    break;
                case OrigamiForm.Boat:
                    _stateMachine.Initialize(_boatState);
                    break;
                case OrigamiForm.Humanoid:
                    _stateMachine.Initialize(_humanoidState);
                    break;
                default:
                    _stateMachine.Initialize(_humanoidState);
                    break;
            }
            
            // Managers
            _playerManager = PlayerManager.Instance;

            // Set up input handlers
            InputManager.Instance.OnFormChange += ChangeForm;
        }

        public void Update()
        {
            _stateMachine.Update(Time.deltaTime);
        }

        public void FixedUpdate()
        {
            _stateMachine.FixedUpdate(Time.fixedDeltaTime);
        }

        /// <summary>
        ///     Change the form of the origami
        /// </summary>
        /// <param name="form">The form to change to</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the form is not recognized</exception>
        private void ChangeForm(OrigamiForm form)
        {
            switch (form)
            {
                case OrigamiForm.Crane:
                    if (!_playerManager.GainedForms[OrigamiForm.Crane]) return; // Form not gained
                    _stateMachine.ChangeState(_craneState);
                    break;
                case OrigamiForm.Airplane:
                    if (!_playerManager.GainedForms[OrigamiForm.Airplane]) return; // Form not gained
                    _stateMachine.ChangeState(_airPlaneState);
                    break;
                case OrigamiForm.Boat:
                    if (!_playerManager.GainedForms[OrigamiForm.Boat]) return; // Form not gained
                    _stateMachine.ChangeState(_boatState);
                    break;
                case OrigamiForm.Humanoid:
                    if (!_playerManager.GainedForms[OrigamiForm.Humanoid]) return; // Form not gained
                    _stateMachine.ChangeState(_humanoidState);
                    break;
                case OrigamiForm.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(form), form, null);
            }
        }

        #region Form

        /// <summary>
        ///     Activate the selected form
        /// </summary>
        /// <param name="form">The form to activate</param>
        public void SetForm(OrigamiForm form)
        {
            if (form == OrigamiForm.None) return;
            forms[form].SetActive(true);
        }

        /// <summary>
        ///     Deactivate the selected form
        /// </summary>
        /// <param name="form">The form to deactivate</param>
        public void UnsetForm(OrigamiForm form)
        {
            if (form == OrigamiForm.None) return;
            forms[form].SetActive(false);
        }

        /// <summary>
        ///     Deactivate all forms
        /// </summary>
        public void UnsetAllForms()
        {
            foreach (var form in forms)
                form.Value.SetActive(false);
        }

        #endregion
    }
}