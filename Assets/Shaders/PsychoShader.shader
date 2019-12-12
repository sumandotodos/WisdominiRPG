// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/PsychoShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" 
				"Queue" = "Background" }
		LOD 100

		ZTest LEqual
		ZWrite On



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
				//float4 scrPos : POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//o.scrPos = ComputeScreenPos(o.vertex);
				//o.scrPos = float4(0, 0, 0, 0);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 scrPos = ComputeScreenPos(i.vertex);
				// sample the texture
				float2 newUV = i.uv;
				//newUV.x += 0.03* cos(scrPos.y/_ScreenParams.y * 16.0f + _Time.y);
				//newUV.y += 0.03* sin(scrPos.x/_ScreenParams.x * 16.0f + _Time.y);

				newUV.x += 0.03 * cos(i.uv.y * 8.0f + _Time.y);
				newUV.y += 0.03 * sin(i.uv.x * 8.0f + _Time.y);

				fixed4 texel = tex2D(_MainTex, newUV);
				//fixed4 texel = fixed4(scrPos.y/_ScreenParams.y,0.0,0.0,1.0);
				// apply fog

				return texel;
			}
			ENDCG
		}
	}
}
