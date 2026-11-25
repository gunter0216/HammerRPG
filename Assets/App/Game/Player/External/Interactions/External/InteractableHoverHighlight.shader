Shader "Autumn/2D/Interactable Hover Highlight"
{
    Properties
    {
        [HideInInspector] _MainTex ("Mask", 2D) = "black" {}
        [HideInInspector] _SpriteOpacity ("Sprite Opacity", Float) = 1
        [HideInInspector] _MaskStep ("Mask Texel Size", Vector) = (1, 1, 0, 0)
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }
        Cull Off
        ZWrite Off

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);

        CBUFFER_START(UnityPerMaterial)
            float4 _MaskStep;
            half _SpriteOpacity;
        CBUFFER_END

        struct Attributes
        {
            float3 positionOS : POSITION;
            float2 uv : TEXCOORD0;
        };

        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float2 uv : TEXCOORD0;
        };

        half MaskAlpha(float2 uv)
        {
            return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).a;
        }
        ENDHLSL

        Pass
        {
            Name "Highlight"
            Tags { "LightMode" = "SRPDefaultUnlit" }
            Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex HighlightVertex
            #pragma fragment HighlightFragment

            Varyings HighlightVertex(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS);
                output.uv = input.uv;
                return output;
            }

            half4 HighlightFragment(Varyings input) : SV_Target
            {
                half alpha = MaskAlpha(input.uv);
                const half fillOpacity = .12h;
                if (alpha == 1)
                    return half4(1, 1, 1, fillOpacity);

                half dilated = alpha;
                [unroll] for (int y = -2; y <= 2; y++)
                {
                    [unroll] for (int x = -2; x <= 2; x++)
                    {
                        if (abs(x) + abs(y) <= 3)
                            dilated = max(dilated, MaskAlpha(input.uv + _MaskStep.xy * float2(x, y)));
                    }
                }
                return half4(1, 1, 1, saturate(dilated - alpha + alpha * fillOpacity));
            }
            ENDHLSL
        }

        Pass
        {
            Name "Sprite Mask"
            Tags { "LightMode" = "HoverMask" }
            Blend One OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex MaskVertex
            #pragma fragment MaskFragment

            Varyings MaskVertex(Attributes input)
            {
                Varyings output;
                // The command buffer supplies a transform from the source sprite to mask clip space.
                output.positionCS = float4(mul(GetObjectToWorldMatrix(), float4(input.positionOS, 1)).xy, 0.5, 1);
                #if UNITY_UV_STARTS_AT_TOP
                    output.positionCS.y = -output.positionCS.y;
                #endif
                output.uv = input.uv;
                return output;
            }

            half4 MaskFragment(Varyings input) : SV_Target
            {
                return (MaskAlpha(input.uv) * _SpriteOpacity).xxxx;
            }
            ENDHLSL
        }
    }
}
