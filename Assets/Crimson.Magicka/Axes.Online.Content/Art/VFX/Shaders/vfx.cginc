//v 1.3 - 23.09.2020

#define pi 3.14159265359
#define tau 6.28318530718

float2 transform (float2 uv, float4 st) {
	uv = uv * st.xy + st.zw;
return uv;
}

half desaturate (half3 pic) {
	pic.r = dot(pic.rgb, float3(0.3,0.59,0.11));
return pic.r;
}

half fresnel (float3 viewdir, float3 normal, half exp) {
	exp = pow(1.0 - max(0, dot(normal, viewdir)), exp);
return exp;
}

half fresnelinv (float3 viewdir, float3 normal, half exp) {
	exp = pow(max(0, dot(normal, viewdir)), exp);
return exp;
}

void gerstner (out half3 vertexoffset, out half3 normaloffset, half amplitude, half frequency, half speed, half4 vertex) {
	speed = _Time.y * speed;
	half dir = dot(amplitude, vertex.xz) * frequency * tau;

	half2 wave;
	wave.x = dir + speed;
	sincos(wave.x, wave.x, wave.y);

   	vertexoffset.x = dot(wave.y, amplitude);
   	vertexoffset.z = vertexoffset.x;
   	vertexoffset.y = dot(wave.x, amplitude);

	normaloffset = half3(0, 2.0, 0);
	normaloffset.xz = normaloffset.xz - vertexoffset.y;
}