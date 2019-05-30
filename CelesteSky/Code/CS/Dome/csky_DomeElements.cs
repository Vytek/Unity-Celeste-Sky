/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Skydome.
///----------------------------------------------
/// Elements.
///----------------------------------------------
/// Skydome elements.
/////////////////////////////////////////////////

using System;
using UnityEngine;
using CelesteSky.Utility;

namespace CelesteSky
{
    public partial class csky_Dome : MonoBehaviour
    {
    #region [Fields]

        // General Settings. 
        /////////////////////
        [Header("General Settings")]
        [SerializeField] float m_DomeRadius = 10000f;

        /// <summary></summary>
        public float OldDomeRadius{ get; private set; }

        // Atmosphere.
        //////////////////////
        [Header("Atmosphere")]
        [SerializeField] bool m_RenderAtmosphere = true;
        [SerializeField] csky_Quality4 m_AtmosphereMeshQuality = csky_Quality4.Ultra;
        [SerializeField, Range(0, 31)] int m_AtmosphereLayerIndex = 0;

        // Deep Space-
        //////////////////////
        [Header("Deep Space")]
        [SerializeField] bool m_RenderDeepSpace = true;
        [SerializeField, Range(0, 31)] int m_DeepSpaceLayerIndex = 0;
        [SerializeField] bool m_CustomDeepSpaceRotation = false;
        [SerializeField] Vector3 m_DeepSpaceEuler = Vector3.zero;
        Quaternion m_DeepSpaceRotation;

        // Near Space.
        //////////////////////
        [Header("NearSpace")]
        [SerializeField] bool m_RenderSun = true;
        [SerializeField, Range(0, 31)] int m_SunLayerIndex = 0;

        [SerializeField] float m_SunMeshSize = 0.025f;
        [SerializeField] float m_SunAzimuth = 0.0f, m_SunAltitude = 0.0f;

        // Moon
        [SerializeField] bool m_RenderMoon = true;
        [SerializeField, Range(0, 31)] int m_MoonLayerIndex = 0;

        [SerializeField] float m_MoonMeshSize = 0.025f;
        [SerializeField] float m_MoonAzimuth = 0.0f, m_MoonAltitude = 0.0f;

        Vector3 m_OldSunPos,  m_OldSunScale, 
                m_OldMoonPos, m_OldMoonScale;
        
        // Clouds
        //////////////////////
        [Header("Clouds")]
        [SerializeField] bool m_RenderClouds = true;
        [SerializeField, Range(0, 31)] int m_CloudsLayerIndex = 0;
        [SerializeField, Range(0, 1)] float m_CloudsDomeHeight = 0.5f;
        [SerializeField] float m_CloudsAltitude = 0.0f;
        [SerializeField] float m_CloudsOrientation = 0.0f;

    #endregion

    #region [References]

        // Transforms.
        ////////////////

        Transform m_Transform = null;

        /// <summary>Skydome Transform</summary>
        public Transform _Transform => m_Transform;
        

        // Atmosphere.
        private csky_EmptyObjectReference m_AtmosphereTrRef = new csky_EmptyObjectReference();

        // Deep Space.
        private csky_EmptyObjectReference m_GalaxyBrgTrRef  = new csky_EmptyObjectReference();
        private csky_EmptyObjectReference m_StarsFieldTrRef = new csky_EmptyObjectReference();

        // Near Space.
        private csky_EmptyObjectReference m_SunTrRef        = new csky_EmptyObjectReference();
        private csky_EmptyObjectReference m_MoonTrRef       = new csky_EmptyObjectReference();

        // Clouds.
        private csky_EmptyObjectReference m_CloudsTrRef = new csky_EmptyObjectReference();

        /// <summary></summary>
        bool CheckTransformReferences
        {
            get
            {
                if(!m_AtmosphereTrRef.CheckComponents) return false;
                if(!m_GalaxyBrgTrRef.CheckComponents)  return false;
                if(!m_StarsFieldTrRef.CheckComponents) return false;
                if(!m_SunTrRef.CheckComponents)        return false;
                if(!m_MoonTrRef.CheckComponents)       return false;
                if(!m_CloudsTrRef.CheckComponents)     return false;
                return true;
            }
        }

    #endregion

    #region [Properties]

        /// <summary></summary>
        public Vector3 DomeRadius3D => Vector3.one * m_DomeRadius;

        /// <summary> Get sun direction: SunMatrix * Vector3.forward. </summary>
        public Vector3 SunDirection => -m_SunTrRef.transform.forward;

        /// <summary> Get local sun direction. </summary>
        public Vector3 LocalSunDirection => m_Transform.InverseTransformDirection(SunDirection);

        /// <summary> Get moon direction: MoonMatrix * Vector3.forward. </summary>
        public Vector3 MoonDirection => -m_MoonTrRef.transform.forward;

        /// <summary> Get local moon direction </summary>
        public Vector3 LocalMoonDirection => m_Transform.InverseTransformDirection(MoonDirection);

        /// <summary></summary>
        public Vector3 SunPosition => csky_Mathf.SphericalToCartesian(m_SunAltitude, m_SunAzimuth);

        /// <summary></summary>
        public Vector3 SunScale => Vector3.one * m_SunMeshSize;

        /// <summary></summary>
        public Vector3 MoonPosition => csky_Mathf.SphericalToCartesian(m_MoonAltitude, m_MoonAzimuth);
        
        /// <summary></summary>
        public Vector3 MoonScale => Vector3.one * m_MoonMeshSize;

        /// <summary></summary>
        public bool IsDay
        {
            get
            {
                if(Mathf.Abs(m_SunAltitude) > 1.7f)
                    return false;

                return true;
            }
        }
    #endregion

    #region [Methods]
        private void InstantiateTransformReferences()
        {
            m_AtmosphereTrRef.Instantiate(this.name, "Atmosphere Transform");
            m_AtmosphereTrRef.InitTransform(_Transform, Vector3.zero);

            m_GalaxyBrgTrRef.Instantiate(this.name, "Galaxy Background Transform");
            m_GalaxyBrgTrRef.InitTransform(_Transform, Vector3.zero);

            m_StarsFieldTrRef.Instantiate(this.name, "Stars Field Transform");
            m_StarsFieldTrRef.InitTransform(_Transform, Vector3.zero);

            m_SunTrRef.Instantiate(this.name, "Sun Transform");
            m_SunTrRef.InitTransform(_Transform, Vector3.zero);

            m_MoonTrRef.Instantiate(this.name, "Moon Transform");
            m_MoonTrRef.InitTransform(_Transform, Vector3.zero);

            m_CloudsTrRef.Instantiate(this.name, "Clouds Transform");
            m_CloudsTrRef.InitTransform(_Transform, Vector3.zero);
        }

        public void UpdateDomeTransform()
        {

            // Scale Dome.
            if(OldDomeRadius != m_DomeRadius)
            {
                _Transform.localScale = DomeRadius3D;
                OldDomeRadius = m_DomeRadius;
            }

            // Celestials Transform.
            if(m_OldSunPos != SunPosition)
            {
                m_SunTrRef.transform.localPosition = SunPosition;
                m_SunTrRef.transform.LookAt(_Transform, Vector3.forward);
                m_OldSunPos = SunPosition;
            }

            if(m_OldSunScale != SunScale)
            {
                m_SunTrRef.transform.localScale = SunScale;
                m_OldSunScale = SunScale;
            }

            if(m_OldMoonPos != MoonPosition)
            {
                m_MoonTrRef.transform.localPosition = MoonPosition;
                m_MoonTrRef.transform.LookAt(_Transform, Vector3.forward);
                m_OldMoonPos = MoonPosition;
            }

            if(m_OldMoonScale != MoonScale)
            {
                m_MoonTrRef.transform.localScale = MoonScale;
                m_OldMoonScale = MoonScale;
            }

            // Outer Space Rotation.
            if(!m_CustomDeepSpaceRotation)
            {
                m_DeepSpaceRotation = Quaternion.Euler(m_DeepSpaceEuler);
            }

            if(m_RenderDeepSpace)
                m_GalaxyBrgTrRef.transform.localRotation = m_DeepSpaceRotation;

            // Clouds transform.
            Vector3 cloudsPosition;
            cloudsPosition.x = m_CloudsTrRef.transform.localPosition.x;
            cloudsPosition.y = m_CloudsAltitude;
            cloudsPosition.z = m_CloudsTrRef.transform.localPosition.z;
            m_CloudsTrRef.transform.localPosition = cloudsPosition;

            m_CloudsTrRef.transform.localRotation = Quaternion.Euler(
                m_CloudsTrRef.transform.localEulerAngles.x,
                m_CloudsOrientation,
                m_CloudsTrRef.transform.localEulerAngles.z
            );

            Vector3 cloudsScale;
            cloudsScale.x = m_CloudsTrRef.transform.localScale.x;
            cloudsScale.y = m_CloudsDomeHeight;
            cloudsScale.z = m_CloudsTrRef.transform.localScale.z;
            m_CloudsTrRef.transform.localScale = cloudsScale;
        }
    #endregion

    #region [Accessors]
        /// <summary></summary>
        public Quaternion DeepSpaceRotation
        {
            get => m_DeepSpaceRotation;
            set
            {
                if(m_CustomDeepSpaceRotation)
                {
                    m_DeepSpaceRotation = value;
                }
            }
        }

        public float SunAltitude
        {
            get => m_SunAltitude;
            set => m_SunAltitude = value;
        }

        public float SunAzimuth
        {
            get => m_SunAzimuth;
            set => m_SunAzimuth = value;
        }

        public float MoonAltitude
        {
            get => m_MoonAltitude;
            set => m_MoonAltitude = value;
        }

        public float MoonAzimuth
        {
            get => m_MoonAzimuth;
            set => m_MoonAzimuth = value;
        }
    #endregion
    }
}