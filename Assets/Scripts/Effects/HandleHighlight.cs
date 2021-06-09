using System.Collections.Generic;
using UnityEngine;

namespace NeanderthalTools.Effects
{
    [RequireComponent(typeof(Renderer))]
    public class HandleHighlight : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private List<Material> highlightMaterials;

        #endregion

        #region Fields

        private new Renderer renderer;
        private readonly List<Material> originalMaterials = new List<Material>();

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
        }

        #endregion

        #region Methods

        public void ShowHighlight()
        {
            if (originalMaterials.Count == 0)
            {
                originalMaterials.AddRange(renderer.materials);
            }

            renderer.materials = highlightMaterials.ToArray();
        }

        public void ClearHighlight()
        {
            renderer.materials = originalMaterials.ToArray();
        }

        public void RemoveHighlight()
        {
            ClearHighlight();
            Destroy(this);
        }

        #endregion
    }
}
