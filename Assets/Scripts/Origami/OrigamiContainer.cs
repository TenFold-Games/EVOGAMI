using System;
using System.Collections.Generic;
using Cinemachine;
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
            Bug,
            Frog,
            Human,
            Plane,
            None
        }
        
        // Managers
        private PlayerManager _playerManager;

        // Forms
        [SerializeField] private OriObjMapping[] formsMeshMapping;
        public Dictionary<OrigamiForm, GameObject> Forms;
        
        // Cameras
        public CinemachineVirtualCameraBase mainCamera;
        public CinemachineVirtualCameraBase planeCamera;

        // States
        private OrigamiBugState _bugState;
        private OrigamiFrogState _frogState;
        private OrigamiHumanState _humanState;
        private OrigamiPlaneState _planeState;

        // State machine
        private OrigamiStateMachine _stateMachine;
        
        // Initial form
        [SerializeField] private OrigamiSettings settings;
        
        public void Awake()
        {
            // State machine
            _stateMachine = new OrigamiStateMachine(this);

            // States
            _frogState = new OrigamiFrogState(this, OrigamiForm.Frog);
            _planeState = new OrigamiPlaneState(this, OrigamiForm.Plane);
            _bugState = new OrigamiBugState(this, OrigamiForm.Bug);
            _humanState = new OrigamiHumanState(this, OrigamiForm.Human);
            
            // Initialize forms
            Forms = new Dictionary<OrigamiForm, GameObject>();
            foreach (var mapping in formsMeshMapping)
                Forms.Add(mapping.form, mapping.gameObject);
        }

        public void Start()
        {
            switch (settings.initialForm)
            {
                case OrigamiForm.Frog:
                    _stateMachine.Initialize(_frogState);
                    break;
                case OrigamiForm.Plane:
                    _stateMachine.Initialize(_planeState);
                    break;
                case OrigamiForm.Bug:
                    _stateMachine.Initialize(_bugState);
                    break;
                case OrigamiForm.Human:
                    _stateMachine.Initialize(_humanState);
                    break;
                default:
                    _stateMachine.Initialize(_humanState);
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
                case OrigamiForm.Frog:
                    if (!_playerManager.GainedForms[OrigamiForm.Frog]) return; // Form not gained
                    _stateMachine.ChangeState(_frogState);
                    break;
                case OrigamiForm.Plane:
                    if (!_playerManager.GainedForms[OrigamiForm.Plane]) return; // Form not gained
                    _stateMachine.ChangeState(_planeState);
                    break;
                case OrigamiForm.Bug:
                    if (!_playerManager.GainedForms[OrigamiForm.Bug]) return; // Form not gained
                    _stateMachine.ChangeState(_bugState);
                    break;
                case OrigamiForm.Human:
                    if (!_playerManager.GainedForms[OrigamiForm.Human]) return; // Form not gained
                    _stateMachine.ChangeState(_humanState);
                    break;
                case OrigamiForm.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(form), form, null);
            }
        }

        #region Camera

        public void SetPlaneCamera()
        {
            planeCamera.Priority = 20;
            Debug.Log("Set plane camera");
        }
        
        public void UnsetPlaneCamera()
        {
            planeCamera.Priority = -1;
            Debug.Log("Unset plane camera");
        }

        #endregion

        #region Form

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