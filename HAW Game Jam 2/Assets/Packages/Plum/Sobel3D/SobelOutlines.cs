using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plum.Rendering.URP.Sobel
{
    public enum SobelMode
    {
        FULL = 0,
        DEPTHTEX = 1,
        NORMALTEX = 2,
        DEPTHSOBEL = 3,
        NORMALSOBEL = 4,
        NORMALDOT = 5,
        NOISELUT = 6,
        ALBEDOTEX = 7,
        ALBEDOSOBEL = 8,
        DEPTHCORRECTION = 9
    }

    [System.Serializable]
    public class SobelValues
    {
        public SobelMode mode = SobelMode.FULL;
        public bool useCorrectedDepth = true;
        [Range(0, 1)] public float finalIntensity = .1f;
        public Color sobelColor;
        [Range(0.001f, 4.0f)] public float sobelExtrusion = 1.0f;
        public float noiseExtrusion = .1f;
        [Range(0, 1)] public float sobelSmoothness = 0;
        public Texture2D noiseLUT;
        [Range(0, 1)] public float noiseRange = 0.0f;
        [Range(0, .505f)] public float edgeDepth = .5f;
        [Range(0, 14)] public float edgeNormal = 1;
        [Range(0, 4)] public float edgeAlbedo = 1.0f;

        [Range(0, 1)] public float depthT = 1.0f;
        [Range(0, 1)] public float normalT = .5f;
        [Range(0, 1)] public float albedoT = .1f;
    }
    public class SobelPass : BlitPass
    {
        private SobelValues values;
        public void RecieveValues(SobelValues v)
        {
            values = v;
        }

        protected override void ConfigVariablesInit(Material m)
        {
            m.SetFloat("_Mode", (float)values.mode);
            m.SetFloat("_UDepth", values.useCorrectedDepth? 1.0f : 0.0f);
            m.SetFloat("_Lerp", values.finalIntensity);
            m.SetColor("_color", values.sobelColor);
            m.SetFloat("_Extrusion", values.sobelExtrusion);
            m.SetFloat("_NoiseExtrusion", values.noiseExtrusion);
            m.SetFloat("_Smoothness", values.sobelSmoothness);
            if (values.noiseLUT != null) m.SetTexture("_NoiseLUT", values.noiseLUT);
            m.SetFloat("_NoiseT", values.noiseRange);
            m.SetFloat("_edgeDepth", values.edgeDepth);
            m.SetFloat("_edgeNormal", values.edgeNormal);
            m.SetFloat("_edgeAlbedo", values.edgeAlbedo);
            m.SetFloat("_depthT", values.depthT);
            m.SetFloat("_normalT", values.normalT);
            m.SetFloat("_albedoT", values.albedoT);
        }
    }

    public class SobelOutlines : BlitFeature<SobelPass>
    {
        protected override void PreCreate()
        {
            refShader = Shader.Find("Plum/SobelFilter");
        }
        public SobelValues values = new SobelValues();
        protected override void AdditionalInit(SobelPass input)
        {
            input.RecieveValues(values);
        }
    }

}
