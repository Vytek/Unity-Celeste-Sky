/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Utility.
///----------------------------------------------
/// Refresh base.
///----------------------------------------------
/// Refresh function in defined time.
/////////////////////////////////////////////////

using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace CelesteSky.Utility
{

    /// <summary></summary>
    public abstract class csky_Refresh : MonoBehaviour
    {

        [SerializeField] protected float m_RefreshTime = 0.5f;

        /// <summary></summary>
        public float RefreshTime
        {
            get 
            {
               m_RefreshTime = Mathf.Clamp(m_RefreshTime, 0.0f, m_RefreshTime);
               return m_RefreshTime;
            } 
            set => m_RefreshTime = value;
        }

        protected float m_Timer = 0.0f;
        protected virtual void Update()
        {
            m_Timer += Time.deltaTime;
            
            if(m_Timer >= RefreshTime)
            {
                Refresh();
                m_Timer = 0;
            }
           
        }

        /// <summary> Refresh in defined time </summary>
        protected abstract void Refresh();

       

    }

}
