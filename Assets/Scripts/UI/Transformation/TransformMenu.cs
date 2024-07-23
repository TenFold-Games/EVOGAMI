using System;
using EVOGAMI.Core;
using EVOGAMI.UI.PanelMenu;
using UnityEngine;

namespace EVOGAMI.UI.Transformation
{
    public class TransformMenu : SubMenuBase
    {
        private TransformPanel _transformPanel;

        #region Input Events

        public void OnTransformPerformed()
        {
            _transformPanel.Toggle();
        }

        public override void OnCancelPerformed(out bool isPanelClosed)
        {
            isPanelClosed = _transformPanel.isOffScreen;

            if (_transformPanel.isOffScreen) return; // Ignore if panel is already closed

            _transformPanel.Toggle();
            
            controller.SetCancelPerformedFlag();
        }

        #endregion

        #region Unity Functions

        private void Start()
        {
            _transformPanel = panel.GetComponent<TransformPanel>();
        }
        
        #endregion
    }
}