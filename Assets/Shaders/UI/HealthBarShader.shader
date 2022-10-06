Shader "Unlit/HealthBarShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Health("Health", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            float _Health;

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
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

            float4 InverseLerp(float a, float b, float v)
            {
                return (v - a) / (b-a);
            }

            fixed4 frag (v2f i) : SV_Target
            {

                float healthbaeMask = _Health > i.uv.x;

                float3 healthbarColor = tex2D(_MainTex, float2(_Health, i.uv.y));

                if (_Health < 0.2)
                {
                    float flash = cos(_Time.y * 4) * 0.4 + 1;
                    healthbarColor *= flash;
                }
                
                return float4(healthbarColor * healthbaeMask, 1 );
            }
            ENDCG
        }
    }
}
