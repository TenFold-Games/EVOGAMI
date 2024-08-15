using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Control
{
    public class ControlsActiveSetter : MonoBehaviour
    {
        public void EnableAllControls()
        {
            InputManager.Instance.EnableAllControls();
        }

        public void DisableAllControls()
        {
            InputManager.Instance.DisableAllControls();
        }
    }
}