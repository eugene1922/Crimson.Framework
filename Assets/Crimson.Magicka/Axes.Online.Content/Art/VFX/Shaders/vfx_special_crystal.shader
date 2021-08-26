Shader "VFX/Special/Crystal" {
    Properties {
	[Header(Textures)]
        [NoScaleOffset] _MainTex ("Crystal Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
	[Header(Color)]
        _ColorIn ("Color In", Color) = (1,0.2941176,0.8636914,1)
        _ColorOut ("Color Out", Color) = (0.196501,0,0.2279412,1)
        _MaximumPower ("Maximum Power", Range(0.001, 6)) = 6
        _MaximumEmissive ("Maximum Emissive", Range(0, 1)) = 0.7
        _CrysalTextureExponent ("Crysal Texture Exponent", Range(0.001, 4)) = 1.4
        _NoiseEmissive ("Noise Emissive", Range(0, 5)) = 1
	[Header(Fresnel)]
        _FresnelExponent ("Fresnel Exponent", Range(0, 1)) = 1
        _FresnelEmissive ("Fresnel Emissive", Range(0, 7)) = 0.8
	[Header(Motion)]
        _SpeedX ("Speed X", Float ) = 0.4
        _SpeedY ("Speed Y", Float ) = 0.4
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
            sampler2D _NoiseTex;
	    float4 _NoiseTex_ST;
            half4 _ColorIn;
	    half4 _ColorOut;
            half _CrysalTextureExponent;
            half _FresnelEmissive;
            half _FresnelExponent;
            half _MaximumPower;
            half _MaximumEmissive;
            half _NoiseEmissive;
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
		o.uv0.zw = transform(v.texcoord0.xy, _NoiseTex_ST) + float2(_SpeedX, _SpeedY) * _Time.y;
                float3 normalDir = UnityObjectToWorldNormal(v.normal);
                float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
		float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);
		o.uv1.x = fresnel(viewDir, normalDir, _FresnelExponent);
		o.uv1.y = 1.0 - o.uv1.x;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {
                half4 noise = tex2D(_NoiseTex, i.uv0.zw);
                half4 tex = tex2D(_MainTex, i.uv0.xy);
                tex.r = pow(desaturate(tex.rgb), _CrysalTextureExponent);
                tex.g = saturate(i.uv1.y * 3.0 - 2.0);
		tex.b = i.uv1.x * noise.r * _NoiseEmissive;
                tex.r = (tex.b + tex.r) + (tex.g * _FresnelEmissive);
		tex.g = pow(saturate(tex.r * _MaximumEmissive), _MaximumPower);
                tex.rgb = lerp(_ColorOut.rgb, _ColorIn.rgb, tex.r) + tex.g;
                tex.a = 1.0;
                return tex;
            }
            ENDCG
        }
    }
}
