Shader "VFX/Mesh/Noise Burn Alpha Blended" {
    Properties {
	[Enum(Off,0,On,1)] _ZWrite ("ZWrite", Float) = 0.0
	[Header(Textures)]
        [NoScaleOffset] _MainTex ("MainTex", 2D) = "white" {}
        _NoiseTex ("NoiseTex", 2D) = "white" {}
	[Header(Color)]
        _EmissiveExponent ("Emissive Exponent", Range(0, 8)) = 5
        _DarknessPower ("Darkness Power", Range(0, 1)) = 0.4
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite [_ZWrite]
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "vfx.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseTex;
	    float4 _NoiseTex_ST;
            half _EmissiveExponent;
            half _DarknessPower;
            half _EmissivePower;

            struct VertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 vertexColor : COLOR;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
		o.uv0.z = o.uv0.z - 1.0;
		o.uv0.w = max(0.0, o.uv0.w + 8.0);
		o.uv1.xy = transform(v.texcoord0.xy, _NoiseTex_ST);
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {
                half4 tex = tex2D(_MainTex, i.uv0.xy);
                half4 noise = tex2D(_NoiseTex, i.uv1.xy);

                noise.r = noise.r - i.uv0.z;
                noise.r = noise.r * 2.0 - 1.0;
                noise.g = saturate(noise.r);
		
		noise.b = (1.0 - noise.g) + 0.3;
		noise.b = pow(noise.b, _EmissiveExponent) * i.uv0.w;

		noise.r = saturate(noise.r - _DarknessPower);

                tex.rgb = tex.rgb * noise.r + noise.b * i.vertexColor.rgb;
		tex.a = round(noise.g) * i.vertexColor.a;
                return tex;
            }
            ENDCG
        }
    }
}
