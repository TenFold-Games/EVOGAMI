using UnityEngine;

namespace EVOGAMI.UI.OptionsMenu.Tabs
{
    public abstract class TabBase : MonoBehaviour
    {
        [SerializeField] [Tooltip("The options menu")]
        protected OptionsMenu optionsMenu;

        public virtual void OnTabEnter()
        {
            optionsMenu.currentTab?.OnTabExit();

            optionsMenu.currentTab = this;
            gameObject.SetActive(true);
        }

        protected virtual void OnTabExit()
        {
            gameObject.SetActive(false);
        }
    }
}