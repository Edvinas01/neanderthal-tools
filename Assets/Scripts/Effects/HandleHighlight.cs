using UnityEngine;

namespace NeanderthalTools.Effects
{
    [RequireComponent(typeof(Renderer))]
    public class HandleHighlight : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private Material highlightMaterial;

        #endregion

        #region Fields

        private new Renderer renderer;
        private Material originalMaterial;

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
            if (originalMaterial == null)
            {
                originalMaterial = renderer.material;
            }

            renderer.material = highlightMaterial;
        }

        public void ClearHighlight()
        {
            renderer.material = originalMaterial;
        }

        #endregion
    }
}
