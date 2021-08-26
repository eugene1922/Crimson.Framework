// Upgrade NOTE: upgraded instancing buffer 'GroundShader4' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TAshaders/GroundShaderEditor"
{
	Properties
	{
		_Mask1("Mask1", 2D) = "white" {}
		_Texture1R_Color("Texture1(R)_Color", Color) = (1,1,1,0)
		_Texture1R("Texture1(R)", 2D) = "white" {}
		_Texture1R_Tile("Texture1(R)_Tile", Vector) = (1,1,0,0)
		_Texture1R_Intensity("Texture1(R)_Intensity", Range( 0 , 5)) = 1
		_Texture1R_Saturation("Texture1(R)_Saturation", Float) = 0
		_Texture2G_Color("Texture2(G)_Color", Color) = (1,1,1,0)
		_Texture2G("Texture2(G)", 2D) = "white" {}
		_Texture2G_Tile("Texture2(G)_Tile", Vector) = (1,1,0,0)
		_Texture2G_Intensity("Texture2(G)_Intensity", Range( 0 , 5)) = 1
		_Texture2G_Saturation("Texture2(G)_Saturation", Float) = 0
		_Texture3B_Color("Texture3(B)_Color", Color) = (1,1,1,0)
		_Texture3B("Texture3(B)", 2D) = "white" {}
		_Texture3B_Tile("Texture3(B)_Tile", Vector) = (0,0,0,0)
		_Texture3B_Intensity("Texture3(B)_Intensity", Float) = 1
		_Texture3B_Saturation("Texture3(B)_Saturation", Float) = 0
		_Texture4A_Color("Texture4(A)_Color", Color) = (1,1,1,0)
		_Texture4A("Texture4(A)", 2D) = "white" {}
		_Texture4A_Tile("Texture4(A)_Tile", Vector) = (1,1,0,0)
		_Texture4A_intensity("Texture4(A)_intensity", Range( 0 , 5)) = 1
		_Texture4A_Saturation("Texture4(A)_Saturation", Range( -1 , 3)) = 0
		[Toggle]_Texture_Pack_2("Texture_Pack_2", Float) = 0
		_Mask2("Mask2", 2D) = "white" {}
		_Texture5R_Color("Texture5(R)_Color", Color) = (1,1,1,0)
		_Texture5R("Texture5(R)", 2D) = "white" {}
		_Texture5R_Tile("Texture5(R)_Tile", Vector) = (1,1,0,0)
		_Texture5R_Intensity("Texture5(R)_Intensity", Float) = 1
		_Texture5R_Saturation("Texture5(R)_Saturation", Float) = 0
		_Texture6G_Color("Texture6(G)_Color", Color) = (1,1,1,0)
		_Texture6G("Texture6(G)", 2D) = "white" {}
		_Texture6G_Tile("Texture6(G)_Tile", Vector) = (1,1,0,0)
		_Texture6G_Intensity("Texture6(G)_Intensity", Float) = 1
		_Texture6G_Saturation("Texture6(G)_Saturation", Float) = 0
		_Texture7B("Texture7(B)", 2D) = "white" {}
		_Texture7B_Tile("Texture7(B)_Tile", Vector) = (1,1,0,0)
		_Texture7B_Saturation("Texture7(B)_Saturation", Float) = 0
		_Texture7B_Intensity("Texture7(B)_Intensity", Float) = 1
		_Texture7B_Color("Texture7(B)_Color", Color) = (1,1,1,0)
		_Mask3("Mask3", 2D) = "white" {}
		_Texture8R_Color("Texture8(R)_Color", Color) = (1,1,1,0)
		_Texture8R("Texture8(R)", 2D) = "white" {}
		[Toggle]_Texture_Pack_3("Texture_Pack_3", Float) = 0
		_Texture8R_Tile("Texture8(R)_Tile", Vector) = (1,1,0,0)
		_Texture8R_Saturation("Texture8(R)_Saturation", Float) = 0
		_Texture8R_Intensity("Texture8(R)_Intensity", Float) = 1
		_Texture9G_Color("Texture9(G)_Color", Color) = (1,1,1,0)
		_Texture9G("Texture9(G)", 2D) = "white" {}
		_Texture9G_Tile("Texture9(G)_Tile", Vector) = (1,1,0,0)
		_Texture9G_Saturation("Texture9(G)_Saturation", Float) = 0
		_Texture9G_Intensity("Texture9(G)_Intensity", Float) = 1
		_Texture10B_Color("Texture10(B)_Color", Color) = (1,1,1,0)
		_Texture10B("Texture10(B)", 2D) = "white" {}
		_Texture10B_Tile("Texture10(B)_Tile", Vector) = (1,1,0,0)
		_Texture10B_Saturation("Texture10(B)_Saturation", Float) = 0
		_texture10B_Intensity("texture10(B)_Intensity", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 2.0
		#pragma multi_compile_instancing
		#pragma surface surf Lambert keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Texture_Pack_3;
		uniform float _Texture_Pack_2;
		uniform sampler2D _Texture4A;
		uniform sampler2D _Texture1R;
		uniform sampler2D _Mask1;
		uniform sampler2D _Texture2G;
		uniform sampler2D _Texture3B;
		uniform sampler2D _Texture5R;
		uniform sampler2D _Mask2;
		uniform sampler2D _Texture6G;
		uniform sampler2D _Texture7B;
		uniform sampler2D _Texture8R;
		uniform sampler2D _Mask3;
		uniform sampler2D _Texture9G;
		uniform sampler2D _Texture10B;

		UNITY_INSTANCING_BUFFER_START(GroundShader4)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture4A_Color)
#define _Texture4A_Color_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture9G_Color)
#define _Texture9G_Color_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float4, _Mask3_ST)
#define _Mask3_ST_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture8R_Color)
#define _Texture8R_Color_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture7B_Color)
#define _Texture7B_Color_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture6G_Color)
#define _Texture6G_Color_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture5R_Color)
#define _Texture5R_Color_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture3B_Color)
#define _Texture3B_Color_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture2G_Color)
#define _Texture2G_Color_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float4, _Mask2_ST)
#define _Mask2_ST_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture10B_Color)
#define _Texture10B_Color_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture1R_Color)
#define _Texture1R_Color_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float4, _Mask1_ST)
#define _Mask1_ST_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float2, _Texture10B_Tile)
#define _Texture10B_Tile_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float2, _Texture4A_Tile)
#define _Texture4A_Tile_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float2, _Texture9G_Tile)
#define _Texture9G_Tile_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float2, _Texture8R_Tile)
#define _Texture8R_Tile_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float2, _Texture1R_Tile)
#define _Texture1R_Tile_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float2, _Texture6G_Tile)
#define _Texture6G_Tile_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float2, _Texture7B_Tile)
#define _Texture7B_Tile_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float2, _Texture2G_Tile)
#define _Texture2G_Tile_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float2, _Texture5R_Tile)
#define _Texture5R_Tile_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float2, _Texture3B_Tile)
#define _Texture3B_Tile_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture10B_Saturation)
#define _Texture10B_Saturation_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture2G_Intensity)
#define _Texture2G_Intensity_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture2G_Saturation)
#define _Texture2G_Saturation_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture9G_Intensity)
#define _Texture9G_Intensity_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture9G_Saturation)
#define _Texture9G_Saturation_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture1R_Saturation)
#define _Texture1R_Saturation_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture4A_intensity)
#define _Texture4A_intensity_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture4A_Saturation)
#define _Texture4A_Saturation_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture8R_Intensity)
#define _Texture8R_Intensity_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture8R_Saturation)
#define _Texture8R_Saturation_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture3B_Intensity)
#define _Texture3B_Intensity_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture7B_Intensity)
#define _Texture7B_Intensity_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture7B_Saturation)
#define _Texture7B_Saturation_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture3B_Saturation)
#define _Texture3B_Saturation_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture1R_Intensity)
#define _Texture1R_Intensity_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture6G_Intensity)
#define _Texture6G_Intensity_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture6G_Saturation)
#define _Texture6G_Saturation_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _texture10B_Intensity)
#define _texture10B_Intensity_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture5R_Intensity)
#define _Texture5R_Intensity_arr GroundShader4
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture5R_Saturation)
#define _Texture5R_Saturation_arr GroundShader4
		UNITY_INSTANCING_BUFFER_END(GroundShader4)

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 _Texture4A_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture4A_Color_arr, _Texture4A_Color);
			float2 _Texture4A_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture4A_Tile_arr, _Texture4A_Tile);
			float2 uv_TexCoord82 = i.uv_texcoord * _Texture4A_Tile_Instance;
			float _Texture4A_intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture4A_intensity_arr, _Texture4A_intensity);
			float _Texture4A_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture4A_Saturation_arr, _Texture4A_Saturation);
			float3 desaturateInitialColor28 = ( tex2D( _Texture4A, uv_TexCoord82 ) * _Texture4A_intensity_Instance ).rgb;
			float desaturateDot28 = dot( desaturateInitialColor28, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar28 = lerp( desaturateInitialColor28, desaturateDot28.xxx, ( _Texture4A_Saturation_Instance * -1.0 ) );
			float4 _Texture1R_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture1R_Color_arr, _Texture1R_Color);
			float2 _Texture1R_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture1R_Tile_arr, _Texture1R_Tile);
			float2 uv_TexCoord75 = i.uv_texcoord * _Texture1R_Tile_Instance;
			float _Texture1R_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture1R_Intensity_arr, _Texture1R_Intensity);
			float _Texture1R_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture1R_Saturation_arr, _Texture1R_Saturation);
			float3 desaturateInitialColor18 = ( tex2D( _Texture1R, uv_TexCoord75 ) * _Texture1R_Intensity_Instance ).rgb;
			float desaturateDot18 = dot( desaturateInitialColor18, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar18 = lerp( desaturateInitialColor18, desaturateDot18.xxx, ( _Texture1R_Saturation_Instance * -1.0 ) );
			float4 _Mask1_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Mask1_ST_arr, _Mask1_ST);
			float2 uv_Mask1 = i.uv_texcoord * _Mask1_ST_Instance.xy + _Mask1_ST_Instance.zw;
			float4 tex2DNode1 = tex2D( _Mask1, uv_Mask1 );
			float4 lerpResult6 = lerp( ( _Texture4A_Color_Instance * float4( desaturateVar28 , 0.0 ) ) , ( _Texture1R_Color_Instance * float4( desaturateVar18 , 0.0 ) ) , tex2DNode1.r);
			float4 _Texture2G_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture2G_Color_arr, _Texture2G_Color);
			float2 _Texture2G_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture2G_Tile_arr, _Texture2G_Tile);
			float2 uv_TexCoord79 = i.uv_texcoord * _Texture2G_Tile_Instance;
			float _Texture2G_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture2G_Intensity_arr, _Texture2G_Intensity);
			float _Texture2G_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture2G_Saturation_arr, _Texture2G_Saturation);
			float3 desaturateInitialColor39 = ( tex2D( _Texture2G, uv_TexCoord79 ) * _Texture2G_Intensity_Instance ).rgb;
			float desaturateDot39 = dot( desaturateInitialColor39, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar39 = lerp( desaturateInitialColor39, desaturateDot39.xxx, ( _Texture2G_Saturation_Instance * -1.0 ) );
			float4 lerpResult7 = lerp( lerpResult6 , ( _Texture2G_Color_Instance * float4( desaturateVar39 , 0.0 ) ) , tex2DNode1.g);
			float4 _Texture3B_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture3B_Color_arr, _Texture3B_Color);
			float2 _Texture3B_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture3B_Tile_arr, _Texture3B_Tile);
			float2 uv_TexCoord81 = i.uv_texcoord * _Texture3B_Tile_Instance;
			float _Texture3B_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture3B_Intensity_arr, _Texture3B_Intensity);
			float _Texture3B_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture3B_Saturation_arr, _Texture3B_Saturation);
			float3 desaturateInitialColor47 = ( tex2D( _Texture3B, uv_TexCoord81 ) * _Texture3B_Intensity_Instance ).rgb;
			float desaturateDot47 = dot( desaturateInitialColor47, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar47 = lerp( desaturateInitialColor47, desaturateDot47.xxx, ( _Texture3B_Saturation_Instance * -1.0 ) );
			float4 lerpResult8 = lerp( lerpResult7 , ( _Texture3B_Color_Instance * float4( desaturateVar47 , 0.0 ) ) , tex2DNode1.b);
			float4 _Texture5R_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture5R_Color_arr, _Texture5R_Color);
			float2 _Texture5R_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture5R_Tile_arr, _Texture5R_Tile);
			float2 uv_TexCoord86 = i.uv_texcoord * _Texture5R_Tile_Instance;
			float _Texture5R_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture5R_Intensity_arr, _Texture5R_Intensity);
			float _Texture5R_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture5R_Saturation_arr, _Texture5R_Saturation);
			float3 desaturateInitialColor55 = ( tex2D( _Texture5R, uv_TexCoord86 ) * _Texture5R_Intensity_Instance ).rgb;
			float desaturateDot55 = dot( desaturateInitialColor55, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar55 = lerp( desaturateInitialColor55, desaturateDot55.xxx, ( _Texture5R_Saturation_Instance * -1.0 ) );
			float4 _Mask2_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Mask2_ST_arr, _Mask2_ST);
			float2 uv_Mask2 = i.uv_texcoord * _Mask2_ST_Instance.xy + _Mask2_ST_Instance.zw;
			float4 tex2DNode9 = tex2D( _Mask2, uv_Mask2 );
			float4 lerpResult14 = lerp( lerpResult8 , ( _Texture5R_Color_Instance * float4( desaturateVar55 , 0.0 ) ) , tex2DNode9.r);
			float2 _Texture6G_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture6G_Tile_arr, _Texture6G_Tile);
			float2 uv_TexCoord87 = i.uv_texcoord * _Texture6G_Tile_Instance;
			float _Texture6G_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture6G_Saturation_arr, _Texture6G_Saturation);
			float3 desaturateInitialColor63 = tex2D( _Texture6G, uv_TexCoord87 ).rgb;
			float desaturateDot63 = dot( desaturateInitialColor63, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar63 = lerp( desaturateInitialColor63, desaturateDot63.xxx, ( _Texture6G_Saturation_Instance * -1.0 ) );
			float _Texture6G_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture6G_Intensity_arr, _Texture6G_Intensity);
			float4 _Texture6G_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture6G_Color_arr, _Texture6G_Color);
			float4 lerpResult15 = lerp( lerpResult14 , ( float4( desaturateVar63 , 0.0 ) * _Texture6G_Intensity_Instance * _Texture6G_Color_Instance ) , tex2DNode9.g);
			float2 _Texture7B_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture7B_Tile_arr, _Texture7B_Tile);
			float2 uv_TexCoord90 = i.uv_texcoord * _Texture7B_Tile_Instance;
			float _Texture7B_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture7B_Saturation_arr, _Texture7B_Saturation);
			float3 desaturateInitialColor70 = tex2D( _Texture7B, uv_TexCoord90 ).rgb;
			float desaturateDot70 = dot( desaturateInitialColor70, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar70 = lerp( desaturateInitialColor70, desaturateDot70.xxx, ( _Texture7B_Saturation_Instance * -1.0 ) );
			float _Texture7B_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture7B_Intensity_arr, _Texture7B_Intensity);
			float4 _Texture7B_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture7B_Color_arr, _Texture7B_Color);
			float4 lerpResult16 = lerp( lerpResult15 , ( float4( desaturateVar70 , 0.0 ) * _Texture7B_Intensity_Instance * _Texture7B_Color_Instance ) , tex2DNode9.b);
			float2 _Texture8R_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture8R_Tile_arr, _Texture8R_Tile);
			float2 uv_TexCoord107 = i.uv_texcoord * _Texture8R_Tile_Instance;
			float _Texture8R_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture8R_Saturation_arr, _Texture8R_Saturation);
			float3 desaturateInitialColor98 = tex2D( _Texture8R, uv_TexCoord107 ).rgb;
			float desaturateDot98 = dot( desaturateInitialColor98, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar98 = lerp( desaturateInitialColor98, desaturateDot98.xxx, ( _Texture8R_Saturation_Instance * -1.0 ) );
			float _Texture8R_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture8R_Intensity_arr, _Texture8R_Intensity);
			float4 _Texture8R_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture8R_Color_arr, _Texture8R_Color);
			float4 _Mask3_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Mask3_ST_arr, _Mask3_ST);
			float2 uv_Mask3 = i.uv_texcoord * _Mask3_ST_Instance.xy + _Mask3_ST_Instance.zw;
			float4 tex2DNode93 = tex2D( _Mask3, uv_Mask3 );
			float4 lerpResult104 = lerp( (( _Texture_Pack_2 )?( lerpResult16 ):( lerpResult8 )) , ( float4( desaturateVar98 , 0.0 ) * _Texture8R_Intensity_Instance * _Texture8R_Color_Instance ) , tex2DNode93.r);
			float2 _Texture9G_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture9G_Tile_arr, _Texture9G_Tile);
			float2 uv_TexCoord109 = i.uv_texcoord * _Texture9G_Tile_Instance;
			float _Texture9G_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture9G_Saturation_arr, _Texture9G_Saturation);
			float3 desaturateInitialColor111 = tex2D( _Texture9G, uv_TexCoord109 ).rgb;
			float desaturateDot111 = dot( desaturateInitialColor111, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar111 = lerp( desaturateInitialColor111, desaturateDot111.xxx, ( _Texture9G_Saturation_Instance * -1.0 ) );
			float _Texture9G_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture9G_Intensity_arr, _Texture9G_Intensity);
			float4 _Texture9G_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture9G_Color_arr, _Texture9G_Color);
			float4 lerpResult118 = lerp( lerpResult104 , ( float4( desaturateVar111 , 0.0 ) * _Texture9G_Intensity_Instance * _Texture9G_Color_Instance ) , tex2DNode93.g);
			float2 _Texture10B_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture10B_Tile_arr, _Texture10B_Tile);
			float2 uv_TexCoord122 = i.uv_texcoord * _Texture10B_Tile_Instance;
			float _Texture10B_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture10B_Saturation_arr, _Texture10B_Saturation);
			float3 desaturateInitialColor123 = tex2D( _Texture10B, uv_TexCoord122 ).rgb;
			float desaturateDot123 = dot( desaturateInitialColor123, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar123 = lerp( desaturateInitialColor123, desaturateDot123.xxx, ( _Texture10B_Saturation_Instance * -1.0 ) );
			float _texture10B_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_texture10B_Intensity_arr, _texture10B_Intensity);
			float4 _Texture10B_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture10B_Color_arr, _Texture10B_Color);
			float4 lerpResult130 = lerp( lerpResult118 , ( float4( desaturateVar123 , 0.0 ) * _texture10B_Intensity_Instance * _Texture10B_Color_Instance ) , tex2DNode93.b);
			o.Albedo = (( _Texture_Pack_3 )?( lerpResult130 ):( (( _Texture_Pack_2 )?( lerpResult16 ):( lerpResult8 )) )).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1536;47.2;1536;718;1510.204;627.548;2.341641;True;True
Node;AmplifyShaderEditor.Vector2Node;76;-5458.634,586.6207;Inherit;False;InstancedProperty;_Texture1R_Tile;Texture1(R)_Tile;3;0;Create;False;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;84;-5471.1,-1718.713;Inherit;False;InstancedProperty;_Texture4A_Tile;Texture4(A)_Tile;18;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;33;-4877.819,-1880.092;Inherit;False;960.1301;552.7144;Comment;8;5;27;26;29;30;32;31;28;Texture 4;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;25;-4900.651,448.5537;Inherit;False;1549.507;793.6517;Comment;8;24;23;18;21;19;2;20;22;Texture 1;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;75;-5034.834,738.7208;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;82;-5140.874,-1715.882;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;78;-5549.997,-192.3786;Inherit;False;InstancedProperty;_Texture2G_Tile;Texture2(G)_Tile;8;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;27;-4827.819,-1580.354;Inherit;False;InstancedProperty;_Texture4A_intensity;Texture4(A)_intensity;19;0;Create;True;0;0;False;0;1;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-4637.001,560.7623;Inherit;True;Property;_Texture1R;Texture1(R);2;0;Create;False;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;80;-5434.408,-1153.531;Inherit;False;InstancedProperty;_Texture3B_Tile;Texture3(B)_Tile;13;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;44;-4897.703,-452.0338;Inherit;False;1108.112;684.4;Comment;8;38;41;42;3;37;40;39;43;Texture 2;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-4774.542,-1442.778;Inherit;False;InstancedProperty;_Texture4A_Saturation;Texture4(A)_Saturation;20;0;Create;True;0;0;False;0;0;0;-1;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;5;-4766.194,-1782.585;Inherit;True;Property;_Texture4A;Texture4(A);17;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;22;-4574.188,1061.223;Inherit;False;InstancedProperty;_Texture1R_Saturation;Texture1(R)_Saturation;5;0;Create;False;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-4622.031,892.6465;Inherit;False;InstancedProperty;_Texture1R_Intensity;Texture1(R)_Intensity;4;0;Create;False;0;0;False;0;1;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;79;-5166.142,-191.3095;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;81;-5198.816,-1084.117;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-4079.289,1015.648;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;85;-3210.451,-151.8279;Inherit;False;InstancedProperty;_Texture5R_Tile;Texture5(R)_Tile;25;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-4447.807,-1516.093;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-4056.831,781.1048;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;52;-4896.59,-1209.854;Inherit;False;972.6313;613.3002;Comment;8;4;46;45;49;48;47;50;51;Texture 3;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-4818.028,-24.03381;Inherit;False;InstancedProperty;_Texture2G_Intensity;Texture2(G)_Intensity;9;0;Create;True;0;0;False;0;1;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-4430.54,-1645.288;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-4847.703,-336.5194;Inherit;True;Property;_Texture2G;Texture2(G);7;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;41;-4775.028,116.9662;Inherit;False;InstancedProperty;_Texture2G_Saturation;Texture2(G)_Saturation;10;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;28;-4280.886,-1587.124;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DesaturateOpNode;18;-3815.459,892.0512;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-4476.028,20.96619;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-4836.745,-825.1577;Inherit;False;InstancedProperty;_Texture3B_Intensity;Texture3(B)_Intensity;14;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-4846.59,-1100.38;Inherit;True;Property;_Texture3B;Texture3(B);12;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;32;-4312.807,-1830.092;Inherit;False;InstancedProperty;_Texture4A_Color;Texture4(A)_Color;16;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;-4479.373,-120.7567;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;60;-2656.833,-374.4086;Inherit;False;968.1357;690.4268;Comment;8;10;54;53;56;57;55;59;58;Texture 5;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;23;-3800.914,595.7539;Inherit;False;InstancedProperty;_Texture1R_Color;Texture1(R)_Color;1;0;Create;False;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;86;-2958.434,-113.177;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;48;-4759.182,-711.9539;Inherit;False;InstancedProperty;_Texture3B_Saturation;Texture3(B)_Saturation;15;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;67;-2641.095,-1256.554;Inherit;False;735.1134;643.4;Comment;6;11;62;65;64;63;66;Texture 6;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-2576.648,200.6185;Inherit;False;InstancedProperty;_Texture5R_Saturation;Texture5(R)_Saturation;27;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;42;-4298.028,-402.0338;Inherit;False;InstancedProperty;_Texture2G_Color;Texture2(G)_Color;6;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;88;-3279.319,-1047.033;Inherit;False;InstancedProperty;_Texture6G_Tile;Texture6(G)_Tile;30;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-4489.367,-990.0679;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-2606.833,69.09225;Inherit;False;InstancedProperty;_Texture5R_Intensity;Texture5(R)_Intensity;26;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-3510.846,813.265;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;39;-4290.201,-78.09411;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-4080.088,-1658.488;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;10;-2584.641,-245.4037;Inherit;True;Property;_Texture5R;Texture5(R);24;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;49;-4407.182,-824.9539;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-3234.692,412.2286;Inherit;True;Property;_Mask1;Mask1;0;0;Create;False;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-3951.99,-173.3502;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-2292.573,-87.69802;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-2212.255,79.87312;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;50;-4334.982,-1159.854;Inherit;False;InstancedProperty;_Texture3B_Color;Texture3(B)_Color;11;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;65;-2566.382,-728.5542;Inherit;False;InstancedProperty;_Texture6G_Saturation;Texture6(G)_Saturation;32;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;74;-2644.033,-2017.297;Inherit;False;1040.002;638.1841;Comment;7;12;69;68;70;72;73;71;Texture 7;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;91;-3175.923,-1882.305;Inherit;False;InstancedProperty;_Texture7B_Tile;Texture7(B)_Tile;34;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.LerpOp;6;-2286.292,596.5529;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;47;-4296.501,-954.9974;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;87;-2892.039,-983.7667;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;59;-2120.618,-324.4085;Inherit;False;InstancedProperty;_Texture5R_Color;Texture5(R)_Color;23;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-2234.382,-808.5542;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-2594.033,-1604.077;Inherit;False;InstancedProperty;_Texture7B_Saturation;Texture7(B)_Saturation;35;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-2581.117,-1064.481;Inherit;True;Property;_Texture6G;Texture6(G);29;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DesaturateOpNode;55;-2094.744,-82.91759;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;90;-2920.009,-1819.586;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-4086.359,-991.3083;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;7;-2151.029,736.5825;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;12;-2553.382,-1923.144;Inherit;True;Property;_Texture7B;Texture7(B);33;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-1851.098,-138.9778;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;8;-1921.152,604.0944;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;63;-2214.382,-1034.554;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-2591.095,-861.0775;Inherit;False;InstancedProperty;_Texture6G_Intensity;Texture6(G)_Intensity;31;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-1412.892,-442.7422;Inherit;True;Property;_Mask2;Mask2;22;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;66;-2140.382,-1206.554;Inherit;False;InstancedProperty;_Texture6G_Color;Texture6(G)_Color;28;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;108;-1683.595,-2178.247;Inherit;False;InstancedProperty;_Texture8R_Tile;Texture8(R)_Tile;42;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;105;-1425.853,-2000.222;Inherit;False;880.1791;640.0446;Comment;7;97;98;99;100;102;101;103;Texture 8;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-2363.667,-1697.543;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;73;-2233.796,-1967.297;Inherit;False;InstancedProperty;_Texture7B_Color;Texture7(B)_Color;37;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;14;-957.705,185.0638;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-876.9775,19.51623;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-2403.99,-1520.963;Inherit;False;InstancedProperty;_Texture7B_Intensity;Texture7(B)_Intensity;36;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;70;-2199.016,-1791.204;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.Vector2Node;110;-963.3443,-2140.721;Inherit;False;InstancedProperty;_Texture9G_Tile;Texture9(G)_Tile;47;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;99;-1375.853,-1630.476;Inherit;False;InstancedProperty;_Texture8R_Saturation;Texture8(R)_Saturation;43;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;107;-1477.753,-2172.608;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;117;-440.3524,-2014.517;Inherit;False;844.7296;634.842;Comment;7;106;113;112;111;116;115;114;Texture 9;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;109;-696.6824,-2150.049;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;15;-719.0696,243.0636;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;113;-390.3524,-1572.978;Inherit;False;InstancedProperty;_Texture9G_Saturation;Texture9(G)_Saturation;48;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-1766.432,-1536.313;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;97;-1287.469,-1889.198;Inherit;True;Property;_Texture8R;Texture8(R);40;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;129;477.675,-2008.255;Inherit;False;835.2037;619.6663;Comment;7;120;123;124;125;126;127;128;Texture 10;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;121;87.28481,-2208.46;Inherit;False;InstancedProperty;_Texture10B_Tile;Texture10(B)_Tile;52;0;Create;True;0;0;False;0;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;-1104.652,-1594.238;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;98;-943.3649,-1733.523;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;112;-127.4569,-1621.095;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;103;-970.3779,-1950.222;Inherit;False;InstancedProperty;_Texture8R_Color;Texture8(R)_Color;39;0;Create;True;0;0;False;0;1,1,1,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;125;527.6749,-1672.525;Inherit;False;InstancedProperty;_Texture10B_Saturation;Texture10(B)_Saturation;53;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;106;-329.0807,-1867.069;Inherit;True;Property;_Texture9G;Texture9(G);46;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;122;283.1307,-2176.021;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;102;-1031.27,-1475.577;Inherit;False;InstancedProperty;_Texture8R_Intensity;Texture8(R)_Intensity;44;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;16;-504.1315,31.79256;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;120;553.3461,-1900.442;Inherit;True;Property;_Texture10B;Texture10(B);51;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;783.2703,-1646.853;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;111;7.839478,-1731.599;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;93;-1040.625,-606.6801;Inherit;True;Property;_Mask3;Mask3;38;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-708.0735,-1703.532;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;116;-160.6906,-1495.075;Inherit;False;InstancedProperty;_Texture9G_Intensity;Texture9(G)_Intensity;49;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;114;28.50297,-1964.517;Inherit;False;InstancedProperty;_Texture9G_Color;Texture9(G)_Color;45;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ToggleSwitchNode;17;-346.6656,425.9079;Inherit;False;Property;_Texture_Pack_2;Texture_Pack_2;21;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;104;-44.84813,-244.5919;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;128;797.7797,-1503.989;Inherit;False;InstancedProperty;_texture10B_Intensity;texture10(B)_Intensity;54;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;127;949.5742,-1958.255;Inherit;False;InstancedProperty;_Texture10B_Color;Texture10(B)_Color;50;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;241.9772,-1630.646;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;123;937.7998,-1746.823;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;126;1150.479,-1642.389;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;118;369.9647,-367.927;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;130;365.519,160.6414;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ToggleSwitchNode;94;294.5157,447.9547;Inherit;False;Property;_Texture_Pack_3;Texture_Pack_3;41;0;Create;True;0;0;False;0;0;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;804.7768,274.041;Float;False;True;-1;0;ASEMaterialInspector;0;0;Standard;GroundShader4;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;75;0;76;0
WireConnection;82;0;84;0
WireConnection;2;1;75;0
WireConnection;5;1;82;0
WireConnection;79;0;78;0
WireConnection;81;0;80;0
WireConnection;21;0;22;0
WireConnection;30;0;29;0
WireConnection;19;0;2;0
WireConnection;19;1;20;0
WireConnection;26;0;5;0
WireConnection;26;1;27;0
WireConnection;3;1;79;0
WireConnection;28;0;26;0
WireConnection;28;1;30;0
WireConnection;18;0;19;0
WireConnection;18;1;21;0
WireConnection;40;0;41;0
WireConnection;4;1;81;0
WireConnection;37;0;3;0
WireConnection;37;1;38;0
WireConnection;86;0;85;0
WireConnection;45;0;4;0
WireConnection;45;1;46;0
WireConnection;24;0;23;0
WireConnection;24;1;18;0
WireConnection;39;0;37;0
WireConnection;39;1;40;0
WireConnection;31;0;32;0
WireConnection;31;1;28;0
WireConnection;10;1;86;0
WireConnection;49;0;48;0
WireConnection;43;0;42;0
WireConnection;43;1;39;0
WireConnection;53;0;10;0
WireConnection;53;1;54;0
WireConnection;56;0;57;0
WireConnection;6;0;31;0
WireConnection;6;1;24;0
WireConnection;6;2;1;1
WireConnection;47;0;45;0
WireConnection;47;1;49;0
WireConnection;87;0;88;0
WireConnection;64;0;65;0
WireConnection;11;1;87;0
WireConnection;55;0;53;0
WireConnection;55;1;56;0
WireConnection;90;0;91;0
WireConnection;51;0;50;0
WireConnection;51;1;47;0
WireConnection;7;0;6;0
WireConnection;7;1;43;0
WireConnection;7;2;1;2
WireConnection;12;1;90;0
WireConnection;58;0;59;0
WireConnection;58;1;55;0
WireConnection;8;0;7;0
WireConnection;8;1;51;0
WireConnection;8;2;1;3
WireConnection;63;0;11;0
WireConnection;63;1;64;0
WireConnection;68;0;69;0
WireConnection;14;0;8;0
WireConnection;14;1;58;0
WireConnection;14;2;9;1
WireConnection;61;0;63;0
WireConnection;61;1;62;0
WireConnection;61;2;66;0
WireConnection;70;0;12;0
WireConnection;70;1;68;0
WireConnection;107;0;108;0
WireConnection;109;0;110;0
WireConnection;15;0;14;0
WireConnection;15;1;61;0
WireConnection;15;2;9;2
WireConnection;71;0;70;0
WireConnection;71;1;72;0
WireConnection;71;2;73;0
WireConnection;97;1;107;0
WireConnection;100;0;99;0
WireConnection;98;0;97;0
WireConnection;98;1;100;0
WireConnection;112;0;113;0
WireConnection;106;1;109;0
WireConnection;122;0;121;0
WireConnection;16;0;15;0
WireConnection;16;1;71;0
WireConnection;16;2;9;3
WireConnection;120;1;122;0
WireConnection;124;0;125;0
WireConnection;111;0;106;0
WireConnection;111;1;112;0
WireConnection;101;0;98;0
WireConnection;101;1;102;0
WireConnection;101;2;103;0
WireConnection;17;0;8;0
WireConnection;17;1;16;0
WireConnection;104;0;17;0
WireConnection;104;1;101;0
WireConnection;104;2;93;1
WireConnection;115;0;111;0
WireConnection;115;1;116;0
WireConnection;115;2;114;0
WireConnection;123;0;120;0
WireConnection;123;1;124;0
WireConnection;126;0;123;0
WireConnection;126;1;128;0
WireConnection;126;2;127;0
WireConnection;118;0;104;0
WireConnection;118;1;115;0
WireConnection;118;2;93;2
WireConnection;130;0;118;0
WireConnection;130;1;126;0
WireConnection;130;2;93;3
WireConnection;94;0;17;0
WireConnection;94;1;130;0
WireConnection;0;0;94;0
ASEEND*/
//CHKSM=3DD514DDB43C91109812DDFE87783807669AAE1C