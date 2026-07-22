Shader "Custom/IrisTransition"
{
    Properties
    {
        _Radius ("Radius", Float) = 1.5
        _Softness ("Edge Softness", Float) = 0.01

        // Required by Unity UI stencil/masking system
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex        : SV_POSITION;
                fixed4 color         : COLOR;
                float2 texcoord      : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float  _Radius;
            float  _Softness;
            float4 _ClipRect;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex        = UnityObjectToClipPos(OUT.worldPosition);
                OUT.texcoord      = v.texcoord;
                OUT.color         = v.color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // Centre UV at origin, correct for aspect ratio -> perfect circle
                float2 uv = IN.texcoord - 0.5;
                uv.x *= _ScreenParams.x / _ScreenParams.y;

                float dist = length(uv);

                // 1 (black) outside the iris circle, 0 (transparent) inside
                float alpha = smoothstep(_Radius - _Softness, _Radius + _Softness, dist);

                // Respect UI clipping (ScrollRect, Mask parents, etc.)
                alpha *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);

                return fixed4(0.0, 0.0, 0.0, alpha * IN.color.a);
            }
            ENDCG
        }
    }
}
