using EVOGAMI.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// using Button = UnityEngine.UI.Button;

namespace EVOGAMI.UI.Common
{
    public class MenuButton :
        Button,
        IAudioPlayer
    {
        [SerializeField] private TextMeshProUGUI textGameObject;

        [Header("Audio")]
        // The audio source to played on hover
        [SerializeField] [Tooltip("The audio source to played on hover")]
        public AudioSource hoverSfx;
        // The audio source to played on click
        [SerializeField] [Tooltip("The audio source to played on click")]
        public AudioSource clickSfx;

        private delegate void HoverEvent();
        private delegate void ClickEvent();

        private HoverEvent _hoverCallback = delegate { };
        private ClickEvent _clickCallback = delegate { };

        #region Interfaces

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            textGameObject.color = colors.pressedColor;

            _clickCallback();
        }

        public override void OnSubmit(BaseEventData eventData)
        {
            base.OnSubmit(eventData);

            textGameObject.color = colors.pressedColor;

            _clickCallback();
        }

        #region IPointerEnterHandler, IPointerExitHandler

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            textGameObject.color = colors.highlightedColor;

            _hoverCallback();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            textGameObject.color = colors.normalColor;
        }

        #endregion

        #region ISelectHandler, IDeselectHandler

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

            textGameObject.color = colors.selectedColor;

            _hoverCallback();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);

            textGameObject.color = colors.normalColor;
        }

        #endregion

        #endregion

        #region Unity Functions

        protected override void Awake()
        {
            base.Awake();

            _hoverCallback += PlayHoverSfx;
            _clickCallback = PlayClickSfx;
        }

        #endregion

        public void PlayAudio(AudioSource source)
        {
            if (source) 
                source.Play();
        }

        private void PlayHoverSfx()
        {
            PlayAudio(hoverSfx);
        }

        private void PlayClickSfx()
        {
            PlayAudio(clickSfx);
        }

        public void Select(bool shouldPlaySfx)
        {
            if (!shouldPlaySfx) _hoverCallback -= PlayHoverSfx;
            base.Select();
            if (!shouldPlaySfx) _hoverCallback += PlayHoverSfx;

            textGameObject.color = colors.selectedColor;
        }
    }
}