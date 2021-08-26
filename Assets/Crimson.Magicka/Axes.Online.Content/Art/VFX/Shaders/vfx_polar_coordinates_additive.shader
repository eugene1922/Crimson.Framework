Shader "VFX/Polar Coord/Masked Additive" {
    Properties {
	[Header(Textures)]
        _MainTex ("Noise Texture", 2D) = "white" {}
        [NoScaleOffset] _MaskTex ("Mask Texture", 2D) = "white" {}
        [NoScaleOffset] _PolarCoord ("Polar Coordinates Map", 2D) = "white" {}
	[Header(Color)]
        _EmissivePower ("Emissive Power", Range(0, 10)) = 2
        _MaximumThreshold ("Maximum Threshold", Range(0.0001, 10)) = 1.5
        _MaximumEmissive ("Maximum Emissive", Range(0, 1)) = 1
	[Header(Motion)]
        _SpeedRotation ("Speed Rotation", Float ) = 0
        _SpeedExtrusion ("Speed Extrusion", Float ) = 0
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
	    Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
	    #include "vfx.cginc"

            sampler2D _PolarCoord;
            sampler2D _MaskTex;
            sampler2D _MainTex;
	    float4 _MainTex_ST;
            half _MaximumThreshold;
            half _MaximumEmissive;
            half _EmissivePower;
            float _SpeedRotation;
            float _SpeedExtrusion;

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
		o.uv0.z = _SpeedRotation * _Time.y + o.uv0.z;
		o.uv0.w = _SpeedExtrusion * _Time.y + o.uv0.w;
                o.vertexColor = v.vertexColor;
		o.vertexColor.a = o.vertexColor.a * _EmissivePower;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {
                float4 polarcoord = tex2Dlod(_PolarCoord, float4(i.uv0.xy, 0.0, 1.0));
                polarcoord.xy = float2(polarcoord.r + i.uv0.z, polarcoord.g + i.uv0.w);
		polarcoord.xy = transform(polarcoord.xy, _MainTex_ST);

                half4 tex = tex2Dlod(_MainTex, float4(polarcoord.xy, 0.0, 1.0));
		half4 mask = tex2D(_MaskTex, i.uv0.xy);

                tex.a = tex.r * mask.r;
                tex.rgb = _MaximumEmissive * pow(tex.a,_MaximumThreshold) + tex.a * i.vertexColor.rgb;
		tex.rgb = tex.rgb * i.vertexColor.a;
                return tex;
            }
            ENDCG
        }
    }
}
