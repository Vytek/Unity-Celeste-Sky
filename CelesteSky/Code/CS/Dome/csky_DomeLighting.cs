/////////////////////////////////////////////////
/// Celeste Sky 
///----------------------------------------------
/// Skydome.
///----------------------------------------------
/// Lighting.
///----------------------------------------------
/// Skydome Lighting.
/////////////////////////////////////////////////

using System;
using UnityEngine;
using CelesteSky.Utility;

namespace CelesteSky
{

    [Serializable] public struct csky_LightParams
    {
        public float intensity;
        public Gradient color;
    }

    [Serializable] public class csky_AmbientParams
    {
        public bool sendSkybox;

        public UnityEngine.Rendering.AmbientMode ambientMode;

        public float updateTime;
        public Gradient skyColor, equatorColor, groundColor;
        public Gradient skyMoonColor, equatorMoonColor, groundMoonColor;
    }

    public partial class csky_Dome : MonoBehaviour
    {
   
        [Header("Lighting")]

        // Directional Light.
        //////////////////////
        [SerializeField] Light m_DirLight = null;
        [SerializeField] float m_DirLightRefreshTime = 0.5f;
        Transform m_DirLightTransform;

        [SerializeField] csky_LightParams m_SunParams = new csky_LightParams
        {
            intensity = 1.0f,
            color     = new Gradient()
        };

        [SerializeField] csky_LightParams m_MoonParams = new csky_LightParams
        {
            intensity = 0.3f,
            color     = new Gradient()
        };

        [csky_AnimationCurveRange(0.0f, 0.0f, 1.0f, 1.0f, 0)]
        [SerializeField] AnimationCurve m_SunMoonLightFade = AnimationCurve.Linear(0.0f, 0.0f, 1.0f , 1.0f);

        float m_DirLightRefreshTimer;

        // Ambient- 
        ////////////////////
        [SerializeField] csky_AmbientParams m_AmbientParams = new csky_AmbientParams
        {
            sendSkybox       = false,
            ambientMode      = UnityEngine.Rendering.AmbientMode.Flat,
            updateTime       = 0.5f,
            skyColor         = new Gradient(),
            equatorColor     = new Gradient(),
            groundColor      = new Gradient(),
            skyMoonColor     = new Gradient(),
            equatorMoonColor = new Gradient(),
            groundMoonColor  = new Gradient()
        };

        float m_AmbientRefreshTimer;

        /// <summary></summary>
        public bool DirLightEnabled
        {
            get
            {
                if(!IsDay && (Mathf.Abs(MoonAltitude) > 1.7f))
                    return false;

                return true;
            }
        }

        /// <summary></summary>
        public bool EnableMoonContribution{ get; set; }

        void InitDirLight()
        {
            if(m_DirLight != null)
            {
                m_DirLightTransform = m_DirLight.transform;
                m_DirLightTransform.parent = _Transform;
            }
        }

        void UpdateLight()
        {
            if(m_DirLight == null) return;
            
            m_DirLightRefreshTimer += Time.deltaTime;
            if(m_DirLightRefreshTime >= m_DirLightRefreshTimer)
            {
                if(IsDay)
                {
                    m_DirLightTransform.localPosition = SunPosition;
                    m_DirLightTransform.LookAt(_Transform);

                    m_DirLight.color = m_SunParams.color.Evaluate(EvaluateTimeBySun);
                    m_DirLight.intensity = m_SunParams.intensity;
                }
                else
                {
                    m_DirLightTransform.transform.localPosition = MoonPosition;
                    m_DirLightTransform.LookAt(_Transform);
                    m_DirLight.color = m_MoonParams.color.Evaluate(EvaluateTimeByMoon);
                    m_DirLight.intensity = m_MoonParams.intensity * m_SunMoonLightFade.Evaluate(EvaluateTimeBySun);
                }
                m_DirLightRefreshTimer = 0.0f;
            }
            m_DirLight.enabled = DirLightEnabled;
        }

     
        void UpdateAmbient()
        {
            if(m_AmbientParams.sendSkybox)
            {
                RenderSettings.skybox = m_Resources.ambientSkyboxMaterial;
                m_AmbientParams.sendSkybox = false;
            }
            RenderSettings.ambientMode = m_AmbientParams.ambientMode;

            m_AmbientRefreshTimer += Time.deltaTime;
            if(m_AmbientParams.updateTime >= m_AmbientRefreshTimer)
            {

                switch(m_AmbientParams.ambientMode)
                {
                    case UnityEngine.Rendering.AmbientMode.Flat:

                    RenderSettings.ambientSkyColor = m_AmbientParams.skyColor.Evaluate(EvaluateTimeBySun);

                    // Add moon contribution.
                    if(EnableMoonContribution)
                        RenderSettings.ambientSkyColor += m_AmbientParams.skyMoonColor.Evaluate(EvaluateTimeByMoon);

                    break;

                    case UnityEngine.Rendering.AmbientMode.Trilight:

                    RenderSettings.ambientSkyColor     = m_AmbientParams.skyColor.Evaluate(EvaluateTimeBySun);
                    RenderSettings.ambientEquatorColor = m_AmbientParams.equatorColor.Evaluate(EvaluateTimeBySun);
                    RenderSettings.ambientGroundColor  = m_AmbientParams.groundColor.Evaluate(EvaluateTimeBySun);

                    // Add moon contribution.
                    if(EnableMoonContribution)
                    {
                        RenderSettings.ambientSkyColor     += m_AmbientParams.skyMoonColor.Evaluate(EvaluateTimeByMoon);
                        RenderSettings.ambientEquatorColor += m_AmbientParams.equatorMoonColor.Evaluate(EvaluateTimeByMoon);
                        RenderSettings.ambientGroundColor  += m_AmbientParams.groundMoonColor.Evaluate(EvaluateTimeByMoon);
                    }

                    break;

                    case UnityEngine.Rendering.AmbientMode.Skybox:

                    if(Application.isPlaying)
                        DynamicGI.UpdateEnvironment(); // GC

                    break;

                }
                m_AmbientRefreshTimer = 0.0f;
            }
        }

    }
}