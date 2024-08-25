using EVOGAMI.UI.PanelMenu;

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

        private void OnPanelStateChange(bool isClosed)
        {
            controller.currentMenu = isClosed ? null : this;
        }

        #region Unity Functioins

        protected override void Start()
        {
            base.Start();

            _transformationPanel = panel.GetComponent<TransformationPanel>();

            _transformationPanel.OnPanelStateChange += OnPanelStateChange;
            // _transformationPanel.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _transformationPanel.OnPanelStateChange -= OnPanelStateChange;
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