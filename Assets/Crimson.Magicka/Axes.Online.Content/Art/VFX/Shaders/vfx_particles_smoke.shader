Shader "VFX/Particles/Smoke" {
    Properties {
	[Header(Textures)]
        [NoScaleOffset] _MainTex ("Smoke Texture", 2D) = "white" {}
        _NoiseTex ("Smoke Fade Texture", 2D) = "white" {}
	[Header(Color)]
        _EmissivePower ("Emissive Power", Range(0, 2)) = 1.3
        _Contrast ("Contrast", Range(0, 1)) = 0.9
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
	    #include "vfx.cginc"

            sampler2D _MainTex;
            sampler2D _NoiseTex;
	    float4 _NoiseTex_ST;
            half _EmissivePower;
            half _Contrast;

            struct VertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
                half4 vertexColor : COLOR;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                half4 vertexColor : COLOR;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;		
                o.uv1 = transform(v.texcoord0, _NoiseTex_ST);
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {
                half4 noise = tex2D(_NoiseTex, i.uv1.xy);
                half4 tex = tex2D(_MainTex, i.uv0.xy);

		tex.a = saturate((1.0 - noise.r) + (i.uv0.b * 2.0 - 1.0));
                tex.a = saturate(tex.r - tex.a);
                tex.rgb = i.vertexColor.rgb * lerp(1.0, tex.a, _Contrast) * _EmissivePower;
		tex.a = tex.a * i.vertexColor.a;
                return tex;
            }
            ENDCG
        }
    }
}
