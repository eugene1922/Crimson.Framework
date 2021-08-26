// Upgrade NOTE: replaced 'defined WATER_REFLECTIVE' with 'defined (WATER_REFLECTIVE)'

// Custom Water v0.2

Shader "Custom/Water_Bay" {
    Properties {
        _Color ("Color", Color) = (0.7016652,0.8427501,0.9264706,1)
        _ReflDistort ("Reflection distort", Range (0,15)) = 0.44
        _Normal_1 ("Normal_1", 2D) = "bump" {}
        _Normal_2 ("Normal_2", 2D) = "bump" {}
        _CubMap ("CubMap", Cube) = "_Skybox" {}
        _Intensity_Waves ("Intensity_Waves", Range(0, 50)) = 10
         _SpeedWaves ("SpeedWaves", Range(0, 2)) = 1
        _Intensity_Color ("Intensity_Color", Range(0, 10)) = 5
        _Spec ("Spec", Range(0, 2)) = 1
        _Gloss ("Gloss", Range(0, 2)) = 1
        _Reflection ("Reflection", Range(0, 2)) = 0
		_ReflectionLevel ("ReflectionLevel", Range(0, 1)) = 1
        _ReflectionTex ("Internal Reflection", 2D) = "" {}
        [NoScaleOffset] _ReflectiveColor ("Reflective color (RGB) fresnel (A) ", 2D) = "" {}
    }







    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase" "WaterMode"="Reflective"
            }
                       CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
          #pragma multi_compile_fog 
            uniform float4 _LightColor0;
            uniform float4 _Color;
            uniform float _Spec;
            uniform float _Gloss;
            uniform sampler2D _Normal_1; uniform float4 _Normal_1_ST;
            uniform float _Intensity_Waves;
            uniform sampler2D _Normal_2; uniform float4 _Normal_2_ST;
            uniform float _Intensity_Color;
            uniform float _Speed;
            uniform float _SpeedWaves;
            uniform samplerCUBE _CubMap;
            uniform float _Reflection;
			uniform float _ReflectionLevel;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                float4 viewReflection : TEXCOORD5;
                float3 viewDir : TEXCOORD6;
                LIGHTING_COORDS(7,8)
                UNITY_FOG_COORDS(9)
            };
            


            

uniform float _ReflDistort;

            
 
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0.xy = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                o.viewReflection = ComputeNonStereoScreenPos(o.pos);
                o.viewDir.xzy = WorldSpaceViewDir(v.vertex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }

sampler2D _ReflectionTex;
sampler2D _ReflectiveColor;


int binpow(int a, int n) {
	int res = 1;
	while (n) {
		if (n & 1)
			res *= a;
		a *= a;
		n >>= 1;
	}
	return res;
}

            float4 frag(VertexOutput i) : COLOR {
              
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				

                float2 node_4184 = (i.uv0+_Time*float2(0.0002,0.0001) * _SpeedWaves);
				float2 node_5576 = (i.uv0+_Time*float2(0.0002,-0.0001)*_SpeedWaves);

				float3 _Normal_1_var = UnpackNormal(tex2D(_Normal_1, TRANSFORM_TEX(node_4184, _Normal_1)));
                float3 _Normal_2_var = UnpackNormal(tex2D(_Normal_2,TRANSFORM_TEX(node_5576, _Normal_2)));

                float3 normalLocal = (_Normal_1_var.rgb+_Normal_2_var.rgb);
				float3 normalDirection = normalize(mul(normalLocal, tangentTransform)); // Perturbed normals
				float3 viewReflectDirection = reflect(-viewDirection, normalDirection);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
////// Specular:
				float3 directSpecular = attenColor *binpow(max(0, dot(halfDirection, normalDirection)), 80)* float3(_Spec, _Spec, _Spec);
/////// Diffuse:
				float NdotL = max(0.0, dot(normalDirection, lightDirection));
                float3 diffuseColor = _Color.rgb;
                diffuseColor *= 1- _Spec;
                float3 diffuse = (NdotL * attenColor + UNITY_LIGHTMODEL_AMBIENT.rgb) * diffuseColor;
////// Emissive:
                float4 _CubMap_var = texCUBE(_CubMap,viewReflectDirection);
				float3 emissive = _CubMap_var.rgb * _Reflection;
// /// Final Color:
				float3 finalColor = diffuse + directSpecular + emissive;


// Reflective 
            i.viewDir = normalize(i.viewDir);
            float3 bump = (_Normal_1_var + _Normal_2_var) * 0.5;
        	float4 uv1 = i.viewReflection; 
            uv1.xy += bump * _ReflDistort;
            float fresnelFac = dot( i.viewDir, bump );
	        float4 water = tex2D( _ReflectiveColor, float2(fresnelFac,fresnelFac) );
	        float4 refl = tex2Dproj( _ReflectionTex, UNITY_PROJ_COORD(uv1) );
            finalColor.rgb = lerp(finalColor.rgb, refl.rgb, (water.a * _ReflectionLevel));
            float4 finalRGBA = float4(finalColor,refl.a);
            UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
            return finalRGBA;
            }
            ENDCG
        }


    }
    FallBack "Diffuse"
}
