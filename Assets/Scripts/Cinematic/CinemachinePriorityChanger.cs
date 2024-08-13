using System.Collections;
using Cinemachine;
using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Cinematic
{
    public class CinemachinePriorityChanger : MonoBehaviour
    {
        [SerializeField] [Tooltip("The cinemachine virtual camera")]
        private CinemachineVirtualCameraBase virtualCamera;

        public void DefaultCinematic()
        {
            SetPriorityForSeconds();
        }

        private void SetPriorityForSeconds(
            int priority = 15,
            float preWait = 1f,
            float wait = 2.5f,
            float postWait = 1.5f)
        {
            StartCoroutine(SetPriorityCoroutine(priority, preWait, wait, postWait));
        }

        private IEnumerator SetPriorityCoroutine(int priority, float preWait, float wait, float postWait)
        {
            InputManager.Instance.DisableAllControls();

            var origPriority = virtualCamera.Priority;

            yield return new WaitForSecondsRealtime(preWait);
            virtualCamera.Priority = priority;

            yield return new WaitForSecondsRealtime(wait);
            virtualCamera.Priority = origPriority;

            yield return new WaitForSecondsRealtime(postWait);
            InputManager.Instance.EnableAllControls();
        }
    }
}