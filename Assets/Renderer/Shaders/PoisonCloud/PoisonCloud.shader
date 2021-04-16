Shader "NightmareFuel/ParticleShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0

        _Opacity("Opacity", Float) = 0.5

        [MaterialToggle] Glow("Glow", Float) = 0
        _GlowRadius("GlowRadius", Float) = 0.5
        _GlowColor("Glow Color", Color) = (1,1,1,1)
        _GlowIntensity("_GlowIntensity", Float) = 0.2

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

            float _Opacity;
            uniform float _Radius;

            v2f vert(appdata v)
            {
                v2f o;

                v.vertex.x *= 1.7*_Radius;
                v.vertex.y *= 1.7*_Radius;
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= _Opacity * ( 1 + sin(_Time[1] * 3.145 / 3) / 4 )  ;
                col.rgb = col.rgb * col.a;
                return col;
            }
            ENDCG
        }

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

            uniform float _Radius;


            v2f vert(appdata v)
            {
                v2f o;

                v.vertex.x *= (1.775 + 0.025 * sin(_Time[1] * 3.145 / 3)) * _Radius;
                v.vertex.y *= (1.775 + 0.025 * sin(_Time[1] * 3.145 / 3)) * _Radius;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.a *= 1;
                col.rgb = col.rgb * col.a;
                return col;
            }
            ENDCG
        }
    }
}
