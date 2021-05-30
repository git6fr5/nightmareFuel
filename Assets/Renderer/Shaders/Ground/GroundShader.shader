Shader "NightmareFuel/GroundShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _Brightness("Brightness", Float) = 1
        _LightWorldPosition("_LightWorldPosition", Vector) = (1, 1, 1)


        _NoiseTex("Noise", 2D) = "white" {}
        _NoiseIntensity("Noise Intensity", Float) = 0.5
        _NoiseScale("NoiseScale", Float) = 16
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
                float3 worldPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            sampler2D _MainTex;
            
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            float _NoiseIntensity;
            float _NoiseScale;

            float _Brightness;

            float3 _LightWorldPosition;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float noiseX = abs( (i.worldPos.x / _NoiseScale )% _NoiseTex_ST.x);
                float noiseY = abs( (i.worldPos.y / _NoiseScale )% _NoiseTex_ST.y);

                fixed4 noise = tex2D(_NoiseTex, float2(noiseX, noiseY));
                noise.rgb = (noise.rgb - 0.5) * _NoiseIntensity;
                // just invert the colors

                float intensity = 1.3 / pow( abs(_LightWorldPosition.x - i.worldPos.x) + abs(_LightWorldPosition.y - i.worldPos.y), 0.3 );

                col.rgb = col.rgb * _Brightness * intensity;

                float4 output = float4( noise.rgb + col.rgb, col.a);

                return output;
            }
            ENDCG
        }
    }
}
