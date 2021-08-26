Shader "VFX/Surface/Ethereal Body" {
    Properties {
	[Header(Textures)]
        [NoScaleOffset] _MainTex ("Diffuse Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
	[Header(Color)]
        _Color ("Color", Color) = (1,0.5,0.5,1)
        _AlphaPremultiplyAmount ("Alpha Premultiply Amount", Range(0, 1)) = 1
        _FresnelExponent ("Fresnel Exponent", Range(0.001, 5)) = 0.7
        _EmissivePower ("Emissive Power", Range(0, 5)) = 1
        _EmissiveTexture ("Emissive Texture", Range(0, 5)) = 0.4
        _EmissiveFresnel ("Emissive Fresnel", Range(0, 5)) = 1
        _EmissiveNoise ("Emissive Noise", Range(0, 5)) = 1
	[Header(Motion)]
        _SpeedX ("Speed X", Float ) = 0
        _SpeedY ("Speed Y", Float ) = -0.7
	[Header(FX Value)]
        _FxValue ("FxValue", Range(0, 1)) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "vfx.cginc"	    

            sampler2D _MainTex;
            sampler2D _NoiseTex;
	    float4 _NoiseTex_ST;
            float _SpeedX;
            float _SpeedY;
            half4 _Color;
            half _FresnelExponent;
            half _EmissivePower;
            half _EmissiveNoise;
            half _EmissiveFresnel;
            half _EmissiveTexture;
            half _AlphaPremultiplyAmount;
            half _FxValue;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 normalDir = normalize(UnityObjectToWorldNormal(v.normal));
		float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - posWorld.xyz);
		o.uv1.x = fresnel(viewDir, normalDir, _FresnelExponent);
                o.pos = UnityObjectToClipPos(v.vertex);
                float4 projPos = ComputeScreenPos(o.pos);
                COMPUTE_EYEDEPTH(projPos.z);
		o.uv1.zw = projPos.xy / projPos.w;
		o.uv1.zw = o.uv1.zw + _Time.y * float2(_SpeedX, _SpeedY);
		o.uv1.zw = transform(o.uv1.zw, _NoiseTex_ST);
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {
                half4 tex = tex2D(_MainTex, i.uv0.xy);
                half4 noise = tex2D(_NoiseTex, i.uv1.zw);
		
		tex.r = _EmissiveTexture * desaturate(tex.rgb);
		tex.g = _EmissiveFresnel * i.uv1.x;
		tex.b = _EmissiveNoise * noise.r;
		tex.a = tex.r + tex.g + tex.b;
		
		tex.rgb = _EmissivePower * _Color.rgb * lerp(max(1.0, tex.a), tex.a, _AlphaPremultiplyAmount);
		tex.a = saturate(saturate(tex.a) - _FxValue);
                return tex;
            }
            ENDCG
        }
    }
}
