Shader "PastGlory/Lambert_Color_Alpha"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Tags 
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
			"LightMode"="ForwardBase" 
		}
		
		LOD 100

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float3 normal : NORMAL;
			};

			float4 _Color;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;
				col.rgb = _Color.rgb * dot(i.normal, _WorldSpaceLightPos0.xyz) * _LightColor0.rgb * i.color.rgb;
				col.a = _Color.a * i.color.a;
				return col;
			}
			ENDCG
		}
	}
}
