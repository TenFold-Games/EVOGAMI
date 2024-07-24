using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EVOGAMI.UI.Common
{
    public class DropdownScroller : 
        MonoBehaviour,
        ISelectHandler
    {
        private ScrollRect _scrollRect;
        private float _scrollPosition = 1;

        private void Start()
        {
            _scrollRect = GetComponentInParent<ScrollRect>();
            
            var childCount = transform.parent.childCount - 1;
            var childIndex = transform.GetSiblingIndex();
            
            childIndex = childIndex < ((float) childCount / 2) ? childIndex - 1 : childIndex;
            _scrollPosition = 1 - (float) childIndex / childCount;
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (!_scrollRect) return;

            _scrollRect.verticalScrollbar.value = _scrollPosition;
        }
    }
}