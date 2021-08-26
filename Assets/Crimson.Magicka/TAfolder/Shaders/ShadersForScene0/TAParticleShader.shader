// Upgrade NOTE: upgraded instancing buffer 'TAshadersTAParticleShader' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TAshaders/TAParticleShader"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_MainColor("MainColor", Color) = (1,1,1,0)
		_OpacityIntensity("OpacityIntensity", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 2.0
		#pragma multi_compile_instancing
		#pragma surface surf Lambert alpha:fade keepalpha noshadow noambient nolightmap  nodynlightmap nodirlightmap 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTexture;
		SamplerState sampler_MainTexture;

		UNITY_INSTANCING_BUFFER_START(TAshadersTAParticleShader)
			UNITY_DEFINE_INSTANCED_PROP(float4, _MainColor)
#define _MainColor_arr TAshadersTAParticleShader
			UNITY_DEFINE_INSTANCED_PROP(float4, _MainTexture_ST)
#define _MainTexture_ST_arr TAshadersTAParticleShader
			UNITY_DEFINE_INSTANCED_PROP(float, _OpacityIntensity)
#define _OpacityIntensity_arr TAshadersTAParticleShader
		UNITY_INSTANCING_BUFFER_END(TAshadersTAParticleShader)

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 _MainColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainColor_arr, _MainColor);
			float4 _MainTexture_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainTexture_ST_arr, _MainTexture_ST);
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST_Instance.xy + _MainTexture_ST_Instance.zw;
			float4 tex2DNode3 = tex2D( _MainTexture, uv_MainTexture );
			float4 temp_output_4_0 = ( _MainColor_Instance * tex2DNode3 );
			float _OpacityIntensity_Instance = UNITY_ACCESS_INSTANCED_PROP(_OpacityIntensity_arr, _OpacityIntensity);
			float temp_output_43_0 = ( tex2DNode3.a * _OpacityIntensity_Instance );
			clip( temp_output_43_0 - 0.5);
			float4 temp_output_41_0 = temp_output_4_0;
			o.Albedo = temp_output_41_0.rgb;
			o.Emission = temp_output_4_0.rgb;
			o.Alpha = temp_output_43_0;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18500
2048;167.2;2048;922;1655.82;488.4753;1.034247;True;True
Node;AmplifyShaderEditor.ColorNode;5;-1137.695,-330.657;Inherit;False;InstancedProperty;_MainColor;MainColor;1;0;Create;True;0;0;False;0;False;1,1,1,0;1,0.9386792,0.9386792,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-1320.003,-97.46655;Inherit;True;Property;_MainTexture;MainTexture;0;0;Create;True;0;0;False;0;False;-1;None;df4600a174caf58489f62ec900cd5df5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;44;-1129.388,177.5801;Inherit;False;InstancedProperty;_OpacityIntensity;OpacityIntensity;2;0;Create;True;0;0;False;0;False;1;0.59;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-854.835,-201.0683;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;21;-939.8181,-74.95509;Inherit;False;Constant;_Al;Al;3;0;Create;True;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-849.1064,89.66911;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;29;-1768.625,-73.78934;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0.62,0.12;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClipNode;41;-540.9001,-304.379;Inherit;False;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;39;-173.7193,-66.1918;Float;False;True;-1;0;ASEMaterialInspector;0;0;Lambert;TAshaders/TAParticleShader;False;False;False;False;True;False;True;True;True;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;False;0;True;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;4;0;5;0
WireConnection;4;1;3;0
WireConnection;43;0;3;4
WireConnection;43;1;44;0
WireConnection;41;0;4;0
WireConnection;41;1;43;0
WireConnection;41;2;21;0
WireConnection;39;0;41;0
WireConnection;39;2;4;0
WireConnection;39;9;43;0
WireConnection;39;10;41;0
ASEEND*/
//CHKSM=9CF82903284E45EE1153722DC22578E8402DDF96