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

        // Serialize fields of the colours
        [SerializeField] private Color normalColor;
        [SerializeField] private Color highlightedColor;
        [SerializeField] private Color pressedColor;
        [SerializeField] private Color selectedColor;

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