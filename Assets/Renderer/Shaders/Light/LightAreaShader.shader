// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "NightmareFuel/LightAreaShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Intensity("Intensity", Float) = 1
        _Threshold("Threshold", Float) = 0

        _Radius("Radius", Float) = 1
        _Position("Position", Vector) = (0, 0, 0)
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
                float2 lightPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float3 _Position;
            float _Intensity;
            float _Threshold;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.lightPos = _Position;
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float2 lightPos = (i.lightPos - 0.5) * 2;
                float2 pos = (i.uv - 0.5) * 2;
                float2 vec = lightPos - pos;

                return float4(col.rgb * col.a * _Intensity, col.a * _Intensity);
            }
            ENDCG
        }
    }
}
