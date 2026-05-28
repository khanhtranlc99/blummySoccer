Shader "custom/builtin/StencilMask" {
    Properties {
        _StencilID ("Stencil ID", Range(0, 255)) = 0
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _AlphaCutoff ("Alpha Cutoff", Range(0.01, 1.0)) = 0.1
    }

    SubShader {
      Tags { "Queue" = "Geometry-1" "RenderType" = "Opaque" }


        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass {
            // Cấu hình Stencil
            Stencil {
                Ref [_StencilID]
                Comp Always
                Pass Replace
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _AlphaCutoff;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw; // Tính UV thủ công
                return o;
            }

            float4 frag(v2f i) : SV_Target {
                float4 col = tex2D(_MainTex, i.uv) * _Color;

                // Alpha cutoff
                clip(col.a - _AlphaCutoff);
                return col;
            }

            ENDCG
        }
    }

    Fallback "Diffuse"
}
