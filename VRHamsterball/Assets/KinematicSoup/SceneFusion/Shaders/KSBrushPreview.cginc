// Unity built-in shader source. Copyright (c) 2016 Unity Technologies.

//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//of the Software, and to permit persons to whom the Software is furnished to do
//so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#ifndef TERRAIN_PREVIEW_INCLUDED
#define TERRAIN_PREVIEW_INCLUDED


// function to convert paint context pixels to heightmap uv
sampler2D _Heightmap;
float2 _HeightmapUV_PCPixelsX;
float2 _HeightmapUV_PCPixelsY;
float2 _HeightmapUV_Offset;
float2 PaintContextPixelsToHeightmapUV(float2 pcPixels)
{
    return _HeightmapUV_PCPixelsX * pcPixels.x +
        _HeightmapUV_PCPixelsY * pcPixels.y +
        _HeightmapUV_Offset;
}

// function to convert paint context pixels to object position (terrain position)
float3 _ObjectPos_PCPixelsX;
float3 _ObjectPos_PCPixelsY;
float3 _ObjectPos_HeightMapSample;
float3 _ObjectPos_Offset;
float3 PaintContextPixelsToObjectPosition(float2 pcPixels, float heightmapSample)
{
    // note: we could assume no object space rotation and make this dramatically simpler
    return _ObjectPos_PCPixelsX * pcPixels.x +
        _ObjectPos_PCPixelsY * pcPixels.y +
        _ObjectPos_HeightMapSample * heightmapSample +
        _ObjectPos_Offset;
}

// function to convert paint context pixels to brush uv
float2 _BrushUV_PCPixelsX;
float2 _BrushUV_PCPixelsY;
float2 _BrushUV_Offset;
float2 PaintContextPixelsToBrushUV(float2 pcPixels)
{
    return _BrushUV_PCPixelsX * pcPixels.x +
        _BrushUV_PCPixelsY * pcPixels.y +
        _BrushUV_Offset;
}

// function to convert terrain object position to world position
// We would normally use the ObjectToWorld / ObjectToClip calls to do this, but DrawProcedural does not set them
// 'luckily' terrains cannot be rotated or scaled, so this transform is very simple
float3 _TerrainObjectToWorldOffset;
float3 TerrainObjectToWorldPosition(float3 objectPosition)
{
    return objectPosition + _TerrainObjectToWorldOffset;
}

// function to build a procedural quad mesh
// based on the quad resolution defined by _QuadRez
// returns integer positions, starting with (0, 0), and ending with (_QuadRez.xy - 1)
float3 _QuadRez;    // quads X, quads Y, vertexCount
float2 BuildProceduralQuadMeshVertex(uint vertexID)
{
    int quadIndex = vertexID / 6;                       // quad index, each quad is made of 6 vertices
    int vertIndex = vertexID - quadIndex * 6;           // vertex index within the quad [0..5]
    int qY = floor((quadIndex + 0.5f) / _QuadRez.x);    // quad coords for current quad (Y)
    int qX = round(quadIndex - qY * _QuadRez.x);        // quad coords for current quad (X)

    // each quad is defined by 6 vertices (two triangles), offset from (qX,qY) as follows:
    // vX = 0, 0, 1, 1, 1, 0
    // vY = 0, 1, 1, 1, 0, 0
    float sequence[6] = { 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 0.0f };
    float vX = sequence[vertIndex];
    float vY = sequence[5 - vertIndex];     // vY is just vX reversed
    float2 coord = float2(qX + vX, qY + vY);
    return coord;
}


float Stripe(in float x, in float stripeX, in float pixelWidth)
{
    // compute derivatives to get ddx / pixel
    float2 derivatives = float2(ddx(x), ddy(x));
    float derivLen = length(derivatives);
    float sharpen = 1.0f / max(derivLen, 0.00001f);
    return saturate(0.5f + 0.5f * (0.5f * pixelWidth - sharpen * abs(x - stripeX)));
}

#endif

#define API_HAS_GUARANTEED_R16_SUPPORT !(SHADER_API_VULKAN || SHADER_API_GLES || SHADER_API_GLES3)

float4 PackHeightmap(float height)
{
#if (API_HAS_GUARANTEED_R16_SUPPORT)
    return height;
#else
    uint a = (uint)(65535.0f * height);
    return float4((a >> 0) & 0xFF, (a >> 8) & 0xFF, 0, 0) / 255.0f;
#endif
}

float UnpackHeightmap(float4 height)
{
#if (API_HAS_GUARANTEED_R16_SUPPORT)
    return height.r;
#else
    return (height.r + height.g * 256.0f) / 257.0f; // (255.0f * height.r + 255.0f * 256.0f * height.g) / 65535.0f
#endif
}
#undef API_HAS_GUARANTEED_R16_SUPPORT

// Tranforms position from world to homogenous space
inline float4 UnityWorldToClipPos(in float3 pos)
{
    return mul(UNITY_MATRIX_VP, float4(pos, 1.0));
}

// Computes world space light direction, from world space position
inline float3 UnityWorldSpaceLightDir(in float3 worldPos)
{
#ifndef USING_LIGHT_MULTI_COMPILE
    return _WorldSpaceLightPos0.xyz - worldPos * _WorldSpaceLightPos0.w;
#else
#ifndef USING_DIRECTIONAL_LIGHT
    return _WorldSpaceLightPos0.xyz - worldPos;
#else
    return _WorldSpaceLightPos0.xyz;
#endif
#endif
}