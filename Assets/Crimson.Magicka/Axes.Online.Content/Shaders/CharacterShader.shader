// Upgrade NOTE: upgraded instancing buffer 'TAshadersCharacterShader' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TAshaders/CharacterShader"
{
	Properties
	{
		_MainColor2("MainColor", Color) = (1,1,1,0)
		_MainTexture2("MainTexture", 2D) = "white" {}
		_TextureIntensity2("TextureIntensity", Range( 0 , 20)) = 1
		_Desaturation2("Desaturation", Range( 0 , 1)) = 0
		_HUE_Intensity2("HUE_Intensity", Range( 0 , 1)) = 0
		_HUE2("HUE", Range( 0 , 1)) = 0
		_NormalTexture1("NormalTexture", 2D) = "white" {}
		_NormalIntensity1("NormalIntensity", Range( 0 , 5)) = 0
		_EmissionTexture1("EmissionTexture", 2D) = "white" {}
		_EmissionIntensity1("EmissionIntensity", Float) = 0
		_SpecGlosTexture("SpecGlosTexture", 2D) = "white" {}
		_SmoothnessIntensity("SmoothnessIntensity", Range( 0 , 4)) = 0
		_SpecularIntensity("SpecularIntensity", Float) = 0
		_SpecularColor("SpecularColor", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _NormalTexture1;
		uniform sampler2D _MainTexture2;
		uniform sampler2D _EmissionTexture1;
		uniform sampler2D _SpecGlosTexture;

		UNITY_INSTANCING_BUFFER_START(TAshadersCharacterShader)
			UNITY_DEFINE_INSTANCED_PROP(float4, _NormalTexture1_ST)
#define _NormalTexture1_ST_arr TAshadersCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _MainColor2)
#define _MainColor2_arr TAshadersCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _MainTexture2_ST)
#define _MainTexture2_ST_arr TAshadersCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _EmissionTexture1_ST)
#define _EmissionTexture1_ST_arr TAshadersCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _SpecularColor)
#define _SpecularColor_arr TAshadersCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _SpecGlosTexture_ST)
#define _SpecGlosTexture_ST_arr TAshadersCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float, _NormalIntensity1)
#define _NormalIntensity1_arr TAshadersCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float, _TextureIntensity2)
#define _TextureIntensity2_arr TAshadersCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Desaturation2)
#define _Desaturation2_arr TAshadersCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float, _HUE2)
#define _HUE2_arr TAshadersCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float, _HUE_Intensity2)
#define _HUE_Intensity2_arr TAshadersCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float, _EmissionIntensity1)
#define _EmissionIntensity1_arr TAshadersCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float, _SpecularIntensity)
#define _SpecularIntensity_arr TAshadersCharacterShader
			UNITY_DEFINE_INSTANCED_PROP(float, _SmoothnessIntensity)
#define _SmoothnessIntensity_arr TAshadersCharacterShader
		UNITY_INSTANCING_BUFFER_END(TAshadersCharacterShader)


		float3 HSVToRGB( float3 c )
		{
			float4 K = float4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
			float3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
			return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
		}


		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float _NormalIntensity1_Instance = UNITY_ACCESS_INSTANCED_PROP(_NormalIntensity1_arr, _NormalIntensity1);
			float4 _NormalTexture1_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_NormalTexture1_ST_arr, _NormalTexture1_ST);
			float2 uv_NormalTexture1 = i.uv_texcoord * _NormalTexture1_ST_Instance.xy + _NormalTexture1_ST_Instance.zw;
			o.Normal = UnpackScaleNormal( tex2D( _NormalTexture1, uv_NormalTexture1 ), _NormalIntensity1_Instance );
			float4 _MainColor2_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainColor2_arr, _MainColor2);
			float4 _MainTexture2_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainTexture2_ST_arr, _MainTexture2_ST);
			float2 uv_MainTexture2 = i.uv_texcoord * _MainTexture2_ST_Instance.xy + _MainTexture2_ST_Instance.zw;
			float _TextureIntensity2_Instance = UNITY_ACCESS_INSTANCED_PROP(_TextureIntensity2_arr, _TextureIntensity2);
			float _Desaturation2_Instance = UNITY_ACCESS_INSTANCED_PROP(_Desaturation2_arr, _Desaturation2);
			float3 desaturateInitialColor19 = ( tex2D( _MainTexture2, uv_MainTexture2 ) * _TextureIntensity2_Instance ).rgb;
			float desaturateDot19 = dot( desaturateInitialColor19, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar19 = lerp( desaturateInitialColor19, desaturateDot19.xxx, _Desaturation2_Instance );
			float _HUE2_Instance = UNITY_ACCESS_INSTANCED_PROP(_HUE2_arr, _HUE2);
			float _HUE_Intensity2_Instance = UNITY_ACCESS_INSTANCED_PROP(_HUE_Intensity2_arr, _HUE_Intensity2);
			float3 hsvTorgb35 = HSVToRGB( float3(_HUE2_Instance,1.0,_HUE_Intensity2_Instance) );
			o.Albedo = ( ( _MainColor2_Instance * float4( desaturateVar19 , 0.0 ) ) + float4( hsvTorgb35 , 0.0 ) ).rgb;
			float4 _EmissionTexture1_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_EmissionTexture1_ST_arr, _EmissionTexture1_ST);
			float2 uv_EmissionTexture1 = i.uv_texcoord * _EmissionTexture1_ST_Instance.xy + _EmissionTexture1_ST_Instance.zw;
			float _EmissionIntensity1_Instance = UNITY_ACCESS_INSTANCED_PROP(_EmissionIntensity1_arr, _EmissionIntensity1);
			o.Emission = ( tex2D( _EmissionTexture1, uv_EmissionTexture1 ) * _EmissionIntensity1_Instance ).rgb;
			float4 _SpecularColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_SpecularColor_arr, _SpecularColor);
			float _SpecularIntensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_SpecularIntensity_arr, _SpecularIntensity);
			float4 _SpecGlosTexture_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_SpecGlosTexture_ST_arr, _SpecGlosTexture_ST);
			float2 uv_SpecGlosTexture = i.uv_texcoord * _SpecGlosTexture_ST_Instance.xy + _SpecGlosTexture_ST_Instance.zw;
			float _SmoothnessIntensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_SmoothnessIntensity_arr, _SmoothnessIntensity);
			float4 temp_output_36_0 = ( tex2D( _SpecGlosTexture, uv_SpecGlosTexture ) * _SmoothnessIntensity_Instance );
			o.Specular = ( ( _SpecularColor_Instance * _SpecularIntensity_Instance ) * temp_output_36_0 ).rgb;
			o.Smoothness = temp_output_36_0.r;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17800
2048;58.4;2048;1027;1258.45;399.4048;1.000147;True;True
Node;AmplifyShaderEditor.CommentaryNode;4;-2394.53,-1642.982;Inherit;False;1342.887;828.6587;Comment;7;32;29;19;15;14;10;9;MainTexture;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;9;-2151.7,-1569.997;Inherit;True;Property;_MainTexture2;MainTexture;1;0;Create;True;0;0;False;0;-1;None;1331cc89db7ce4b539724f747a103a84;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;10;-2126.09,-995.2208;Inherit;False;InstancedProperty;_TextureIntensity2;TextureIntensity;2;0;Create;True;0;0;False;0;1;1;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;13;-2529.176,362.2452;Inherit;False;780.4168;459.9178;Comment;4;35;26;23;22;HUE settings;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;15;-1662.896,-1339.893;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-2102.701,-897.1844;Inherit;False;InstancedProperty;_Desaturation2;Desaturation;3;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-2422.177,565.1;Inherit;False;Constant;_Saturation2;Saturation;2;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;29;-1626.317,-1592.983;Inherit;False;InstancedProperty;_MainColor2;MainColor;0;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;22;-2444.892,706.762;Inherit;False;InstancedProperty;_HUE_Intensity2;HUE_Intensity;4;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;45;-995.6863,37.97536;Inherit;False;InstancedProperty;_SpecularColor;SpecularColor;13;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;20;-2361.242,-363.8145;Inherit;False;643.4628;455.0953;Comment;3;39;34;31;Emission;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-2479.175,412.2441;Inherit;False;InstancedProperty;_HUE2;HUE;5;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;19;-1458.57,-1277.999;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-1200.904,672.8022;Inherit;False;InstancedProperty;_SmoothnessIntensity;SmoothnessIntensity;11;0;Create;True;0;0;False;0;0;1.23;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-817.5659,278.5024;Inherit;False;InstancedProperty;_SpecularIntensity;SpecularIntensity;12;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;24;-1234.565,387.452;Inherit;True;Property;_SpecGlosTexture;SpecGlosTexture;10;0;Create;True;0;0;False;0;-1;None;16d574e53541bba44a84052fa38778df;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;21;-2570.959,-745.8757;Inherit;False;864.5925;303.3217;Comment;2;42;33;Normals;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-1214.042,-1369.748;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;31;-2307.183,-313.8146;Inherit;True;Property;_EmissionTexture1;EmissionTexture;8;0;Create;True;0;0;False;0;-1;None;9a4a55d8d2e54394d97426434477cdcf;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.HSVToRGBNode;35;-1987.549,501.0582;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;34;-2311.24,-24.11925;Inherit;False;InstancedProperty;_EmissionIntensity1;EmissionIntensity;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;36;-723.6532,462.7017;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;46;-539.3322,82.18026;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;33;-2520.959,-557.9539;Inherit;False;InstancedProperty;_NormalIntensity1;NormalIntensity;7;0;Create;True;0;0;False;0;0;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-346.8102,145.5869;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-1880.18,-178.4263;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;-354.4736,-249.6564;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;42;-2027.165,-695.8757;Inherit;True;Property;_NormalTexture1;NormalTexture;6;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;StandardSpecular;TAshaders/CharacterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;15;0;9;0
WireConnection;15;1;10;0
WireConnection;19;0;15;0
WireConnection;19;1;14;0
WireConnection;32;0;29;0
WireConnection;32;1;19;0
WireConnection;35;0;23;0
WireConnection;35;1;26;0
WireConnection;35;2;22;0
WireConnection;36;0;24;0
WireConnection;36;1;25;0
WireConnection;46;0;45;0
WireConnection;46;1;44;0
WireConnection;47;0;46;0
WireConnection;47;1;36;0
WireConnection;39;0;31;0
WireConnection;39;1;34;0
WireConnection;38;0;32;0
WireConnection;38;1;35;0
WireConnection;42;5;33;0
WireConnection;0;0;38;0
WireConnection;0;1;42;0
WireConnection;0;2;39;0
WireConnection;0;3;47;0
WireConnection;0;4;36;0
ASEEND*/
//CHKSM=887741B39934CCA8E9E8283C0EB64FA4FE006F0A