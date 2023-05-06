#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderVariablesFunctions.hlsl"

struct ToVertex
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
    float3 normals : NORMAL;
};

struct VertexToFragment
{
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD0;
    DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
    float3 position : TEXCOORD2;
    float3 normals : TEXCOORD3;
};

TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);
TEXTURE2D(_Windwind); SAMPLER(sampler_Windwind);

float4 _Color;
float _Smoothness;
float _Occlusion;
float _Specular;

float _WindScale;
float _WindSpeed;
float _Elasticity;
float2 _WindDirection;

VertexToFragment Vertex (ToVertex IN)
{
    VertexToFragment OUT;
    
    float noise;

    float3 positionWS = GetVertexPositionInputs(IN.vertex).positionWS;
    float2 windInput = positionWS + _Time[1] * _WindSpeed;
    Unity_GradientNoise_float(windInput, _WindScale, noise);

    float2 wind = float2(noise, noise);

    // Move values from [0,1] to [-0.5, 0.5]
    wind -= 0.5f; 

    // Bind offset with vertex height
    wind *= IN.vertex.y * IN.uv.g;

    // Bind wind with wind direction
    wind *= _WindDirection.xy;

    IN.vertex.xz += wind;

    VertexPositionInputs vertInput = GetVertexPositionInputs(IN.vertex.xyz);
    VertexNormalInputs normalInput = GetVertexNormalInputs(IN.normals);

    OUT.vertex = vertInput.positionCS;
    OUT.uv = IN.uv;
    OUT.normals = normalInput.normalWS;
    OUTPUT_SH(OUT.normals.xyz, OUT.vertexSH);
    OUT.position = vertInput.positionWS;
    return OUT;
}

float4 Fragment (VertexToFragment IN) : SV_TARGET
{
    float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _Color;

    InputData lightingInput = (InputData)0;
    lightingInput.normalWS = NormalizeNormalPerPixel(IN.normals);
    lightingInput.positionWS = IN.position;
    lightingInput.viewDirectionWS = GetWorldSpaceNormalizeViewDir(IN.position);
    lightingInput.shadowCoord = TransformWorldToShadowCoord(IN.position);
    lightingInput.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.vertexSH, lightingInput.normalWS);
    
    SurfaceData surfaceInput = (SurfaceData)0;
    surfaceInput.albedo = col.rgb;
    surfaceInput.alpha = col.a;
    surfaceInput.specular = _Specular;
    surfaceInput.smoothness = _Smoothness;
    surfaceInput.occlusion = _Occlusion;

    half4 color = UniversalFragmentPBR(lightingInput, surfaceInput);
    return color;

}
