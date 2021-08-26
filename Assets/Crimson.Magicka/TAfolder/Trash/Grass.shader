// Upgrade NOTE: upgraded instancing buffer 'TAshadersGrass' to new syntax.

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TAshaders/Grass"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_MainColor("MainColor", Color) = (0,0,0,0)
		_AlphaCutOff("AlphaCutOff", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="TransparentCutout" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend One Zero , SrcAlpha OneMinusSrcAlpha
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
			};

			uniform sampler2D _MainTexture;
			UNITY_INSTANCING_BUFFER_START(TAshadersGrass)
				UNITY_DEFINE_INSTANCED_PROP(half4, _MainTexture_ST)
#define _MainTexture_ST_arr TAshadersGrass
				UNITY_DEFINE_INSTANCED_PROP(half4, _MainColor)
#define _MainColor_arr TAshadersGrass
				UNITY_DEFINE_INSTANCED_PROP(half, _AlphaCutOff)
#define _AlphaCutOff_arr TAshadersGrass
			UNITY_INSTANCING_BUFFER_END(TAshadersGrass)

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				half4 _MainTexture_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainTexture_ST_arr, _MainTexture_ST);
				float2 uv_MainTexture = i.ase_texcoord.xy * _MainTexture_ST_Instance.xy + _MainTexture_ST_Instance.zw;
				half4 tex2DNode1 = tex2D( _MainTexture, uv_MainTexture );
				half4 _MainColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainColor_arr, _MainColor);
				half _AlphaCutOff_Instance = UNITY_ACCESS_INSTANCED_PROP(_AlphaCutOff_arr, _AlphaCutOff);
				clip( tex2DNode1.a - _AlphaCutOff_Instance);
				
				
				finalColor = ( tex2DNode1 * _MainColor_Instance );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=17700
1536;66.4;1536;699;1339.578;491.1987;1.3;True;True
Node;AmplifyShaderEditor.ColorNode;15;-879.3784,-12.79873;Inherit;False;InstancedProperty;_MainColor;MainColor;1;0;Create;True;0;0;False;0;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-974.9323,-278.8421;Inherit;True;Property;_MainTexture;MainTexture;0;0;Create;True;0;0;False;0;-1;None;75f9d245ed7b5f04081fc05479605f5a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-705.3285,337.6754;Half;False;InstancedProperty;_AlphaCutOff;AlphaCutOff;2;0;Create;True;0;0;False;0;0;0.769;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-514.1323,-166.2421;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClipNode;14;-319.5511,-7.355139;Inherit;False;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;13;18.2,-19.5;Half;False;True;-1;2;ASEMaterialInspector;100;1;TAshaders/Grass;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;1;1;False;-1;0;False;-1;2;5;False;-1;10;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=TransparentCutout=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;0
WireConnection;5;0;1;0
WireConnection;5;1;15;0
WireConnection;14;0;5;0
WireConnection;14;1;1;4
WireConnection;14;2;8;0
WireConnection;13;0;14;0
ASEEND*/
//CHKSM=46AE09078DA50E49209DE3906E5C8D854CCC1A74