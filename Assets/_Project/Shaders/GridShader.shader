Shader "Unlit/GridShader"
{
    Properties
    {
        _Width("Width",Range(1,999)) = 16
        _Height("Height", Range(1, 999)) = 9
        _BPM("BPM", Range(40,300)) = 120
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

            #include "UnityCG.cginc"
            #define PI 3.1415926538


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

            float _Width;
            float _Height;
            float _BPM;
           

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {

                float2 uv = float2(i.uv.x * _Width,i.uv.y*_Height);
                uv = frac(uv);


                float borderSize = 0.1;

                float4 black = float4(0.6,0.6,0.6,0.5);
                float4 white = float4(1, 1, 1,1.0);

                float4 color = black;
        
                if (abs(uv.x) <= borderSize/2 || abs(uv.y) <= borderSize/2) {
                    color = white; //lines
                }
          
                if (uv.x >= 1 - borderSize/2 || uv.y >= 1 - borderSize/2){
                    color = white;
                }

                if (i.uv.x <= borderSize/16) {
                    color = white;
                }
                if (i.uv.y <= borderSize/8) {
                    color = white;
                }
                //time.y is measured in seconds
                //sample the texture and offset by time.
               float4 c = tex2D(_MainTex, i.uv + float2(_Time.y/20,_Time.y/20));  /* float4(_Time.zzz,1.0)*/;  

               //creates a flashing pulse effect
               float pulseValue = clamp(cos(((_Time.y * PI) * (_BPM / 60.0)) % PI),0.6,1.0);

               c *= float4(pulseValue,pulseValue,pulseValue, 1.0) * color;

          
             
               return c;
            }
            ENDCG
        }
    }
}
