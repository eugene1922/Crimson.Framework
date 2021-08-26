// Upgrade NOTE: upgraded instancing buffer 'MobileStandardAlphaShader' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Mobile/StandardAlphaShader"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_MainColor("MainColor", Color) = (1,1,1,0)
		_MainTexture("MainTexture", 2D) = "white" {}
		_MainTextureIntensity("MainTextureIntensity", Float) = 0
		_Saturation("Saturation", Range( -1 , 10)) = 1
		_Specular("Specular", Color) = (0,0,0,0)
		_Opacity("Opacity", Float) = 1
		_SmoothnessIntensity("SmoothnessIntensity", Range( 0 , 1)) = 0
		_WindSpeed("WindSpeed", Vector) = (0,0,0,0)
		[Toggle]_WindActive("WindActive", Float) = 1
		_WindDistortionTile("WindDistortionTile", Float) = 0
		_Normals("Normals", 2D) = "white" {}
		_DistortionIntensity("DistortionIntensity", Float) = 0
		_Normal_Intensity("Normal_Intensity", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma multi_compile_instancing
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _WindActive;
		uniform sampler2D _MainTexture;
		uniform sampler2D _Normals;
		uniform float _Cutoff = 0.5;

		UNITY_INSTANCING_BUFFER_START(MobileStandardAlphaShader)
			UNITY_DEFINE_INSTANCED_PROP(float4, _MainColor)
#define _MainColor_arr MobileStandardAlphaShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _MainTexture_ST)
#define _MainTexture_ST_arr MobileStandardAlphaShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Normals_ST)
#define _Normals_ST_arr MobileStandardAlphaShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Specular)
#define _Specular_arr MobileStandardAlphaShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _WindSpeed)
#define _WindSpeed_arr MobileStandardAlphaShader
			UNITY_DEFINE_INSTANCED_PROP(float, _DistortionIntensity)
#define _DistortionIntensity_arr MobileStandardAlphaShader
			UNITY_DEFINE_INSTANCED_PROP(float, _WindDistortionTile)
#define _WindDistortionTile_arr MobileStandardAlphaShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Normal_Intensity)
#define _Normal_Intensity_arr MobileStandardAlphaShader
			UNITY_DEFINE_INSTANCED_PROP(float, _MainTextureIntensity)
#define _MainTextureIntensity_arr MobileStandardAlphaShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Saturation)
#define _Saturation_arr MobileStandardAlphaShader
			UNITY_DEFINE_INSTANCED_PROP(float, _SmoothnessIntensity)
#define _SmoothnessIntensity_arr MobileStandardAlphaShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Opacity)
#define _Opacity_arr MobileStandardAlphaShader
		UNITY_INSTANCING_BUFFER_END(MobileStandardAlphaShader)


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float _DistortionIntensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_DistortionIntensity_arr, _DistortionIntensity);
			float _WindDistortionTile_Instance = UNITY_ACCESS_INSTANCED_PROP(_WindDistortionTile_arr, _WindDistortionTile);
			float2 temp_cast_0 = (_WindDistortionTile_Instance).xx;
			float2 _WindSpeed_Instance = UNITY_ACCESS_INSTANCED_PROP(_WindSpeed_arr, _WindSpeed);
			float2 uv_TexCoord22 = v.texcoord.xy * temp_cast_0 + ( _WindSpeed_Instance * _Time.y );
			float simplePerlin2D23 = snoise( uv_TexCoord22*5.0 );
			simplePerlin2D23 = simplePerlin2D23*0.5 + 0.5;
			float temp_output_30_0 = ( _DistortionIntensity_Instance * simplePerlin2D23 );
			float4 appendResult26 = (float4(( ase_vertex3Pos.x * temp_output_30_0 ) , 0.0 , ( ase_vertex3Pos.z * temp_output_30_0 ) , 0.0));
			v.vertex.xyz += (( _WindActive )?( appendResult26 ):( float4( 0,0,0,0 ) )).xyz;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float4 _MainColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainColor_arr, _MainColor);
			float4 _MainTexture_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainTexture_ST_arr, _MainTexture_ST);
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST_Instance.xy + _MainTexture_ST_Instance.zw;
			float4 tex2DNode1 = tex2D( _MainTexture, uv_MainTexture );
			float4 _Normals_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Normals_ST_arr, _Normals_ST);
			float2 uv_Normals = i.uv_texcoord * _Normals_ST_Instance.xy + _Normals_ST_Instance.zw;
			float _Normal_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Normal_Intensity_arr, _Normal_Intensity);
			float4 lerpResult32 = lerp( tex2DNode1 , tex2D( _Normals, uv_Normals ) , _Normal_Intensity_Instance);
			float _MainTextureIntensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainTextureIntensity_arr, _MainTextureIntensity);
			float _Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Saturation_arr, _Saturation);
			float3 desaturateInitialColor6 = ( lerpResult32 * _MainTextureIntensity_Instance ).rgb;
			float desaturateDot6 = dot( desaturateInitialColor6, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar6 = lerp( desaturateInitialColor6, desaturateDot6.xxx, ( _Saturation_Instance * -1.0 ) );
			o.Albedo = ( _MainColor_Instance * float4( desaturateVar6 , 0.0 ) ).rgb;
			float4 _Specular_Instance = UNITY_ACCESS_INSTANCED_PROP(_Specular_arr, _Specular);
			o.Specular = _Specular_Instance.rgb;
			float _SmoothnessIntensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_SmoothnessIntensity_arr, _SmoothnessIntensity);
			o.Smoothness = _SmoothnessIntensity_Instance;
			o.Alpha = 1;
			float _Opacity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Opacity_arr, _Opacity);
			clip( ( tex2DNode1.a * _Opacity_Instance ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1536;9.6;1536;800;2798.97;1059.783;3.015459;True;True
Node;AmplifyShaderEditor.Vector2Node;16;-1313.295,886.8107;Inherit;False;InstancedProperty;_WindSpeed;WindSpeed;8;0;Create;True;0;0;False;0;0,0;0.01,-0.01;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TimeNode;18;-1304.285,1139.766;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1007.285,1022.766;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1177.892,1304.986;Inherit;False;InstancedProperty;_WindDistortionTile;WindDistortionTile;10;0;Create;True;0;0;False;0;0;1.58;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-914.1193,1198.221;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;11;-1923.321,-722.1114;Inherit;False;1141.531;1133.775;Comment;11;8;7;5;4;10;6;9;1;31;32;33;Main Texture;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;1;-1841.033,-625.1566;Inherit;True;Property;_MainTexture;MainTexture;2;0;Create;True;0;0;False;0;-1;None;4c30bacb224920b4c86ad4ba2889a6a1;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;31;-1846.567,-281.8013;Inherit;True;Property;_Normals;Normals;11;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;-1821.47,-4.736549;Inherit;False;InstancedProperty;_Normal_Intensity;Normal_Intensity;13;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;23;-644.7794,1215.987;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-745.9435,1101.224;Inherit;False;InstancedProperty;_DistortionIntensity;DistortionIntensity;12;0;Create;True;0;0;False;0;0;0.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-410.3662,1183.32;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;32;-1434.584,-532.2823;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1421.261,-344.5356;Inherit;False;InstancedProperty;_MainTextureIntensity;MainTextureIntensity;3;0;Create;True;0;0;False;0;0;0.68;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-1487.608,-147.4651;Inherit;False;InstancedProperty;_Saturation;Saturation;4;0;Create;True;0;0;False;0;1;1;-1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PosVertexDataNode;19;-902.2084,819.7822;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-439.7201,899.0634;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-448.4012,1006.494;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-1211.749,-273.0961;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-1268.151,-462.7258;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;6;-1189.701,-403.7888;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;9;-1268.086,-672.1114;Inherit;False;InstancedProperty;_MainColor;MainColor;1;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;26;-281.2863,965.2581;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-678.3756,757.1225;Inherit;False;InstancedProperty;_Opacity;Opacity;6;0;Create;True;0;0;False;0;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-415.7231,730.6283;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;14;-1107.788,439.4001;Inherit;False;InstancedProperty;_Specular;Specular;5;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;21;-93.78628,856.5511;Inherit;False;Property;_WindActive;WindActive;9;0;Create;True;0;0;False;0;1;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;10;-944.189,-460.6709;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-815.0076,593.1205;Inherit;False;InstancedProperty;_SmoothnessIntensity;SmoothnessIntensity;7;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;263.9297,430.1917;Float;False;True;-1;0;ASEMaterialInspector;0;0;StandardSpecular;Mobile/StandardAlphaShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Opaque;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;17;0;16;0
WireConnection;17;1;18;2
WireConnection;22;0;24;0
WireConnection;22;1;17;0
WireConnection;23;0;22;0
WireConnection;30;0;29;0
WireConnection;30;1;23;0
WireConnection;32;0;1;0
WireConnection;32;1;31;0
WireConnection;32;2;33;0
WireConnection;25;0;19;1
WireConnection;25;1;30;0
WireConnection;28;0;19;3
WireConnection;28;1;30;0
WireConnection;8;0;7;0
WireConnection;4;0;32;0
WireConnection;4;1;5;0
WireConnection;6;0;4;0
WireConnection;6;1;8;0
WireConnection;26;0;25;0
WireConnection;26;2;28;0
WireConnection;2;0;1;4
WireConnection;2;1;3;0
WireConnection;21;1;26;0
WireConnection;10;0;9;0
WireConnection;10;1;6;0
WireConnection;0;0;10;0
WireConnection;0;3;14;0
WireConnection;0;4;15;0
WireConnection;0;10;2;0
WireConnection;0;11;21;0
ASEEND*/
//CHKSM=6BE7B50EE8501A2E51875190F8125E6E81A6CA66