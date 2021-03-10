using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace NeanderthalTools.Rendering
{
    public class BlitFeature : ScriptableRendererFeature
    {
        #region Editor

        [SerializeField]
        private BlitSettings settings;

        #endregion

        #region Fields

        private const string TextureName = "_BlitTexture";

        private RenderTargetHandle blitTexture;
        private BlitPass blitPass;

        #endregion

        public override void Create()
        {
            blitTexture.Init(TextureName);
            blitPass = new BlitPass(settings)
            {
                renderPassEvent = settings.RenderPassEvent
            };
        }

        public override void AddRenderPasses(
            ScriptableRenderer renderer,
            ref RenderingData renderingData
        )
        {
            if (settings.Material == null)
            {
                return;
            }

            blitPass.Setup(renderer.cameraColorTarget, RenderTargetHandle.CameraTarget);
            renderer.EnqueuePass(blitPass);
        }
    }
}
