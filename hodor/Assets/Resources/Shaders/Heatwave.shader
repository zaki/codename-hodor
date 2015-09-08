// vim: sw=4 ts=4
Shader "Hodor/Heatwave"
{
    Properties
    {
        _MainTex  ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            ZTest Always Cull Off ZWrite Off
            Fog { Mode off }

            CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

            sampler2D _MainTex;

            v2f_img vert(appdata_base IN)
            {
                v2f_img o;

                o.pos = mul(UNITY_MATRIX_MVP, IN.vertex);
                o.uv = IN.texcoord;

                return o;
            }

            fixed4 frag(v2f_img IN) : COLOR
            {
                fixed4 c = tex2D(_MainTex, IN.uv + float2(0, sin(IN.uv.x * 16 + _Time * 15.0) * 0.005));

                return c;
            }
            ENDCG
        }
    }
    FallBack off
}
