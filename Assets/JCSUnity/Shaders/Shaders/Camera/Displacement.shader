// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/**
 * $File: Displacement.shader $
 * $Date: 2017-07-23 08:03:02 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */

Shader "JCSUnity/Camera/Displacement"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	    _DisplaceTex("Displacement Texture", 2D) = "white" {}
	    _Magnitude("Magnitude", Range(0, 0.1)) = 1
	}
	SubShader
	{
		Tags{ "" = "" }
		Pass
	    {
		    CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		    struct appdata
	        {
		        float4 vertex : POSITION;
		        float2 uv : TEXCOORD0;
	        };

	        struct v2f
	        {
		        float4 vertex : SV_POSITION;
		        float2 uv : TEXCOORD0;
	        };

	        float4 _MainTex_ST;

	        v2f vert(appdata v)
	        {
		        v2f o;
		        o.vertex = UnityObjectToClipPos(v.vertex);
		        o.uv = v.uv;
		        return o;
	        }

	        sampler2D _MainTex;
	        sampler2D _DisplaceTex;
	        float _Magnitude;

	        fixed4 frag(v2f i) : SV_Target
	        {
		        float2 distuv = float2(i.uv.x + _Time.x * 2, i.uv.y + _Time.x * 2);

		        float2 disp = tex2D(_DisplaceTex, distuv).xy;
		        disp = ((disp * 2) - 1) * _Magnitude;

		        float4 col = tex2D(_MainTex, i.uv + disp);
		        return col;
	        }

		    ENDCG
	    }
	}
}
