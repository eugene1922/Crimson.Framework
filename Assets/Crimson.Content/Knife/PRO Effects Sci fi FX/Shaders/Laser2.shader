Shader "Knife/Laser2"
{
	Properties
	{
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_AlphaMin("Alpha Min", Range( 0 , 1)) = 0
		_AlphaMax("Alpha Max", Range( 0 , 1)) = 1
		_AlphaAdd("Alpha Add", Range( 0 , 2)) = 1
		_AddTreshold("Add Treshold", Range( 0 , 0.999)) = 0.95

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back
		ColorMask RGBA
		ZWrite Off
		ZTest LEqual
		
		
		
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
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
			};

			uniform float4 _Color;
			uniform float _AlphaMin;
			uniform float _AlphaMax;
			uniform float _AlphaAdd;
			uniform float _AddTreshold;

			
			v2f vert ( appdata v )
			{
				v2f o;

				o.ase_color = v.color;
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				o.ase_texcoord1.zw = 0;
				o.vertex = UnityObjectToClipPos(v.vertex);

				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				fixed4 finalColor;
				float4 break8 = ( i.ase_color * _Color );
				float2 uv01 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float alpha = lerp( _AlphaMax , _AlphaMin, uv01.x);
				float sin1 = sin(-5 * uv01.x + _Time.w * 2);
				float mul = (clamp(sin1 * sin1, _AddTreshold, 1) - _AddTreshold) / (1 - _AddTreshold);
				mul = 1 + _AlphaAdd * mul;
				alpha *= mul ;
				float4 appendResult9 = (float4(break8.r , break8.g , break8.b , alpha));
				
				
				finalColor = appendResult9;
				return finalColor;
			}
			ENDCG
		}
	}
	
	
}
