Shader "Sagan/Proland" {

		SubShader{
		   Pass {
			 CGPROGRAM

			 #include "Assets/Resources/Shaders/Sagan.cginc"

			 #pragma vertex vert // vert function is the vertex shader
			 #pragma fragment frag // frag function is the fragment shader

			 float4x4 _TerrainMatrixWTL;
			 float4 _QuadPosition;
			 sampler2D _HeightMap;
			 int _TerrainSize;

			// vertex shader
			saganVertexOutput vert(appdata_base v) {
				saganVertexOutput v_out;

				float4 vertexPos = v.vertex;

				float vertexX = rescale(0, (float)_TerrainSize, 0, 1, _QuadPosition[0] + vertexPos.x);
				float vertexZ = rescale(0, (float)_TerrainSize, 0, 1, _QuadPosition[2] + vertexPos.z);

				float height = tex2Dlod(_HeightMap, float4(vertexX, vertexZ, 0, 0)).r;
				vertexPos.y += height;

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
