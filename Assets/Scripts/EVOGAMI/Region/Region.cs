using UnityEngine;

namespace EVOGAMI.Region
{
    [RequireComponent(typeof(Collider))]
    public abstract class Region : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;

        protected virtual void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            if (_meshRenderer != null) _meshRenderer.enabled = false;
        }
    }
}