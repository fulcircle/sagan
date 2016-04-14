Shader "Sagan/Tessellation" {

	Properties {
		_MainTex("Texture", 2D) = "white" {}
		_Tess("Tessellation", Range(1,32)) = 4
		_DispTex("Disp Texture", 2D) = "gray" {}
		_Displacement("Displacement", Range(0, 1.0)) = 0.3
	}

	SubShader {
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert vertex:disp tessellate:tessFixed nolightmap
		#pragma target 5.0

		sampler2D _DispTex;
		float _Displacement;

		struct appdata {
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
		};

		void disp(inout appdata v) {
			//float d = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, 0)).r * _Displacement;
			//v.vertex.xyz += v.normal * d;
		}

		sampler2D _MainTex;
		struct Input {
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		}

		float _Tess;

		float4 tessFixed() {
			return _Tess;
		}

		ENDCG
   }
}