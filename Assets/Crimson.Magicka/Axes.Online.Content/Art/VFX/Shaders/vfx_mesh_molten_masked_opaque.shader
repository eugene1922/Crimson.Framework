Shader "VFX/Mesh/Molten Masked Opaque" {
    Properties {
	[Header(Textures)]
        [NoScaleOffset] _MainTex ("Diffuse Texture", 2D) = "white" {}
        [NoScaleOffset] _MaskTex ("Emissive Mask", 2D) = "white" {}
        _NoiseTex ("Emissive Noise", 2D) = "white" {}
	_FlowAmount ("Flow Amount", Float ) = 0
	[Header(Color)]
        _Color ("Color", Color) = (1,0.2768762,0.1544118,1)
        _CracksPower ("Emissive Exponent", Range(0, 50)) = 10
        _CracksEmissive ("Emissive Power", Range(0, 10)) = 4
	[Header(Motion)]
        _SpeedX ("Speed X", Float ) = 0
        _SpeedY ("Speed Y", Float ) = 0
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
	    float4 _NoiseTex_ST;
	    float _FlowAmount;
	    half4 _Color;
            half _CracksPower;
            half _CracksEmissive;
            float _SpeedX;
            float _SpeedY;

            struct VertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.uv0 = v.texcoord0;
		o.uv0.zw = transform(float2(posWorld.y, posWorld.z), _NoiseTex_ST) + float2(_SpeedX, _SpeedY) * _Time.y;
                o.vertexColor = v.vertexColor;
		o.vertexColor.rgb = o.vertexColor.rgb * _Color.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {
                half4 tex = tex2D(_MainTex, i.uv0.xy);
                half4 mask = tex2D(_MaskTex, i.uv0.xy);
                mask.r = pow(mask.r, _CracksPower);
		half4 noise = tex2D(_NoiseTex, i.uv0.zw + mask.r * _FlowAmount);
                mask.r = mask.r * _CracksEmissive;
		noise.rgb = mask.r * (mask.r * noise.r * i.vertexColor.rgb);
                tex.rgb = tex.rgb + noise.rgb;
		tex.a = 1.0;
                return tex;
            }
            ENDCG
        }
    }
}
