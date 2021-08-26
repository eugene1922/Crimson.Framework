Shader "VFX/Special/Potion" {
    Properties {
	[Header(Textures)]
        [NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}
        _NoiseTex ("NoiseTex", 2D) = "white" {}
        [NoScaleOffset] _NoiseTexAlt ("NoiseTexAlt", 2D) = "white" {}
        [NoScaleOffset] _MaskTex ("MaskTex", 2D) = "white" {}
	[Header(Color)]
        _ColorIn ("Color In", Color) = (1,0.3869168,0.3161765,1)
        _ColorOut ("Color Out", Color) = (0.3455882,0,0,1)
        _FresnelExponent ("Fresnel Exponent", Range(0, 5)) = 1.2
        _NoiseEmissive ("Noise Emissive", Range(0, 5)) = 2
        _RimLightEmisiive ("RimLight Emisiive", Range(0, 3)) = 1.3
	[Header(Specular)]
        _SpecularExponent ("Specular Exponent", Range(0, 30)) = 15
        _SpecularEmissive ("Specular Emissive", Range(0, 1)) = 0.8
	[Header(Motion)]
        _SpeedX ("Speed X", Float ) = 0.2
        _SpeedY ("Speed Y", Float ) = -0.2
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "vfx.cginc"

            sampler2D _MainTex;
            sampler2D _MaskTex;
            sampler2D _NoiseTex;
            sampler2D _NoiseTexAlt;
	    float4 _NoiseTex_ST;
            half _FresnelExponent;
            half _RimLightEmisiive;
            half _NoiseEmissive;
            half4 _ColorIn;
            half4 _ColorOut;
            half _SpecularExponent;
            half _SpecularEmissive;
            float _SpeedX;
            float _SpeedY;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord0 : TEXCOORD0;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float3 normalDir = normalize(UnityObjectToWorldNormal(v.normal));
                float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
		float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);
		o.uv0.z = fresnel(viewDir, normalDir, _FresnelExponent);
		o.uv0.w = 1.0 - o.uv0.z;
                o.pos = UnityObjectToClipPos(v.vertex);
		float2 panner = float2(_SpeedX, _SpeedY) * _Time.y;
		float2 worlduv = transform(posWorld.xz, _NoiseTex_ST);
		o.uv1.xy = worlduv + panner;
		o.uv1.zw = worlduv + panner * 0.5;
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {
                half4 tex = tex2D(_MainTex, i.uv0.xy);
                half4 mask = tex2D(_MaskTex, i.uv0.xy);
                half4 noise = tex2D(_NoiseTex, i.uv1.xy);
                half4 noisealt = tex2D(_NoiseTexAlt, i.uv1.zw);
                tex.a = desaturate(tex.rgb);
		noise.r = max(noise.r, noisealt.r) * _NoiseEmissive;
                noise.g = saturate(i.uv0.w * 2.0 - 1.0);

		noise.r = lerp(i.uv0.z * _RimLightEmisiive + tex.a, noise.r, noise.g);
		noise.g = saturate(pow(noise.g, _SpecularExponent)) * _SpecularEmissive;
                tex.rgb = lerp(lerp(_ColorOut.rgb, _ColorIn.rgb, noise.r) + noise.g, tex.rgb, mask.r);
		tex.a = 1.0;
                return tex;
            }
            ENDCG
        }
    }
}
