#include "UnityCG.cginc"

float _MaxHeight, _MinHeight;

struct saganVertexOutput {
	float4 position : SV_POSITION;
	float4 color : TEXTCOORD0;
};


// Return color based on scaled vertex height
fixed4 color(float4 vertexPos : POSITION) {
	float vertexHeight = vertexPos.y;

	float range = _MaxHeight  - _MinHeight;
	float scaledHeight = ( ( vertexHeight - _MinHeight ) / range );

	fixed4 col;

	col = float4(0.5, scaledHeight, 0.2, 0.3);

	return col;
}

float rescale(float a1, float a2, float b1, float b2, float s)
{
	return b1 + (s - a1)*(b2 - b1) / (a2 - a1);
}