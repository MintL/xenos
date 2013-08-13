sampler s0;
float2 textureSize;

bool IsMovable(float2 coords)
{
	float3 c = tex2D(s0, coords).rgb;
	return !(c.r > 0.99 && c.g > 0.99 && c.b < 0.01);
}

float4 PixelShaderFunction(float2 coords : TEXCOORD0) : COLOR0
{
	float2 offset = 1 / textureSize;
	
	float4 color = tex2D(s0, coords);

	if (color.r + color.g + color.b < 0.2 && IsMovable(coords)) {
		//return float4(0, 1, 0, 1);
		if (IsMovable(coords + float2(offset.x, 0))) 
			color += 0.2 * tex2D(s0, coords + float2(offset.x, 0));
		if (IsMovable(coords + float2(0, offset.y))) 
			color += 0.2 * tex2D(s0, coords + float2(0, offset.y));
		if (IsMovable(coords + float2(-offset.x, 0))) 
			color += 0.2 * tex2D(s0, coords + float2(-offset.x, 0));
		if (IsMovable(coords + float2(0, -offset.y))) 
			color += 0.2 * tex2D(s0, coords + float2(0, -offset.y));
	}
	else if (!IsMovable(coords)) {
		color = float4(0, 0, 0, 1);
	}
    return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
