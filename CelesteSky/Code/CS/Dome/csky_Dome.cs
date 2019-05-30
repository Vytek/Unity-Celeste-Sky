/////////////////////////////////////////////////
/// Celeste Sky 
///----------------------------------------------
/// Skydome.
///----------------------------------------------
/// Main.
///----------------------------------------------
/// Skydome Main.
/////////////////////////////////////////////////
using System;
using UnityEngine;
using CelesteSky.Utility;

namespace CelesteSky
{
    [ExecuteAlways]
    public partial class csky_Dome : MonoBehaviour
    {
    #region [Curves And Gradients]

        /// <summary> Evaluate time for curves and gradients in full sun cycle. </summary>
        public float EvaluateTimeBySun => (1.0f - SunDirection.y) * 0.5f; 

        /// <summary> Evaluate time for curves and gradients in above horizon sun cycle. </summary>
        public float EvaluateTimeBySunAboveHorizon => (1.0f - SunDirection.y); 

        /// <summary> Evaluate time for curves and gradient in bellow horizon sun cycle. </summary>
        public float EvaluateTimeBySunBelowHorizon => (1.0f - (-SunDirection.y)); 

        /// <summary> Evaluate time for curves and gradients in full moon cycle. </summary>
        public float EvaluateTimeByMoon => (1.0f - MoonDirection.y) * 0.5f; 

        /// <summary> Evaluate time for curves and gradients in above horizon moon cycle. </summary>
        public float EvaluateTimeByMoonAboveHorizon => (1.0f - MoonDirection.y); 

        /// <summary> Evaluate time for curves and gradient in bellow horizon moon cycle. </summary>
        public float EvaluateTimeByMoonBelowHorizon => (1.0f - (-MoonDirection.y)); 

    #endregion

    #region [Initialize]

        void Awake()
        {
            m_Transform = this.transform;
        }

        void Start()
        {
           
            if(!CheckResources)
            {
                enabled = false;
                IsReady = false;
                return;
            }
            IsReady = CheckResources;
            SetShadersToMaterials();

            if(!CheckTransformReferences)
            {
                InstantiateTransformReferences();
            }
            UpdateDomeTransform();
            InitDirLight();
            InitializeCamera();
        }

    #endregion

    #region [Update]

        void Update()
        {
            if(!IsReady) return;
            UpdateDomeTransform();
            RenderElements();
            UpdateLight();
            UpdateAmbient();
        }

        void LateUpdate()
        {
            if(m_FollowCamera)
                FollowCamera();
        }

    #endregion

        /* 
        public static csky_Dome Instance{ get; private set; }
        private csky_Dome()
        {
            Instance = this;
        }

        void Awake()
        {
            if(Instance != this)
                Destroy(gameObject);
        }*/
    }
}