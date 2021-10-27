Shader "Unlit/ShapeMaskShader"
{
    Properties
    {
        _InnerColor("InnerColor",Color) = (0,0,0,1)
        _BorderColor("BorderColor", Color) = (1, 1, 1,1)
        _MaskTexture("MaskTexture", 2D) = "white" {}

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
                float2 mask: TEXCOORD1;
                float4 vertex : SV_POSITION;

            };

            float4 _InnerColor;
            float4 _BorderColor;
            sampler2D _MaskTexture;
            float4 _MaskTexture_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.mask = TRANSFORM_TEX(v.uv, _MaskTexture);


                return o;
            }

            float4 frag(v2f i) : SV_Target
            {

                fixed4 shapeTex = tex2D(_MaskTexture, i.uv); // mask texture


                float value = step(0.3, shapeTex);

                if (value == 0) {
                    return _InnerColor + shapeTex;
                }
                if (value == 1) {
                    return _BorderColor * shapeTex;
                }

          
                return shapeTex;
       
             








  

              
            }
            ENDCG
        }
    }
}
