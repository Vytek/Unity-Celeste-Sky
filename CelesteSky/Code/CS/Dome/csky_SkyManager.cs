/////////////////////////////////////////////////
/// Celeste Sky 
///----------------------------------------------
/// Skydome Manager
///----------------------------------------------
/// Manager for skydome
/////////////////////////////////////////////////

using System;
using UnityEngine;

namespace CelesteSky
{
    [RequireComponent(typeof(csky_Dome)), ExecuteInEditMode]
    public class csky_SkyManager : MonoBehaviour
    {
    #region [References]
        [SerializeField] private csky_Dome m_Dome = null;
    #endregion

    #region [Elements]

        public csky_General general = new csky_General();

        public csky_AtmosphericScattering atmosphericScattering = new csky_AtmosphericScattering();

        public csky_DeepSpace deepSpace = new csky_DeepSpace();

        public csky_Sun sun = new csky_Sun();
        public csky_Moon moon = new csky_Moon();

        public csky_Clouds clouds = new csky_Clouds();

    #endregion

    #region [Initialize]

        private void Awake()
        {
            m_Dome = GetComponent<csky_Dome>();
        }

        private void Start()
        {
           // if(!m_Dome.IsReady) return;

            general.Initialize(m_Dome._Transform);
            atmosphericScattering.Initialize();
        }

    #endregion

    #region [Update]

        private void Update()
        {
            if(!m_Dome.IsReady) return;

            general.SetParams(m_Dome.SunDirection, m_Dome.MoonDirection, m_Dome.LocalSunDirection, m_Dome.LocalMoonDirection);

            atmosphericScattering.SunDir  = m_Dome.LocalSunDirection;
            atmosphericScattering.MoonDir = m_Dome.LocalMoonDirection;
            atmosphericScattering.SunEvaluteTime = m_Dome.EvaluateTimeBySun;
            atmosphericScattering.SetGlobalParams();

            if(atmosphericScattering.MoonRayleighMode == csky_MoonRayleighMode.CelestialContribution)
                m_Dome.EnableMoonContribution = true;

            deepSpace.SetParams(m_Dome.Resources.deepSpaceMaterial, m_Dome.EvaluateTimeBySun);
            sun.SetParams(m_Dome.Resources.sunMaterial);
            moon.SetParams(m_Dome.Resources.moonMaterial);

            clouds.EnableMoonContribution = m_Dome.EnableMoonContribution;
            clouds.SunEvaluateTime = m_Dome.EvaluateTimeBySun;
            clouds.MoonEvaluateTime = m_Dome.EvaluateTimeByMoon;
            clouds.SetParams(m_Dome.Resources.cloudsMaterial);
        }

    #endregion
    }
}