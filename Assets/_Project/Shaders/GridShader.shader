Shader "Unlit/GridShader"
{
    Properties
    {
        _Width("Width",Range(1,999)) = 16
        _Height("Height", Range(1, 999)) = 9
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 1

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // step one - draw a square with the shader.

                float2 uv = float2(i.uv.x * 16,i.uv.y*10);
                uv = frac(uv);

                float borderSize = 0.05;
                
                //bottom left
                float2 bl = step(float2(borderSize, borderSize), uv);
                float pct = bl.x * bl.y;

                //top-right
                float2 tr = step(float2(borderSize, borderSize), 1.0 - uv);
                pct = pct * (tr.x * tr.y);

                //want white lines with black background;
                float3 color = float3(1-pct,1-pct,1-pct);

                
                return float4(color,1.0);
            }
            ENDCG
        }
    }
}
