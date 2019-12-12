// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/AlphaPostMultShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Text Color", Color) = (1, 1, 1, 1)
		_Tint ("Tint", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100

		Lighting Off
		ZWrite Off
		ZTest Always

		//Blend SrcAlpha OneMinusSrcAlpha, DstAlpha Zero



		Pass
		{

			Blend Zero One, DstAlpha Zero
			BlendOp Add, Add

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog





			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			fixed4 _Tint;
			fixed4 _Color;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Tint;
				//if(col.a < 0.8) discard;
				return col;
			}
			ENDCG
		}



		Pass
		{

			Blend DstAlpha OneMinusDstAlpha
			BlendOp Add

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog





			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			fixed4 _Tint;
			fixed4 _Color;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed3 col = tex2D(_MainTex, i.uv).rgb;
				fixed alpha = tex2D(_MainTex, i.uv).a;

				fixed3 inv = (1, 1, 1) - col;
                alpha = alpha * inv.x;

				fixed4 trueCol = (inv.x, inv.y, inv. z, alpha) * _Tint;

				if(alpha < 0.2) discard;
				return trueCol;
			}
			ENDCG
		}


	}
}
