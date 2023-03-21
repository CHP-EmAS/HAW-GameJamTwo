using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Plum.Rendering.URP
{
    public class ResolutionPass : ScriptableRenderPass
    {
        public RTHandle colorHandle;
        public RenderTexture rTex;
        protected ProfilingSampler sampler = new ProfilingSampler("ColorBlit");
        public void Init(RTHandle handle)
        {
            colorHandle = handle;
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ConfigureTarget(rTex);
        }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var camData = renderingData.cameraData;
            if (camData.camera.cameraType != CameraType.Game) return;

            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, sampler))
            {
                cmd.Blit(rTex, colorHandle);
            }
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }

    }


    public class Resolution: ScriptableRendererFeature
    {
        private static bool RightCamType(CameraData data)
        {
            return data.cameraType == CameraType.Game;
        }
        public RenderTexture renTex;
        ResolutionPass m_ScriptablePass;
        protected virtual void PreCreate()
        {

        }
        /// <inheritdoc/>
        public override void Create()
        {
            PreCreate();
            m_ScriptablePass = new ResolutionPass();
        }

        protected override void Dispose(bool disposing)
        {
        }



        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (RightCamType(renderingData.cameraData))
            {
                renderer.EnqueuePass(m_ScriptablePass);
            }
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            if (RightCamType(renderingData.cameraData))
            {
                m_ScriptablePass.ConfigureInput(ScriptableRenderPassInput.Color);
                m_ScriptablePass.Init(renderer.cameraColorTargetHandle);
                m_ScriptablePass.rTex = renTex;
            }
        }
    }
}

