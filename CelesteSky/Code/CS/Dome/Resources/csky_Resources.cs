/////////////////////////////////////////////////
/// Celeste Sky
///----------------------------------------------
/// Sky Dome.
///----------------------------------------------
/// Dome Resources.
///----------------------------------------------
/// Description: Resources for skydome.
/////////////////////////////////////////////////

using System.IO;
using UnityEngine;

namespace CelesteSky
{

    [CreateAssetMenu(fileName = "csky_Resources", menuName = "CelesteSky/Skydome/Resources", order = 1)]
    public partial class csky_Resources : ScriptableObject
    {
    #region [Meshes]

        [Header("Sphere Meshes")]
        public Mesh sphereLOD0;
        public Mesh sphereLOD1;
        public Mesh sphereLOD2;
        public Mesh sphereLOD3;
        //--------------------------

        [Header("Atmosphere Meshes")]
        public Mesh atmosphereLOD0;
        public Mesh atmosphereLOD1;
        public Mesh atmosphereLOD2;
        public Mesh atmosphereLOD3;
        //---------------------------

        [Header("Hemisphere Meshes")]
        public Mesh hemisphereLOD0;
        public Mesh hemisphereLOD1;
        public Mesh hemisphereLOD2;
        public Mesh hemisphereLOD3;
        //---------------------------

        [Header("Other Meshes")]
        public Mesh QuadMesh;
        //---------------------------
    #endregion

    #region [Shaders]

        [Header("Atmosphere Shaders")]
        public Shader atmosphereShader;
        public Shader ambientSkyboxShader;
        //public Shader skymapShader;

        [Header("Deep Space Shaders")]
        public Shader deepSpaceShader;
      
        [Header("Near Space Shaders")]
        public Shader sunShader;
        public Shader moonShader;

        [Header("Clouds Shaders")]
        public Shader cloudsShader;

    #endregion

    #region [Materials]

        [Header("Atmosphere Materials")]
        public Material atmosphereMaterial;
        public Material ambientSkyboxMaterial;
        //public Material skymapMaterial;

        [Header("Deep Space Materials")]
        public Material deepSpaceMaterial;
       
        [Header("Near Space Materials")]
        public Material sunMaterial;
        public Material moonMaterial;

        [Header("Clouds Materials")]
        public Material cloudsMaterial;

    #endregion

    }
}