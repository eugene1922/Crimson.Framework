// Upgrade NOTE: upgraded instancing buffer 'TAfadebottom' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TAfadebottom"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_MainTextureIntensity("MainTextureIntensity", Range(0, 10)) = 1
		_MainColor("MainColor", Color) = (1,1,1,0)
		_MainAlphaIntensity("MainAlphaIntensity", Float) = 1
		_Gradient("Gradient", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 2.0
		#pragma multi_compile_instancing
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTexture;
		uniform sampler2D _Gradient;
		fixed _MainTextureIntensity;
		SamplerState sampler_MainTexture;

		UNITY_INSTANCING_BUFFER_START(TAfadebottom)
			UNITY_DEFINE_INSTANCED_PROP(float4, _MainColor)
#define _MainColor_arr TAfadebottom
			UNITY_DEFINE_INSTANCED_PROP(float4, _MainTexture_ST)
#define _MainTexture_ST_arr TAfadebottom
			UNITY_DEFINE_INSTANCED_PROP(float4, _Gradient_ST)
#define _Gradient_ST_arr TAfadebottom
			UNITY_DEFINE_INSTANCED_PROP(float, _MainAlphaIntensity)
#define _MainAlphaIntensity_arr TAfadebottom
		UNITY_INSTANCING_BUFFER_END(TAfadebottom)

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 _MainColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainColor_arr, _MainColor);
			float4 _MainTexture_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainTexture_ST_arr, _MainTexture_ST);
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST_Instance.xy + _MainTexture_ST_Instance.zw;
			float4 tex2DNode4 = tex2D( _MainTexture, uv_MainTexture );
			float4 temp_output_9_0 = ( _MainColor_Instance * tex2DNode4 );
			o.Albedo = temp_output_9_0.rgb;
			o.Albedo *= _MainTextureIntensity;
			o.Emission = temp_output_9_0.rgb * _MainTextureIntensity ;
			float4 color10 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float4 _Gradient_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Gradient_ST_arr, _Gradient_ST);
			float2 uv_Gradient = i.uv_texcoord * _Gradient_ST_Instance.xy + _Gradient_ST_Instance.zw;
			float _MainAlphaIntensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainAlphaIntensity_arr, _MainAlphaIntensity);
			float4 lerpResult8 = lerp( color10 , tex2D( _Gradient, uv_Gradient ) , ( tex2DNode4.a * _MainAlphaIntensity_Instance ));
			o.Alpha = lerpResult8.r;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Lambert alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
2048;72;2048;928;1769.07;404.7135;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;7;-1391,43;Inherit;False;InstancedProperty;_MainAlphaIntensity;MainAlphaIntensity;2;0;Create;True;0;0;False;0;False;1;0.92;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-1450,-330;Inherit;True;Property;_MainTexture;MainTexture;0;0;Create;True;0;0;False;0;False;-1;None;cd460ee4ac5c1e746b7a734cc7cc64dd;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-1088,12;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-1602.298,178.0378;Inherit;True;Property;_Gradient;Gradient;3;0;Create;True;0;0;False;0;False;-1;None;8c4a7fca2884fab419769ccc0355c0c1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;10;-1082.298,-170.9622;Inherit;False;Constant;_Color0;Color 0;3;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;6;-1077.7,-448.2;Inherit;False;InstancedProperty;_MainColor;MainColor;1;0;Create;True;0;0;False;0;False;1,1,1,0;1,0.004716992,0.004716992,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;8;-870.0859,105.5517;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GradientNode;1;-1578,106;Inherit;False;0;2;2;0,0,0,0;1,1,1,1;1,0;1,1;0;1;OBJECT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-647.4332,-254.048;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;13;-876.2979,299.0378;Inherit;False;InstancedProperty;_RemapValues;RemapValues;4;0;Create;True;0;0;False;0;False;0,1,0,1;0,-10.19,0,-1.2;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;12;-533.2979,214.0378;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;0;ASEMaterialInspector;0;0;Lambert;TAfadebottom;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;5;0;4;4
WireConnection;5;1;7;0
WireConnection;8;0;10;0
WireConnection;8;1;11;0
WireConnection;8;2;5;0
WireConnection;9;0;6;0
WireConnection;9;1;4;0
WireConnection;12;0;8;0
WireConnection;12;1;13;1
WireConnection;12;2;13;2
WireConnection;12;3;13;3
WireConnection;12;4;13;4
WireConnection;0;0;9;0
WireConnection;0;2;9;0
WireConnection;0;9;8;0
ASEEND*/
//CHKSM=3E854B5F1A088FDC6B6FBCD46BC47D674108BC91