Shader "NightmareFuel/CharacterShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        _ShadowDisplacementFactor("ShadowDisplacementFactor", Float) = 4
        _ShadowIntensity("ShadowIntensity", Float) = 0.2

        _Degrees("Degrees", Float) = 180

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
            };

            float3 rotate(float4 vertex, float degrees)
            {
                float radians = degrees; // *UNITY_PI / 180.0;
                float sina, cosa;
                sincos(radians, sina, cosa);
                float2x2 rotationMatrix = float2x2(cosa, -sina, sina, cosa);
                return float3(mul(rotationMatrix, vertex.xy), vertex.z).xyz;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ShadowDisplacementFactor;
            float _ShadowIntensity;
            float _Degrees;
            uniform float3 _LightWorldPosition;

            v2f vert (appdata v)
            {
                v2f o;

                /*
                float3 worldOrigin = mul( unity_ObjectToWorld, float4(0, 0.5, 0, 1));
                float3 lightPos = _LightWorldPosition;
                float3 diff = lightPos - worldOrigin;
                float deg = - UNITY_PI * 0.5 + atan(diff.y / diff.x);

                //float3 rotatedVertex = rotate(v.vertex, deg);
                //o.vertex = UnityObjectToClipPos(rotatedVertex);

                
                //o.vertex = UnityObjectToClipPos(v.vertex);
                //o.vertex = float4(rotate(o.vertex, deg), 1);
                */

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex.y += _MainTex_ST.y / _ShadowDisplacementFactor;
                
                float2 uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.uv = uv;
                o.uv = float2(uv.x, -(uv.y-0.5) + 0.5);
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = float3(0, 0, 0);
                col.a = col.a * _ShadowIntensity;
                return col;
            }
            ENDCG
        }

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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
