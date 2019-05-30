/////////////////////////////////////////////////
/// Celeste Sky.
///----------------------------------------------
/// Utility.
///----------------------------------------------
/// Refresh Reflection Probe
///----------------------------------------------
/// Description: Refresh reflection probe.
/////////////////////////////////////////////////

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace CelesteSky.Utility
{
    [ExecuteInEditMode]
    public class csky_RefreshReflectionProbe : csky_Refresh
    {

        // Custom Render Texture.
        [SerializeField] protected bool m_UseRenderTexture = false;
        [SerializeField] protected RenderTexture m_RenderTexture = null;

        // Target.
        protected ReflectionProbe m_Probe = null;

        protected void Awake()
        {
            m_Probe             = GetComponent<ReflectionProbe>();
            m_Probe.mode        = ReflectionProbeMode.Realtime;
            m_Probe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
        }

        protected override void Refresh()
        {
            if(m_UseRenderTexture)
                m_Probe.RenderProbe(m_RenderTexture);
            else
                m_Probe.RenderProbe(null);
        }  

        /// <summary></summary>
        public bool SetRenderTexture
        {
            get => m_UseRenderTexture; 
            set => m_UseRenderTexture = value;
        }

        /// <summary></summary>
        public RenderTexture RenderTexture
        {
            get => m_RenderTexture; 
            set => m_RenderTexture = value;
        }
        
    }
}
