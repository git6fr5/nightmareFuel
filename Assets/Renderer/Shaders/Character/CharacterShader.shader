// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "NightmareFuel/CharacterShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        _HullYOffset("Hull Y Offset", Float) = 0
        _HullXOffset("Hull X Offset", Float) = 0

        _ShadowIntensity("ShadowIntensity", Float) = 0.2

        _isFlipped("Flipped", Float) = 1
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
                float intensity : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            float3 rotate(float3 vec, float radians)
            {
                float sina, cosa;
                sincos(radians, sina, cosa);
                float2x2 rotationMatrix = float2x2(cosa, -sina, sina, cosa);
                return float3(mul(rotationMatrix, vec.xy), vec.z).xyz;
            };

            float3 stretch(float3 vec, float x, float y)
            {
                float2x2 stretchMatrix = float2x2(x, 0, 0, y);
                return float3(mul(stretchMatrix, vec.xy), vec.z).xyz;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            uniform float3 _LightWorldPosition;

            float _HullYOffset;
            float _HullXOffset;

            float _ShadowIntensity;
            float _isFlipped;

            float _Degrees;

            v2f vert (appdata v)
            {
                v2f o;
                
                // rotation point in object space
                float3 rotationPoint = float3(0, -_HullYOffset, 0);

                // get the angle from the light position
                float3 diff = _LightWorldPosition - mul(unity_ObjectToWorld, float4(rotationPoint, 1));
                float3 scale = diff - mul(unity_ObjectToWorld, v.vertex);

                float degQuad = (diff.x / abs(diff.x + 0.001)) * 90;
                float flip = _isFlipped;
                float deg = flip * ( degQuad + atan(diff.y / diff.x) / UNITY_PI * 180.0);
                //float deg = _Degrees % 360;
                float rad =  deg * UNITY_PI / 180.0;

                // update the rotation point using the angle
                //rotationPoint = float3(_HullXOffset * cos(rad), _HullYOffset * sin(rad), 0);

                // translate the vertex to new origin in object space
                float3 translatedVertex = v.vertex.xyz  - rotationPoint;

                // stretch the matrix
                float3 stretchedTranslatedMatrix = stretch(translatedVertex, 1, abs(diff.y) + abs(diff.x));
                
                // rotate the translated vertex in object space
                float3 rotatedTranslatedVertex = rotate(stretchedTranslatedMatrix, rad);

                // translate the vertex back
                float3 untranslatedVertex = rotatedTranslatedVertex.xyz + rotationPoint;

                // transform to clip space
                float4 clipVertex = UnityObjectToClipPos(untranslatedVertex);

                // pass the data to the fragment shader
                o.vertex = clipVertex;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); 
                o.intensity = 1 / pow((diff.x * diff.x + diff.y * diff.y), 0.25) - 0.2;


                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //return (i.vertex);

                fixed4 col = tex2D(_MainTex, i.uv);
                col.rgb = float3(0, 0, 0);
                float intensity = col.a * i.intensity;

                return float4(col.rgb * intensity, intensity);
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
