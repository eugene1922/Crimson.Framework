Shader "VFX/Particles/Additive" {
    Properties {
	[Header(Texture)]
        [NoScaleOffset] _MainTex ("Particle Texture", 2D) = "white" {}
	[Header(Color)]
        _EmissivePower ("Emissive Power", Range(0, 6)) = 1
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
	    Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            half _EmissivePower;

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
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
		o.vertexColor.a = o.vertexColor.a * _EmissivePower;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {
                half4 tex = tex2D(_MainTex, i.uv0.xy);
                tex.r = saturate(tex.r - i.uv0.b);
		tex.a = tex.r;
		tex.g = saturate(tex.r - i.uv0.a);
		tex.r = tex.r * i.vertexColor.a;
                tex.rgb = saturate(i.vertexColor.rgb + tex.g) * tex.r;
                return tex;
            }
            ENDCG
        }
    }
}
