// Upgrade NOTE: upgraded instancing buffer 'TAshadersTerrainShader' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TAshaders/TerrainShader"
{
	Properties
	{
		_ColorMap1("ColorMap1", Color) = (1,1,1,0)
		_ColorMap2("ColorMap2", Color) = (0,0,0,0)
		_ColorMap_Tile("ColorMap_Tile", Range( 0 , 20)) = 1
		_Texture_R_Color("Texture_R_Color", Color) = (1,1,1,0)
		_Texture_R("Texture_R", 2D) = "white" {}
		_Texture_R_Normals("Texture_R_Normals", 2D) = "white" {}
		_Texture_R_Normal_Intensity("Texture_R_Normal_Intensity", Float) = 0
		_Texture_R_Intensity("Texture_R_Intensity", Float) = 1
		_Texture_R_Saturation("Texture_R_Saturation", Range( -1 , 5)) = 0
		_Texture_G_Color("Texture_G_Color", Color) = (1,1,1,0)
		_Texture_G("Texture_G", 2D) = "white" {}
		_Texture_G_Normals("Texture_G_Normals", 2D) = "white" {}
		_Texture_G_Normal_Intensity("Texture_G_Normal_Intensity", Range( 0 , 1)) = 0
		_Texture_G_Intensity("Texture_G_Intensity", Float) = 1
		_Texture_G_Saturation("Texture_G_Saturation", Range( -1 , 5)) = 0
		_Texture_B_Color("Texture_B_Color", Color) = (1,1,1,0)
		_Texture_B("Texture_B", 2D) = "white" {}
		_Texture_B_Normals("Texture_B_Normals", 2D) = "white" {}
		_Texture_B_Normal_Intensity("Texture_B_Normal_Intensity", Range( 0 , 1)) = 0
		_Texture_B_Intensity("Texture_B_Intensity", Float) = 1
		_Texture_B_Saturation("Texture_B_Saturation", Range( -1 , 5)) = 0
		_Texture_A_Color("Texture_A_Color", Color) = (1,1,1,0)
		_Texture_A("Texture_A", 2D) = "white" {}
		_Texture_A_Normals("Texture_A_Normals", 2D) = "white" {}
		_Texture_A_Nornal_Intensity("Texture_A_Nornal_Intensity", Range( 0 , 1)) = 1
		_Texture_A_Intensity("Texture_A_Intensity", Float) = 0
		_Texture_A_Saturation("Texture_A_Saturation", Range( -1 , 5)) = 0
		_Mask("Mask", 2D) = "white" {}
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
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture_A;
		uniform sampler2D _Texture_A_Normals;
		uniform sampler2D _Texture_R;
		uniform sampler2D _Texture_R_Normals;
		uniform sampler2D _Mask;
		uniform sampler2D _Texture_G;
		uniform sampler2D _Texture_G_Normals;
		uniform sampler2D _Texture_B;
		uniform sampler2D _Texture_B_Normals;

		UNITY_INSTANCING_BUFFER_START(TAshadersTerrainShader)
			UNITY_DEFINE_INSTANCED_PROP(float4, _ColorMap1)
#define _ColorMap1_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture_G_Color)
#define _Texture_G_Color_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Mask_ST)
#define _Mask_ST_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture_R_Normals_ST)
#define _Texture_R_Normals_ST_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture_R_ST)
#define _Texture_R_ST_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture_G_ST)
#define _Texture_G_ST_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture_B_Color)
#define _Texture_B_Color_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture_R_Color)
#define _Texture_R_Color_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture_B_Normals_ST)
#define _Texture_B_Normals_ST_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture_A_Normals_ST)
#define _Texture_A_Normals_ST_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture_A_ST)
#define _Texture_A_ST_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture_A_Color)
#define _Texture_A_Color_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _ColorMap2)
#define _ColorMap2_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture_B_ST)
#define _Texture_B_ST_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Texture_G_Normals_ST)
#define _Texture_G_Normals_ST_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture_G_Saturation)
#define _Texture_G_Saturation_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture_G_Intensity)
#define _Texture_G_Intensity_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture_G_Normal_Intensity)
#define _Texture_G_Normal_Intensity_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture_B_Normal_Intensity)
#define _Texture_B_Normal_Intensity_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture_R_Intensity)
#define _Texture_R_Intensity_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture_B_Intensity)
#define _Texture_B_Intensity_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture_R_Normal_Intensity)
#define _Texture_R_Normal_Intensity_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture_A_Saturation)
#define _Texture_A_Saturation_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture_A_Intensity)
#define _Texture_A_Intensity_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture_A_Nornal_Intensity)
#define _Texture_A_Nornal_Intensity_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float, _ColorMap_Tile)
#define _ColorMap_Tile_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture_R_Saturation)
#define _Texture_R_Saturation_arr TAshadersTerrainShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Texture_B_Saturation)
#define _Texture_B_Saturation_arr TAshadersTerrainShader
		UNITY_INSTANCING_BUFFER_END(TAshadersTerrainShader)


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


		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float4 _ColorMap1_Instance = UNITY_ACCESS_INSTANCED_PROP(_ColorMap1_arr, _ColorMap1);
			float4 _ColorMap2_Instance = UNITY_ACCESS_INSTANCED_PROP(_ColorMap2_arr, _ColorMap2);
			float _ColorMap_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_ColorMap_Tile_arr, _ColorMap_Tile);
			float2 temp_cast_0 = (_ColorMap_Tile_Instance).xx;
			float2 uv_TexCoord67 = i.uv_texcoord * temp_cast_0;
			float simplePerlin2D65 = snoise( uv_TexCoord67*7.0 );
			simplePerlin2D65 = simplePerlin2D65*0.5 + 0.5;
			float4 lerpResult66 = lerp( _ColorMap1_Instance , _ColorMap2_Instance , simplePerlin2D65);
			float4 _Texture_A_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_A_Color_arr, _Texture_A_Color);
			float4 _Texture_A_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_A_ST_arr, _Texture_A_ST);
			float2 uv_Texture_A = i.uv_texcoord * _Texture_A_ST_Instance.xy + _Texture_A_ST_Instance.zw;
			float4 _Texture_A_Normals_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_A_Normals_ST_arr, _Texture_A_Normals_ST);
			float2 uv_Texture_A_Normals = i.uv_texcoord * _Texture_A_Normals_ST_Instance.xy + _Texture_A_Normals_ST_Instance.zw;
			float _Texture_A_Nornal_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_A_Nornal_Intensity_arr, _Texture_A_Nornal_Intensity);
			float4 lerpResult16 = lerp( tex2D( _Texture_A, uv_Texture_A ) , tex2D( _Texture_A_Normals, uv_Texture_A_Normals ) , _Texture_A_Nornal_Intensity_Instance);
			float _Texture_A_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_A_Intensity_arr, _Texture_A_Intensity);
			float _Texture_A_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_A_Saturation_arr, _Texture_A_Saturation);
			float3 desaturateInitialColor17 = ( lerpResult16 * _Texture_A_Intensity_Instance ).rgb;
			float desaturateDot17 = dot( desaturateInitialColor17, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar17 = lerp( desaturateInitialColor17, desaturateDot17.xxx, ( _Texture_A_Saturation_Instance * -1.0 ) );
			float4 _Texture_R_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_R_Color_arr, _Texture_R_Color);
			float4 _Texture_R_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_R_ST_arr, _Texture_R_ST);
			float2 uv_Texture_R = i.uv_texcoord * _Texture_R_ST_Instance.xy + _Texture_R_ST_Instance.zw;
			float4 _Texture_R_Normals_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_R_Normals_ST_arr, _Texture_R_Normals_ST);
			float2 uv_Texture_R_Normals = i.uv_texcoord * _Texture_R_Normals_ST_Instance.xy + _Texture_R_Normals_ST_Instance.zw;
			float _Texture_R_Normal_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_R_Normal_Intensity_arr, _Texture_R_Normal_Intensity);
			float4 lerpResult27 = lerp( tex2D( _Texture_R, uv_Texture_R ) , tex2D( _Texture_R_Normals, uv_Texture_R_Normals ) , _Texture_R_Normal_Intensity_Instance);
			float _Texture_R_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_R_Intensity_arr, _Texture_R_Intensity);
			float _Texture_R_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_R_Saturation_arr, _Texture_R_Saturation);
			float3 desaturateInitialColor30 = ( lerpResult27 * _Texture_R_Intensity_Instance ).rgb;
			float desaturateDot30 = dot( desaturateInitialColor30, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar30 = lerp( desaturateInitialColor30, desaturateDot30.xxx, ( _Texture_R_Saturation_Instance * -1.0 ) );
			float4 _Mask_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Mask_ST_arr, _Mask_ST);
			float2 uv_Mask = i.uv_texcoord * _Mask_ST_Instance.xy + _Mask_ST_Instance.zw;
			float4 break3 = tex2D( _Mask, uv_Mask );
			float4 lerpResult48 = lerp( ( _Texture_A_Color_Instance * float4( desaturateVar17 , 0.0 ) ) , ( _Texture_R_Color_Instance * float4( desaturateVar30 , 0.0 ) ) , break3.r);
			float4 _Texture_G_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_G_Color_arr, _Texture_G_Color);
			float4 _Texture_G_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_G_ST_arr, _Texture_G_ST);
			float2 uv_Texture_G = i.uv_texcoord * _Texture_G_ST_Instance.xy + _Texture_G_ST_Instance.zw;
			float4 _Texture_G_Normals_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_G_Normals_ST_arr, _Texture_G_Normals_ST);
			float2 uv_Texture_G_Normals = i.uv_texcoord * _Texture_G_Normals_ST_Instance.xy + _Texture_G_Normals_ST_Instance.zw;
			float _Texture_G_Normal_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_G_Normal_Intensity_arr, _Texture_G_Normal_Intensity);
			float4 lerpResult37 = lerp( tex2D( _Texture_G, uv_Texture_G ) , tex2D( _Texture_G_Normals, uv_Texture_G_Normals ) , _Texture_G_Normal_Intensity_Instance);
			float _Texture_G_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_G_Intensity_arr, _Texture_G_Intensity);
			float _Texture_G_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_G_Saturation_arr, _Texture_G_Saturation);
			float3 desaturateInitialColor42 = ( lerpResult37 * _Texture_G_Intensity_Instance ).rgb;
			float desaturateDot42 = dot( desaturateInitialColor42, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar42 = lerp( desaturateInitialColor42, desaturateDot42.xxx, ( _Texture_G_Saturation_Instance * -1.0 ) );
			float4 lerpResult49 = lerp( lerpResult48 , ( _Texture_G_Color_Instance * float4( desaturateVar42 , 0.0 ) ) , break3.g);
			float4 _Texture_B_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_B_Color_arr, _Texture_B_Color);
			float4 _Texture_B_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_B_ST_arr, _Texture_B_ST);
			float2 uv_Texture_B = i.uv_texcoord * _Texture_B_ST_Instance.xy + _Texture_B_ST_Instance.zw;
			float4 _Texture_B_Normals_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_B_Normals_ST_arr, _Texture_B_Normals_ST);
			float2 uv_Texture_B_Normals = i.uv_texcoord * _Texture_B_Normals_ST_Instance.xy + _Texture_B_Normals_ST_Instance.zw;
			float _Texture_B_Normal_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_B_Normal_Intensity_arr, _Texture_B_Normal_Intensity);
			float4 lerpResult52 = lerp( tex2D( _Texture_B, uv_Texture_B ) , tex2D( _Texture_B_Normals, uv_Texture_B_Normals ) , _Texture_B_Normal_Intensity_Instance);
			float _Texture_B_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_B_Intensity_arr, _Texture_B_Intensity);
			float _Texture_B_Saturation_Instance = UNITY_ACCESS_INSTANCED_PROP(_Texture_B_Saturation_arr, _Texture_B_Saturation);
			float3 desaturateInitialColor56 = ( lerpResult52 * _Texture_B_Intensity_Instance ).rgb;
			float desaturateDot56 = dot( desaturateInitialColor56, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar56 = lerp( desaturateInitialColor56, desaturateDot56.xxx, ( _Texture_B_Saturation_Instance * -1.0 ) );
			float4 lerpResult61 = lerp( lerpResult49 , ( _Texture_B_Color_Instance * float4( desaturateVar56 , 0.0 ) ) , break3.b);
			o.Albedo = ( lerpResult66 * lerpResult61 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1536;9.6;1536;800;2437.544;1000.535;2.844815;True;True
Node;AmplifyShaderEditor.CommentaryNode;35;-2312.665,-525.8143;Inherit;False;1180.63;807.1652;Comment;11;4;25;26;27;29;28;31;32;30;33;34;Texture R;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;24;-3816.359,-732.1429;Inherit;False;1149.606;747.2096;Comment;11;11;13;16;15;19;18;20;21;23;17;22;Texture A;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-2262.664,52.85767;Inherit;False;InstancedProperty;_Texture_R_Normal_Intensity;Texture_R_Normal_Intensity;6;0;Create;True;0;0;False;0;0;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-2222.607,-475.8142;Inherit;True;Property;_Texture_R;Texture_R;4;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;25;-2249.949,-217.5627;Inherit;True;Property;_Texture_R_Normals;Texture_R_Normals;5;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-3766.359,-216.081;Inherit;False;InstancedProperty;_Texture_A_Nornal_Intensity;Texture_A_Nornal_Intensity;24;0;Create;True;0;0;False;0;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;47;-2324.235,452.3062;Inherit;False;1348.937;758.111;Comment;11;38;36;39;37;41;40;42;43;44;45;46;Texture G;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;11;-3730.336,-682.1429;Inherit;True;Property;_Texture_A;Texture_A;22;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-3736.477,-431.8221;Inherit;True;Property;_Texture_A_Normals;Texture_A_Normals;23;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;16;-3341.585,-487.2139;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1896.357,-66.3226;Inherit;False;InstancedProperty;_Texture_R_Intensity;Texture_R_Intensity;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;38;-2274.235,750.4421;Inherit;True;Property;_Texture_G_Normals;Texture_G_Normals;11;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;36;-2216.632,508.666;Inherit;True;Property;_Texture_G;Texture_G;10;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;62;-2474.996,1334.459;Inherit;False;1594.403;673.4729;Comment;11;50;52;53;54;56;51;57;55;58;59;60;Texture B;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-2250.932,988.8821;Inherit;False;InstancedProperty;_Texture_G_Normal_Intensity;Texture_G_Normal_Intensity;12;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-1918.335,165.9508;Inherit;False;InstancedProperty;_Texture_R_Saturation;Texture_R_Saturation;8;0;Create;True;0;0;False;0;0;0;-1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-3425.772,-224.9474;Inherit;False;InstancedProperty;_Texture_A_Intensity;Texture_A_Intensity;25;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-3382.93,-100.3334;Inherit;False;InstancedProperty;_Texture_A_Saturation;Texture_A_Saturation;26;0;Create;True;0;0;False;0;0;0;-1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;27;-1864.135,-306.7318;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;37;-1809.011,671.4202;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-2395.785,1828.917;Inherit;False;InstancedProperty;_Texture_B_Normal_Intensity;Texture_B_Normal_Intensity;18;0;Create;True;0;0;False;0;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-1879.494,885.9524;Inherit;False;InstancedProperty;_Texture_G_Intensity;Texture_G_Intensity;13;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-3173.903,-402.5823;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1664.736,-210.638;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-3107.75,-247.6308;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1600.303,-1.523685;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;12;-3271.749,222.4527;Inherit;False;717.3193;280.4163;Comment;2;1;3;Mask ;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;50;-2303.14,1384.459;Inherit;True;Property;_Texture_B;Texture_B;16;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;44;-1891.966,1095.017;Inherit;False;InstancedProperty;_Texture_G_Saturation;Texture_G_Saturation;14;0;Create;True;0;0;False;0;0;0;-1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;53;-2424.996,1601.261;Inherit;True;Property;_Texture_B_Normals;Texture_B_Normals;17;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;58;-2015.929,1892.532;Inherit;False;InstancedProperty;_Texture_B_Saturation;Texture_B_Saturation;20;0;Create;True;0;0;False;0;0;0;-1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1539.636,935.5923;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;40;-1595.573,711.9191;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;70;-1045.917,-542.1666;Inherit;False;806.5861;672.4637;Comment;6;68;67;63;64;65;66;Color map;1,1,1,1;0;0
Node;AmplifyShaderEditor.DesaturateOpNode;30;-1495.161,-178.5704;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;52;-1883.552,1523.911;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DesaturateOpNode;17;-3025.31,-373.3905;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;22;-3124.847,-603.3967;Inherit;False;InstancedProperty;_Texture_A_Color;Texture_A_Color;21;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;55;-2004.183,1685.466;Inherit;False;InstancedProperty;_Texture_B_Intensity;Texture_B_Intensity;19;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-3221.748,272.869;Inherit;True;Property;_Mask;Mask;27;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;33;-1575.524,-404.8814;Inherit;False;InstancedProperty;_Texture_R_Color;Texture_R_Color;3;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;46;-1484.328,502.3062;Inherit;False;InstancedProperty;_Texture_G_Color;Texture_G_Color;9;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-2829.153,-422.1717;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-1650.575,1767.524;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;-1;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;3;-2808.029,272.4527;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DesaturateOpNode;42;-1376.302,776.5016;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-1715.745,1539.371;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-1294.433,-280.496;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-995.9168,-459.5154;Inherit;False;InstancedProperty;_ColorMap_Tile;ColorMap_Tile;2;0;Create;True;0;0;False;0;1;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;56;-1486.574,1624.125;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;48;-978.1192,208.7813;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-1137.697,704.4854;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;59;-1502.217,1403.081;Inherit;False;InstancedProperty;_Texture_B_Color;Texture_B_Color;15;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;67;-886.3026,-309.0874;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-1042.993,1520.639;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;49;-787.6813,360.4603;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;65;-866.812,-71.62039;Inherit;False;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;7;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;63;-670.5734,-492.1667;Inherit;False;InstancedProperty;_ColorMap1;ColorMap1;0;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;64;-588.176,-274.3524;Inherit;False;InstancedProperty;_ColorMap2;ColorMap2;1;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;61;-660.6859,507.6357;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;66;-421.3307,-26.90329;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-292.9106,588.4102;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-36.86626,710.9149;Float;False;True;-1;0;ASEMaterialInspector;0;0;StandardSpecular;TAshaders/TerrainShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;0;11;0
WireConnection;16;1;13;0
WireConnection;16;2;15;0
WireConnection;27;0;4;0
WireConnection;27;1;25;0
WireConnection;27;2;26;0
WireConnection;37;0;36;0
WireConnection;37;1;38;0
WireConnection;37;2;39;0
WireConnection;20;0;16;0
WireConnection;20;1;21;0
WireConnection;28;0;27;0
WireConnection;28;1;29;0
WireConnection;19;0;18;0
WireConnection;31;0;32;0
WireConnection;43;0;44;0
WireConnection;40;0;37;0
WireConnection;40;1;41;0
WireConnection;30;0;28;0
WireConnection;30;1;31;0
WireConnection;52;0;50;0
WireConnection;52;1;53;0
WireConnection;52;2;54;0
WireConnection;17;0;20;0
WireConnection;17;1;19;0
WireConnection;23;0;22;0
WireConnection;23;1;17;0
WireConnection;57;0;58;0
WireConnection;3;0;1;0
WireConnection;42;0;40;0
WireConnection;42;1;43;0
WireConnection;51;0;52;0
WireConnection;51;1;55;0
WireConnection;34;0;33;0
WireConnection;34;1;30;0
WireConnection;56;0;51;0
WireConnection;56;1;57;0
WireConnection;48;0;23;0
WireConnection;48;1;34;0
WireConnection;48;2;3;0
WireConnection;45;0;46;0
WireConnection;45;1;42;0
WireConnection;67;0;68;0
WireConnection;60;0;59;0
WireConnection;60;1;56;0
WireConnection;49;0;48;0
WireConnection;49;1;45;0
WireConnection;49;2;3;1
WireConnection;65;0;67;0
WireConnection;61;0;49;0
WireConnection;61;1;60;0
WireConnection;61;2;3;2
WireConnection;66;0;63;0
WireConnection;66;1;64;0
WireConnection;66;2;65;0
WireConnection;69;0;66;0
WireConnection;69;1;61;0
WireConnection;0;0;69;0
ASEEND*/
//CHKSM=1EB0F39C106F4F18DB27E973067E50488E9A5AC6