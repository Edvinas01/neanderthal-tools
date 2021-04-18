using System.Collections.Generic;
using NeanderthalTools.ToolCrafting.Hafting;
using NeanderthalTools.ToolCrafting.Knapping;
using UnityEngine;

namespace NeanderthalTools.Effects
{
    public class FlakeHighlight : MonoBehaviour
    {
        #region Editor

        [SerializeField]
        private Material highlightMaterial;

        #endregion

        #region Fields

        private Dictionary<Flake, Material> originalMaterials =
            new Dictionary<Flake, Material>();

        #endregion

        #region Unity Lifecycle

        private void OnDisable()
        {
            originalMaterials.Clear();
        }

        #endregion

        #region Methods

        public void ShowHighlight(FlakeEventArgs args)
        {
            var flake = args.Flake;
            if (flake.IsAttachable)
            {
                ShowHighlight(flake);
            }
        }

        public void ClearHighlight(HaftEventArgs args)
        {
            var toolPart = args.ToolPart;
            if (toolPart is Flake {IsAttachable: true} flake)
            {
                ClearHighlight(flake);
            }
        }

        private void ShowHighlight(Flake flake)
        {
            var flakeRenderer = flake.GetComponent<Renderer>();
            if (flakeRenderer == null)
            {
                return;
            }

            var originalMaterial = flakeRenderer.material;
            flakeRenderer.material = highlightMaterial;
            originalMaterials[flake] = originalMaterial;
        }

        private void ClearHighlight(Flake flake)
        {
            var flakeRenderer = flake.GetComponent<Renderer>();
            if (flakeRenderer == null)
            {
                return;
            }

            if (originalMaterials.TryGetValue(flake, out var originalFlakeMaterial))
            {
                flakeRenderer.material = originalFlakeMaterial;
                originalMaterials.Remove(flake);
            }
        }

        #endregion
    }
}
