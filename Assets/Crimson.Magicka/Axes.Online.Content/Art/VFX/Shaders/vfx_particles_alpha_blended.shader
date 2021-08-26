Shader "VFX/Particles/Alpha Blended" {
    Properties {
	[Header(Textures)]
        [NoScaleOffset] _MainTex ("Particle Texture", 2D) = "white" {}
	[Header(Color)]
        _AlphaPremultiplyAmount ("Alpha Premultiply", Range(0, 1)) = 0
        [Toggle] _AlphaFromGrayScale ("Alpha From Gray Scale", Float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }
        Pass {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            half _AlphaPremultiplyAmount;
            half _AlphaFromGrayScale;

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
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {
                half4 tex = tex2D(_MainTex, i.uv0.xy);
                tex = lerp(tex, half4(1.0, 1.0, 1.0, tex.r), _AlphaFromGrayScale);
                tex.a = saturate(tex.a - i.uv0.b);
                tex.rgb = tex.rgb * i.vertexColor.rgb * lerp(1.0, tex.a, _AlphaPremultiplyAmount);
                tex.a = tex.a * i.vertexColor.a;
                return tex;
            }
            ENDCG
        }
    }
}
