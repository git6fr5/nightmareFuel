Shader "PostEffects/ScreenShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _AddIntensity("Add Intensity", Float) = 1
        _AddColor("Add Color", Color) = (1,0,0,1)
        _MultiplyIntensity("Multiply Intensity", Float) = 1
        _MultiplyColor("Multiply Color", Color) = (0,1,0,1)

    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
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

            uniform float3 _LightSourcePosition;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _MultiplyColor;
            float4 _AddColor;
            float _MultiplyIntensity;
            float _AddIntenisty;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float4 m = _MultiplyColor;
                float4 mCol = col.a * m.a * float4(m.r * col.r, m.g * col.g, m.b * col.b, 1) * _MultiplyIntensity;

                float4 a = _AddColor;
                float4 aCol = col.a * _AddColor * _AddIntenisty;

                float4 o = (col + mCol + aCol) * col.a;
                return o;
            }
            ENDCG
        }
    }
}
