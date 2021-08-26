Shader "VFX/Water/Foam" {
    Properties {
        _MainTex ("Foam Texture", 2D) = "white" {}
        [NoScaleOffset] _NoiseTex ("Foam Flow Map", 2D) = "white" {}
        [NoScaleOffset] _MaskTex ("Foam Mask Texture", 2D) = "white" {}
	[Header(Color)]
        _ColorIn ("Color In", Color) = (0.9044118,0.9044118,0.9044118,1)
        _ColorOut ("Color Out", Color) = (0.5441177,1,0.8302231,1)
        _GradientPosition ("Gradient Position", Float ) = -0.7
	[Header(Motion)]
        _Speed ("Wave Speed", Float ) = 0.7
        _WaveMotionAmount ("Wave Motion Amount", Float ) = 1
        _WaveFlowAmount ("Wave Flow Amount", Range(0, 0.5)) = 0.08
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
            sampler2D _MaskTex;
	    float4 _MainTex_ST;
            float _WaveMotionAmount;
            float _WaveFlowAmount;
            float _Speed;
            half4 _ColorOut;
            half4 _ColorIn;
            half _GradientPosition;

            struct VertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };

            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
            };

            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                float3 worldpos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv0 = v.texcoord0;
		o.uv0.y = 1 - o.uv0.y;
		o.uv1.zw = transform(o.uv0.xy, _MainTex_ST);
		o.uv0.w = o.uv1.w + cos(_Speed * _Time.y + 0.5) * _WaveMotionAmount * 0.4;
		o.uv0.z = o.uv1.w + sin(_Speed * _Time.y) * _WaveMotionAmount;
		o.uv1.xy = float2(o.uv1.z, o.uv0.z * -0.3 + o.uv1.w);
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(VertexOutput i) : COLOR {

                float4 flowmap = tex2D(_NoiseTex, i.uv1.xy);
                flowmap.x = (flowmap.x * 2.0 - 1.0) * _WaveFlowAmount;
                flowmap.x = i.uv0.z + flowmap.r;
                flowmap.xy = float2(i.uv1.z, i.uv1.w + flowmap.x * _WaveMotionAmount - 0.2);

                half4 tex1 = tex2D(_MainTex, float2(i.uv0.x, i.uv0.z));
		half4 tex2 = tex2D(_MainTex, float2(i.uv0.x + 0.5, i.uv0.w));
		tex1.r = tex1.r + tex2.r;
                half4 mask = tex2D(_MaskTex, i.uv0.xy);

                mask.g = 1.0 - saturate(flowmap.y + _GradientPosition);
                tex1.a = tex1.r * mask.g * mask.r;
                tex1.rgb = lerp(_ColorOut.rgb, _ColorIn.rgb, tex1.a);
                return tex1;
            }
            ENDCG
        }
    }
}
