Shader "Sagan/ColorHeight" {

   Properties {
    _MaxHeight ("MaxHeight", Float) = 0.0
    _MinHeight ("MinHeight", Float) = 0.0
   }
   SubShader {
      Pass {
         CGPROGRAM

         #pragma vertex vert // vert function is the vertex shader
         #pragma fragment frag // frag function is the fragment shader

         // for multiple vertex output parameters an output structure
         // is defined:
         struct vertexOutput {
            float4 pos : SV_POSITION;
            float4 col : TEXTCOORD0;
         };

         float _MaxHeight, _MinHeight;

         vertexOutput vert(float4 vertexPos : POSITION)
            // vertex shader
         {

            float vertexHeight = vertexPos.y;

            float range = _MaxHeight  - _MinHeight;
            float scaledHeight = ( ( vertexHeight - _MinHeight ) / range );

            vertexOutput output; // we don't need to type 'struct' here
            output.col = float4(0.5, scaledHeight, 0.2, 0.3);
            output.pos =  mul(UNITY_MATRIX_MVP, vertexPos);

            return output;
         }

         float4 frag(vertexOutput input) : COLOR // fragment shader
         {
            return input.col;
               // Here the fragment shader returns the "col" input
               // parameter with semantic TEXCOORD0 as nameless
               // output parameter with semantic COLOR.
         }

         ENDCG
      }
   }
}