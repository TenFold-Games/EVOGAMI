using Cinemachine;
using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Custom.Cinemachine
{
    public class ControlledCamera : MonoBehaviour
    {
        // The input provider for the camera
        [SerializeField] [Tooltip("The input provider for the camera.")]
        private CinemachineInputProvider inputProvider;

        private void Start()
        {
            if (!inputProvider) inputProvider = GetComponent<CinemachineInputProvider>();

            if (inputProvider) InputManager.Instance.VirtualCameras.Add(inputProvider);
        }

        private void OnDestroy()
        {
            if (inputProvider) InputManager.Instance.VirtualCameras.Remove(inputProvider);
        }
    }
}