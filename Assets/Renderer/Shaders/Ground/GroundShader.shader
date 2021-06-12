Shader "NightmareFuel/GroundShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _Brightness("Brightness", Float) = 1
        _NoiseTex("Noise", 2D) = "white" {}
        _NoiseIntensity("Noise Intensity", Float) = 0.5
        _NoiseScale("NoiseScale", Float) = 16
        _Flux("Flux", Float) = 0
        _FluxRate("Flux Rate", Float) = 1
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

            sampler2D _MainTex;

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            float _NoiseIntensity;
            float _NoiseScale;

            float _Brightness;
            float _Flux;
            float _FluxRate;

            float3 rotate(float3 vec, float radians)
            {
                float sina, cosa;
                sincos(radians, sina, cosa);
                float2x2 rotationMatrix = float2x2(cosa, -sina, sina, cosa);
                return float3(mul(rotationMatrix, vec.xy), vec.z).xyz;
            };

            float3 stretch(float3 vec, float x, float y)
            {
                float2x2 stretchMatrix = float2x2(x, 0, 0, y);
                return float3(mul(stretchMatrix, vec.xy), vec.z).xyz;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos( v.vertex );
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);// stretch(v.vertex, _Flux * sin(_Time[1] * _FluxRate), _Flux * sin(_Time[1] * _FluxRate)) );
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float noiseX = abs( (i.worldPos.x / _NoiseScale )% _NoiseTex_ST.x);
                float noiseY = abs( (i.worldPos.y / _NoiseScale )% _NoiseTex_ST.y);

                fixed4 noise = tex2D(_NoiseTex, float2(noiseX, noiseY));
                float factor = _Flux * sin(_Time[1] * _FluxRate);
                noise.rgb = (noise.rgb - 0.5) * _NoiseIntensity + (noise.rgb - 0.5) * factor;

                float4 output = _Brightness * float4( noise.rgb + col.rgb, col.a);

                return output;
            }
            ENDCG
        }
    }
}
