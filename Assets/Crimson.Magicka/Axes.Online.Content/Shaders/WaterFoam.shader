
Shader "FX/WaterFoam" {
Properties {
	_horizonColor ("Horizon color", COLOR)  = ( .172 , .463 , .435 , 0)
	_WaveScale ("Wave scale", Range (0.02,0.15)) = .07
	_Intersection ("Intersection", Range(0,100)) = 0.1
	[NoScaleOffset] _ColorControl ("Reflective color (RGB) fresnel (A) ", 2D) = "" { }
	[NoScaleOffset] _BumpMap ("Waves Normalmap ", 2D) = "" { }
	[NoScaleOffset] _Foam("Foam", 2D) = "black" {}
	WaveSpeed ("Wave speed (map1 x,y; map2 x,y)", Vector) = (19,9,-16,-7)
	}


Subshader {
	Tags { "RenderType"="Opaque" }
	Pass {

	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma multi_compile_fog

	#include "UnityCG.cginc"

	sampler2D _BumpMap,_CameraDepthTexture;
	sampler2D _ColorControl, _Foam;
	uniform half4 _horizonColor;

	uniform half4 WaveSpeed;
	uniform half _WaveScale, _Intersection;
	uniform half4 _WaveOffset;

	struct appdata {
		half4 vertex : POSITION;
		half3 normal : NORMAL;
	};

	struct v2f {
		half4 pos : SV_POSITION;
		half4 screenPos : TEXCOORD4;
		half2 bumpuv[2] : TEXCOORD0;
		half3 viewDir : TEXCOORD2;
		UNITY_FOG_COORDS(3)
	};

	v2f vert(appdata v)
	{
		v2f o;
		half4 s;

		o.pos = UnityObjectToClipPos(v.vertex);

		// scroll bump waves
		half4 temp;
		half4 wpos = mul(unity_ObjectToWorld, v.vertex);
		temp.xyzw = wpos.xzxz * _WaveScale + _WaveOffset;
		o.bumpuv[0] = temp.xy * half2(.4, .45);
		o.bumpuv[1] = temp.wz;

		// object space view direction
		o.viewDir.xzy = normalize(WorldSpaceViewDir(v.vertex));

		o.screenPos = ComputeScreenPos(o.pos);

		COMPUTE_EYEDEPTH(o.screenPos.z);

		UNITY_TRANSFER_FOG(o, o.pos);
		return o;
	}

	half4 frag( v2f i ) : COLOR
	{
		half3 bump1 = UnpackNormal(tex2D( _BumpMap, i.bumpuv[0] )).rgb;
		half3 bump2 = UnpackNormal(tex2D( _BumpMap, i.bumpuv[1] )).rgb;
		half3 bump = (bump1 + bump2) * 0.5;
	
		half fresnel = dot( i.viewDir, bump );
		half4 water = tex2D( _ColorControl, half2(fresnel,fresnel) );
	
		half4 col;
		col.rgb = lerp( water.rgb, _horizonColor.rgb, water.a );
		col.a = _horizonColor.a;


		half sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
		half partZ = i.screenPos.z;

		half diff = sceneZ - partZ;
		half intersect = (1 - diff) * _Intersection;

		fixed3 foam1 = tex2D(_Foam, i.bumpuv[0]).rgb;
		fixed3 foam2 = tex2D(_Foam, i.bumpuv[1]).rgb;
		fixed3 foam = (foam1 + foam2) * 0.5;

		col.rgb += foam.r * max(0,intersect);

		return col;
	}

	ENDCG

	}

}

}
