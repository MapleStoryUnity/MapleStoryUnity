// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

/**
 * $File: FogOfWar.shader $
 * $Date: 2016-11-20 21:58:21 $
 * $Revision: $
 * $Creator: Jen-Chieh Shen $
 * $Notice: See LICENSE.txt for modification and distribution information
 *	                 Copyright (c) 2016 by Shen, Jen-Chieh $
 */

Shader "JCSUnity/Fog of War/FogOfWar" {
    Properties{
        _Color("Main Color", Color) = (1, 1, 1, 1)
        _MainTex("Base (RGB) Trans (A)", 2D) = "white" {}

        _FogRadius("FogRadius", Float) = 1.0
        _FogMaxRadius("FogMaxRadius", Float) = 0.5

        _isSight0("_isSight0", Float) = 0.0
        _isSight1("_isSight1", Float) = 0.0
        _isSight2("_isSight2", Float) = 0.0
        _isSight3("_isSight3", Float) = 0.0
        _isSight4("_isSight4", Float) = 0.0
        _isSight5("_isSight5", Float) = 0.0
        _isSight6("_isSight6", Float) = 0.0
        _isSight7("_isSight7", Float) = 0.0
        _isSight8("_isSight8", Float) = 0.0
        _isSight9("_isSight9", Float) = 0.0
        _isSight10("_isSight10", Float) = 0.0
        _isSight11("_isSight11", Float) = 0.0
        _isSight12("_isSight12", Float) = 0.0
        _isSight13("_isSight13", Float) = 0.0
        _isSight14("_isSight14", Float) = 0.0
        _isSight15("_isSight15", Float) = 0.0
        _isSight16("_isSight16", Float) = 0.0
        _isSight17("_isSight17", Float) = 0.0
        _isSight18("_isSight18", Float) = 0.0
        _isSight19("_isSight19", Float) = 0.0
        _isSight20("_isSight20", Float) = 0.0
        _isSight21("_isSight21", Float) = 0.0
        _isSight22("_isSight22", Float) = 0.0
        _isSight23("_isSight23", Float) = 0.0
        _isSight24("_isSight24", Float) = 0.0
        _isSight25("_isSight25", Float) = 0.0
        _isSight26("_isSight26", Float) = 0.0
        _isSight27("_isSight27", Float) = 0.0
        _isSight28("_isSight28", Float) = 0.0
        _isSight29("_isSight29", Float) = 0.0

        _Player0_Pos("_Player0_Pos", Vector) = (0, 0, 0, 1)
        _Player1_Pos("_Player1_Pos", Vector) = (0, 0, 0, 1)
        _Player2_Pos("_Player2_Pos", Vector) = (0, 0, 0, 1)
        _Player3_Pos("_Player3_Pos", Vector) = (0, 0, 0, 1)
        _Player4_Pos("_Player4_Pos", Vector) = (0, 0, 0, 1)
        _Player5_Pos("_Player5_Pos", Vector) = (0, 0, 0, 1)
        _Player6_Pos("_Player6_Pos", Vector) = (0, 0, 0, 1)
        _Player7_Pos("_Player7_Pos", Vector) = (0, 0, 0, 1)
        _Player8_Pos("_Player8_Pos", Vector) = (0, 0, 0, 1)
        _Player9_Pos("_Player9_Pos", Vector) = (0, 0, 0, 1)
        _Player10_Pos("_Player10_Pos", Vector) = (0, 0, 0, 1)
        _Player11_Pos("_Player11_Pos", Vector) = (0, 0, 0, 1)
        _Player12_Pos("_Player12_Pos", Vector) = (0, 0, 0, 1)
        _Player13_Pos("_Player13_Pos", Vector) = (0, 0, 0, 1)
        _Player14_Pos("_Player14_Pos", Vector) = (0, 0, 0, 1)
        _Player15_Pos("_Player15_Pos", Vector) = (0, 0, 0, 1)
        _Player16_Pos("_Player16_Pos", Vector) = (0, 0, 0, 1)
        _Player17_Pos("_Player17_Pos", Vector) = (0, 0, 0, 1)
        _Player18_Pos("_Player18_Pos", Vector) = (0, 0, 0, 1)
        _Player19_Pos("_Player19_Pos", Vector) = (0, 0, 0, 1)
        _Player20_Pos("_Player20_Pos", Vector) = (0, 0, 0, 1)
        _Player21_Pos("_Player21_Pos", Vector) = (0, 0, 0, 1)
        _Player22_Pos("_Player22_Pos", Vector) = (0, 0, 0, 1)
        _Player23_Pos("_Player23_Pos", Vector) = (0, 0, 0, 1)
        _Player24_Pos("_Player24_Pos", Vector) = (0, 0, 0, 1)
        _Player25_Pos("_Player25_Pos", Vector) = (0, 0, 0, 1)
        _Player26_Pos("_Player26_Pos", Vector) = (0, 0, 0, 1)
        _Player27_Pos("_Player27_Pos", Vector) = (0, 0, 0, 1)
        _Player28_Pos("_Player28_Pos", Vector) = (0, 0, 0, 1)
        _Player29_Pos("_Player29_Pos", Vector) = (0, 0, 0, 1)
    }

    SubShader{
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
        LOD 200
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        CGPROGRAM
#pragma surface surf Lambert vertex:vert alpha:blend

        sampler2D _MainTex;
        fixed4 	_Color;

        float 	_FogRadius;
        float 	_FogMaxRadius;
        float _isSight0, _isSight1, _isSight2, _isSight3, _isSight4,
        _isSight5, _isSight6, _isSight7, _isSight8, _isSight9,
        _isSight10, _isSight11, _isSight12, _isSight13, _isSight14,
        _isSight15, _isSight16, _isSight17, _isSight18, _isSight19,
        _isSight20, _isSight21, _isSight22, _isSight23, _isSight24,
        _isSight25, _isSight26, _isSight27, _isSight28, _isSight29;


        float4 	_Player0_Pos, _Player1_Pos, _Player2_Pos, _Player3_Pos, _Player4_Pos,
        _Player5_Pos, _Player6_Pos, _Player7_Pos, _Player8_Pos, _Player9_Pos,
        _Player10_Pos, _Player11_Pos, _Player12_Pos, _Player13_Pos, _Player14_Pos,
        _Player15_Pos, _Player16_Pos, _Player17_Pos, _Player18_Pos, _Player19_Pos,
        _Player20_Pos, _Player21_Pos, _Player22_Pos, _Player23_Pos, _Player24_Pos,
        _Player25_Pos, _Player26_Pos, _Player27_Pos, _Player28_Pos, _Player29_Pos;


        struct Input
        {
            float2 uv_MainTex;
            float2 location;
        };

        // forward declare function...
        float powerForPos(float4 pos, float2 nearVertex, float isSight);

        void vert(inout appdata_full vertexData, out Input outData)
        {
            float4 pos = UnityObjectToClipPos(vertexData.vertex);
            float4 posWorld = mul(unity_ObjectToWorld, vertexData.vertex);

            outData.uv_MainTex = vertexData.texcoord;
            outData.location = posWorld.xz;
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            float alpha = (1.0 - (baseColor.a +
                powerForPos(_Player0_Pos, IN.location, _isSight0) +
                powerForPos(_Player1_Pos, IN.location, _isSight1) +
                powerForPos(_Player2_Pos, IN.location, _isSight2) +
                powerForPos(_Player3_Pos, IN.location, _isSight3) +
                powerForPos(_Player4_Pos, IN.location, _isSight4) +
                powerForPos(_Player5_Pos, IN.location, _isSight5) +
                powerForPos(_Player6_Pos, IN.location, _isSight6) +
                powerForPos(_Player7_Pos, IN.location, _isSight7) +
                powerForPos(_Player8_Pos, IN.location, _isSight8) +
                powerForPos(_Player9_Pos, IN.location, _isSight9) +
                powerForPos(_Player10_Pos, IN.location, _isSight10) +
                powerForPos(_Player11_Pos, IN.location, _isSight11) +
                powerForPos(_Player12_Pos, IN.location, _isSight12) +
                powerForPos(_Player13_Pos, IN.location, _isSight13) +
                powerForPos(_Player14_Pos, IN.location, _isSight14) +
                powerForPos(_Player15_Pos, IN.location, _isSight15) +
                powerForPos(_Player16_Pos, IN.location, _isSight16) +
                powerForPos(_Player17_Pos, IN.location, _isSight17) +
                powerForPos(_Player18_Pos, IN.location, _isSight18) +
                powerForPos(_Player19_Pos, IN.location, _isSight19) +
                powerForPos(_Player20_Pos, IN.location, _isSight20) +
                powerForPos(_Player21_Pos, IN.location, _isSight21) +
                powerForPos(_Player22_Pos, IN.location, _isSight22) +
                powerForPos(_Player23_Pos, IN.location, _isSight23) +
                powerForPos(_Player24_Pos, IN.location, _isSight24) +
                powerForPos(_Player25_Pos, IN.location, _isSight25) +
                powerForPos(_Player26_Pos, IN.location, _isSight26) +
                powerForPos(_Player27_Pos, IN.location, _isSight27) +
                powerForPos(_Player28_Pos, IN.location, _isSight28) +
                powerForPos(_Player29_Pos, IN.location, _isSight29)));

            o.Albedo = baseColor.rgb;
            o.Alpha = alpha;
        }

        // return 0 if (pos - nearVertex) > _FogRadius
        float powerForPos(float4 pos, float2 nearVertex, float isSight)
        {
            float atten = clamp(_FogRadius - length(pos.xz - nearVertex.xy), 0.0, _FogRadius);

            return ((1.0 / _FogMaxRadius)*atten / _FogRadius) * isSight;
        }

        ENDCG
    }

    Fallback "Transparent/VertexLit"
}
