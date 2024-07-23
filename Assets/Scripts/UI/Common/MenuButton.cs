using EVOGAMI.Audio;
using EVOGAMI.Core;
using FMODUnity;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// using Button = UnityEngine.UI.Button;

namespace EVOGAMI.UI.Common
{
    public class MenuButton : Button
    {
        [Header("Text")]
        [SerializeField] private bool changeTextColor = true;
        [SerializeField] private TextMeshProUGUI textGameObject;

        [Header("Audio")]
        [SerializeField] public StudioEventEmitter hoverSfx;
        [SerializeField] public StudioEventEmitter clickSfx;

        private delegate void HoverEvent();
        private delegate void ClickEvent();

        private HoverEvent _hoverCallback = delegate { };
        private ClickEvent _clickCallback = delegate { };

        #region Interfaces

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            if (changeTextColor) textGameObject.color = colors.pressedColor;

            _clickCallback();
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);

            if (changeTextColor) textGameObject.color = colors.pressedColor;

            _clickCallback();
        }

        #region IPointerEnterHandler, IPointerExitHandler

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            if (changeTextColor) textGameObject.color = colors.highlightedColor;

            _hoverCallback();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            if (changeTextColor) textGameObject.color = colors.normalColor;
        }

        #endregion

        #region ISelectHandler, IDeselectHandler

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            if (changeTextColor) textGameObject.color = colors.selectedColor;

            _hoverCallback();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            if (changeTextColor) textGameObject.color = colors.normalColor;
        }

        #endregion

        #endregion

        #region Unity Functions

        protected override void Awake()
        {
            base.Awake();

            _hoverCallback += PlayHoverSfx;
            _clickCallback += PlayClickSfx;
            
            _hoverCallback += VibrateController;
            _clickCallback += VibrateController;
        }
        
        #endregion
        
        private void VibrateController()
        {
            InputManager.Instance.VibrateController(0.05f, 0.05f, 0.025f);
        }

        private void PlayHoverSfx()
        {
            hoverSfx?.Play();
        }

        private void PlayClickSfx()
        {
            clickSfx?.Play();
        }

        public void Select(bool shouldPlaySfx)
        {
            if (!shouldPlaySfx) _hoverCallback -= PlayHoverSfx;
            base.Select();
            if (!shouldPlaySfx) _hoverCallback += PlayHoverSfx;

            if (changeTextColor) textGameObject.color = colors.selectedColor;
        }
    }
}