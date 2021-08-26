Shader "VFX/Mesh/Panner Min Masked Additive" {
    Properties {
	[Header(Textures)]
        _MainTex ("Noise First", 2D) = "white" {}
        _NoiseTex ("Noise Second", 2D) = "white" {}
        [NoScaleOffset] _MaskTex ("Mask Texture", 2D) = "white" {}
	[Header(Color)]
        _EmissivePower ("Emissive Power", Range(0, 5)) = 3
        _EmissiveExponent ("Emissive Exponent", Range(0.001, 6)) = 1.5
	[Header(Motion)]
        _SpeedX ("Speed X", Float ) = 0.5
        _SpeedY ("Speed Y", Float ) = 0
        _SpeedDifference ("Speed Difference", Float ) = 1.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "vfx.cginc"

            sampler2D _MaskTex;
            sampler2D _NoiseTex;
            sampler2D _MainTex;
	    float4 _NoiseTex_ST;
	    float4 _MainTex_ST;
            half _EmissivePower;
            half _EmissiveExponent;
            float _SpeedDifference;
            float _SpeedX;
            float _SpeedY;

            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                half4 vertexColor : COLOR;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                half4 vertexColor : COLOR;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
		o.uv1.zw = float2(_SpeedX, _SpeedY) * _Time.y;
		o.uv1.xy = transform(o.uv0.xy, _MainTex_ST) + o.uv1.zw;
		o.uv1.zw = transform(o.uv0.xy, _NoiseTex_ST) + o.uv1.zw * _SpeedDifference;
                o.vertexColor = v.vertexColor;
		o.vertexColor.rgb = o.vertexColor.rgb * _EmissivePower * o.vertexColor.a;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {
                half4 tex1 = tex2D(_MainTex, i.uv1.xy);
                half4 tex2 = tex2D(_NoiseTex, i.uv1.zw);
                half4 mask = tex2D(_MaskTex, i.uv0.xy);
		tex1.a = min(tex1.r, tex2.r);
		tex1.a = pow(tex1.a,_EmissiveExponent) * mask.r;
		tex1.rgb = i.vertexColor.rgb * tex1.a;
                return tex1;
            }
            ENDCG
        }
    }
}
