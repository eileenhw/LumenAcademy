Shader "Custom/SwirlShader"
{
    

    Properties
    {
        _CaptureTex ("Captured Scene", 2D) = "black" {}
        _SwirlStrength ("Swirl Strength", Float) = 8
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_CaptureTex);
            SAMPLER(sampler_CaptureTex);

            CBUFFER_START(UnityPerMaterial)
                float _SwirlStrength;
                float4 _LensCenter;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;

                output.positionHCS =
                    TransformObjectToHClip(input.positionOS.xyz);

                output.screenPos =
                    ComputeScreenPos(output.positionHCS);

                output.uv = input.uv;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 screenUV =
                    input.screenPos.xy / input.screenPos.w;

                half4 color = SAMPLE_TEXTURE2D(
                    _CaptureTex,
                    sampler_CaptureTex,
                    screenUV
                );

                return color;
            }

 /*
            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;

                // Circular lens mask
                float2 centered = uv - 0.5;
                float radius = length(centered) * 2.0;

                float alpha = 1.0 - smoothstep(0.96, 1.0, radius);

                // Screen coordinates
                float2 screenUV =
                    input.screenPos.xy / input.screenPos.w;

                // Distance from lens center
                float2 delta = screenUV - _LensCenter.xy;

                // Correct for screen aspect ratio
                float aspect = _ScreenParams.x / _ScreenParams.y;
                delta.x *= aspect;

                // Swirl falloff: strongest at center
                float strength =
                    _SwirlStrength *
                    pow(1.0 - saturate(radius), 2.0);

                // Rotate sampling coordinates
                float s = sin(strength);
                float c = cos(strength);

                float2 rotated;

                rotated.x = delta.x * c - delta.y * s;
                rotated.y = delta.x * s + delta.y * c;

                rotated.x /= aspect;

                float2 sampleUV = _LensCenter.xy + rotated;

                // Sample the captured sprites
                half4 color = SAMPLE_TEXTURE2D(
                    _CaptureTex,
                    sampler_CaptureTex,
                    sampleUV
                );

                color.a *= alpha;

                return color; 
            }*/

            ENDHLSL
        }
    }


}
