// Upgrade NOTE: upgraded instancing buffer 'TAshadersProceduralOceanShader' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TAshaders/ProceduralOceanShader"
{
	Properties
	{
		_Base_Color_1("Base_Color_1", Color) = (1,1,1,0)
		_Base_Color_2("Base_Color_2", Color) = (0,0,0,0)
		_Ocean_Texture_1("Ocean_Texture_1", 2D) = "white" {}
		_Ocean_1_Tile("Ocean_1_Tile", Vector) = (2,2,0,0)
		_Ocean_Texture_1_Intensity("Ocean_Texture_1_Intensity", Float) = 0
		_Ocean_1_Speed("Ocean_1_Speed", Vector) = (0,0,0,0)
		_Ocean_Texture_2("Ocean_Texture_2", 2D) = "white" {}
		_Ocean_2_Tile("Ocean_2_Tile", Vector) = (1,1,0,0)
		_Ocean_Texture_2_Intensity("Ocean_Texture_2_Intensity", Float) = 0
		_Ocean_2_Speed("Ocean_2_Speed", Vector) = (0,0,0,0)
		_Foam_Color("Foam_Color", Color) = (0,0,0,0)
		_Foam_Texture("Foam_Texture", 2D) = "white" {}
		_Foam_Texture_Tile("Foam_Texture_Tile", Vector) = (1,1,0,0)
		_Foam_Intensity("Foam_Intensity", Float) = 0
		_Foam_Speed("Foam_Speed", Vector) = (0,0,0,0)
		_Specular_Tile("Specular_Tile", Vector) = (1,1,0,0)
		_Specular_Color("Specular_Color", Color) = (0,0,0,0)
		_Specular_Intensity("Specular_Intensity", Float) = 1
		_Specular_Texture_1("Specular_Texture_1", 2D) = "white" {}
		_Specular_1_Speed("Specular_1_Speed", Vector) = (0,0,0,0)
		_Specular_Texture_2("Specular_Texture_2", 2D) = "white" {}
		_Specular_2_Speed("Specular_2_Speed", Vector) = (0,0,0,0)
		_Specular_Noise_Texture("Specular_Noise_Texture", 2D) = "white" {}
		_Specular_Noise_Speed("Specular_Noise_Speed", Vector) = (0,0,0,0)
		_Specular_Noise_Intensity("Specular_Noise_Intensity", Float) = 0
		_Specular_Noise_Tile("Specular_Noise_Tile", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 2.0
		#pragma multi_compile_instancing
		#pragma surface surf Lambert alpha:fade keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Ocean_Texture_1;
		uniform sampler2D _Ocean_Texture_2;
		uniform sampler2D _Specular_Texture_1;
		uniform sampler2D _Specular_Texture_2;
		uniform sampler2D _Specular_Noise_Texture;
		uniform sampler2D _Foam_Texture;

		UNITY_INSTANCING_BUFFER_START(TAshadersProceduralOceanShader)
			UNITY_DEFINE_INSTANCED_PROP(float4, _Base_Color_1)
#define _Base_Color_1_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Base_Color_2)
#define _Base_Color_2_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Foam_Color)
#define _Foam_Color_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _Specular_Color)
#define _Specular_Color_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _Foam_Texture_Tile)
#define _Foam_Texture_Tile_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _Specular_Noise_Speed)
#define _Specular_Noise_Speed_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _Specular_2_Speed)
#define _Specular_2_Speed_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _Foam_Speed)
#define _Foam_Speed_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _Specular_1_Speed)
#define _Specular_1_Speed_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _Ocean_2_Speed)
#define _Ocean_2_Speed_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _Ocean_2_Tile)
#define _Ocean_2_Tile_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _Ocean_1_Speed)
#define _Ocean_1_Speed_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _Ocean_1_Tile)
#define _Ocean_1_Tile_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float2, _Specular_Tile)
#define _Specular_Tile_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Ocean_Texture_2_Intensity)
#define _Ocean_Texture_2_Intensity_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Specular_Intensity)
#define _Specular_Intensity_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Specular_Noise_Tile)
#define _Specular_Noise_Tile_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Ocean_Texture_1_Intensity)
#define _Ocean_Texture_1_Intensity_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Specular_Noise_Intensity)
#define _Specular_Noise_Intensity_arr TAshadersProceduralOceanShader
			UNITY_DEFINE_INSTANCED_PROP(float, _Foam_Intensity)
#define _Foam_Intensity_arr TAshadersProceduralOceanShader
		UNITY_INSTANCING_BUFFER_END(TAshadersProceduralOceanShader)

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 _Base_Color_1_Instance = UNITY_ACCESS_INSTANCED_PROP(_Base_Color_1_arr, _Base_Color_1);
			float4 _Base_Color_2_Instance = UNITY_ACCESS_INSTANCED_PROP(_Base_Color_2_arr, _Base_Color_2);
			float2 _Ocean_1_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Ocean_1_Tile_arr, _Ocean_1_Tile);
			float2 _Ocean_1_Speed_Instance = UNITY_ACCESS_INSTANCED_PROP(_Ocean_1_Speed_arr, _Ocean_1_Speed);
			float2 uv_TexCoord2 = i.uv_texcoord * _Ocean_1_Tile_Instance + ( _Ocean_1_Speed_Instance * _Time.y );
			float _Ocean_Texture_1_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Ocean_Texture_1_Intensity_arr, _Ocean_Texture_1_Intensity);
			float2 _Ocean_2_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Ocean_2_Tile_arr, _Ocean_2_Tile);
			float2 _Ocean_2_Speed_Instance = UNITY_ACCESS_INSTANCED_PROP(_Ocean_2_Speed_arr, _Ocean_2_Speed);
			float2 uv_TexCoord20 = i.uv_texcoord * _Ocean_2_Tile_Instance + ( _Ocean_2_Speed_Instance * _Time.y );
			float _Ocean_Texture_2_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Ocean_Texture_2_Intensity_arr, _Ocean_Texture_2_Intensity);
			float4 lerpResult51 = lerp( _Base_Color_1_Instance , _Base_Color_2_Instance , ( ( tex2D( _Ocean_Texture_1, uv_TexCoord2 ) * _Ocean_Texture_1_Intensity_Instance ) + ( tex2D( _Ocean_Texture_2, uv_TexCoord20 ) * _Ocean_Texture_2_Intensity_Instance ) ));
			float2 _Specular_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Specular_Tile_arr, _Specular_Tile);
			float2 _Specular_1_Speed_Instance = UNITY_ACCESS_INSTANCED_PROP(_Specular_1_Speed_arr, _Specular_1_Speed);
			float2 uv_TexCoord42 = i.uv_texcoord * _Specular_Tile_Instance + ( _Specular_1_Speed_Instance * _Time.y );
			float4 tex2DNode43 = tex2D( _Specular_Texture_1, uv_TexCoord42 );
			float2 _Specular_2_Speed_Instance = UNITY_ACCESS_INSTANCED_PROP(_Specular_2_Speed_arr, _Specular_2_Speed);
			float2 uv_TexCoord60 = i.uv_texcoord * _Specular_Tile_Instance + ( _Specular_2_Speed_Instance * _Time.y );
			float4 tex2DNode58 = tex2D( _Specular_Texture_2, uv_TexCoord60 );
			float _Specular_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Specular_Intensity_arr, _Specular_Intensity);
			float4 lerpResult46 = lerp( float4( 0,0,0,0 ) , ( tex2DNode43 + tex2DNode58 ) , ( ( tex2DNode43.a + tex2DNode58.a ) * _Specular_Intensity_Instance ));
			float4 _Specular_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Specular_Color_arr, _Specular_Color);
			float _Specular_Noise_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Specular_Noise_Tile_arr, _Specular_Noise_Tile);
			float2 temp_cast_0 = (_Specular_Noise_Tile_Instance).xx;
			float2 _Specular_Noise_Speed_Instance = UNITY_ACCESS_INSTANCED_PROP(_Specular_Noise_Speed_arr, _Specular_Noise_Speed);
			float2 uv_TexCoord76 = i.uv_texcoord * temp_cast_0 + ( _Specular_Noise_Speed_Instance * _Time.y );
			float _Specular_Noise_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Specular_Noise_Intensity_arr, _Specular_Noise_Intensity);
			float4 clampResult71 = clamp( ( tex2D( _Specular_Noise_Texture, uv_TexCoord76 ) * _Specular_Noise_Intensity_Instance ) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float4 lerpResult50 = lerp( float4( 0,0,0,0 ) , ( lerpResult46 * _Specular_Color_Instance ) , clampResult71);
			float4 _Foam_Color_Instance = UNITY_ACCESS_INSTANCED_PROP(_Foam_Color_arr, _Foam_Color);
			float2 _Foam_Texture_Tile_Instance = UNITY_ACCESS_INSTANCED_PROP(_Foam_Texture_Tile_arr, _Foam_Texture_Tile);
			float2 _Foam_Speed_Instance = UNITY_ACCESS_INSTANCED_PROP(_Foam_Speed_arr, _Foam_Speed);
			float2 uv_TexCoord36 = i.uv_texcoord * _Foam_Texture_Tile_Instance + ( _Foam_Speed_Instance * _Time.y );
			float _Foam_Intensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_Foam_Intensity_arr, _Foam_Intensity);
			float4 lerpResult40 = lerp( ( lerpResult51 + lerpResult50 ) , ( _Foam_Color_Instance * ( tex2D( _Foam_Texture, uv_TexCoord36 ) * _Foam_Intensity_Instance ) ) , i.vertexColor.r);
			o.Emission = lerpResult40.rgb;
			float4 color55 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 lerpResult56 = lerp( color55 , color55 , i.vertexColor.r);
			float4 color54 = IsGammaSpace() ? float4(0,0,0,0) : float4(0,0,0,0);
			float4 lerpResult57 = lerp( lerpResult56 , color54 , i.vertexColor.g);
			o.Alpha = lerpResult57.r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
1536;19.2;1536;787;6112.209;2485.015;3.7;True;True
Node;AmplifyShaderEditor.CommentaryNode;63;-3567.821,-1659.02;Inherit;False;1121.03;298.0016;Comment;4;58;60;61;62;Specular 2;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;53;-3540.695,-2280.199;Inherit;False;1254.306;482.2341;Comment;5;49;47;48;43;42;Specular 1;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;47;-3490.695,-2038.424;Inherit;False;InstancedProperty;_Specular_1_Speed;Specular_1_Speed;19;0;Create;True;0;0;False;0;0,0;0,-0.09;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;62;-3517.821,-1609.02;Inherit;False;InstancedProperty;_Specular_2_Speed;Specular_2_Speed;21;0;Create;True;0;0;False;0;0,0;0,-0.05;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TimeNode;11;-3994.494,-217.1885;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-3225.796,-1540.047;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;33;-3689.142,-650.4527;Inherit;False;1316.724;392.9873;Comment;7;12;13;8;2;16;27;26;Ocean texture 1;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;-3197.002,-1971.306;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;49;-3390.656,-2230.199;Inherit;False;InstancedProperty;_Specular_Tile;Specular_Tile;15;0;Create;True;0;0;False;0;1,1;0.56,-1.91;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;34;-3785.917,-85.688;Inherit;False;1494.607;509.9039;Comment;7;21;23;22;20;29;30;28;Ocean texture 2;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;12;-3635.516,-464.5831;Inherit;False;InstancedProperty;_Ocean_1_Speed;Ocean_1_Speed;5;0;Create;True;0;0;False;0;0,0;0.003,0.003;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;42;-2971.53,-2152.773;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;21;-3735.917,112.2563;Inherit;False;InstancedProperty;_Ocean_2_Speed;Ocean_2_Speed;9;0;Create;True;0;0;False;0;0,0;0.001,-0.005;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;60;-3015.756,-1554.576;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;77;-2930.818,-1047.892;Inherit;False;InstancedProperty;_Specular_Noise_Speed;Specular_Noise_Speed;23;0;Create;True;0;0;False;0;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-3373.758,-393.6132;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;8;-3453.769,-598.7963;Inherit;False;InstancedProperty;_Ocean_1_Tile;Ocean_1_Tile;3;0;Create;True;0;0;False;0;2,2;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;78;-2896.044,-1139.904;Inherit;False;InstancedProperty;_Specular_Noise_Tile;Specular_Noise_Tile;25;0;Create;True;0;0;False;0;0;0.02;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-3490.868,201.6657;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;23;-3520.691,3.341688;Inherit;False;InstancedProperty;_Ocean_2_Tile;Ocean_2_Tile;7;0;Create;True;0;0;False;0;1,1;0.56,0.82;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.CommentaryNode;41;-3623.564,607.6003;Inherit;False;1106.867;401.8947;Comment;5;38;37;39;36;35;Foam 1;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;58;-2768.391,-1591.018;Inherit;True;Property;_Specular_Texture_2;Specular_Texture_2;20;0;Create;True;0;0;False;0;-1;None;b0022992dcabfcf4abef34649b3a373a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;43;-2607.988,-2027.965;Inherit;True;Property;_Specular_Texture_1;Specular_Texture_1;18;0;Create;True;0;0;False;0;-1;None;b0022992dcabfcf4abef34649b3a373a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;-2682.555,-993.2336;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;68;-2074.114,-1800.402;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-2151.396,-1553.077;Inherit;False;InstancedProperty;_Specular_Intensity;Specular_Intensity;17;0;Create;True;0;0;False;0;1;1.81;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-3184.299,-551.8078;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;76;-2475.334,-1083.721;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;20;-3273.831,50.30074;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;38;-3594.101,809.0405;Inherit;False;InstancedProperty;_Foam_Speed;Foam_Speed;14;0;Create;True;0;0;False;0;0,0;0.002,0.004;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;37;-3378.031,657.6003;Inherit;False;InstancedProperty;_Foam_Texture_Tile;Foam_Texture_Tile;12;0;Create;True;0;0;False;0;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;67;-2069.432,-1940.185;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-2172.134,-924.6789;Inherit;False;InstancedProperty;_Specular_Noise_Intensity;Specular_Noise_Intensity;24;0;Create;True;0;0;False;0;0;1.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;-3347.443,869.7903;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;69;-1879.005,-1694.32;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;74;-2183.784,-1192.98;Inherit;True;Property;_Specular_Noise_Texture;Specular_Noise_Texture;22;0;Create;True;0;0;False;0;-1;None;9356f0e24f2d97a48a3749d258147bb9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-2885.155,-602.0051;Inherit;True;Property;_Ocean_Texture_1;Ocean_Texture_1;2;0;Create;True;0;0;False;0;-1;None;0c1ba59a6b2a65a4aa75a0e41264e5a7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;30;-2944.435,194.2157;Inherit;True;Property;_Ocean_Texture_2;Ocean_Texture_2;6;0;Create;True;0;0;False;0;-1;None;0c1ba59a6b2a65a4aa75a0e41264e5a7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-2904.709,-35.68801;Inherit;False;InstancedProperty;_Ocean_Texture_2_Intensity;Ocean_Texture_2_Intensity;8;0;Create;True;0;0;False;0;0;0.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-2980.263,-381.1617;Inherit;False;InstancedProperty;_Ocean_Texture_1_Intensity;Ocean_Texture_1_Intensity;4;0;Create;True;0;0;False;0;0;-0.01;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;80;-1753.366,-1512.28;Inherit;False;InstancedProperty;_Specular_Color;Specular_Color;16;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-2453.709,102.312;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-2522.401,-455.7356;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;31;-2256.085,-751.7014;Inherit;False;313.2794;461.7164;Comment;2;4;5;Base color 2;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-1752.653,-998.1214;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;46;-1507.316,-1779.573;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;36;-3101.603,751.7183;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-1826.173,-216.2769;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;4;-2206.085,-701.7014;Inherit;False;InstancedProperty;_Base_Color_1;Base_Color_1;0;0;Create;True;0;0;False;0;1,1,1,0;0,0.6803213,0.8392157,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;35;-2850.239,766.7691;Inherit;True;Property;_Foam_Texture;Foam_Texture;11;0;Create;True;0;0;False;0;-1;None;9356f0e24f2d97a48a3749d258147bb9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;71;-1537.26,-804.8552;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-1351.507,-1519.612;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;5;-2177.206,-498.9857;Inherit;False;InstancedProperty;_Base_Color_2;Base_Color_2;1;0;Create;True;0;0;False;0;0,0,0,0;0.1001246,0.2287462,0.4716981,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;85;-2375.216,795.3292;Inherit;False;InstancedProperty;_Foam_Intensity;Foam_Intensity;13;0;Create;True;0;0;False;0;0;0.38;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;82;-2484.398,451.2139;Inherit;False;InstancedProperty;_Foam_Color;Foam_Color;10;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;51;-1604.798,-458.6907;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-2185.273,678.8975;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexColorNode;6;-1223.837,165.1021;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;50;-1270.328,-950.9651;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;55;-1350.24,550.9131;Inherit;False;Constant;_Color1;Color 1;17;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;54;-1404.313,318.2609;Inherit;False;Constant;_Color0;Color 0;17;0;Create;True;0;0;False;0;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-974.0031,-277.7535;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;56;-839.4448,106.8072;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1977.761,455.1974;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;57;-505.3249,202.326;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;40;-721.5884,-222.3368;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;88,-148;Float;False;True;-1;0;ASEMaterialInspector;0;0;Lambert;TAshaders/ProceduralOceanShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;False;Transparent;;Transparent;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;61;0;62;0
WireConnection;61;1;11;2
WireConnection;48;0;47;0
WireConnection;48;1;11;2
WireConnection;42;0;49;0
WireConnection;42;1;48;0
WireConnection;60;0;49;0
WireConnection;60;1;61;0
WireConnection;13;0;12;0
WireConnection;13;1;11;2
WireConnection;22;0;21;0
WireConnection;22;1;11;2
WireConnection;58;1;60;0
WireConnection;43;1;42;0
WireConnection;79;0;77;0
WireConnection;79;1;11;2
WireConnection;68;0;43;4
WireConnection;68;1;58;4
WireConnection;2;0;8;0
WireConnection;2;1;13;0
WireConnection;76;0;78;0
WireConnection;76;1;79;0
WireConnection;20;0;23;0
WireConnection;20;1;22;0
WireConnection;67;0;43;0
WireConnection;67;1;58;0
WireConnection;39;0;38;0
WireConnection;39;1;11;2
WireConnection;69;0;68;0
WireConnection;69;1;70;0
WireConnection;74;1;76;0
WireConnection;16;1;2;0
WireConnection;30;1;20;0
WireConnection;28;0;30;0
WireConnection;28;1;29;0
WireConnection;26;0;16;0
WireConnection;26;1;27;0
WireConnection;73;0;74;0
WireConnection;73;1;75;0
WireConnection;46;1;67;0
WireConnection;46;2;69;0
WireConnection;36;0;37;0
WireConnection;36;1;39;0
WireConnection;52;0;26;0
WireConnection;52;1;28;0
WireConnection;35;1;36;0
WireConnection;71;0;73;0
WireConnection;81;0;46;0
WireConnection;81;1;80;0
WireConnection;51;0;4;0
WireConnection;51;1;5;0
WireConnection;51;2;52;0
WireConnection;84;0;35;0
WireConnection;84;1;85;0
WireConnection;50;1;81;0
WireConnection;50;2;71;0
WireConnection;25;0;51;0
WireConnection;25;1;50;0
WireConnection;56;0;55;0
WireConnection;56;1;55;0
WireConnection;56;2;6;1
WireConnection;83;0;82;0
WireConnection;83;1;84;0
WireConnection;57;0;56;0
WireConnection;57;1;54;0
WireConnection;57;2;6;2
WireConnection;40;0;25;0
WireConnection;40;1;83;0
WireConnection;40;2;6;1
WireConnection;0;2;40;0
WireConnection;0;9;57;0
ASEEND*/
//CHKSM=5E2FF8E3F0E1375577D6F13CF09261E1BE5750D3