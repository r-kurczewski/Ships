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

TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
float4 _Color;
float _Smoothness;
float _Occlusion;
float _Specular;

VertexToFragment Vertex (ToVertex IN)
{
    VertexToFragment OUT;

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

    color.rgb = MixFog(color.rgb, lightingInput.fogCoord);
    color.a = OutputAlpha(color.a, 0);

    return color;

}
