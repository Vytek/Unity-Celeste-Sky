////////////////////////////////////////////////////////
/// Celeste Sky
///-----------------------------------------------------
/// Celestial Orbital Elements
///-----------------------------------------------------
/// Orbital Elements for celestials
/// calculations.
/// All calculations are based on Paul Schlyter papers.
/// See: http://www.stjarnhimlen.se/comp/ppcomp.html
/// See: http://stjarnhimlen.se/comp/tutorial.html
////////////////////////////////////////////////////////

using System;
using UnityEngine;

namespace CelesteSky
{
    public struct csky_OrbitalElements
    {
        /// <summary>
        /// Longitude of the ascending node.
        /// </summary>
        public float N;

        /// <summary>
        /// The Inclination to the ecliptic.
        /// </summary>
        public float i;

        /// <summary>
        /// Argument of perihelion.
        /// </summary>
        public float w;

        /// <summary>
        /// Semi-major axis, or mean distance from sun.
        /// </summary>
        public float a;

        /// <summary>
        /// Eccentricity.
        /// </summary>
        public float e;

        /// <summary>
        /// Mean anomaly.
        /// </summary>
        public float M;

        /// <summary></summary>
        public csky_OrbitalElements(float _N, float _i, float _w, float _a, float _e, float _M)
        {
            this.N = _N;
            this.i = _i;
            this.w = _w;
            this.a = _a;
            this.e = _e;
            this.M = _M;
        }

        static readonly csky_OrbitalElements m_Zero = new csky_OrbitalElements
        {
            N = 0.0f, 
            i = 0.0f, 
            w = 0.0f, 
            a = 0.0f, 
            e = 0.0f, 
            M = 0.0f
        };

        /// <summary></summary>
        public static csky_OrbitalElements Zero{ get{ return m_Zero; } }

        public static csky_OrbitalElements SetOrbitalElements(int index, float timeScale)
        {
            csky_OrbitalElements re = m_Zero;

            switch(index)
            {
                // Sun.
                /////////////
                case 0: 

                re.N = 0.0f;
                re.i = 0.0f;
                re.w = 282.9404f + 4.70935e-5f   * timeScale;
                re.a = 0.0f;
                re.e = 0.016709f - 1.151e-9f     * timeScale;
                re.M = 356.0470f + 0.9856002585f * timeScale;

                break;

                // Moon.
                //////////////
                case 1:

                re.N = 125.1228f - 0.0529538083f * timeScale;
                re.i = 5.1454f;
                re.w = 318.0634f + 0.1643573223f * timeScale;
                re.a = 60.2666f; 
                re.e = 0.054900f;  
                re.M = 115.3654f + 13.0649929509f * timeScale;

                break;
            }
            return re;
        }

    }
}