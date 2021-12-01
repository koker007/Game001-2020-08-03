Shader "FlexibleCelShader/Cel Outline Cutoff Test" {
    Properties
    {
        _Color("Global Color Modifier", Color) = (1, 1, 1, 1)
        _MainTex("Texture", 2D) = "white" {}
        _NormalTex("Normal", 2D) = "bump" {}

        _CutOff("CutOff", Range(0,1)) = 0.5

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
         Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
         LOD 200
         CGPROGRAM
         #pragma surface surf Lambert alpha noforwardadd

         sampler2D _MainTex;
         fixed4 _Color;
         float _Cutoff;
         struct Input {
          float2 uv_MainTex;

         };
         void surf(Input IN, inout SurfaceOutput o) {
          fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
          o.Albedo = c.rgb;
          if (c.a > _Cutoff)
           o.Alpha = c.a;
          else
           o.Alpha = 0;
         }
         ENDCG
     }
         Fallback "Diffuse"
}