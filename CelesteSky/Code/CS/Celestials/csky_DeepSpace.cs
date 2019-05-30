/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Celestials
///----------------------------------------------
/// Deep Space
///----------------------------------------------
/// Galaxy background and stars field.
/////////////////////////////////////////////////

using System;
using UnityEngine;
using CelesteSky.Utility;

namespace CelesteSky
{

    [Serializable] public class csky_DeepSpaceParams
    {
        public Cubemap cubemap;
        
        [ColorUsage(false, true)] public Color tint;
        public float intensity;
    }

    [Serializable] public class csky_GalaxyBackgroundParams : csky_DeepSpaceParams
    {
        [Range(0.0f, 1.0f)] public float contrast;
    }

    [Serializable] public class csky_StarsFieldParams : csky_DeepSpaceParams
    {
        
        // Scintillation.
        public Cubemap noiseCubemap;

        [Range(0.0f, 1.0f)] 
        public float scintillation;
        public float scintillationSpeed;
    }

    /// <summary><summary>
    [Serializable] public class csky_DeepSpace
    {

        [SerializeField] private csky_GalaxyBackgroundParams m_GalaxyParams = new csky_GalaxyBackgroundParams
        {
            cubemap   = null,
            tint      = Color.white,
            intensity = 0.5f,
            contrast  = 0.3f
        };

        [SerializeField] private csky_StarsFieldParams m_StarsParams = new csky_StarsFieldParams
        {
            cubemap   = null,
            tint      = Color.white,
            intensity = 1.0f,
            noiseCubemap        = null,
            scintillation       = 1.0f,
            scintillationSpeed  = 7.0f   
        };

        [csky_AnimationCurveRange(0.0f, 0.0f, 1.0f, 1.0f, 0)]
        [SerializeField] private AnimationCurve m_IntensityMultiplier = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 1.0f);

        private float m_StarsFieldNoiseXAngle;

        /// <summary> Set parameters to material. </summary>
        public void SetParams(Material material, float evaluateTime)
        {
            // Galaxy Background.
            material.SetTexture(csky_PropertyIDs.gb_CubemapID, m_GalaxyParams.cubemap);
            material.SetColor(csky_PropertyIDs.gb_TintID, m_GalaxyParams.tint);
            material.SetFloat(csky_PropertyIDs.gb_IntensityID, m_GalaxyParams.intensity * m_IntensityMultiplier.Evaluate(evaluateTime));
            material.SetFloat(csky_PropertyIDs.gb_ContrastID, m_GalaxyParams.contrast);

            // Stars Field.
            material.SetTexture(csky_PropertyIDs.sf_CubemapID, m_StarsParams.cubemap);
            material.SetTexture(csky_PropertyIDs.sf_NoiseCubemapID, m_StarsParams.noiseCubemap);
            material.SetColor(csky_PropertyIDs.sf_TintID, m_StarsParams.tint);
            material.SetFloat(csky_PropertyIDs.sf_IntensityID, m_StarsParams.intensity * m_IntensityMultiplier.Evaluate(evaluateTime));
            material.SetFloat(csky_PropertyIDs.sf_ScintillationID, m_StarsParams.scintillation);
            material.SetFloat(csky_PropertyIDs.sf_ScintillationSpeedID, m_StarsParams.scintillationSpeed);

            // Scroll the x Axis of the noise cubemap.
            m_StarsFieldNoiseXAngle += Time.deltaTime * m_StarsParams.scintillationSpeed;
            Matrix4x4 starsFieldNoiseMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(m_StarsFieldNoiseXAngle, 0.0f, 0.0f), Vector3.one);
            material.SetMatrix(csky_PropertyIDs.sf_NoiseMatrixID, starsFieldNoiseMatrix);
        }
    }
}