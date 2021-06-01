Shader "NightmareFuel/PoisonCircleShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Opacity("Opacity", Float) = 0.5
        _Brightness("Brightness", Float) = 1
        _NoiseTex("Noise", 2D) = "white" {}
        _NoiseIntensity("Noise Intensity", Float) = 0.5
        _NoiseScale("NoiseScale", Float) = 16
    }
    SubShader
    {
        Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

        Pass // Normal + Distory
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
                float3 worldPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Opacity;

            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            float _NoiseIntensity;
            float _NoiseScale;

            float _Brightness;

            v2f vert(appdata v)
            {
                v2f o;
                //v.vertex += float4(0.1 * sin(_Time[1] * UNITY_PI / 2), 0, 0, 0 );
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float noiseX = abs((i.worldPos.x / _NoiseScale + _Time[1] / 10 ) % _NoiseTex_ST.x);
                float noiseY = abs((i.worldPos.y / _NoiseScale + _Time[1] / 10 ) % _NoiseTex_ST.y);
                fixed4 noise = tex2D(_NoiseTex, float2(noiseX, noiseY));
                noise.rgb = noise.rgb * _NoiseIntensity;                
                
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= _Opacity;
                col.rbg *= col.a;

                float brightness = _Brightness;

                float4 output = brightness * float4(noise.r * col.rgb, noise.r * col.a);
                return output;
            }
            ENDCG
        }
    }
}
