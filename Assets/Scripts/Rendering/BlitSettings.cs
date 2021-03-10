using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace NeanderthalTools.Rendering
{
    [Serializable]
    public class BlitSettings
    {
        #region Editor

        [SerializeField]
        private RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;

        [SerializeField]
        private Material material;

        #endregion

        #region Properties

        public RenderPassEvent RenderPassEvent => renderPassEvent;

        public Material Material => material;

        #endregion
    }
}
