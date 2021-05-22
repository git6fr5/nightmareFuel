Shader "NightmareFuel/ParticleShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0

        _Opacity("Opacity", Float) = 0.5

        [MaterialToggle] Glow("Glow", Float) = 0
        //_GlowRadius("GlowRadius", Float) = 0.5
        _GlowColor("Glow Color", Color) = (1,1,1,1)
        _GlowIntensity("_GlowIntensity", Float) = 0.2


        [MaterialToggle] PixelSnap("Shadow", Float) = 0
        _ShadowDisplacementFactor("ShadowDisplacementFactor", Float) = 4
        _ShadowIntensity("ShadowIntensity", Float) = 0.2
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

        Pass // Glow
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uniform float _GlowRadius;
            float4 _GlowColor;
            float _GlowIntensity;

            v2f vert(appdata v)
            {
                v2f o;

                v.vertex.x += (v.vertex.x) * 2 / 10 * _GlowRadius;
                v.vertex.y += (v.vertex.y) * 2 / 10 * _GlowRadius;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= _GlowIntensity;
                col.rgb = _GlowColor.rgb * col.a;
                return col;
            }
            ENDCG
        }

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
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Opacity;

            v2f vert(appdata v)
            {
                v2f o;
                //v.vertex += float4(0.1 * sin(_Time[1] * UNITY_PI / 2), 0, 0, 0 );
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //float2 circularShimmer = float2(0.1 * sin(_Time[1] * UNITY_PI / 2),0.1 * sin(_Time[1] * UNITY_PI / 2));
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= _Opacity;
                col.rbg *= col.a;
                return col;
            }
            ENDCG
        }
    }
}
