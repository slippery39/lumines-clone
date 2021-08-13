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
            // make fog work
            #pragma multi_compile_fog

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
                // step one - draw a square with the shader.

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

                //bpm = 60/
                //time.y is measured in seconds
               float4 c = tex2D(_MainTex, i.uv + float2(_Time.y/20,_Time.y/20));  /* float4(_Time.zzz,1.0)*/;
               //float4 wave = float4(clamp(sin(12.0 * _Time.yyy),0.7,1.0),1.0);
               //c *= wave * color; //float4(clamp(_SinTime.w*20,0.3,0.6), clamp(_SinTime.w*20,0.3,0.6), clamp(_SinTime.w*20,0.3,0.6),1.0);

               //float baseValue = Mathf.Cos(((Time.time * Mathf.PI) * (BPM / 60f)) % Mathf.PI);

               //brighten up the texture
               float baseValue = clamp(cos(((_Time.y * PI) * (_BPM / 60.0)) % PI),0.6,1.0);


               c *= float4(baseValue, baseValue, baseValue, 1.0) * color;

          
             
               return c;

         
              //how do we collapse borders?
                
                
                /*
                * keeping this just in case, 
                //bottom left
                float2 bl = step(float2(borderSize, borderSize), uv);
                        float pct = bl.x * bl.y;

                //top-right
                float2 tr = step(float2(borderSize, borderSize), 1.0 - uv);
                pct = pct * (tr.x * tr.y);

                //want white lines with black background;
                //float3 color = float3(1-pct,1-pct,1-pct);

                
                return float4(color,1.0);
                */
            }
            ENDCG
        }
    }
}
