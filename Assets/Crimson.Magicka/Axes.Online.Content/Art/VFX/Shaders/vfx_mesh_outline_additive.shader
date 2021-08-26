Shader "VFX/Mesh/Outline Additive" {
    Properties {
	[Header(Textures)]
        [NoScaleOffset] _MainTex ("Diffuse Texture", 2D) = "bump" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
	[Header(Color)]
        _Color ("Color", Color) = (1,0.3931034,0,1)
	_TextureEmissive ("Texture Emissive", Range(0, 5)) = 1
	_NoiseEmissive ("Noise Emissive", Range(0, 15)) = 2
	_EmissivePower ("Emissive Power", Range(0, 15)) = 1
	[Header(Motion)]
	_SpeedX ("Speed X", Float) = 0
	_SpeedY ("Speed Y", Float) = 0
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Blend One One
            Cull Front
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "vfx.cginc"

            half4 _Color;
            sampler2D _MainTex;
            sampler2D _NoiseTex;
	    float4 _NoiseTex_ST;
	    half _TextureEmissive;
	    half _NoiseEmissive;
	    half _EmissivePower;
	    float _SpeedX;
	    float _SpeedY;

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
                float4 projPos = ComputeScreenPos(o.pos);
                COMPUTE_EYEDEPTH(projPos.z);
		o.uv0.zw = projPos.xy / projPos.w;
		o.uv0.zw = o.uv0.zw + float2(_SpeedX, _SpeedY) * _Time.y;
		o.uv0.zw = transform(o.uv0.zw, _NoiseTex_ST);
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {
                half4 tex = tex2D(_MainTex, i.uv0.xy);
                half4 noise = tex2D(_NoiseTex, i.uv0.zw);
		tex.r = desaturate(tex.rgb);
		tex.r = tex.r * _TextureEmissive + (tex.r * noise.r * _NoiseEmissive);
		tex.rgb = tex.r * _Color.rgb * _EmissivePower * i.vertexColor.a;
		tex.a = 1.0;
                return tex;
            }
            ENDCG
        }
    }
}
