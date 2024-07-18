using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

namespace EVOGAMI
{
    public class pianonotesfx : MonoBehaviour
    {

        [SerializeField] StudioEventEmitter pianoSfx;

        private void OnTriggerEnter(Collider other)
        {
            pianoSfx.Play();
        }

    }
}
