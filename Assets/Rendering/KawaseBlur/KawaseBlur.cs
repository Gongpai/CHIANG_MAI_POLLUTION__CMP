using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
namespace GDD
{
    public class KawaseBlur : ScriptableRendererFeature
    {
        [Serializable]
        public class KawaseBlurSettings
        {
            public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
            public Material blurMaterial = null;

            [Range(0, 15)] public float blurPasses = 1;
        
            [Range(1, 4)] public float downsample = 1;

            [Range(0, 2)] public float offset_first_pass = 1.5f;
            
            [Range(0, 1)] public float offset_loop_pass = 1f;
            
            public bool copyToFramebuffer;
            public string targetName = "_blurTexture";
        }

        public KawaseBlurSettings settings = new KawaseBlurSettings();

        public class CustomRenderPass : ScriptableRenderPass
        {
            public Material blurMaterial;
            public float passes;
            public float downsample;
            public float offsetfirstpass;
            public float offsetlooppass;
            public bool copyToFramebuffer;
            public string targetName;
            string profilerTag;

            int tmpId1;
            int tmpId2;

            RenderTargetIdentifier tmpRT1;
            RenderTargetIdentifier tmpRT2;

            private RenderTargetIdentifier source { get; set; }

            public void Setup(RenderTargetIdentifier source)
            {
                this.source = source;
            }

            public CustomRenderPass(string profilerTag)
            {
                this.profilerTag = profilerTag;
            }

            public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
            {
                var width = cameraTextureDescriptor.width / downsample;
                var height = cameraTextureDescriptor.height / downsample;

                tmpId1 = Shader.PropertyToID("tmpBlurRT1");
                tmpId2 = Shader.PropertyToID("tmpBlurRT2");
                cmd.GetTemporaryRT(tmpId1, (int)width, (int)height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);
                cmd.GetTemporaryRT(tmpId2, (int)width, (int)height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32);

                tmpRT1 = new RenderTargetIdentifier(tmpId1);
                tmpRT2 = new RenderTargetIdentifier(tmpId2);

                ConfigureTarget(tmpRT1);
                ConfigureTarget(tmpRT2);
            }

            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                CommandBuffer cmd = CommandBufferPool.Get(profilerTag);

                RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
                opaqueDesc.depthBufferBits = 0;

                // first pass
                //cmd.GetTemporaryRT(tmpId1, opaqueDesc, FilterMode.Bilinear);
                cmd.SetGlobalFloat("_offset", offsetfirstpass);
                cmd.Blit(source, tmpRT1, blurMaterial);

                for (var i = 1; i < passes - 1; i++)
                {
                    cmd.SetGlobalFloat("_offset", offsetlooppass + i);
                    cmd.Blit(tmpRT1, tmpRT2, blurMaterial);

                    // pingpong
                    var rttmp = tmpRT1;
                    tmpRT1 = tmpRT2;
                    tmpRT2 = rttmp;
                }

                
                // final pass
                cmd.SetGlobalFloat("_offset", offsetlooppass + passes - 1f);
                if (copyToFramebuffer)
                {
                    cmd.Blit(tmpRT1, source, blurMaterial);
                }
                else
                {
                    cmd.Blit(tmpRT1, tmpRT2, blurMaterial);
                    cmd.SetGlobalTexture(targetName, tmpRT2);
                }

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                CommandBufferPool.Release(cmd);
            }

            public override void FrameCleanup(CommandBuffer cmd)
            {
            }
        }

        CustomRenderPass scriptablePass;

        public override void Create()
        {
            scriptablePass = new CustomRenderPass("KawaseBlur");
            scriptablePass.blurMaterial = settings.blurMaterial;
            scriptablePass.passes = settings.blurPasses;
            scriptablePass.downsample = settings.downsample;
            scriptablePass.offsetfirstpass = settings.offset_first_pass;
            scriptablePass.offsetlooppass = settings.offset_loop_pass;
            scriptablePass.copyToFramebuffer = settings.copyToFramebuffer;
            scriptablePass.targetName = settings.targetName;

            scriptablePass.renderPassEvent = settings.renderPassEvent;
        }

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            var src = renderer.cameraColorTarget;
            scriptablePass.Setup(src);
            renderer.EnqueuePass(scriptablePass);
        }
    }
}


