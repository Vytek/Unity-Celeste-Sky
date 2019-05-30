/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Global.
///----------------------------------------------
/// Description:
/// General properties.
/////////////////////////////////////////////////
using System;
using UnityEngine;

namespace CelesteSky
{
    [Serializable] public class csky_General
    {

        [SerializeField] 
        private float m_GlobalExposure = 1.3f;
        public float GlobalExposure => m_GlobalExposure;
        
        private Transform m_Transform;

        /// <summary></summary>
        public void Initialize(Transform transform)
        {
            m_Transform = transform;
        }

        /// <summary> Set parameters to material. </summary>
        public void SetParams()
        {
            Shader.SetGlobalFloat(csky_PropertyIDs.g_ExposureID, m_GlobalExposure);
            Shader.SetGlobalMatrix(csky_PropertyIDs.g_ObjectToWorldID, m_Transform.localToWorldMatrix);
            Shader.SetGlobalMatrix(csky_PropertyIDs.g_WorldToObjectID, m_Transform.worldToLocalMatrix);
        }

        /// <summary> Set parameters to material. </summary>
        /// <param="sunDir"> Sun Direction(-Vector3.forward).  </param>
        /// <param="moonDir"> Moon Direction(-Vector3.forward).  </param>
        public void SetParams(Vector3 sunDir, Vector3 moonDir, Vector3 localSunDir, Vector3 localMoonDir)
        {
         
            SetParams();
            Shader.SetGlobalVector(csky_PropertyIDs.g_WorldSunDirectionID, sunDir);
            Shader.SetGlobalVector(csky_PropertyIDs.g_LocalSunDirectionID, localSunDir);
            Shader.SetGlobalVector(csky_PropertyIDs.g_WorldMoonDirectionID, moonDir);
            Shader.SetGlobalVector(csky_PropertyIDs.g_LocalMoonDirectionID, localMoonDir);
        }

    }
}