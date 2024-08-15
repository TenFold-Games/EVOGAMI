using System;
using System.Collections;
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
            isPanelClosed = false;

            if (_transformPanel.isOffScreen) return; // Ignore if panel is already closed

            _transformPanel.Toggle();
            
            controller.SetCancelPerformedFlag();
        }

        #endregion

        #region Unity Functions

        private new void Start()
        {
            base.Start();

            _transformPanel = panel.GetComponent<TransformPanel>();
            
            _transformPanel.OnPanelMovementStopped += OnPanelMovementStopped;
        }
        
        private void OnDestroy()
        {
            _transformPanel.OnPanelMovementStopped -= OnPanelMovementStopped;
        }
        
        private void OnPanelMovementStopped(bool isOffScreen)
        {
            if (isOffScreen)
                controller.currentMenu = null;
        }
        
        #endregion
    }
}