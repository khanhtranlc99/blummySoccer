Shader "Custom/9SlicedMaskedSprite" {
    Properties {
        [PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _StencilRef ("Stencil Reference", Int) = 1
    }

    SubShader {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "RenderType"="Transparent" }
        LOD 100

        Pass {
            Name "Masked"
            Tags { "LightMode"="UniversalForward" }
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha
            Stencil {
                Ref [_StencilRef]
                Comp Equal // Chỉ vẽ khi stencil bằng Ref
                Pass Keep
            }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _Color;

            Varyings vert(Attributes v) {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                return o;
            }

            float4 frag(Varyings i) : SV_TARGET {
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                return texColor * _Color;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}