Shader "NightmareFuel/GroundShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _Brightness("Brightness", Float) = 1
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

            float3 getIntensity(float3 lightPos, float3 worldPos, float radius)
            {
                float3 vec = lightPos - worldPos;
                float dist = (vec.x * vec.x + vec.y * vec.y);
                float intensity = exp(- dist / radius);
                //intensity = intensity + 1 / log(abs(lightPos.x - worldPos.x) + abs(lightPos.y - worldPos.y) + 1);
                return intensity;
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

            float3 _Light0WorldPosition;
            float3 _Light1WorldPosition;
            float3 _Light2WorldPosition;

            float _Light0Radius;
            float _Light1Radius;
            float _Light2Radius;

            float4 _Light0Color;
            float4 _Light1Color;
            float4 _Light2Color;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float noiseX = abs( (i.worldPos.x / _NoiseScale )% _NoiseTex_ST.x);
                float noiseY = abs( (i.worldPos.y / _NoiseScale )% _NoiseTex_ST.y);

                fixed4 noise = tex2D(_NoiseTex, float2(noiseX, noiseY));
                noise.rgb = (noise.rgb - 0.5) * _NoiseIntensity;
                // just invert the colors

                /*float intensity0 = getIntensity(_Light0WorldPosition, i.worldPos, _Light0Radius);
                float intensity1 = getIntensity(_Light1WorldPosition, i.worldPos, _Light1Radius);
                float intensity2 = getIntensity(_Light2WorldPosition, i.worldPos, _Light2Radius);

                float3 l0 = _Light0Color;
                float4 col0 = float4(l0.r * col.r, l0.g * col.g, l0.b * col.b, 1/ intensity0);
                float3 l1 = _Light1Color;
                float4 col1 = float4(l1.r * col.r, l1.g * col.g, l1.b * col.b, 1 / intensity1);
                float3 l2 = _Light2Color;
                float4 col2 = float4(l2.r * col.r, l2.g * col.g, l2.b * col.b, 1 / intensity2);
                col.rgb =_( intensity0 * col0 + intensity1 * col1 + intensity2 * col2);*/

                float4 output = _Brightness * float4( noise.rgb + col.rgb, col.a);

                return output;
            }
            ENDCG
        }
    }
}
