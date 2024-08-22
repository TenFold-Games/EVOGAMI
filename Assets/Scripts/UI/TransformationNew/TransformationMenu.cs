using EVOGAMI.UI.PanelMenu;
using UnityEngine;

namespace EVOGAMI.UI.TransformationNew
{
    public class TransformationMenu : SubMenuBase
    {
        private TransformationPanel _transformationPanel;

        #region Input Events

        public void OnTransformPerformed()
        {
            _transformationPanel.Toggle();
        }

        #region Unity Functioins

        protected override void Start()
        {
            base.Start();

            _transformationPanel = panel.GetComponent<TransformationPanel>();

            _transformationPanel.gameObject.SetActive(false);
        }

        #endregion

        public override void OnCancelPerformed(out bool isPanelClosed)
        {
            isPanelClosed = false;

            if (!_transformationPanel.gameObject.activeSelf) return; // Ignore if panel is already closed

            _transformationPanel.Toggle();

            controller.SetCancelPerformedFlag();
        }

        #endregion
    }
}