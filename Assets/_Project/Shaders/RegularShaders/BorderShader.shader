Shader "Unlit/BorderShader"
{
    Properties
    {
        _BorderColor("BorderColor",Color) = (0,0,1,0.8)
        _InnerColor("InnerColor",Color) = (0,0,1,0.3)
        _BorderSize("BorderSize",Range(0,0.4)) = 0.1
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

            float4 _BorderColor;
            float4 _InnerColor;
            float _BorderSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {

                float4 colors = float4(0,0,0,0.3);



                if ( (i.uv.x < _BorderSize || i.uv.x > 1 - _BorderSize) || (i.uv.y < _BorderSize || i.uv.y>1 -_BorderSize) ) {
                    //return white
                    colors = _BorderColor;
                }
                else {
                    colors = _InnerColor;
                }


                return colors;

            }
            ENDCG
        }
    }
}
