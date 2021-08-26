// Upgrade NOTE: upgraded instancing buffer 'MobileStandardGeoShader' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Mobile/StandardGeoShader"
{
	Properties
	{
		_MainColor("MainColor", Color) = (1,1,1,0)
		_MainTexture("MainTexture", 2D) = "white" {}
		_Saturation("Saturation", Range( -1 , 10)) = 0
		_MainTextureIntensity("MainTextureIntensity", Range( 0 , 10)) = 1
		_Normals("Normals", 2D) = "white" {}
		_NormalIntensity("NormalIntensity", Range( 0 , 5)) = 0
		_SpecularColor("SpecularColor", Color) = (0,0,0,0)
		_SpecularTexture("SpecularTexture", 2D) = "white" {}
		_SmoothnessIntensity("SmoothnessIntensity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#pragma target 2.0
		#pragma multi_compile_instancing
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Normals;
		uniform sampler2D _MainTexture;
		uniform sampler2D _SpecularTexture;

		UNITY_INSTANCING_BUFFER_START(MobileStandardGeoShader)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Normals_ST)
#define _Normals_ST_arr MobileStandardGeoShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _MainColor)
#define _MainColor_arr MobileStandardGeoShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _MainTexture_ST)
#define _MainTexture_ST_arr MobileStandardGeoShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _SpecularTexture_ST)
#define _SpecularTexture_ST_arr MobileStandardGeoShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _SpecularColor)
#define _SpecularColor_arr MobileStandardGeoShader
			UNITY_DEFINE_INSTANCED_PROP(float, _NormalIntensity)
#define _NormalIntensity_arr MobileStandardGeoShader
			UNITY_DEFINE_INSTANCED_PROP(float, _MainTextureIntensity)
#define _MainTextureIntensity_arr MobileStandardGeoShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Saturation)
#define _Saturation_arr MobileStandardGeoShader
			UNITY_DEFINE_INSTANCED_PROP(float, _SmoothnessIntensity)
#define _SmoothnessIntensity_arr MobileStandardGeoShader
		UNITY_INSTANCING_BUFFER_END(MobileStandardGeoShader)

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float4 _Normals_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Normals_ST_arr, _Normals_ST);
			float2 uv_Normals = i.uv_texcoord * _Normals_ST_Instance.xy + _Normals_ST_Instance.zw;
			float4 tex2DNode2 = tex2D( _Normals, uv_Normals );
			o.Normal = tex2DNode2.rgb;
			float4 _MainColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainColor_arr, _MainColor);
			float4 _MainTexture_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainTexture_ST_arr, _MainTexture_ST);
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST_Instance.xy + _MainTexture_ST_Instance.zw;
			float _NormalIntensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_NormalIntensity_arr, _NormalIntensity);
			float4 lerpResult16 = lerp( tex2D( _MainTexture, uv_MainTexture ) , tex2DNode2 , _NormalIntensity_Instance);
			float _MainTextureIntensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainTextureIntensity_arr, _MainTextureIntensity);
			float _Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Saturation_arr, _Saturation);
			float3 desaturateInitialColor5 = ( lerpResult16 * _MainTextureIntensity_Instance ).rgb;
			float desaturateDot5 = dot( desaturateInitialColor5, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar5 = lerp( desaturateInitialColor5, desaturateDot5.xxx, ( _Saturation_Instance * -1.0 ) );
			o.Albedo = ( _MainColor_Instance * float4( desaturateVar5 , 0.0 ) ).rgb;
			float4 _SpecularTexture_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_SpecularTexture_ST_arr, _SpecularTexture_ST);
			float2 uv_SpecularTexture = i.uv_texcoord * _SpecularTexture_ST_Instance.xy + _SpecularTexture_ST_Instance.zw;
			float4 _SpecularColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_SpecularColor_arr, _SpecularColor);
			o.Specular = ( tex2D( _SpecularTexture, uv_SpecularTexture ) * _SpecularColor_Instance ).rgb;
			float _SmoothnessIntensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_SmoothnessIntensity_arr, _SmoothnessIntensity);
			o.Smoothness = _SmoothnessIntensity_Instance;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1536;9.6;1536;800;1927.068;744.6055;1.348839;True;True
Node;AmplifyShaderEditor.CommentaryNode;10;-1385.295,-614.1309;Inherit;False;932.0878;482.7366;Comment;8;9;7;6;1;5;8;12;13;Main Texture;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;1;-1367.295,-563.9351;Inherit;True;Property;_MainTexture;MainTexture;1;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;11;-1609.108,180.1661;Inherit;False;InstancedProperty;_NormalIntensity;NormalIntensity;5;0;Create;True;0;0;False;0;0;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1533.11,-81.42529;Inherit;True;Property;_Normals;Normals;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;16;-1123.16,-120.0937;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;13;-1321.362,-232.0859;Inherit;False;InstancedProperty;_Saturation;Saturation;2;0;Create;True;0;0;False;0;0;0;-1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1358.552,-325.5348;Inherit;False;InstancedProperty;_MainTextureIntensity;MainTextureIntensity;3;0;Create;True;0;0;False;0;1;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-967.3618,-270.0859;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-1042.546,-402.4121;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;8;-913.5167,-569.5516;Inherit;False;InstancedProperty;_MainColor;MainColor;0;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;5;-804.6055,-352.1049;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;14;-909.7555,209.3951;Inherit;True;Property;_SpecularTexture;SpecularTexture;7;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;3;-935.4691,442.6121;Inherit;False;InstancedProperty;_SpecularColor;SpecularColor;6;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;9;-621.2009,-404.6694;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-764.5568,610.602;Inherit;False;InstancedProperty;_SmoothnessIntensity;SmoothnessIntensity;8;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-484.932,315.6009;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;46.8,276.9;Float;False;True;-1;0;ASEMaterialInspector;0;0;StandardSpecular;Mobile/StandardGeoShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.98;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;1;0
WireConnection;16;1;2;0
WireConnection;16;2;11;0
WireConnection;12;0;13;0
WireConnection;6;0;16;0
WireConnection;6;1;7;0
WireConnection;5;0;6;0
WireConnection;5;1;12;0
WireConnection;9;0;8;0
WireConnection;9;1;5;0
WireConnection;15;0;14;0
WireConnection;15;1;3;0
WireConnection;0;0;9;0
WireConnection;0;1;2;0
WireConnection;0;3;15;0
WireConnection;0;4;4;0
ASEEND*/
//CHKSM=BFA68659FD799B72A420F27FED338B46A717829D