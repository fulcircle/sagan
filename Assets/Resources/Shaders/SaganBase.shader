Shader "Sagan/Base" {

	Properties {
		_MaxHeight ("MaxHeight", Float) = 0.0
		_MinHeight ("MinHeight", Float) = 0.0
	}
   SubShader {
      Pass {
        CGPROGRAM

		#include "Assets/Resources/Shaders/Sagan.cginc"
		 
        #pragma vertex vert // vert function is the vertex shader
        #pragma fragment frag // frag function is the fragment shader

		// vertex shader
		saganVertexOutput vert(float4 vertexPos : POSITION) {
			saganVertexOutput v_out;

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