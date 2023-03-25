Shader "Custom/WaterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0, 0.51, 1, 0.8)
        _RippleColor ("Ripple Color", Color) = (0.48, 0.81, 1, 1)
        _Speed ("Wave Speed", Float) = 0.3
        _Height ("Wave Height", Float) = 0.3
        _Waviness ("Waviness", Float) = 0.3
        _Direction ("Wave Direction", Vector) = (1,1,0,0)
        _RippleCount ("Ripple Count", Float) = 10
        _RippleStrength ("Ripple Strength", Float) = 3
    }
    SubShader
    {
       Tags { "Queue" = "Transparent" } 

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
    
            inline float2 unity_voronoi_noise_randomVector (float2 UV, float offset)
            {
                float2x2 m = float2x2(15.27, 47.63, 99.41, 89.98);
                UV = frac(sin(mul(UV, m)) * 46839.32);
                return float2(sin(UV.y*+offset)*0.5+0.5, cos(UV.x*offset)*0.5+0.5);
            }

            void Unity_Voronoi_float(float2 UV, float AngleOffset, float CellDensity, out float Out, out float Cells)
            {
                float2 g = floor(UV * CellDensity);
                float2 f = frac(UV * CellDensity);
                float t = 8.0;
                float3 res = float3(8.0, 0.0, 0.0);

                for(int y=-1; y<=1; y++)
                {
                    for(int x=-1; x<=1; x++)
                    {
                        float2 lattice = float2(x,y);
                        float2 offset = unity_voronoi_noise_randomVector(lattice + g, AngleOffset);
                        float d = distance(lattice + offset, f);
                        if(d < res.x)
                        {
                            res = float3(d, offset.x, offset.y);
                            Out = res.x;
                            Cells = res.y;
                        }
                    }
                }
            }

            half3 ObjectScale() 
            {
                return half3(
                    length(unity_ObjectToWorld._m00_m10_m20),
                    length(unity_ObjectToWorld._m01_m11_m21),
                    length(unity_ObjectToWorld._m02_m12_m22)
                );
            }


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _RippleColor;
            float _Speed;
            float _Height;
            float _Waviness;
            float2 _Direction;
            float _RippleStrength;
            float _RippleCount;

            v2f vert (appdata v)
            {
                v2f OUT;
                OUT.uv = v.uv; // pass
       
                _Direction = normalize(_Direction) * _Waviness;
     
                v.vertex = mul(unity_ObjectToWorld, v.vertex);
                v.vertex.y += _Height * sin((_Direction.x * v.vertex.x + _Speed * _Time[1]));
                v.vertex.y += _Height * sin((_Direction.y * v.vertex.z + _Speed * _Time[1]));
                v.vertex = mul(unity_WorldToObject, v.vertex);

                OUT.vertex = UnityObjectToClipPos(v.vertex);
                return OUT;
            }

            float4 frag (v2f IN) : SV_Target
            {
                float StartOffset = 10;
                float4 col = tex2D(_MainTex, IN.uv) * _Color;
                float _out, _cells;
                Unity_Voronoi_float(IN.uv, _Time[1] + StartOffset, _RippleCount * ObjectScale().x, _out, _cells);
                float4 ripple = float4(_out, _out, 0, 0) * _RippleColor;
                col+= pow(ripple, _RippleStrength);
                return col;
            }
            ENDCG
        }
    }
}
