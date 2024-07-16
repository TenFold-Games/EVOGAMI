using EVOGAMI.Core;
using EVOGAMI.Custom.Serializable;
using UnityEngine;

namespace EVOGAMI.UI
{
    public class SchemeSwitcher : MonoBehaviour
    {
        [SerializeField] [Tooltip("The mappings between input devices and objects.")]
        private DvcObjMapping[] mappings;

        private void Start()
        {
            InputManager.Instance.OnInputSchemeChanged += SwitchScheme;

            foreach (var mapping in mappings)
                mapping.gameObject.SetActive(mapping.device == InputManager.Instance.InputScheme);
        }

        private void SwitchScheme(InputManager.InputDeviceScheme oldScheme, InputManager.InputDeviceScheme scheme)
        {
            foreach (var mapping in mappings)
                mapping.gameObject.SetActive(mapping.device == scheme);
        }
    }
}