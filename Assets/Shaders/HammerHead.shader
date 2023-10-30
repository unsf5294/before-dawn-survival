// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// shader for make weapon 'energized' when attack
Shader "Unlit/HammerHead"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _AttackEffect ("Attack Effect", Float) = 0

        _PointLightColor("Point Light Color", Color) = (0, 0, 0)
		_PointLightPosition("Point Light Position", Vector) = (0.0, 0.0, 0.0)
		_Ka("Ka", Float) = 1.0
		_Kd("Kd", Float) = 1.0
		_Ks("Ks", Float) = 1.0
		_fAtt("fAtt", Float) = 1.0
		_specN("specN", Float) = 1.0

    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;    
            uniform float _AttackEffect;

            uniform float3 _PointLightColor;
			uniform float3 _PointLightPosition;
			uniform float _Ka;
			uniform float _Kd;
			uniform float _Ks;
			uniform float _fAtt;
			uniform float _specN;


            struct vertIn
            {
				float4 vertex : POSITION;
				float4 normal : NORMAL;
                float2 uv : TEXCOORD2;
            };

            struct vertOut
            {
				float4 vertex : SV_POSITION;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            vertOut vert(vertIn v)
            {
                vertOut o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
                float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));
                o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;

                o.uv = v.uv;
                return o;
            }

            fixed4 frag (vertOut v) : SV_Target
            {
                float3 interpNormal = normalize(v.worldNormal);
                fixed4 baseColor = tex2D(_MainTex, v.uv);

                // when attack, energize the weapon with some red light intensity and high reflective factor
                if (_AttackEffect > 0.5)
                {
                    _PointLightColor.rgb += float3(0.2 + 0.05 * sin(3 * _Time.y), 0, 0);
                    _specN = 0.5;
                    _Ka = 2;
                    _fAtt = 2;
                }

                // ambient stage
                float3 amb = baseColor * UNITY_LIGHTMODEL_AMBIENT.rgb * _Ka;

                // diffuse stage
                float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);
                float LdotN = dot(L, interpNormal);
                float3 dif = _fAtt * _PointLightColor.rgb * _Kd * baseColor.rgb * saturate(LdotN);

                // specular stage
                float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
                float3 H = normalize(V + L);
                float3 spe = _fAtt * _PointLightColor.rgb * _Ks * pow(saturate(dot(interpNormal, H)), _specN);

                float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
                returnColor.rgb = amb.rgb + dif.rgb + spe.rgb;
                returnColor.a = baseColor.a;
                
                return returnColor;
            }
            ENDCG
        }
    }
}
