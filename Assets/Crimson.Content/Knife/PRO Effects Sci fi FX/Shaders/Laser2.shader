Shader "Knife/Laser2"
{
	Properties
	{
		_Noise("Noise", 2D) = "white" {}
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_AlphaMin("Alpha Min", Range( 0 , 1)) = 0
		_AlphaMax("Alpha Max", Range( 0 , 1)) = 1
		_NoiseAdd("Noise Add", Range( 0 , 1)) = 1
		_Scale("Scale", float) = 5

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

			

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 ase_color : COLOR;
				float4 ase_texcoord1 : TEXCOORD1;
			};

			uniform sampler2D _Noise;
			uniform float4 _Noise_ST;

			uniform float4 _Color;
			uniform float _AlphaMin;
			uniform float _AlphaMax;
			uniform float _NoiseAdd;
			uniform float _Scale;

			
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
				float sin1 = sin(-_Scale * uv01.x + _Time.w * 2);
				float noise = tex2D(_Noise, float2(i.ase_texcoord1.x * 5 * _Scale - _Time.w * 2, .5)).r * _AlphaMax;
				alpha = lerp(alpha, noise, _NoiseAdd);
				float4 appendResult9 = (float4(break8.r , break8.g , break8.b , alpha));
				
				
				finalColor = appendResult9;
				return finalColor;
			}
			ENDCG
		}
	}
	
	
}
