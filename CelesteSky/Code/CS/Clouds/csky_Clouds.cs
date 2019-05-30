/////////////////////////////////////////////////
/// Celeste SKy
///----------------------------------------------
/// Clouds
///----------------------------------------------
/// Description: Clouds
/////////////////////////////////////////////////

using System;
using UnityEngine;
using CelesteSky.Utility;

namespace CelesteSky
{
    /// <summary></summary>
    [Serializable] public class csky_CloudsParams
    {
        // Texture.
        public Texture2D tex;
        public Vector2 texSize;
        public Vector2 texOffset;

        // Color.
        public Color tint;
        public Gradient sunGradient, moonGradient;
        public float intensity;

        // Density.
        public float density;
        [Range(0.0f, 1.0f)] public float coverage;

        public float speed, speed2;
    }

    /// <summary></summary>
    [Serializable] public class csky_Clouds
    {

        [SerializeField] private csky_CloudsParams m_Parameters = new csky_CloudsParams
        {
            tex       = null,
            texSize   = Vector2.one,
            texOffset = Vector2.zero,
            tint      = Color.white,
            sunGradient  = new Gradient(),
            moonGradient = new Gradient(),
            intensity = 1.0f,
            density   = 0.3f,
            coverage  = 0.5f,
            speed     = 0.01f,
            speed2    = 0.05f
        };

        /// <summary></summary>
        public bool EnableMoonContribution{ get; set; }

        /// <summary></summary>
        public float SunEvaluateTime{ get; set; }

        /// <summary></summary>
        public float MoonEvaluateTime{ get; set; }

        public Color SunCol => m_Parameters.sunGradient.Evaluate(SunEvaluateTime);
        public Color MoonCol => m_Parameters.moonGradient.Evaluate(MoonEvaluateTime);

        public Color CloudsTint
        {
            get
            {
                Color re;
                {
                    if(EnableMoonContribution)
                        re = SunCol + MoonCol;
                    else
                        re = SunCol;

                    re *= m_Parameters.tint;
                }
                return re;
            }
        }

        /// <summary></summary>
        public void SetParams(Material material)
        {
            material.SetTexture(csky_PropertyIDs.c_TexID, m_Parameters.tex);
            material.SetTextureScale(csky_PropertyIDs.c_TexID, m_Parameters.texSize);
            material.SetTextureOffset(csky_PropertyIDs.c_TexID, m_Parameters.texOffset);

            material.SetColor(csky_PropertyIDs.c_TintID, CloudsTint);
            material.SetFloat(csky_PropertyIDs.c_IntensityID, m_Parameters.intensity);

            material.SetFloat(csky_PropertyIDs.c_DensityID, m_Parameters.density);
            material.SetFloat(csky_PropertyIDs.c_CoverageID, m_Parameters.coverage);

            material.SetFloat(csky_PropertyIDs.c_SpeedID, m_Parameters.speed);
            material.SetFloat(csky_PropertyIDs.c_Speed2ID, m_Parameters.speed2);
        }

    } 
}