using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Button = UnityEngine.UIElements.Button;

namespace EVOGAMI.UI.MenuButton
{
    [RequireComponent(typeof(Button))]
    public class MenuButton :
        MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        IPointerClickHandler,
        ISelectHandler, IDeselectHandler
    {
        [SerializeField] private TextMeshProUGUI textGameObject;

        [Header("Colors")]
        // The normal color of the button
        [SerializeField] [Tooltip("The normal color of the button")]
        private Color normalColor;
        // The highlighted color of the button
        [SerializeField] [Tooltip("The highlighted color of the button")]
        private Color highlightedColor;
        // The pressed color of the button
        [SerializeField] [Tooltip("The pressed color of the button")]
        private Color pressedColor;
        // The selected color of the button
        [SerializeField] [Tooltip("The selected color of the button")]
        private Color selectedColor;

        public void OnPointerClick(PointerEventData eventData)
        {
            textGameObject.color = pressedColor;
        }

        #region IPointerEnterHandler, IPointerExitHandler

        public void OnPointerEnter(PointerEventData eventData)
        {
            textGameObject.color = highlightedColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            textGameObject.color = normalColor;
        }

        #endregion

        #region ISelectHandler, IDeselectHandler

        public void OnSelect(BaseEventData eventData)
        {
            textGameObject.color = selectedColor;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            textGameObject.color = normalColor;
        }

        #endregion
    }
}