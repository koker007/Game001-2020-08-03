Shader "FlexibleCelShader/Cel OutlineNew"
{
	Properties
	{
		_Color("Global Color Modifier", Color) = (1, 1, 1, 1)
		_MainTex("Texture", 2D) = "white" {}
		_NormalTex("Normal", 2D) = "bump" {}

		_RampLevels("Ramp Levels", Range(2, 50)) = 2

		_HighIntensity("High Light Intensity", Range(0, 10)) = 1.5

		_LowIntensity("Low Light Intensity", Range(0, 10)) = 1

		_OutlineColor("Outline Color", Color) = (0, 0, 0, 1)
		_OutlineSize("Outline Size", float) = 10

		_FresnelColor("Soft Edge Light Color", Color) = (1,1,1,1)
		_FresnelBrightness("Soft Edge Light Brightness", Range(0, 1)) = 0
		_FresnelPower("Soft Edge Light Size", Range(0, 1)) = 0
	}
	
	SubShader
	{
		
		// This pass renders the object
		Cull back
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			#include "AutoLight.cginc"

			struct v2f
			{
				float2 uv : TEXCOORD0;
				SHADOW_COORDS(1)
				float3 worldNormal : TEXCOORD2;
				float3 worldTangent : TEXCOORD3;
				float3 worldBitangent : TEXCOORD4;
				float4 worldPos : TEXCOORD5;
				float4 pos : SV_POSITION;
			};

			v2f vert(appdata_tan v)
			{
				v2f o;

				// UV data
				o.uv = v.texcoord;

				// Position data
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.pos = mul(UNITY_MATRIX_VP, o.worldPos);

				// Normal data
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldTangent = UnityObjectToWorldNormal(v.tangent);
				o.worldBitangent = cross(o.worldTangent, o.worldNormal);

				// Compute shadows data
				TRANSFER_SHADOW(o);

				return o;
			}

			float4    _Color;
			sampler2D _MainTex;
			uniform float4 _MainTex_ST;
			sampler2D _NormalTex;
			uniform float4 _NormalTex_ST;
			uniform float4 _EmmisTex_ST;
			int       _RampLevels;
			float     _HighIntensity;
			float     _LowIntensity;

			float     _FresnelBrightness;
			float     _FresnelPower;
			float4    _FresnelColor;

			fixed4 frag(v2f i) : SV_Target
			{
				_RampLevels -= 1;

				// Get view direction && light direction for rim lighting
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);

				// Sample textures
				fixed4 col = tex2D(_MainTex, i.uv * _MainTex_ST.xy + _MainTex_ST.zw);
				fixed3 tangentNormal = tex2D(_NormalTex, i.uv * _NormalTex_ST.xy + _NormalTex_ST.zw) * 2 - 1;

				// Get normal
				float3 worldNormal = float3(i.worldTangent * tangentNormal.r + i.worldBitangent * tangentNormal.g + i.worldNormal * tangentNormal.b);

				// Rim Lighting
				half factor = dot(viewDirection, worldNormal);
				half fresnelFactor = 1 - min(pow(max(1 - factor, 0), (1 - _FresnelPower) * 10), 1);

				// Get shadow attenuation
				fixed shadow = SHADOW_ATTENUATION(i);

				// Calculate light intensity
				float intensity = dot(worldNormal, lightDirection);
				intensity = clamp(intensity, 0, 1);

				// Factor in the shadow
				intensity *= shadow;

				// Determine level
				float rampLevel = round(intensity * _RampLevels);

				// Get light multiplier based on level
				float lightMultiplier = _LowIntensity + ((_HighIntensity - _LowIntensity) / (_RampLevels)) * rampLevel;

				// Get color multiplier based on level
				float4 highColor = rampLevel / _RampLevels;
				float4 lowColor = (_RampLevels - rampLevel) / _RampLevels;
				float4 mixColor = (highColor + lowColor) / 2;

				// Apply light multiplier and color
				col *= lightMultiplier;
				col *= _Color * mixColor;

				// Apply soft Fresnel
				float rampPercentSoftFresnel = 1 - (1 - rampLevel / _RampLevels);
				col.rgb = col.rgb + _FresnelColor * (_FresnelBrightness * 10 - fresnelFactor * _FresnelBrightness * 10) * rampPercentSoftFresnel;


				return col;
			}

			ENDCG
		} // End Main Pass

		// This Pass Renders the outlines
		Cull Front
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			float _OutlineSize;
			v2f vert(appdata v)
			{
				v2f o;
				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				worldPos.xyz = worldPos.xyz + worldNormal * _OutlineSize * 0.001;
				o.vertex = mul(UNITY_MATRIX_VP, worldPos);
				return o;
			}

			float4 _OutlineColor;
			fixed4 frag(v2f i) : SV_Target
			{
				return _OutlineColor;
			}
				ENDCG
		}// End Outline Pass

		// Shadow casting
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}

	CustomEditor "CelCustomEditor"
}