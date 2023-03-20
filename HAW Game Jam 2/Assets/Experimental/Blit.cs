using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Plum.Rendering.URP{
    public class BlitPass : ScriptableRenderPass
    {  
        public RTHandle colorHandle;
        protected RTHandle intermediateHandle;
        public Material mat;
        protected ProfilingSampler sampler = new ProfilingSampler("ColorBlit");
        public void Init(RTHandle handle, Material mat){
            colorHandle = handle;
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
            intermediateHandle = RTHandles.Alloc("_MainTex", name: "_MainTex");
            this.mat = mat;
            ConfigVariablesInit(this.mat);
        }
        protected virtual void ConfigVariablesInit(Material m)
        {

        }
        protected virtual void ConfigVariablesRealtime(Material m){

        }
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            //ConfigureTarget(colorHandle);
            cmd.GetTemporaryRT(Shader.PropertyToID(intermediateHandle.name), renderingData.cameraData.cameraTargetDescriptor, FilterMode.Point);
            ConfigureTarget(intermediateHandle);
        }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var camData = renderingData.cameraData;
            if(camData.camera.cameraType != CameraType.Game) return;
            if(mat == null) return;

            CommandBuffer cmd = CommandBufferPool.Get();
            using(new ProfilingScope(cmd, sampler)){
                ConfigVariablesRealtime(this.mat);
                cmd.Blit(colorHandle.rt, intermediateHandle);
                cmd.Blit(intermediateHandle.rt, colorHandle, mat);
            }
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(Shader.PropertyToID(intermediateHandle.name));
        }
    }

    public class Blit : BlitFeature<BlitPass>{

    }

    public abstract class BlitFeature<T> : ScriptableRendererFeature where T : BlitPass, new()
    {
        private static bool RightCamType(CameraData data){
            return data.cameraType == CameraType.Game;
        }

        public Shader refShader;
        T m_ScriptablePass;
        private Material heldMat;
        protected virtual void PreCreate()
        {

        }
        /// <inheritdoc/>
        public override void Create()
        {
            PreCreate();
            if (refShader == null) return;
            heldMat = CoreUtils.CreateEngineMaterial(refShader);
            m_ScriptablePass = new T();
        }

        protected override void Dispose(bool disposing)
        {
            CoreUtils.Destroy(heldMat);
        }

        protected virtual void AdditionalInit(T input){
            
        }


        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (heldMat == null) return;
            if(RightCamType(renderingData.cameraData)){
                renderer.EnqueuePass(m_ScriptablePass);
            }
        }

        public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
        {
            if (heldMat == null) return;
            if(RightCamType(renderingData.cameraData)){
                m_ScriptablePass.ConfigureInput(ScriptableRenderPassInput.Color);
                AdditionalInit(m_ScriptablePass);
                m_ScriptablePass.Init(renderer.cameraColorTargetHandle, heldMat);
            }
        }
    }
}

