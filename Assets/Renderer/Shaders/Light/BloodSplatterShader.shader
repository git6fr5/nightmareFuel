Shader "PostEffects/BloodSplatterShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _BloodTint("Blood Tint", Color) = (1,1,1,1)
        _BloodIntensity("Blood Intensity", Float) = 1
        _BloodTintIntensity("Blood Tint Intensity", Float) = 0.2
        _BloodRadius("Blood Radius", Float) = 0.2

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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.lightPos = _LightSourcePosition;
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            float4 _BloodTint;
            float _BloodIntensity;

            float _BloodTintIntensity;
            float _BloodRadius;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float2 vec = (i.uv - 0.5) * 2;
                float dist5 = pow((vec.x * vec.x + vec.y * vec.y),5) / _BloodRadius + 1 + 0.1 * sin(_Time[1] * 3.145 * 0.5);
                float dist3 = pow((vec.x * vec.x + vec.y * vec.y), 3) / _BloodRadius + 1 + 0.1 * sin(_Time[1] * 3.145 * 0.5);
                float dist = pow((vec.x * vec.x + vec.y * vec.y), 1) / _BloodRadius + 1 + 0.1 * sin(_Time[1] * 3.145 * 0.5);

                float3 darkness = dist5 * _BloodTint.rgb + dist3 * _BloodTint.rgb + dist * _BloodTint.rgb;
                float3 ambience = _BloodTintIntensity * (1-darkness);

                return float4(col.rgb + ambience, 1);
            }
            ENDCG
        }
    }
}
