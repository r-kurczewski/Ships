Shader "Custom/GrassShader"
{
    Properties
    {
        [MainColor] _Color ("Color", Color) = (1,1,1,1)
        [MainTexture] _MainTex ("Texture", 2D) = "white" {}
        _WindNoise ("Texture", 2D) = "white" {}
        _Smoothness("Smoothness", Range(0,1)) = 0
        _Occlusion("Occlusion", Range(0,1)) = 1
        _Specular("Specular", Range(0,1)) = 1
        _WindScale("Wind Scale", float) = 1
        _WindSpeed("Wind Speed", float) = 1
        _WindDirection("Wind Direction", Vector) = (1,1.0,0)
    }
    SubShader
    {
        Tags 
        {
            "RenderType"="Opaque"
            "RenderPipeline" = "UniversalPipeline" 
        }

        Pass
        {
            HLSLPROGRAM

            #pragma target 3.0
            // #pragma multi_compile_instancing
            // #pragma multi_compile _ DOTS_INSTANCING_ON
            #if UNITY_VERSION >= 202120
                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #else
                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #endif
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            #pragma multi_compile _ SHADOWS_SHADOWMASK

            #pragma vertex Vertex
            #pragma fragment Fragment

            #define SPECULAR_COLOR

            #include "../GradientNoise.hlsl"
            #include "GrassShaderPass.hlsl"


            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags
            {
                "LightMode" = "ShadowCaster"    
            }

            ColorMask 0

            HLSLPROGRAM

            #pragma vertex Vertex
            #pragma fragment Fragment

            #include "GrassShaderShadowCasterPass.hlsl"

            ENDHLSL
        }
    }
}
