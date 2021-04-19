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
            float _TintIntensity;

            float _Intensity;
            float _Threshold;
            float _Radius;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors

                float2 lightPos = (i.lightPos - 0.5) * 2;
                float2 pos = (i.uv - 0.5) * 2;

                float2 vec = lightPos - pos;

                //float vec.y = vec.y * min(1, vec.y * 10) 

                float dist5 = pow(((vec.x * vec.x + vec.y * vec.y) / _Radius), 5)  + 1 + 0.05 * sin(_Time[1] * 3.145 * 0.25);
                float dist3 = pow(((vec.x * vec.x + vec.y * vec.y) / _Radius), 3) + 1 + 0.05 * sin(_Time[1] * 3.145 * 0.1);
                float dist = pow(((vec.x * vec.x + vec.y * vec.y) / _Radius), 1) + 1 + 0.05 * sin(_Time[1] * 3.145 * 0.5);

                float invDist = 1 / (dist*dist3*dist5) - _Threshold;
                
                return float4( (col.rgb + _LightTint * _TintIntensity * invDist) * invDist  , invDist);
            }
            ENDCG
        }
    }
}
