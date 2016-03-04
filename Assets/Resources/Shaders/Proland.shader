Shader "Sagan/Proland" {

		SubShader{
		   Pass {
			 CGPROGRAM

			 #include "Assets/Resources/Shaders/Sagan.cginc"

			 #pragma vertex vert // vert function is the vertex shader
			 #pragma fragment frag // frag function is the fragment shader

			 float4x4 _TerrainMatrixWTL, _QuadPosition;
			 sampler2D _HeightMap;

			// vertex shader
			saganVertexOutput vert(appdata_base v) {
				saganVertexOutput v_out;
				float2 coords;

				float4 vertexPos = v.vertex;
				half alpha = tex2D(_HeightMap, vertexPos.xz).a;

				vertexPos.y = alpha;

				float4 localSpaceCamerPos = mul(_TerrainMatrixWTL, _WorldSpaceCameraPos);
				v_out.position = mul(UNITY_MATRIX_MVP, vertexPos);
				v_out.color = color(vertexPos);

				return v_out;
			}

			// fragment shader
			float4 frag(saganVertexOutput input) : COLOR {
			   return input.color;
				  // Here the fragment shader returns the "col" input
				  // parameter with semantic TEXCOORD0 as nameless
				  // output parameter with semantic COLOR.
			}

        ENDCG
      }
   }
}
