using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace GDD
{
    public class KawaseBlur_Animation_Control : MonoBehaviour
    {
        [SerializeField] private int Blur_URP_index = 1;
        [SerializeField] public KawaseBlur_Setting settings;
        private KawaseBlur _kawaseBlur;
        
        [Serializable]
        public struct KawaseBlur_Setting
        {
            [Range(0, 15)] public float blurPasses;
            [Range(1, 4)] public float downsample;
            [Range(0, 2)] public float offset_first_pass;
            [Range(0, 1)] public float offset_loop_pass;

            public KawaseBlur_Setting(float _blurPasses, float _downsample, float _offset_first_pass, float _offset_loop_pass)
            {
                blurPasses = _blurPasses;
                downsample = _downsample;
                offset_first_pass = _offset_first_pass;
                offset_loop_pass = _offset_loop_pass;
            }
        }
        
        public KawaseBlur get_kawaseBlur 
        {
            get
            {
                if (_kawaseBlur == null)
                {
                    var renderer =
                        (GraphicsSettings.currentRenderPipeline as UniversalRenderPipelineAsset).GetRenderer(Blur_URP_index);
                    var property = typeof(ScriptableRenderer).GetProperty("rendererFeatures",
                        BindingFlags.NonPublic | BindingFlags.Instance);

                    List<ScriptableRendererFeature> features =
                        property.GetValue(renderer) as List<ScriptableRendererFeature>;
                    Parallel.ForEach(features, feature =>
                    {
                        if (feature.GetType() == typeof(KawaseBlur_For_Animation))
                        {
                            _kawaseBlur = feature as KawaseBlur_For_Animation;
                        }
                    });
                }
                
                return _kawaseBlur;
            }
        }

        private void Start()
        {
            _kawaseBlur = get_kawaseBlur;
        }

        private void Update()
        {
            _kawaseBlur = get_kawaseBlur;
            _kawaseBlur.Create();
            _kawaseBlur.settings.blurPasses = settings.blurPasses;
            _kawaseBlur.settings.downsample = settings.downsample;
            _kawaseBlur.settings.offset_first_pass = settings.offset_first_pass;
            _kawaseBlur.settings.offset_loop_pass = settings.offset_loop_pass;
        }
    }
}