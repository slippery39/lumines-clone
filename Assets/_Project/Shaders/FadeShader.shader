Shader "Unlit/FadeShader"
{
    Properties
    {
        _Color("Color",Color) = (0,0,1,0.8)
    }
        SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            float4 _Color;


            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {

                //Alpha should be most on the left and then it should lerp to the color
                float4 color = lerp(float4(_Color.r, _Color.g, _Color.b, 0), _Color, i.uv.x);
                return color;
            }
            ENDCG
        }
    }
}
