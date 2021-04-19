Shader "PostEffects/LightSourceShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightTint("Light Tint", Color) = (1, 1, 1, 1)
        _DarkTint("Dark Tint", Color) = (0, 0, 0, 0)
        
        _TintIntensity("Tint Intensity", Float) = 0.2

        _Intensity("Intensity", Float) = 1
        _Threshold("Threshold", Float) = 0
        _Radius("Radius", Float) = 0.2

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
                float2 lightPos : TEXCOORD1;

                float4 vertex : SV_POSITION;
            };

            uniform float3 _LightSourcePosition;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.lightPos = _LightSourcePosition;
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _LightTint;
            float4 _DarkTint;
            float _TintIntensity;

            float _Threshold;
            float _Intensity;
            float _Radius;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors

                float2 lightPos = (i.lightPos - 0.5) * 2;
                float2 pos = (i.uv - 0.5) * 2;

                float2 vec = lightPos - pos;

                //return float4(vec, 0, 1);
                float dist5 = pow((vec.x * vec.x + vec.y * vec.y),5) / _Intensity + 1 + 0.1 * sin(_Time[1] * 3.145 * 0.5);
                float dist3 = pow((vec.x * vec.x + vec.y * vec.y), 3) / _Intensity + 1 + 0.1 * sin(_Time[1] * 3.145 * 0.5);
                float dist = pow((vec.x * vec.x + vec.y * vec.y), 1) / _Intensity + 1 + 0.1 * sin(_Time[1] * 3.145 * 0.5);


                float invDist = 1 / dist - _Threshold;
                //invDist = min(_Radius, invDist);
                //float invDist = max(1 / _Radius, 1 / dist - _Threshold);

                //invDist = min(invDist, _Thresholds.x); // 0.8
                //invDist = min(invDist, _Thresholds.y); // 0.5
                //invDist = min(invDist, _Thresholds.z); // 0.3

                //return (float3(1, 1, 1) * invDist, invDist);

                float3 darkness = dist5 * _DarkTint.rgb + dist3 * _DarkTint.rgb + dist * _DarkTint.rgb;

                //float3 ambience = _TintIntensity * normalize(darkness + invDist * _LightTint.rgb);


                
                return float4(1-darkness, invDist);
            }
            ENDCG
        }
    }
}
