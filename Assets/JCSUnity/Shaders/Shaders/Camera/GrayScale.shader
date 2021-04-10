/**
 * $File: GrayScale.shader $
 * $Date: 2017-07-23 07:09:57 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *                   Copyright (c) 2017 by Shen, Jen-Chieh $
 */

Shader "JCSUnity/Camera/GrayScale"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}

	    [MaterialToggle]
	    _GrayScale("Grayscale", int) = 0
	}
	SubShader
	{
		Tags { "Queue" = "Transparent" }
		LOD 100

		// Optional Blend Type
		// Blend X Y
		// Blend One One (Additive)
		// Blend SrcAlpha OneMinusSrcAlpha
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
	    {
		    CGPROGRAM

#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		    // Data structures
		    struct AppData
	        {
		        float4 vertex : POSITION;
		        float2 uv : TEXCOORD0;  // Texture Coordinate 0
	        };

	        struct v2f
	        {
		        float4 pos : SV_POSITION;
		        float2 uv : TEXCOORD0;
	        };

	        int _GrayScale;
	        float4 _Tint;
	        float4 _ScreenColor;

	        // To use a special Unity Macro
            //
	        // This requires that we have a variable with the same name as our
            // sampler2D but with _ST at the end
	        float4 _MainTex_ST;

	        v2f vert(AppData v)
	        {
		        v2f o;
		        o.pos = UnityObjectToClipPos(v.vertex);
		        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		        return o;
	        }

	        sampler2D _MainTex;

	        fixed4 frag(v2f i) : SV_Target
	        {
		        float4 c = tex2D(_MainTex, i.uv);

		        if (_GrayScale)
		        {
			        float4 grayScale = float4(0, 0, 0, 1);

			        // Gray Scale formula.
			        grayScale.rgb = dot(c.rgb, float3(0.3, 0.59, 0.11));

			        return grayScale;
		        }

		        return c;
	        }

		    ENDCG
	    }
	}
}
