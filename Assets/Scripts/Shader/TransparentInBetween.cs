using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace EVOGAMI.Shader
{
    public class TransparentInBetween : MonoBehaviour
    {
        // The first object.
        [SerializeField] [Tooltip("The first object.")]
        private GameObject obj1;
        // The second object.
        [SerializeField] [Tooltip("The second object.")]
        private GameObject obj2;
        // The layer mask used to check for obstacles.
        [SerializeField] [Tooltip("The layer mask used to check for obstacles.")]
        private LayerMask obstacleLayer;

        // Cached renderers of objects
        private readonly Dictionary<GameObject, Renderer[]> _cachedRenderers = new();

        // Cached shader properties
        private static readonly int Surface = UnityEngine.Shader.PropertyToID("_Surface");
        private static readonly int SrcBlend = UnityEngine.Shader.PropertyToID("_SrcBlend");
        private static readonly int DstBlend = UnityEngine.Shader.PropertyToID("_DstBlend");
        private static readonly int ZWrite = UnityEngine.Shader.PropertyToID("_ZWrite");

        // Raycast results
        private readonly RaycastHit[] _results = new RaycastHit[3];

        #region Unity Functions

        private void FixedUpdate()
        {
            // Ray cast all objects between obj1 and obj2
            var size = Physics.RaycastNonAlloc(
                obj1.transform.position,
                obj2.transform.position - obj1.transform.position,
                _results,
                Vector3.Distance(obj1.transform.position, obj2.transform.position),
                obstacleLayer
            );

            var transparentObjects = new List<GameObject>();

            // Make all objects between obj1 and obj2 transparent
            for (var i = 0; i < size; i++)
            {
                // Get hit object, ignore if end points
                var hitObj = _results[i].collider.gameObject;
                if (hitObj == obj1 || hitObj == obj2) continue;

                // Cache renderers of object
                if (!_cachedRenderers.ContainsKey(hitObj))
                    _cachedRenderers[hitObj] = hitObj.GetComponentsInChildren<Renderer>();

                // Make object transparent
                if (MakeObjectTransparent(hitObj, _cachedRenderers[hitObj], 0.5f))
                    transparentObjects.Add(hitObj);
            }

            // Reset transparency of all objects that are no longer between obj1 and obj2
            foreach (var cachedObj in _cachedRenderers.Keys.Where(
                         cachedObj => !transparentObjects.Contains(cachedObj)))
                foreach (var r in _cachedRenderers[cachedObj])
                    foreach (var material in r.materials)
                        ResetMaterial(material);
        }

        #endregion

        private static bool MakeObjectTransparent(GameObject obj, Renderer[] renderers, float transparency)
        {
            if (renderers == null) return false;

            foreach (var r in renderers)
            foreach (var material in r.materials)
                SetMaterialTransparency(material, transparency);

            return true;
        }

        private static void ResetMaterial(Material material)
        {
            material.SetFloat(Surface, 0);
            material.SetOverrideTag("RenderType", "Opaque");
            material.renderQueue = (int)RenderQueue.Geometry;
            material.SetFloat(SrcBlend, (float)BlendMode.One);
            material.SetFloat(DstBlend, (float)BlendMode.Zero);
            material.SetFloat(ZWrite, 1);

            material.color = new Color(material.color.r, material.color.g, material.color.b, 1);
        }

        private static void SetMaterialTransparency(Material material, float transparency)
        {
            // https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@17.0/manual/lit-shader.html

            // Set material to transparent
            material.SetFloat(Surface, 1);
            material.SetOverrideTag("RenderType", "Transparent");
            material.renderQueue = (int)RenderQueue.Transparent;

            // Enable alpha blending
            material.SetFloat(SrcBlend, (float)BlendMode.SrcAlpha);
            material.SetFloat(DstBlend, (float)BlendMode.OneMinusSrcAlpha);
            material.SetFloat(ZWrite, 0);

            material.color = new Color(material.color.r, material.color.g, material.color.b, transparency);
        }
    }
}