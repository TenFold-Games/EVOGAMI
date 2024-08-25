using UnityEngine;
using UnityEngine.UI;

namespace EVOGAMI.UI.Common
{
    public class Rectile : MonoBehaviour
    {
        [SerializeField] [Tooltip("The rectile image.")]
        private Image rectile;
        [SerializeField] [Tooltip("The rectile image for the fwoosh effect.")]
        private Image rectileFwoosh;

        #region Unity Functions

        private void Start()
        {
            rectile.gameObject.SetActive(false);
            rectileFwoosh.gameObject.SetActive(false);
        }

        #endregion

        public void ShowRectile()
        {
            rectile.gameObject.SetActive(true);
            rectileFwoosh.gameObject.SetActive(false);
        }

        public void ShowRectileFwoosh()
        {
            rectile.gameObject.SetActive(false);
            rectileFwoosh.gameObject.SetActive(true);
        }

        public void ShowAll()
        {
            rectile.gameObject.SetActive(true);
            rectileFwoosh.gameObject.SetActive(true);
        }

        public void HideAll()
        {
            rectile.gameObject.SetActive(false);
            rectileFwoosh.gameObject.SetActive(false);
        }
    }
}