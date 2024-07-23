using Cinemachine;
using UnityEngine;

namespace EVOGAMI.Options
{
    public class CinemachineFreeLookInputController :
        MonoBehaviour,
        ISensitivityController,
        IXInverter, IYInverter
    {
        // The Cinemachine FreeLook camera
        [SerializeField] [Tooltip("The Cinemachine FreeLook camera")]
        private CinemachineFreeLook freeLookCamera;

        // The min speed of the x-axis
        [SerializeField] [Tooltip("The min speed of the x-axis")]
        private float xMinSpeed = 50f;
        // The max speed of the x-axis
        [SerializeField] [Tooltip("The max speed of the x-axis")]
        private float xMaxSpeed = 250f;

        private void Start()
        {
            OptionsManager.Instance.SensitivityControllers.Add(this);
            OptionsManager.Instance.XInverters.Add(this);
            OptionsManager.Instance.YInverters.Add(this);
        }

        public void SetSensitivity(float sensitivity)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = xMinSpeed + sensitivity * (xMaxSpeed - xMinSpeed);
            freeLookCamera.m_YAxis.m_MaxSpeed = sensitivity;
        }

        public void InvertX(bool invertX)
        {
            freeLookCamera.m_XAxis.m_InvertInput = invertX;
        }

        public void InvertY(bool invertX)
        {
            freeLookCamera.m_YAxis.m_InvertInput = invertX;
        }
    }
}