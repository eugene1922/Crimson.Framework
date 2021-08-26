Shader "Custom/CharacterShadowShader"
{
    Properties
    {
        _ShadowTexture("ShadowTexture", 2D) = "" {}
        _ShadowColor ("ShadowColor", Color) = (1, 1, 1, 1)
        _ShadowIntensity ("ShadowIntensity", Range(0, 10)) = 1
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType"="Transparent" }

        CGPROGRAM
        #pragma surface surf Lambert alpha

        #pragma target 2.0


        struct Input
        {
            float2 uv_ShadowTexture;
        };

        sampler2D _ShadowTexture;
        float4 _ShadowColor;
        fixed _ShadowIntensity;

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_ShadowTexture, IN.uv_ShadowTexture) * _ShadowColor;
            o.Alpha = tex2D(_ShadowTexture, IN.uv_ShadowTexture).a * _ShadowIntensity;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
