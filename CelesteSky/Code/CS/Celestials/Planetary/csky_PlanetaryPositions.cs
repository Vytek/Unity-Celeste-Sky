////////////////////////////////////////////////////////
/// Celeste Sky
///-----------------------------------------------------
/// Celestial Planetary Positions
///-----------------------------------------------------
/// Panets position.
/// All calculations are based on Paul Schlyter papers.
/// See: http://www.stjarnhimlen.se/comp/ppcomp.html
/// See: http://stjarnhimlen.se/comp/tutorial.html
////////////////////////////////////////////////////////

using System;
using UnityEngine;
using CelesteSky.Utility;

namespace CelesteSky
{
    [Serializable] public class csky_PlanetaryPositions
    {
    
    #region [Date Time]

        /// <summary></summary>
        public DateTime dateTime{ get; set; }

    #endregion

    #region [Location]

        [Serializable] struct Location
        {
            [SerializeField, Range(-90f, 90f)] 
            public float latitude;

            [SerializeField, Range(-180f, 180f)] 
            public float longitude;

            [SerializeField, Range(-12f, 12f)] 
            public float uTC;
        }
        [SerializeField] Location m_Location = new Location();

        /// <summary></summary>
        public float Latitude
        {
            get => m_Location.latitude;
            set => m_Location.latitude = value;
        }

        /// <summary> Latitude in radians <summary>
        public float LatitudeRad => Mathf.Deg2Rad * m_Location.latitude;

        /// <summary></summary>
        public float Longitude 
        {
            get => m_Location.longitude;
            set => m_Location.longitude = value;
        }

 
        /// <summary></summary>
        public float UTC 
        {
            get => m_Location.uTC;
            set => m_Location.uTC = value;
        }

        /// <summary></summary>
        public float TotalHoursUTC
        {
            get{ return (float)dateTime.TimeOfDay.TotalHours - m_Location.uTC; }
        }

    #endregion

    #region [Coordinates]

        /// <summary> Sun distance(r). </summary>
        public float SunDistance{ get; private set; }

        /// <summary><summary>
        public float TrueSunLongitude{ get; private set; }

        /// <summary></summary>
        public float MeanSunLongitude{ get; private set; }

        /// <summary></summary>
        public float SideralTime{ get; private set; }

        /// <summary></summary>
        public float LocalSideralTime{ get; private set; }

        /// <summary> Time Scale (d). </summary>
        public float TimeScale
        {
            get
            {
                return (367 * dateTime.Year - (7 * (dateTime.Year + ((dateTime.Month + 9) / 12))) / 4 +
                    (275 * dateTime.Month) / 9 + dateTime.Day - 730530) + (float)dateTime.TimeOfDay.TotalHours / 24;
            }
        }

        /// <summary> Obliquity of the ecliptic. </summary>
        public float Oblecl => Mathf.Deg2Rad * (23.4393f - 3.563e-7f * TimeScale); 
    
        /// <summary></summary>
        public float SunAltitude{ get; private set; }

        /// <summary></summary>
        public float SunAzimuth{ get; private set; }

        /// <summary></summary>
        public float MoonAltitude{ get; private set; }

        /// <summary></summary>
        public float MoonAzimuth{ get; private set; }

    #endregion

    #region [Compute]

        /// <summary>
        /// Sun Orbital Elements.
        /// Mean Anomaly is Normalized.
        /// </summary>
        public csky_OrbitalElements SunOE
        {
            get
            {
                csky_OrbitalElements oe = csky_OrbitalElements.SetOrbitalElements(0, TimeScale);
                oe.M = csky_Mathf.Rev(oe.M);
                return oe;
            }
        }

        /// <summary>
        /// Moon Orbital Elements.
        /// Mean Anomaly is Normalized.
        /// </summary>
        public csky_OrbitalElements MoonOE
        {
            get
            {
                csky_OrbitalElements oe = csky_OrbitalElements.SetOrbitalElements(1, TimeScale);

                // Acending node.
                oe.N =  csky_Mathf.Rev(oe.N);

                // Argument of perihelion.
                oe.w =  csky_Mathf.Rev(oe.w);
 
                // Mean anomaly.
                oe.M =  csky_Mathf.Rev(oe.M);

                return oe;
            }
        }

        
        /// <summary>
        /// Compute Sun Coordinates.
        /// Get sun azimuth and altitude.
        /// </summary>
        public void ComputeSunCoords()
        {
        #region |Orbital Elements|

            // Mean Anomaly in radians.
            float M_Rad = SunOE.M * Mathf.Deg2Rad;
        
        #endregion

        #region |Eccentric Anomaly|

            // Compute eccentric anomaly.
            float E = SunOE.M + Mathf.Rad2Deg * SunOE.e * Mathf.Sin(M_Rad) * (1 + SunOE.e * Mathf.Cos(M_Rad));

            // Eccentric anomaly to radians.
            float E_Rad = Mathf.Deg2Rad * E;// Debug.Log(E);

        #endregion

        #region |Rectangular Coordinates|

            // Rectangular coordinates of the sun in the plane of the ecliptic.
            float xv = (Mathf.Cos(E_Rad) - SunOE.e);                                 // Debug.Log(xv);
            float yv = (Mathf.Sin(E_Rad) * Mathf.Sqrt(1 - SunOE.e * SunOE.e)); // Debug.Log(yv);

            // Convert to distance and true anomaly(r = radians, v = degrees).
            float r = Mathf.Sqrt(xv * xv + yv * yv);       // Debug.Log(r);
            float v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv); // Debug.Log(v);

            // Get sun distance.
            SunDistance = r;

        #endregion

        #region |True Longitude|

            // True sun longitude.
            float lonsun = v + SunOE.w;

            // Normalize sun longitude
            lonsun = csky_Mathf.Rev(lonsun); // Debug.Log(lonsun);

            // True sun longitude to radians.
            float lonsun_Rad = Mathf.Deg2Rad * lonsun;

            // Set true sun longitude(radians) for use in others celestials calculations.
            TrueSunLongitude = lonsun_Rad;

        #endregion

        #region |Ecliptic And Equatorial Coordinates|

            // Ecliptic rectangular coordinates(radians):
            float xs = r * Mathf.Cos(lonsun_Rad);
            float ys = r * Mathf.Sin(lonsun_Rad);

            // Ecliptic rectangular coordinates rotate these to equatorial coordinates(radians).
            float oblecl_Cos = Mathf.Cos(Oblecl);
            float oblecl_Sin = Mathf.Sin(Oblecl);

            float xe = xs;
            float ye = ys * oblecl_Cos - 0.0f * oblecl_Sin;
            float ze = ys * oblecl_Sin + 0.0f * oblecl_Cos;

        #endregion

        #region |Ascension And Declination|

            // Right ascension(degrees):
            float RA = Mathf.Rad2Deg * Mathf.Atan2(ye, xe) / 15;

            // Declination(radians).
            float Decl = Mathf.Atan2(ze, Mathf.Sqrt(xe * xe + ye * ye));

        #endregion

        #region |Mean Longitude|

            // Mean sun longitude(degrees).
            float L = SunOE.w + SunOE.M;

            // Rev mean sun longitude.
            L = csky_Mathf.Rev(L);

            // Set mean sun longitude for use in other celestials calculations.
            MeanSunLongitude = L;

        #endregion

        #region |Sideral Time|

            // Sideral time(degrees).
            float GMST0 = /*(L + 180) / 15;*/  ((L / 15) + 12);

            SideralTime = GMST0 + TotalHoursUTC + Longitude / 15 + 15/15 ;
            LocalSideralTime = Mathf.Deg2Rad * SideralTime * 15;

            // Hour angle(degrees).
            float HA = (SideralTime - RA) * 15; // Debug.Log(HA);

            // Hour angle in radians.
            float HA_Rad = Mathf.Deg2Rad * HA;  // Debug.Log(HA);

        #endregion

        #region |Hour Angle And Declination In Rectangular Coordinates|

            // HA anf Decl in rectangular coordinates(radians).
            float Decl_Cos = Mathf.Cos(Decl);

            // X axis points to the celestial equator in the south.
            float x = Mathf.Cos(HA_Rad) * Decl_Cos;// Debug.Log(x);

            // Y axis points to the horizon in the west.
            float y = Mathf.Sin(HA_Rad) * Decl_Cos; // Debug.Log(y);

            // Z axis points to the north celestial pole.
            float z = Mathf.Sin(Decl);// Debug.Log(z);

            // Rotate the rectangualar coordinates system along of the Y axis(radians).
            float sinLatitude = Mathf.Sin(LatitudeRad);
            float cosLatitude = Mathf.Cos(LatitudeRad);

            float xhor = x * sinLatitude - z * cosLatitude; // Debug.Log(xhor);
            float yhor = y;
            float zhor = x * cosLatitude + z * sinLatitude; // Debug.Log(zhor);

        #endregion

        #region Azimuth, Altitude And Zenith[Radians].

            SunAzimuth  = Mathf.Atan2(yhor, xhor) + Mathf.PI; // Azimuth.
            SunAltitude = csky_Mathf.k_HalfPI - Mathf.Atan2 (zhor, Mathf.Sqrt(xhor * xhor + yhor * yhor)); // Altitude.

        #endregion
        }


        /// <summary>
        /// Compute Moon Coordinates without perturbations.
        /// Get moon azimuth and altitude.
        /// </summary>
        public void ComputeMoonCoords()
        {

        #region |Orbital Elements|

            // Orbital elements in radians.
            float N_Rad = Mathf.Deg2Rad * MoonOE.N;
            float i_Rad = Mathf.Deg2Rad * MoonOE.i;
            float M_Rad = Mathf.Deg2Rad * MoonOE.M;

        #endregion

        #region |Eccentric Anomaly|

            // Compute eccentric anomaly.
            float E = MoonOE.M + Mathf.Rad2Deg * MoonOE.e * Mathf.Sin(M_Rad) * (1 + SunOE.e * Mathf.Cos(M_Rad));

            // Eccentric anomaly to radians.
            float E_Rad = Mathf.Deg2Rad * E;

        #endregion

        #region |Rectangular Coordinates|

            // Rectangular coordinates of the sun in the plane of the ecliptic.
            float xv = MoonOE.a * (Mathf.Cos(E_Rad) - MoonOE.e); // Debug.Log(xv);
            float yv = MoonOE.a * (Mathf.Sin(E_Rad) * Mathf.Sqrt(1 - MoonOE.e * MoonOE.e)) * Mathf.Sin(E_Rad); // Debug.Log(yv);

            // Convert to distance and true anomaly(r = radians, v = degrees).
            float r = Mathf.Sqrt(xv * xv + yv * yv);         // Debug.Log(r);
            float v = Mathf.Rad2Deg * Mathf.Atan2(yv, xv);   // Debug.Log(v);

            v = csky_Mathf.Rev(v);

            // Longitude in radians.
            float l = Mathf.Deg2Rad * (v + MoonOE.w);

            float Cos_l = Mathf.Cos(l);
            float Sin_l = Mathf.Sin(l);
            float Cos_N_Rad = Mathf.Cos(N_Rad);
            float Sin_N_Rad = Mathf.Sin(N_Rad);
            float Cos_i_Rad = Mathf.Cos(i_Rad);

            float xeclip = r * (Cos_N_Rad * Cos_l - Sin_N_Rad * Sin_l * Cos_i_Rad);
            float yeclip = r * (Sin_N_Rad * Cos_l + Cos_N_Rad * Sin_l * Cos_i_Rad);
            float zeclip = r * (Sin_l * Mathf.Sin(i_Rad));

        #endregion

        #region Geocentric Coordinates.

            // Geocentric position for the moon and Heliocentric position for the planets.
            float lonecl = Mathf.Rad2Deg * Mathf.Atan2(yeclip, xeclip);

            // Rev lonecl
            lonecl = csky_Mathf.Rev(lonecl);     // Debug.Log(lonecl);

            float latecl = Mathf.Rad2Deg * Mathf.Atan2(zeclip, Mathf.Sqrt(xeclip * xeclip + yeclip * yeclip));   // Debug.Log(latecl);

            // Get true sun longitude.
            // float lonSun = TrueSunLongitude;

            // Ecliptic longitude and latitude in radians.
            float lonecl_Rad = Mathf.Deg2Rad * lonecl;
            float latecl_Rad = Mathf.Deg2Rad * latecl;

            float nr = 1.0f;
            float xh = nr * Mathf.Cos(lonecl_Rad) * Mathf.Cos(latecl_Rad);
            float yh = nr * Mathf.Sin(lonecl_Rad) * Mathf.Cos(latecl_Rad);
            float zh = nr * Mathf.Sin(latecl_Rad);

            // Geocentric posisition.
            float xs = 0.0f;
            float ys = 0.0f;

            // Convert the geocentric position to heliocentric position.
            float xg = xh + xs;
            float yg = yh + ys;
            float zg = zh;

        #endregion

        #region |Equatorial Coordinates|

            // Convert xg, yg in equatorial coordinates.
            float oblecl_Cos = Mathf.Cos(Oblecl);
            float oblecl_Sin = Mathf.Sin(Oblecl);

            float xe = xg;
            float ye = yg * oblecl_Cos - zg * oblecl_Sin;
            float ze = yg * oblecl_Sin + zg * oblecl_Cos;

        #endregion

        #region |Ascension, Declination And Hour Angle|

            // Right ascension.
            float RA = Mathf.Rad2Deg * Mathf.Atan2(ye, xe); //Debug.Log(RA);

            // Normalize right ascension.
            RA = csky_Mathf.Rev(RA);  //Debug.Log(RA);

            // Declination.
            float Decl = Mathf.Rad2Deg * Mathf.Atan2(ze, Mathf.Sqrt(xe * xe + ye * ye));

            // Declination in radians.
            float Decl_Rad = Mathf.Deg2Rad * Decl;

            // Hour angle.
            float HA = ((SideralTime * 15) - RA); //Debug.Log(HA);

            // Rev hour angle.
            HA = csky_Mathf.Rev(HA);     //Debug.Log(HA);

            // Hour angle in radians.
            float HA_Rad = Mathf.Deg2Rad * HA;

        #endregion

        #region |Declination in rectangular coordinates|

            // HA y Decl in rectangular coordinates.
            float Decl_Cos = Mathf.Cos(Decl_Rad);
            float xr = Mathf.Cos(HA_Rad) * Decl_Cos;
            float yr = Mathf.Sin(HA_Rad) * Decl_Cos;
            float zr = Mathf.Sin(Decl_Rad);

            // Rotate the rectangualar coordinates system along of the Y axis(radians).
            float sinLatitude = Mathf.Sin(LatitudeRad);
            float cosLatitude = Mathf.Cos(LatitudeRad);

            float xhor = xr * sinLatitude - zr * cosLatitude;
            float yhor = yr;
            float zhor = xr * cosLatitude + zr * sinLatitude;

        #endregion

        #region |Azimuth, Altitude And Zenith[Radians]|

            MoonAzimuth  = Mathf.Atan2(yhor, xhor) + Mathf.PI;
            MoonAltitude = csky_Mathf.k_HalfPI - Mathf.Atan2 (zhor, Mathf.Sqrt(xhor * xhor + yhor * yhor)); // Altitude.

        #endregion
        }
    #endregion

    }

}