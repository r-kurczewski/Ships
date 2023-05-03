#include "Packages\com.unity.render-pipelines.core\ShaderLibrary\Common.hlsl"
#include "Packages\com.unity.render-pipelines.core\ShaderLibrary\CommonMaterial.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary\Shadows.hlsl"

struct ToVertex
{
    float3 positionOS : POSITION;
    float3 normalOS : NORMAL;
};

struct VertexToFragment
{
    float4 positionCS : SV_POSITION;
};

float3 _LightDirection;

float4 GetShadowCasterPositionCS(float3 positionWS, float3 normalWS)
{
    float3 lightDirectionWS = _LightDirection;
    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

    #if UNITY_REVERSED_Z
        positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
    #else
        positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
    #endif

    return positionCS;
}

VertexToFragment Vertex(ToVertex IN)
{
    VertexToFragment OUT;

    VertexPositionInputs posnInputs = GetVertexPositionInputs(IN.positionOS);
    VertexNormalInputs normInputs = GetVertexNormalInputs(IN.normalOS);

    OUT.positionCS = GetShadowCasterPositionCS(posnInputs.positionWS, normInputs.normalWS);

    return OUT;
}

float4 Fragment(VertexToFragment IN) : SV_TARGET
{
    return 0;
}

