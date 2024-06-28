using System;
using EVOGAMI.Custom.Serializable;
using EVOGAMI.Origami;
using UnityEngine;
using UnityEngine.Serialization;

namespace EVOGAMI.UI.Transformation
{
    public class FoldingAnimation : MonoBehaviour
    {
        public GameObject[] steps;
        public OriObjMapping[] lastStep;
        
        private GameObject _currentStep;

        #region Callbacks

        public void OnSequenceRead(string buffer, int sequenceLength)
        {
            if (buffer is null) 
                Reset();
            else if (buffer.Length < sequenceLength)
                ToggleStep(steps[buffer.Length]);
        }

        public void OnSequenceBreak(string buffer)
        {
            Reset();
        }
        
        public void OnSequenceComplete(OrigamiContainer.OrigamiForm form)
        {
            var oriObjMapping = Array.Find(lastStep, obj => obj.form == form);
            if (oriObjMapping is null) return; // Should not happen.
            
            ToggleStep(oriObjMapping.gameObject);
        }
        
        #endregion

        #region Unity Functions

        private void Awake()
        {
            Debug.Assert(steps.Length > 0, "No steps found.");
            Reset();
        }

        #endregion
        
        private void ToggleStep(GameObject step)
        {
            if (_currentStep == step) return;
            
            _currentStep.SetActive(false);
            _currentStep = step;
            _currentStep.SetActive(true);
        }
        
        public void Reset()
        {
            foreach (var step in steps) step.SetActive(false);
            foreach (var obj in lastStep) obj.gameObject.SetActive(false);

            _currentStep = steps[0];
            _currentStep.SetActive(true);
        }
    }
}