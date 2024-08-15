using FMODUnity;
using UnityEngine;

namespace EVOGAMI.Media
{
    public class PianoNoteSfx : MonoBehaviour
    {
        // The piano sound effect.
        [SerializeField] [Tooltip("The piano sound effect.")]
        private StudioEventEmitter pianoSfx;

        private void OnTriggerEnter(Collider other)
        {
            pianoSfx.Play();
        }
    }
}