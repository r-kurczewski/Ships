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
    float3 position : TEXCOORD1;
    float3 normals : TEXCOORD2;
};

TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
float4 _Color;
float _Smoothness;

VertexToFragment Vertex (ToVertex IN)
{
    VertexToFragment OUT;

    VertexPositionInputs vertInput = GetVertexPositionInputs(IN.vertex);
    VertexNormalInputs normalInput = GetVertexNormalInputs(IN.normals);

    OUT.vertex = vertInput.positionCS;
    OUT.uv = IN.uv;
    OUT.normals = normalInput.normalWS;
    OUT.position = vertInput.positionWS;
    return OUT;
}

float4 Fragment (VertexToFragment IN) : SV_TARGET
{
    float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv) * _Color;

    InputData lightingInput = (InputData)0;
    lightingInput.normalWS = normalize(IN.normals);
    lightingInput.positionWS = IN.position;
    lightingInput.viewDirectionWS = GetWorldSpaceNormalizeViewDir(IN.position);
    lightingInput.shadowCoord = TransformWorldToShadowCoord(IN.position);
    
    SurfaceData surfaceInput = (SurfaceData)0;
    surfaceInput.albedo = col.rgb;
    surfaceInput.alpha = col.a;
    surfaceInput.specular = 1;
    surfaceInput.smoothness = _Smoothness;

    #if UNITY_VERSION >= 202120
        return UniversalFragmentBlinnPhong(lightingInput, surfaceInput);
    #else
        return UniversalFragmentBlinnPhong(lightingInput, surfaceInput.albedo, float4(surfaceInput.specular, 1), surfaceInput.smoothness, 0, surfaceInput.alpha);
    #endif

}
