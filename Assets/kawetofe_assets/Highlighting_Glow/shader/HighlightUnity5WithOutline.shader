// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "kawetofe/highlightShaderUnity5WithOutline" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimPower("Rim Power",Range(1.0,30.0)) = 3.0
		_outlineColor("Outline Color", Color) = (0,0,0,1)
		_outlineThickness("Outline Thickness", Range(0.0, 0.025)) = 0.01
		_outlineThickness(" ", Float) = 0.01
		_outlineShift("Outline Light Shift", Range(0.0, 0.025)) = 0.01
		_outlineShift(" ", Float) = 0.01
	}
	SubShader {
			//prepass for outline

			Pass
		{
			ZWrite On
			Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
			   
			Cull Front

			CGPROGRAM
			
			#pragma vertex vShader
			#pragma fragment pShader
			#include "UnityCG.cginc"

			uniform fixed4 _outlineColor;
			uniform fixed _outlineThickness;
			uniform fixed _outlineShift;

			struct app2vert {
				float4 vertex 	: 	POSITION;
				fixed4 normal : NORMAL;
			};

			struct vert2Pixel
			{
				float4 pos 	: 	SV_POSITION;
			};
			vert2Pixel vShader(app2vert IN)
			{
				vert2Pixel OUT;
				float4x4 WorldViewProjection = UNITY_MATRIX_MVP;
				float4x4 WorldInverseTranspose = unity_WorldToObject;
				float4x4 World = unity_ObjectToWorld;

				float4 deformedPosition = mul(World, IN.vertex);
				//fixed3 norm = normalize(mul(IN.normal.xyz , WorldInverseTranspose).xyz);
				fixed3 norm = IN.normal.xyz;

				half3 pixelToLightSource = _WorldSpaceLightPos0.xyz - (deformedPosition.xyz *_WorldSpaceLightPos0.w);
				fixed3 lightDirection = normalize(-pixelToLightSource);
				fixed diffuse = saturate(ceil(dot(IN.normal, lightDirection)));

				deformedPosition.xyz += (norm * _outlineThickness) + (lightDirection * _outlineShift);

				deformedPosition.xyz = mul(WorldInverseTranspose, float4 (deformedPosition.xyz, 1)).xyz * 1.0;


				OUT.pos = mul(WorldViewProjection, deformedPosition);

				return OUT;
			}

			fixed4 pShader(vert2Pixel IN) : COLOR
			{
				fixed4 outColor;
				outColor = _outlineColor;
				return outColor;

			}
				ENDCG
			}


		
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows

			// Use shader model 3.0 target, to get nicer looking lighting
			//#pragma target 3.0
			sampler2D _MainTex;
			struct Input {
				float2 uv_MainTex;
				float3 viewDir;

			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			float4 _RimColor;
			float _RimPower;

			void surf(Input IN, inout SurfaceOutputStandard o) {
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
				half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
				o.Emission = 20.0* _RimColor.rgb * _RimColor.a * pow(rim, _RimPower);
			}
			ENDCG
		
	}
	FallBack "Diffuse"
}
