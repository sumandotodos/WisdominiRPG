// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/BadTVShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Lighting Off

		Pass
		{
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
			float4 _MainTex_ST;

			fixed pseudoRandom(float inTime) {

				return (cos(inTime*76.112)*sin(inTime*13.11)*sin(inTime*61.12) + cos(inTime*55.0)*sin(inTime*7.0));

			}
			
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
				i.uv.x += pseudoRandom(i.uv.y + _Time.z) * 0.002;
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed noise = pseudoRandom(i.uv.y*10.0f * i.uv.x*10.0f + _Time.z*20.0f) * 0.04;
				fixed4 noiseCol = (noise, noise, noise, noise);
				col += noiseCol;
				fixed f = 0.9 + (1.0 + sin(i.uv.y*640.0))/20.0;
				return col * f;
			}
			ENDCG
		}
	}
}
