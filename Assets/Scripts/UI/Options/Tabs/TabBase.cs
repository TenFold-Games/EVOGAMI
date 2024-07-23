using UnityEngine;
using UnityEngine.UI;

namespace EVOGAMI.UI.Options.Tabs
{
    public abstract class TabBase : MonoBehaviour
    {
        [SerializeField] [Tooltip("The options menu")]
        protected OptionsMenu optionsMenu;
        [SerializeField] [Tooltip("The default element to be selected when the tab is opened")]
        protected Selectable defaultSelectable;

        protected void Awake()
        {
            Initialize();

            RegisterCallbacks();
        }

        protected abstract void Initialize();

        protected abstract void RegisterCallbacks();

        public virtual void OnTabEnter()
        {
            optionsMenu.currentTab?.OnTabExit();

            optionsMenu.currentTab = this;
            gameObject.SetActive(true);
            
            defaultSelectable?.Select();
        }

        protected virtual void OnTabExit()
        {
            gameObject.SetActive(false);
        }
    }
}