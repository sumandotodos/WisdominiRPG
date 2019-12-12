// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/mirrorShader"
{
    Properties
    {
        [PerRendererData] _MainTex ( "Sprite Texture", 2D ) = "white" {}
        _Color ( "Tint", Color ) = ( 1, 1, 1, 1 )
        [MaterialToggle] PixelSnap ( "Pixel snap", Float ) = 0
        _OccludedColor ( "Occluded Tint", Color ) = ( 0, 0, 0, 0.5 )
    }


CGINCLUDE

// shared structs and vert program used in both the vert and frag programs
struct appdata_t  
{
    float4 vertex   : POSITION;
    float4 color    : COLOR;
    float2 texcoord : TEXCOORD0;
};

struct v2f  
{
    float4 vertex   : SV_POSITION;
    fixed4 color    : COLOR;
    half2 texcoord  : TEXCOORD0;
};


fixed4 _Color;  
sampler2D _MainTex;


v2f vert( appdata_t IN )  
{
    v2f OUT;
    OUT.vertex = UnityObjectToClipPos( IN.vertex );
    OUT.texcoord = IN.texcoord;
    OUT.color = IN.color * _Color;

    return OUT;
}

ENDCG



    SubShader
    {
        Tags
        {
            "Queue" = "Geometry"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }

        Cull Off
        Lighting Off
        ZTest LEqual
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            Stencil
            {
                Ref 4
                Comp Always
                Pass replace

            }


        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"


            fixed4 frag( v2f IN ) : SV_Target
            {
                fixed4 c = fixed4(0.2, 0.3, 0.5, 0.2);
                return c;
            }
        ENDCG
        }

       // Pass
       // {
       //     Stencil
       //     {
       //         Ref 4
       //         Comp Equal
       //     }
       //
       //
       //CGPROGRAM
       //     #pragma vertex vert
       //     #pragma fragment frag
       //     #pragma multi_compile _ PIXELSNAP_ON
       //     #include "UnityCG.cginc"
       //
       //
       //     fixed4 frag( v2f IN ) : SV_Target
       //     {
       //         fixed4 c = tex2D( _MainTex, IN.texcoord ) * IN.color;
       //         c.rgb *= c.a;
       //
       //         return c;
       //     }
       // ENDCG
       // }



    }
}