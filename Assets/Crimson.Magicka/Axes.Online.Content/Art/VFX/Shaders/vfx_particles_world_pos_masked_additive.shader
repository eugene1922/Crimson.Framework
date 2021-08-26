Shader "VFX/Particles/World Pos Masked Additive" {
    Properties {
	[Header(Textures)]
        _MainTex ("Mask Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
	[Header(Color)]
        _EmissivePower ("Emissive Power", Range(0, 5)) = 1
        _MaskEmissive ("Mask Emissive", Range(0, 10)) = 1
        _NoiseEmissive ("Noise Emissive", Range(0, 10)) = 1
        _MaximumThreshold ("Maximum Threshold", Range(0, 10)) = 3
        _MaximumEmissive ("Maximum Emissive", Range(0, 1)) = 1
	[Header(Motion Mask)]
        _SpeedXMask ("Speed X Mask", Float ) = 0
        _SpeedYMask ("Speed Y Mask", Float ) = 0
	[Header(Motion Noise)]
        _SpeedXNoise ("Speed X Noise", Float ) = 0
        _SpeedYNoise ("Speed Y Noise", Float ) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }
        Pass {
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
	    #include "vfx.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseTex;
	    float4 _MainTex_ST;
	    float4 _NoiseTex_ST;
            half _EmissivePower;
            half _NoiseEmissive;
            half _MaskEmissive;
            half _MaximumThreshold;
            half _MaximumEmissive;
            float _SpeedXNoise;
            float _SpeedYNoise;
            float _SpeedXMask;
            float _SpeedYMask;

            struct VertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
                half4 vertexColor : COLOR;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                half4 vertexColor : COLOR;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float4 wpos = mul(unity_ObjectToWorld, v.vertex);
                o.uv0 = v.texcoord0;
		o.uv0.xy = transform(o.uv0.xy, _MainTex_ST);
		o.uv0.xy = o.uv0.xy + float2(_SpeedXMask, _SpeedYMask) * _Time.y;
		o.uv0.zw = transform(wpos.xz, _NoiseTex_ST);
		o.uv0.zw = o.uv0.zw + float2(_SpeedXNoise, _SpeedYNoise) * _Time.y;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {
                half4 tex = tex2D(_MainTex, i.uv0.xy);
                half4 noise = tex2D(_NoiseTex, i.uv0.zw);
                tex.r = tex.r * i.vertexColor.a;
                tex.r = tex.r * _MaskEmissive + tex.r * noise.r * _NoiseEmissive;
                tex.rgb = _MaximumEmissive * saturate(pow(saturate(tex.r),_MaximumThreshold)) + tex.r * i.vertexColor.rgb;
		tex.rgb = tex.rgb * _EmissivePower;
		tex.a = 1.0;
                return tex;
            }
            ENDCG
        }
    }
}
