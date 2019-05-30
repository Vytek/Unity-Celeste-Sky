#ifndef CELESTE_SKY_CLOUDS_INCLUDE
#define CELESTE_SKY_CLOUDS_INCLUDE

uniform sampler2D csky_CloudsTex;
float4 csky_CloudsTex_ST;

uniform half csky_CloudsDensity;
uniform half csky_CloudsCoverage;
uniform half csky_CloudsSpeed, csky_CloudsSpeed2;

fixed2 GetNoise(float2 coords)
{
    fixed2 n;
    n.x = tex2D(csky_CloudsTex, coords.xy + _Time.x * csky_CloudsSpeed).r;
    n.y = tex2D(csky_CloudsTex, coords.yx + _Time.x * csky_CloudsSpeed2).b;
    
    fixed re = ((n.x + n.y) * 0.5) - csky_CloudsCoverage;
    return fixed2(re, re * csky_CloudsDensity);
}


#endif // CELESTE SKY CLOUDS INCLUDED.
