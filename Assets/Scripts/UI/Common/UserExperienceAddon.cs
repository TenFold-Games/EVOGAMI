using System;
using EVOGAMI.Core;
using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EVOGAMI.UI.Common
{
    public class UserExperienceAddon :
        MonoBehaviour,
        ISelectHandler, IPointerEnterHandler, 
        IPointerClickHandler, ISubmitHandler 
    {
        [Header("Audio")] 
        [SerializeField] public StudioEventEmitter hoverSfx;
        [SerializeField] public StudioEventEmitter clickSfx;

        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverSfx.Play();
            InputManager.Instance.VibrateController();
        }

        public void OnSelect(BaseEventData eventData)
        {
            hoverSfx.Play();
            InputManager.Instance.VibrateController();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clickSfx.Play();
            InputManager.Instance.VibrateController();
        }

        public void OnSubmit(BaseEventData eventData)
        {
            clickSfx.Play();
            InputManager.Instance.VibrateController();
        }
    }
}