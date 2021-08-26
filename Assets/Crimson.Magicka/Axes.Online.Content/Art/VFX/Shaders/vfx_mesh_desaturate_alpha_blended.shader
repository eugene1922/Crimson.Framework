Shader "VFX/Mesh/Desaturate Alpha Blended" {
    Properties {
	[Header(Texture)]
        [NoScaleOffset] _MainTex ("Mesh Texture", 2D) = "white" {}
	[Header(Color)]
        _ColorDepth ("Color Depth", Range(0, 1)) = 0.7
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
            half _ColorDepth;

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
		o.uv0.z = o.uv0.z - 1.0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {
                half4 tex = tex2D(_MainTex, i.uv0.xy);
		tex.a = desaturate(tex.rgb);
                tex.rgb = lerp(6.0 * i.vertexColor.rgb * tex.a, tex.aaa, _ColorDepth);
		tex.a = i.vertexColor.a * saturate(tex.a - i.uv0.z);
                return tex;
            }
            ENDCG
        }
    }
}
