// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_Vie"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_T_Vie("T_Vie", 2D) = "white" {}
		_Vie("Vie", Float) = 0.74
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _T_Vie;
		uniform float4 _T_Vie_ST;
		uniform float _Vie;
		uniform float _Cutoff = 0.5;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_T_Vie = i.uv_texcoord * _T_Vie_ST.xy + _T_Vie_ST.zw;
			float4 tex2DNode1 = tex2D( _T_Vie, uv_T_Vie );
			o.Emission = tex2DNode1.rgb;
			o.Alpha = 1;
			float temp_output_3_0_g1 = ( (0.2 + (_Vie - 0.0) * (0.75 - 0.2) / (1.0 - 0.0)) - tex2DNode1.r );
			clip( ( 1.0 - saturate( ( temp_output_3_0_g1 / fwidth( temp_output_3_0_g1 ) ) ) ) - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
1706.667;76.33334;1920;995;1368.852;416.6361;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;4;-1038.121,164.6973;Inherit;False;Property;_Vie;Vie;2;0;Create;True;0;0;False;0;0.74;0.35;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-924.4265,-91.8067;Inherit;True;Property;_T_Vie;T_Vie;1;0;Create;True;0;0;False;0;-1;84bf7c30173fd4945882bfd036a07cca;84bf7c30173fd4945882bfd036a07cca;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;10;-789.6843,170.1193;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.2;False;4;FLOAT;0.75;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;5;-589.1206,133.6973;Inherit;True;Step Antialiasing;-1;;1;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;9;-340.6843,131.1193;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;3;-80,-133;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;S_Vie;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;False;TransparentCutout;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;10;0;4;0
WireConnection;5;1;1;1
WireConnection;5;2;10;0
WireConnection;9;0;5;0
WireConnection;3;2;1;0
WireConnection;3;10;9;0
ASEEND*/
//CHKSM=C419450066FCFE07FBF519F11FFE00C588E7ED7F