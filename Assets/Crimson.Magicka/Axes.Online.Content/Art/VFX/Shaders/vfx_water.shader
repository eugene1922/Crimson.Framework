Shader "VFX/Water/Water"
{
    Properties 
    {	[Header(Water Texture)]
        [NoScaleOffset] _MainTex ("Water Texture", 2D) = "White" {}
        [NoScaleOffset] _NoiseTex ("Water Color", 2D) = "White" {}
        _TilingFirst ("Tiling Fisrt", Float) = 2.5
        _TilingSecond ("Tiling Second", Float) = 1.5
	_TilingFoam ("Tiling Foam", Float) = 1
	_WaveHeight ("Wave Height", Range(0, 1)) = 0.175
	[Header(Waves)]
	_SpeedWater ("Speed Water", Float) = 1
	_SpeedWaves ("Speed Waves", Float) = 1
	_WavesAmplitude ("Waves Amplitude", Float) = 0.25
	_WavesFrequency ("Waves Frequency", Range(0, 1)) = 0.5
	_WavesNormalInclusion ("Waves Normal Inclusion", Float) = 0.5
	_WavesDepthColorInclusion ("Waves Depth Color Inclusion", Range(0, 1)) = 0.25
	[Header(Color)]
        _WaterColor ("Water Color", Color) =          (0, 0.4,  1,    1)
        _DeepWaterColor ("Water Color Deep", Color) = (0, 0.3,  0.45, 1)
        _FresnelColor ("Fresnel Color", Color) =      (0, 0.35, 0.55, 1)
        _FresnelPower ("Fresnel Exponent", Range(0.01, 10)) = 0.3    
	[Header(Depth)]
	_AbsorptionDepth ("Depth", Float) = 0.085
	_AbsorptionStrength ("Strength", Float) = 0.584
	[Header(PBR)]
        [NoScaleOffset] _Reflection ("Reflection Texture", 2D) = "White" {}
        _SpecColor ("Specular color", Color) = (1, 1, 1, 1)
        _SpecPower ("Specular", Range(0.01, 10)) = 0.078125
        _Gloss("Gloss", Range(0, 2)) = 0.078125
    }

    SubShader 
    {
        Tags {"IgnoreProjector"="True"
	      "Queue"="Transparent+2"
	      "RenderType"="Transparent"}
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag            
            #include "UnityCG.cginc"
	    #include "vfx.cginc"
            
            fixed _TilingFirst;
            fixed _TilingSecond;
	    float _TilingFoam;
            fixed _WaveHeight;
            sampler2D _CameraDepthTexture;
            sampler2D _Reflection;
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _NormalFoam_ST;
            fixed4 _WaterColor;
            fixed4 _DeepWaterColor;
            fixed4 _SpecColor;
            fixed _SpecPower;
            fixed _Gloss;
            fixed4 _FresnelColor;
            fixed _FresnelPower;
	    half _SpeedWater;
	    half _SpeedWaves;
	    half _WavesAmplitude;
	    half _WavesFrequency;
	    half _WavesNormalInclusion;
	    half _WavesDepthColorInclusion;
	    half _AbsorptionDepth;
	    half _AbsorptionStrength;
            
            struct appdata
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal: NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f 
            {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float3 lightDir : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float4 projPos : TEXCOORD3;
                float4 uv1 : TEXCOORD4;
                float3 normalOffset : TEXCOORD5;
                float3 worldPos : TEXCOORD6;
            };	    
	    

            v2f vert (appdata v) 
            {
                v2f o;

		half3 vertexoffset;
                o.worldPos.xyz = mul(unity_ObjectToWorld, v.pos).xyz;
		gerstner(vertexoffset, o.normalOffset.xyz, _WavesAmplitude, _WavesFrequency, _SpeedWaves, half4(o.worldPos.xyz, 1.0));
		o.normalOffset.xz = o.normalOffset.xz * _WavesNormalInclusion;
		v.pos.xyz = v.pos.xyz + vertexoffset;

                o.pos = UnityObjectToClipPos(v.pos);

		float flow = _Time.y * 0.01 * _SpeedWater;
		o.uv0.xy = v.uv.xy * _TilingFirst + flow;
		o.uv0.zw = v.uv.xy * _TilingSecond - flow;		

                fixed3 worldNormal = UnityObjectToWorldNormal(v.normal); 
                fixed3 worldTangent = normalize(mul((fixed3x3)unity_ObjectToWorld, v.tangent.xyz));
                fixed3 bitangent = normalize(cross(worldNormal, worldTangent) * v.tangent.w);
                
		worldNormal.xz = worldNormal.xz + o.normalOffset.xz;

                fixed3 lightDir = normalize(UnityWorldSpaceLightDir(o.worldPos.xyz));
                o.lightDir.x = dot(lightDir, worldTangent);
                o.lightDir.y = dot(lightDir, bitangent);
                o.lightDir.z = dot(lightDir, worldNormal);
                o.lightDir.xyz = normalize(o.lightDir.xyz);
                
                fixed3 viewDir = normalize(UnityWorldSpaceViewDir(o.worldPos.xyz));
                o.viewDir.x = dot(viewDir, worldTangent);
                o.viewDir.y = dot(viewDir, bitangent);
                o.viewDir.z = dot(viewDir, worldNormal);
                o.viewDir.xyz = normalize(o.viewDir.xyz);
                
                o.projPos = ComputeScreenPos(o.pos);
		o.uv1.zw = v.uv.xy * _TilingFoam + _Time.y * 0.1 * _SpeedWater;
                
                return o;
            }

            half4 frag(v2f i) : COLOR
            {           
                half4 texup = tex2D(_MainTex, i.uv0.xy);
                half4 texdown = tex2D(_MainTex, i.uv0.zw);
                texup.rgb = texup.rgb + texdown.rgb + i.normalOffset.xyz * 0.7;
		texdown = tex2D(_NoiseTex, i.uv1.zw + i.normalOffset.xz * _WavesDepthColorInclusion);
                
                half3 normal = normalize(2 * (texup.rgb - 1));
                normal = lerp(fixed3(0,0,1), normal, _WaveHeight);
                
                fixed3 halfDir = normalize(i.lightDir.xyz + i.viewDir.xyz);

                fixed diff = max(0, dot(normal, i.lightDir.xyz));
                fixed spec = pow(max(0, dot(normal, halfDir)), _SpecPower) * _Gloss;
                fixed fresnel = pow(1 - max(0, dot(normal, i.viewDir.xyz)), _FresnelPower);

                fixed3 reflDir = dot(normal, i.viewDir.xyz) * normal * 2 - i.viewDir.xyz;
                fixed2 uvRefl = fixed2(dot(reflDir, fixed3(1, 0, 0)), dot(reflDir, fixed3(0, 1, 0)));
                uvRefl = uvRefl * 0.5 + 0.5;
                fixed4 reflection = tex2D(_Reflection, uvRefl);
		
		//=== DEPTH ===
                float sceneDepth = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)).r;
		sceneDepth = 1.0 / (_ZBufferParams.z * sceneDepth + _ZBufferParams.w);
		float viewDepth = sceneDepth - i.projPos.w;
		
		//=== FOG ===
		float4 wpos = saturate(float4(_ProjectionParams.zzz * sceneDepth, 1));
		wpos.xyz = mul(unity_CameraToWorld, wpos).xyz;
		float attenuation = saturate((i.worldPos.y - wpos.y) * 0.0625);
		attenuation = saturate(1.0 - exp(-attenuation * 8.0));
        	float depthBelowSurface = saturate(i.worldPos.y - wpos.y);
        	float depth = exp2(-viewDepth * _AbsorptionDepth);
       		depth = lerp(1, saturate(depth), _AbsorptionStrength);
        	depth = saturate(depth + attenuation * 0.5);

		texup.rgb = lerp(lerp(_DeepWaterColor, _WaterColor, texdown.r), _WaterColor, depth);
		texup.rgb = texup.rgb * diff + _SpecColor.rgb * spec + fresnel * (reflection + _FresnelColor.rgb);
		texup.a = 1 - depth;

                return texup;
            }
            ENDCG
        }
    }
}
