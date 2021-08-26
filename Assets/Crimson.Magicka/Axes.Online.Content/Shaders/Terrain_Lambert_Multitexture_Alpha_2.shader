Shader "Custom/Terrain_Lambert_Multitexture_AlphaTest"
{
	Properties
	{
		_Texture_R("Texture_R", 2D) = "white" {}
		_Texture_G("Texture_G", 2D) = "white" {}
		_Texture_B("Texture_B", 2D) = "white" {}
		_Texture_A("Texture_A", 2D) = "white" {}
		_Mask("Mask", 2D) = "gray" {}
		_Details("Details", 2D) = "white" {}
		_Depth_Blend("Depth_Blend", Range(0, 5)) = 0
		[HDR]_Color("Color", Color) = (1,1,1,1)
		_Coastline("Coastline Color", Color) = (1,1,1,1)
		_Water("Water Color", Color) = (1,1,1,1)
	}

		SubShader
	{
		Pass {
		Tags { "RenderType" = "Opaque"  "Queue" = "Transparent" }
			ColorMask 0
		}


		CGPROGRAM
		#pragma surface surf Lambert // noforwardadd alpha:fade
		#pragma vertex vert

		uniform sampler2D _Texture_R; uniform half4 _Texture_R_ST;
		uniform sampler2D _Texture_G; uniform half4 _Texture_G_ST;
		uniform sampler2D _Texture_B; uniform half4 _Texture_B_ST;
		uniform sampler2D _Mask; uniform half4 _Mask_ST;
		uniform sampler2D _Texture_A; uniform half4 _Texture_A_ST;
		uniform sampler2D _Details; uniform half4 _Details_ST;
		uniform half _Depth_Blend;
		fixed4 _Color, _Coastline, _Water;

		struct Input {
			half2 texcoord0;
			half2 texcoord1;
			half4 color : COLOR;
		};

		void vert(inout appdata_full v, out Input o)
		{
			o.texcoord0 = v.texcoord;
			o.texcoord1 = v.texcoord1;
			o.color = v.color;
		}

		void surf(Input i, inout SurfaceOutput o)
		{

			half3 mask = tex2Dlod(_Mask, half4(TRANSFORM_TEX(i.texcoord1, _Mask), 0.0, _Depth_Blend));
			half3 r = tex2D(_Texture_R, TRANSFORM_TEX(i.texcoord0, _Texture_R));
			half3 g = tex2D(_Texture_G, TRANSFORM_TEX(i.texcoord0, _Texture_G));
			half3 b = tex2D(_Texture_B, TRANSFORM_TEX(i.texcoord0, _Texture_B));
			half3 a = tex2D(_Texture_A, TRANSFORM_TEX(i.texcoord0, _Texture_A));
			half3 detail = tex2D(_Details, TRANSFORM_TEX(i.texcoord1, _Details));

			half3 final = ((lerp(lerp(lerp(a, r, mask.r), g, mask.g), b, mask.b)) * detail);

			float coastline = pow(i.color.r, 1.5);
			half3 col = lerp(_Water, _Coastline, coastline);
			o.Albedo = lerp(col, final * _Color, coastline);

			//o.Albedo = final * _Color.rgb;

			o.Alpha = coastline;

		}

		ENDCG
	}

	Fallback "VertexLit"

}