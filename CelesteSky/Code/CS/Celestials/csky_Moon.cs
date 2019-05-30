/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Near Space.
///----------------------------------------------
/// Moon
///----------------------------------------------
/// Moon Params.
/////////////////////////////////////////////////

using System;
using UnityEngine;
using CelesteSky.Utility;

namespace CelesteSky
{

    /// <summary></summary>
    [Serializable] public class csky_MoonParams
    {
        public Texture2D tex;
        public Vector2 texOffsets;
        public Color tint;
        public float intensity;
        [Range(0.0f, 1.0f)] public float contrast;
    }
    
    /// <summary></summary>
    [Serializable] public class csky_Moon
    {
        
        [SerializeField] private csky_MoonParams m_Params = new csky_MoonParams
        {
            tex        = null,
            texOffsets = Vector2.zero,
            tint       = Color.white,
            intensity  = 1.0f,
            contrast   = 0.3f
        };

        /// <summary></summary>
        public void SetParams(Material material)
        {
            material.SetTexture(csky_PropertyIDs.m_TexID, m_Params.tex);
            material.SetTextureOffset(csky_PropertyIDs.m_TexID, m_Params.texOffsets);
            material.SetColor(csky_PropertyIDs.m_TintID, m_Params.tint);
            material.SetFloat(csky_PropertyIDs.m_IntensityID, m_Params.intensity);
            material.SetFloat(csky_PropertyIDs.m_ContrastID, m_Params.contrast);
        }
    }
    
}
