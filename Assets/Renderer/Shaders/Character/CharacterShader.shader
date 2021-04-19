// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

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
        _var1("Var1", Float) = 0
        _var2("Var2", Float) = 0

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
                float2 worldPos : TEXCOORD1;
                float2 lightPos : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            float3 rotate(float4 vertex, float degrees)
            {
                float radians = degrees * UNITY_PI / 180.0;
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

            float _var1;
            float _var2;    

            v2f vert (appdata v)
            {
                v2f o;

                
                float3 worldOrigin = mul( unity_ObjectToWorld, float4(0, 0.5, 0, 1));
                float3 lightPos = _LightWorldPosition;
                float3 diff = lightPos - worldOrigin;
                float deg = - UNITY_PI * 0.5 + atan(diff.y / diff.x);

                //float3 rotatedVertex = rotate(v.vertex, deg);
                //o.vertex = UnityObjectToClipPos(rotatedVertex);

                float rad = _Degrees % 360;// *UNITY_PI / 180.0;
                _var1 = 0.0433 + -0.0117 * rad + 1.84 * pow(10, -4) * pow(rad, 2) + -6.92 * pow(10, -7) * pow(rad, 3) + 7.5 * pow(10, -10) * pow(rad, 4);
                _var2 = -0.0428 + 0.0114 * rad + -1.26 * pow(10, -5) * pow(rad, 2) + -2.47 * pow(10, -7) * pow(rad, 3) + 5.46 * pow(10, -10) * pow(rad, 4);
                
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 vertex = float4(o.vertex.x - _var1, o.vertex.y -_var2, o.vertex.zw);
                vertex = float4(rotate(vertex, _Degrees), 1);
                o.vertex = float4(vertex.x, vertex.yzw);

                //o.vertex = UnityObjectToClipPos(v.vertex);
                //o.vertex.y += _MainTex_ST.y / _ShadowDisplacementFactor;
                
                float2 uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.uv = uv;
                o.uv = float2(uv.x, uv.y);

                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.lightPos = (_LightWorldPosition);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //return (i.vertex);

                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = float3(0, 0, 0);

                float2 diff = i.worldPos - i.lightPos;
                float intensity = 1 / sqrt(diff.x * diff.x + diff.y * diff.y);
                intensity = 1;
                col.a = col.a * intensity;

                return float4(col.rgb * intensity, col.a);
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
