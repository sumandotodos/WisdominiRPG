Shader "Custom/chromaReplaceShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_RedReplace ("Red Replacement", Color) = (1, 0, 0, 1)
		_GreenReplace ("Green Replacement", Color) = (0, 1, 0, 1)
		_BlueReplace ("Blue Replacement", Color) = (0, 0, 1, 1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Transparent" 
			
		 }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _GreenReplace;
		fixed4 _RedReplace;

		int isRedPixel(fixed4 c) {

			fixed r, g, b;
			r = c.r;
			g = c.g;
			b = c.b;

			if((r > g*1.1) && (r > b*1.1)) return 1;

			return 0;

		}

		int isGreenPixel(fixed4 c) {

			fixed r, g, b;
			r = c.r;
			g = c.g;
			b = c.b;

			if((g > r*1.1) && (g > b*1.1)) return 1;

			return 0;

		}

		fixed4 luma(fixed4 c) {

			fixed l = (c.r + c.g + c.b)/3.0;

			return fixed4(l, l, l, 1);

		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;

			if(isGreenPixel(c)) {
				o.Albedo = fixed4(1, 1, 1, 1) * luma(c) * _GreenReplace;
			}
			else {
				o.Albedo = c.rgb;
			}

			if(isRedPixel(c)) {
				o.Albedo = fixed4(1, 1, 1, 1) * luma(c) * _RedReplace;
			}
			else {
				o.Albedo = c.rgb;
			}

			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
