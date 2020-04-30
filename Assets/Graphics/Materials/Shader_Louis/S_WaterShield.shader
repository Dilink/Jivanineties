// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_WaterShield"
{
	Properties
	{
		_appearValue("appearValue", Range( 0.2 , 1)) = 1
		_desapearValue("desapearValue", Range( 0 , 3)) = 0
		_MotifValue("MotifValue", Range( 0.5 , 3)) = 0
		_OpacitySubstract("OpacitySubstract", Range( 0 , 1)) = 0.2
		_Perlin("Perlin", 2D) = "white" {}
		_WaterShieldTexture("WaterShieldTexture", 2D) = "white" {}
		_RAmp("RAmp", 2D) = "white" {}
		_Float1("Float 1", Range( 0.01 , 1)) = 0.01
		_Float2("Float 2", Range( 1 , 10)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		AlphaToMask On
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Perlin;
		uniform float _Float2;
		uniform float _Float1;
		uniform sampler2D _RAmp;
		uniform sampler2D _WaterShieldTexture;
		uniform float _MotifValue;
		uniform float _appearValue;
		uniform float _desapearValue;
		uniform float _OpacitySubstract;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float2 panner25 = ( 1.0 * _Time.y * float2( 1,0 ) + v.texcoord.xy);
			v.vertex.xyz += ( ase_vertexNormal * ( pow( tex2Dlod( _Perlin, float4( panner25, 0, 0.0) ).r , _Float2 ) * _Float1 * 0.01 ) );
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord60 = i.uv_texcoord * float2( 1,2 );
			float2 panner61 = ( 1.0 * _Time.y * float2( 0,-1 ) + uv_TexCoord60);
			float2 uv_TexCoord69 = i.uv_texcoord * float2( 1.4,1.3 );
			float2 panner70 = ( 0.25 * _Time.y * float2( 1,1 ) + uv_TexCoord69);
			float2 panner66 = ( 0.25 * _Time.y * float2( 1,0 ) + i.uv_texcoord);
			float2 temp_cast_0 = (( tex2D( _WaterShieldTexture, panner61 ).r - ( ( tex2D( _WaterShieldTexture, panner70 ).b * tex2D( _WaterShieldTexture, panner66 ).b ) * _MotifValue ) )).xx;
			o.Emission = tex2D( _RAmp, temp_cast_0 ).rgb;
			float saferPower49 = max( ( 1.0 - i.vertexColor.r ) , 0.0001 );
			float2 uv_TexCoord44 = i.uv_texcoord * float2( 1.5,1.5 );
			float2 panner45 = ( 1.0 * _Time.y * float2( -1,0.2 ) + uv_TexCoord44);
			float2 panner47 = ( 0.7 * _Time.y * float2( -0.5,0.4 ) + i.uv_texcoord);
			float clampResult75 = clamp( ( step( _appearValue , ( i.vertexColor.r - ( pow( saferPower49 , 3.0 ) * tex2D( _Perlin, panner45 ).r * tex2D( _Perlin, panner47 ).r ) ) ) - ( ( i.vertexColor.r * _desapearValue ) + _OpacitySubstract ) ) , 0.0 , 1.0 );
			o.Alpha = clampResult75;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
0;18;1920;1001;1756.839;-645.1064;1.477768;True;False
Node;AmplifyShaderEditor.CommentaryNode;51;-1366.284,399.1993;Inherit;False;1800.837;745.1136;Opacity Gestion;17;44;48;45;47;42;1;46;49;43;2;39;40;13;72;84;85;86;;1,1,1,1;0;0
Node;AmplifyShaderEditor.VertexColorNode;1;-519.5029,485.5406;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;48;-1180.676,449.1993;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;44;-1316.284,875.6013;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.5,1.5;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;42;-1151.636,629.3751;Inherit;True;Property;_Perlin;Perlin;4;0;Create;True;0;0;False;0;805af41b6e803c748bc9986722ba9df2;805af41b6e803c748bc9986722ba9df2;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.PannerNode;45;-1027.859,901.7455;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,0.2;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.OneMinusNode;2;-201.6643,690.6954;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;47;-892.2513,476.3435;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-0.5,0.4;False;1;FLOAT;0.7;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;65;-1402.152,-10.767;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;69;-1359.252,205.0331;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1.4,1.3;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;50;-1353.594,1329.466;Inherit;False;1998.848;468.9488;Displacme,nt;10;9;25;32;8;38;31;27;36;26;29;;1,1,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;66;-1053.727,-9.622787;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;0.25;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;54;-1159.52,-396.9606;Inherit;True;Property;_WaterShieldTexture;WaterShieldTexture;5;0;Create;True;0;0;False;0;3a7849761157ff743ad92b91c31820f9;3a7849761157ff743ad92b91c31820f9;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;46;-702.5623,706.7794;Inherit;True;Property;_TextureSample2;Texture Sample 2;0;0;Create;True;0;0;False;0;-1;805af41b6e803c748bc9986722ba9df2;805af41b6e803c748bc9986722ba9df2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;49;-102.8705,825.9933;Inherit;False;True;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;43;-697.2436,914.3129;Inherit;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;0;False;0;-1;805af41b6e803c748bc9986722ba9df2;805af41b6e803c748bc9986722ba9df2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-1303.594,1405.559;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;70;-1010.827,206.1773;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;0.25;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;61.05859,937.8295;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;62;-757.5491,-91.29382;Inherit;True;Property;_TextureSample4;Texture Sample 4;6;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;86;-337.8082,464.5826;Inherit;False;Property;_desapearValue;desapearValue;1;0;Create;True;0;0;False;0;0;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;63;-729.5491,123.7062;Inherit;True;Property;_TextureSample5;Texture Sample 5;6;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;25;-1049.968,1419.004;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;60;-1465.761,-181.8659;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,2;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;40;113.2178,688.6738;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-66.0113,559.9211;Inherit;False;Property;_OpacitySubstract;OpacitySubstract;3;0;Create;True;0;0;False;0;0.2;0.47;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;-66.25385,452.1927;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2.56;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;32;-530.1707,1465.131;Inherit;False;Property;_Float2;Float 2;8;0;Create;True;0;0;False;0;1;6.8;1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;61;-1185.336,-181.7217;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-352.5491,52.70618;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-201.7134,274.41;Inherit;False;Property;_MotifValue;MotifValue;2;0;Create;True;0;0;False;0;0;1.23;0.5;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;4.20199,1098.194;Inherit;False;Property;_appearValue;appearValue;0;0;Create;True;0;0;False;0;1;1;0.2;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;8;-852.1101,1386.737;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;805af41b6e803c748bc9986722ba9df2;805af41b6e803c748bc9986722ba9df2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;27;-270.5888,1663.756;Inherit;False;Property;_Float1;Float 1;7;0;Create;True;0;0;False;0;0.01;0.16;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;3;364.8178,659.311;Inherit;True;2;0;FLOAT;0.02;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;56;-788.5195,-374.9606;Inherit;True;Property;_TextureSample3;Texture Sample 3;6;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;38;100.5665,1682.415;Inherit;False;Constant;_Float3;Float 3;1;0;Create;True;0;0;False;0;0.01;0.01;0.01;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;84;208.7462,461.1927;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;87;-65.01343,-42.29004;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;31;-221.4013,1403.555;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;59;-271.8347,-658.5689;Inherit;True;Property;_RAmp;RAmp;6;0;Create;True;0;0;False;0;4edf59f9699caef46970d85e32231dd6;4edf59f9699caef46970d85e32231dd6;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.NormalVertexDataNode;36;196.3866,1459.878;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;58;-98.13461,-290.2689;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;40.77793,1379.466;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;73;375.5311,445.0102;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;71;213.7599,-450.895;Inherit;True;Property;_TextureSample6;Texture Sample 6;6;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;75;536.7374,386.845;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;483.2542,1395.473;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;688.2255,100.4783;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;S_WaterShield;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;9;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;45;0;44;0
WireConnection;2;0;1;1
WireConnection;47;0;48;0
WireConnection;66;0;65;0
WireConnection;46;0;42;0
WireConnection;46;1;47;0
WireConnection;49;0;2;0
WireConnection;43;0;42;0
WireConnection;43;1;45;0
WireConnection;70;0;69;0
WireConnection;39;0;49;0
WireConnection;39;1;43;1
WireConnection;39;2;46;1
WireConnection;62;0;54;0
WireConnection;62;1;66;0
WireConnection;63;0;54;0
WireConnection;63;1;70;0
WireConnection;25;0;9;0
WireConnection;40;0;1;1
WireConnection;40;1;39;0
WireConnection;85;0;1;1
WireConnection;85;1;86;0
WireConnection;61;0;60;0
WireConnection;64;0;63;3
WireConnection;64;1;62;3
WireConnection;8;0;42;0
WireConnection;8;1;25;0
WireConnection;3;0;13;0
WireConnection;3;1;40;0
WireConnection;56;0;54;0
WireConnection;56;1;61;0
WireConnection;84;0;85;0
WireConnection;84;1;72;0
WireConnection;87;0;64;0
WireConnection;87;1;88;0
WireConnection;31;0;8;1
WireConnection;31;1;32;0
WireConnection;58;0;56;1
WireConnection;58;1;87;0
WireConnection;26;0;31;0
WireConnection;26;1;27;0
WireConnection;26;2;38;0
WireConnection;73;0;3;0
WireConnection;73;1;84;0
WireConnection;71;0;59;0
WireConnection;71;1;58;0
WireConnection;75;0;73;0
WireConnection;29;0;36;0
WireConnection;29;1;26;0
WireConnection;0;2;71;0
WireConnection;0;9;75;0
WireConnection;0;11;29;0
ASEEND*/
//CHKSM=1ED0F5573043F99D029CC833007FD225BDB3BFB8