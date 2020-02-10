#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
Texture2D lightMask;

int useLight;

sampler2D lightSampler = sampler_state 
{
	Texture = <lightMask>; 
};

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
	if (useLight != 0)
	{
		float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates);
		float4 lightColor = tex2D(lightSampler, input.TextureCoordinates);
		return color * lightColor;
	}
	return tex2D(SpriteTextureSampler, input.TextureCoordinates)* input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};