// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "kawetofe/HighlightShaderWithOutline" {
	Properties {
		//_MainColor("Highlight Color", Color) = (1,1,1,1)
		_Color("Highlight Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BumpMap ("Normal Map", 2D) = "bump" {}
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimPower("Rim Power", Range(1.0,30.0)) = 3.0
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
				float4 vertex : POSITION;
				fixed4 normal : NORMAL;
			};

			struct vert2Pixel
			{
				float4 pos : SV_POSITION;
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




		// RIM Work

		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
			float4 color : Color;
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		};
		
		float4 _MainColor;
		sampler2D _MainTex;
		sampler2D _BumpMap;
		float4 _RimColor;
		float _RimPower;

		void surf (Input IN, inout SurfaceOutput o) {
			IN.color = _MainColor;
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * IN.color;
			o.Normal = UnpackNormal(tex2D(_BumpMap,IN.uv_BumpMap));
			half rim = 1.0 - saturate(dot(normalize(IN.viewDir),o.Normal));
			o.Emission =20.0* _RimColor.rgb * _RimColor.a * pow(rim,_RimPower);
		}
		ENDCG
	} 

	FallBack "Diffuse"
}
