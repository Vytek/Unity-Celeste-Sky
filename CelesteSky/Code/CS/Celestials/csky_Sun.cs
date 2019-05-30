/////////////////////////////////////////////////
/// Celeste SKy
///----------------------------------------------
/// Near Space.
///----------------------------------------------
/// Sun.
///----------------------------------------------
/// Sun Params.
/////////////////////////////////////////////////

using System;
using UnityEngine;
using CelesteSky.Utility;

namespace CelesteSky
{

    [Serializable] public class csky_SunParams
    {
        public Texture2D tex;

        [ColorUsage(false, true)] public Color tint;
        public float intensity;
    }

    [Serializable] public class csky_Sun
    {

        [SerializeField] private csky_SunParams m_Params = new csky_SunParams
        {
            tex       = null,
            tint      = Color.white,
            intensity = 1.0f
        };

        /// <summary></summary>
        public void SetParams(Material material)
        {
            material.SetTexture(csky_PropertyIDs.s_TexID, m_Params.tex);
            material.SetColor(csky_PropertyIDs.s_TintID, m_Params.tint);
            material.SetFloat(csky_PropertyIDs.s_IntensityID, m_Params.intensity);
        }
    }

}