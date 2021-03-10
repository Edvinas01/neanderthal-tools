using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace NeanderthalTools.Rendering
{
    public class BlitPass : ScriptableRenderPass
    {
        #region Fields

        private const string BufferName = "_BlitPass";

        private readonly BlitSettings settings;
        private RenderTargetHandle temporaryColorTexture;

        private RenderTargetIdentifier source;
        private RenderTargetHandle destination;

        #endregion

        #region Methods

        public BlitPass(BlitSettings settings)
        {
            this.settings = settings;
        }

        public void Setup(RenderTargetIdentifier sourceId, RenderTargetHandle destinationHandle)
        {
            source = sourceId;
            destination = destinationHandle;
        }

        public override void Execute(
            ScriptableRenderContext context,
            ref RenderingData renderingData
        )
        {
            var cmd = CommandBufferPool.Get(BufferName);

            var opaqueDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDescriptor.depthBufferBits = 0;

            if (destination == RenderTargetHandle.CameraTarget)
            {
                cmd.GetTemporaryRT(temporaryColorTexture.id, opaqueDescriptor, FilterMode.Bilinear);
                Blit(cmd, source, temporaryColorTexture.Identifier(), settings.Material);
                Blit(cmd, temporaryColorTexture.Identifier(), source);
            }
            else
            {
                Blit(cmd, source, destination.Identifier(), settings.Material);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            if (destination == RenderTargetHandle.CameraTarget)
            {
                cmd.ReleaseTemporaryRT(temporaryColorTexture.id);
            }
        }

        #endregion
    }
}
