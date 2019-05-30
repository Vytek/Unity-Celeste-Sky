/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Atmospheric Scattering enums.
///----------------------------------------------
/// Enums for atmospheric scattering.
/////////////////////////////////////////////////

using System;
using UnityEngine;

namespace CelesteSky
{

    /// <summary>
    /// The way a moon affects 
    /// the rayleigh effect of the atmosphere.
    /// </summary>
    public enum csky_MoonRayleighMode
    {
        Off                   = 0,
        OpossiteSun           = 1,
        CelestialContribution = 2,
    }


}
