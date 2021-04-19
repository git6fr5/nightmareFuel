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

        _varDeg("Degrees", Float) = 180
        _diffX("Diff X", Float) = 0
        _diffY("Diff Y", Float) = 0
        _varX("X Offset", Float) = 0
        _varY("Y Offset", Float) = 0

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

            float _diffX;
            float _diffY;
            float _varX;
            float _varY;
            float _varDeg;

            v2f vert (appdata v)
            {
                v2f o;

                
                float3 worldOrigin = mul( unity_ObjectToWorld, float4(0, 0.5, 0, 1));
                float3 lightPos = _LightWorldPosition;
                float3 diff = lightPos - worldOrigin;

                //_varDeg = (_diffY / abs(_diffY + 0.001) - 1) * 90;
                float deg = _varDeg % 360;
                float rad = deg / 180 * UNITY_PI;

                float x = -_varX * sin(rad);
                float y = -_varY * cos(rad) - _varY;

                x = x / _MainTex_ST.x / 2;
                y = y / _MainTex_ST.y / 2;

                float3 rotatedVertex = rotate(v.vertex, deg);
                float4 vertex = UnityObjectToClipPos(rotatedVertex);
                o.vertex = float4(vertex.x - x, vertex.y - y, vertex.zw);

                //o.vertex = UnityObjectToClipPos(v.vertex);
                //float4 vertex = float4(o.vertex.x - x, o.vertex.y -y, o.vertex.zw);
                //vertex = float4(rotate(vertex, deg), 1);
                //o.vertex = float4(vertex.x, vertex.yzw);

                //o.vertex = UnityObjectToClipPos(v.vertex);
                //o.vertex.y += _MainTex_ST.y / _ShadowDisplacementFactor;
                
                float2 uv = TRANSFORM_TEX(v.uv, _MainTex);
                //o.uv = uv;
                o.uv = float2(uv.x, -(uv.y-0.5) + 0.5);

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

                return float4(col * 1);
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
