Shader "Custom/9SlicedStencilMask" {
    Properties {
        [PerRendererData] _MainTex ("Main Texture", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _StencilRef ("Stencil Reference", Int) = 1
    }

    SubShader {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "RenderType"="Transparent" }
        LOD 100

        Pass {
            Name "Mask"
            Tags { "LightMode"="UniversalForward" }
            ZWrite On
            ColorMask 0 // Không vẽ màu, chỉ vẽ stencil
            Stencil {
                Ref [_StencilRef]
                Comp Always
                Pass Replace
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
                return float4(0, 0, 0, 0); // Không xuất bất kỳ dữ liệu nào
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/InternalErrorShader"
}