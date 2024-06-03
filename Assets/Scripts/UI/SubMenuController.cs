using System;
using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.UI
{
    public class SubMenuController : MonoBehaviour
    {
        [SerializeField] private Canvas mainCanvas;
        [SerializeField] private SubMenuBase pauseMenu;

        public void Start()
        {
            InputManager.Instance.OnPausePerformed += OnPausePerformed;
        }

        public void OnPausePerformed()
        {
            var isEnabled = pauseMenu.gameObject.activeInHierarchy;
            
            // Toggle menu
            mainCanvas.gameObject.SetActive(isEnabled);
            pauseMenu.gameObject.SetActive(!isEnabled);
            if (!isEnabled)
            {
                pauseMenu.Enable(null);
            }
            else
            {
                pauseMenu.Disable();
            }
        }
    }
}