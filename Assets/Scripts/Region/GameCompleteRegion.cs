using System;
using EVOGAMI.Core;
using UnityEngine;

namespace EVOGAMI.Region
{
    [RequireComponent(typeof(Collider))]
    public class GameCompleteRegion : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") || other.CompareTag("OrigamiMesh"))
                GameManager.Instance.onGameComplete.Invoke();
        }
    }
}