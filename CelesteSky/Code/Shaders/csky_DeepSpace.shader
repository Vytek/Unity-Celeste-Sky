Shader "Celeste Sky/Deep Space"
{

    //Properties{}
    CGINCLUDE  
    #include "UnityCG.cginc"
    #include "csky_Include.hlsl"

    // Cubemap.
    ////////////////
    uniform samplerCUBE csky_GalaxyBackgroundCubemap;
    uniform samplerCUBE csky_StarsFieldCubemap;
    uniform samplerCUBE csky_StarsFieldNoiseCubemap;

    //#ifndef SHADER_API_MOBILE
    //half4 csky_DeepSpace_HDR;
    //#endif

    uniform float4x4 csky_StarsFieldNoiseMatrix;
    #define CSKY_STARS_FIELD_NOISE_COORDS(vertex) mul((float3x3) csky_StarsFieldNoiseMatrix, vertex.xyz)

    // Params
    //////////////
    uniform half3 csky_GalaxyBackgroundTint;
    uniform half  csky_GalaxyBackgroundIntensity;
    uniform half  csky_GalaxyBackgroundContrast;

    uniform half3 csky_StarsFieldTint;
    uniform half  csky_StarsFieldIntensity;
    uniform half  csky_StarsFieldScintillation;
    uniform half  csky_StarsFieldScintillationSpeed;
   
    struct v2f
    {
        float4 vertex    : SV_POSITION;
        float3 texcoord  : TEXCOORD0;
        float3 texcoord2 : TEXCOORD1;
        half3  col       : TEXCOORD2;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    v2f vert(appdata_base v)
    {
        v2f o;
        //UNITY_INITIALIZE_OUTPUT(v2f, o);
        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
        o.vertex    = CSky_DomeToClipPos(v.vertex);
        o.texcoord  = v.vertex.xyz;
        o.texcoord2 = CSKY_STARS_FIELD_NOISE_COORDS(v.vertex);
        o.col.rgb   = CSKY_HORIZON_FADE(v.vertex) * csky_GlobalIntensity;
        return o;
    }

    inline half3 ApplyScintillation(half3 c, float3 coords)
    {
        fixed noiseCube = texCUBE(csky_StarsFieldNoiseCubemap, coords).r;
        return lerp(c.rgb, 2.0 * c.rgb * noiseCube, csky_StarsFieldScintillation);
    }

    fixed4 frag(v2f i) : SV_Target
    {
        // Galaxy Background.
        fixed3 galaxy = texCUBE(csky_GalaxyBackgroundCubemap, i.texcoord.rgb);
        galaxy = CSky_Pow3(galaxy, csky_GalaxyBackgroundContrast);
        galaxy *= csky_GalaxyBackgroundTint * csky_GalaxyBackgroundIntensity * i.col.rgb;

        // Stars Field.
        fixed3 starsField  = texCUBE(csky_StarsFieldCubemap, i.texcoord).rgb;
        starsField.rgb     = ApplyScintillation(starsField.rgb, i.texcoord2.xyz);
        starsField.rgb    *= csky_StarsFieldTint.rgb * i.col.rgb * csky_StarsFieldIntensity;

        fixed4 re = fixed4(galaxy.rgb + starsField.rgb, 1.0);
        //#ifndef SHADER_API_MOBILE
        //re.rgb = DecodeHDR(re, csky_DeepSpace_HDR);
        //re.rgb *= unity_ColorSpaceDouble.rgb;
        //#endif

        return re;
    }

    ENDCG

    SubShader
    {
        Tags{ "Queue"="Background+5" "RenderType"="Background" "IgnoreProjector"= "true" }
        Pass
        {
            Cull Front ZWrite Off ZTest Lequal
            //Blend One One
            Fog{ Mode Off }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            ENDCG
        }
    }

}