﻿// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "NightmareFuel/DeformOutlineShader"
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        _BackgroundColor("Background Color", Color) = (1,1,1,1)
        _BackgroundOpacity("Background Opacity", Float) = 1
        _OutlineColor("Outline Color", Color) = (1,1,1,1)
        _OutlineWidth("Outline Width", Float) = 0.1
        _Deform("Deform Amount", Float) = 0.1

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

        Pass // Outline
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

            float3 deform(float3 vec, float r)
            {
                float2x2 stretchMatrix = float2x2(1, r, 0, 1);
                return float3(mul(stretchMatrix, vec.xy), vec.z).xyz;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _OutlineWidth;
            float4 _OutlineColor;
            float _BackgroundOpacity;

            float _Deform;

            v2f vert(appdata v)
            {

                float3 vertex = v.vertex + float3(_OutlineWidth, _OutlineWidth, 0);

                v2f o;
                o.vertex = UnityObjectToClipPos(deform(vertex, _Deform));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = _BackgroundOpacity * tex2D(_MainTex, i.uv) + (1 - _BackgroundOpacity) * tex2D(_MainTex, i.uv).a * _OutlineColor;
                return col;
            }
            ENDCG
        }

        Pass // Outline
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

            float3 deform(float3 vec, float r)
            {
                float2x2 stretchMatrix = float2x2(1, r, 0, 1);
                return float3(mul(stretchMatrix, vec.xy), vec.z).xyz;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _OutlineWidth;
            float4 _OutlineColor;

            float _BackgroundOpacity;

            float _Deform;

            v2f vert(appdata v)
            {

                float3 vertex = v.vertex + float3(-_OutlineWidth, -_OutlineWidth, 0);

                v2f o;
                o.vertex = UnityObjectToClipPos(deform(vertex, _Deform));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = _BackgroundOpacity * tex2D(_MainTex, i.uv) + (1-_BackgroundOpacity) * tex2D(_MainTex, i.uv).a * _OutlineColor;
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

            float3 deform(float3 vec, float r)
            {
                float2x2 stretchMatrix = float2x2(1, r, 0, 1);
                return float3(mul(stretchMatrix, vec.xy), vec.z).xyz;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _BackgroundColor;
            float _BackgroundOpacity;
            float _Deform;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(deform(v.vertex, _Deform));
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float4 col = _BackgroundOpacity * _BackgroundColor + (1- _BackgroundOpacity) * tex2D(_MainTex, i.uv).a * _BackgroundColor;
                return col;
            }
            ENDCG
        }
    }
}