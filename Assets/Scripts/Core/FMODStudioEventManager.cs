using FMODUnity;
using UnityEngine;

namespace EVOGAMI.Core
{
    public class FMODStudioEventManager : MonoBehaviour
    {
        public static FMODStudioEventManager Instance { get; private set; }

        // The studio event emitter
        [SerializeField] [Tooltip("The studio event emitter")]
        private StudioEventEmitter studioEventEmitter;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void Play()
        {
            studioEventEmitter.Play();
        }

        public void Stop()
        {
            studioEventEmitter.Stop();
        }
    }
}