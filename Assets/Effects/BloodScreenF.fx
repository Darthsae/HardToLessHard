sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

// This is a shader. You are on your own with shaders. Compile shaders in an XNB project.

const float maxProgress = 400.0f;
float4 BloodScreen(float2 coords : TEXCOORD0) : COLOR0
{
    //Red Tint
    float4 color = tex2D(uImage0, coords);
    
    float4 tempcolor = color;

    tempcolor.r = (color.r + color.g + color.b) / 3;
    tempcolor.g = 0;
    tempcolor.b = 0;

    color = tempcolor;

	return color;
}

technique Technique1
{
    pass BloodScreen
    {
        PixelShader = compile ps_2_0 BloodScreen();
    }
}