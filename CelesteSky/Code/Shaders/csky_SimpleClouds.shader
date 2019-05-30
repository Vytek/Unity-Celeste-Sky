Shader "Celeste Sky/Simple Clouds"
{

    //Properties{}
    CGINCLUDE
    #include "UnityCG.cginc"
    #include "csky_Include.hlsl"
    #include "csky_CloudsInclude.hlsl"

    struct appdata
    {
        float4 vertex   : POSITION;
        float2 texcoord : TEXCOORD0;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct v2f
    {
        float2 texcoord : TEXCOORD0;
        float3 texcoord1 : TEXCOORD1;
        half4  col      : TEXCOORD3;
        float4 vertex   : SV_POSITION;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    uniform half4 csky_CloudsTint;
    uniform half  csky_CloudsIntensity;
    
    v2f vert(appdata_base v)
    {
        v2f o;
        UNITY_INITIALIZE_OUTPUT(v2f, o);

        UNITY_SETUP_INSTANCE_ID(v);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
        //----------------------------------------------------------------------------

        o.vertex   = CSky_DomeToClipPos(v.vertex);
        o.texcoord = TRANSFORM_TEX(v.texcoord, csky_CloudsTex);

        o.texcoord1.xy = o.texcoord;
        o.texcoord1.y *= csky_CloudsTint.a;
        //----------------------------------------------------------------------------
             
        o.col.rgb = csky_CloudsTint.rgb * csky_CloudsIntensity * csky_GlobalIntensity;
        o.col.a   = normalize(v.vertex.xyz-float3(0.0, 0.01, 0.0)).y*2;
        //----------------------------------------------------------------------------

        return o;
    }

    fixed3 GetColor(fixed noise, fixed3 col)
    {
        return (1.0 - noise) * col; 
    }

    fixed4 frag(v2f i) : SV_TARGET
    {
        fixed4 re    = half4(0.0, 0.0, 0.0, 1.0);
        fixed2 noise  = GetNoise(i.texcoord).xy;
        re.a         = saturate(noise.y * i.col.a);
        re.rgb       = GetColor(noise.x, i.col.rgb);
        //re.rgb      *= re.rgb;
        return re;
    }

    ENDCG

    SubShader
    {
        
        Tags{ "Queue"="Background+1745" "RenderType"="Background" "IgnoreProjector"="true" }

        Pass
        {

            Cull Front ZWrite Off ZTest Lequal Blend SrcAlpha OneMinusSrcAlpha
            Fog{ Mode Off }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            ENDCG
        }

    }

}