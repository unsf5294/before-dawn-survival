// simple shader to make particles breathing like energy shards

Shader "Custom/ShinyParticle"
{
	Properties
	{
        // white color
		_MainColor("Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform float4 _MainColor;

			struct vertIn
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct vertOut
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			vertOut vert(vertIn v)
			{
				vertOut o;
                // handle MVP transformation
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				return o;
			}

			fixed4 frag(vertOut v) : SV_Target
			{
				fixed4 color = _MainColor;
                // color breathing
                color.rgb *= sin(_Time.y * 25) * 0.2 + 0.8;
				return color;
			}
			ENDCG
		}
	}
}