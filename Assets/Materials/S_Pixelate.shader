// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_Pixelate"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_PixelX("Pixel X", Range( 0 , 320)) = 0
		_PixelY("Pixel Y", Range( 0 , 320)) = 0
		[Toggle(_SWITCHRATIO43ALAMANO_ON)] _Switchratio43alamano("Switch/ratio4/3-alamano", Float) = 1

	}

	SubShader
	{
		LOD 0

		
		
		ZTest Always
		Cull Off
		ZWrite Off

		
		Pass
		{ 
			CGPROGRAM 

			

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			#pragma shader_feature_local _SWITCHRATIO43ALAMANO_ON


			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform float _PixelX;
			uniform float _PixelY;


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;

				// ase common template code
				float2 uv04 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				#ifdef _SWITCHRATIO43ALAMANO_ON
				float staticSwitch12 = ( ( _PixelY * 4.0 ) / 3.0 );
				#else
				float staticSwitch12 = _PixelX;
				#endif
				float pixelWidth3 =  1.0f / staticSwitch12;
				float pixelHeight3 = 1.0f / _PixelY;
				half2 pixelateduv3 = half2((int)(uv04.x / pixelWidth3) * pixelWidth3, (int)(uv04.y / pixelHeight3) * pixelHeight3);
				

				finalColor = tex2D( _MainTex, pixelateduv3 );

				return finalColor;
			} 
			ENDCG 
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18000
1706.667;69.33334;1920;1019;2575.83;485.5975;1.902832;True;True
Node;AmplifyShaderEditor.RangedFloatNode;6;-1841.946,174.6627;Inherit;False;Property;_PixelY;Pixel Y;1;0;Create;True;0;0;False;0;0;270;0;320;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1725.725,346.1656;Inherit;False;Constant;_4;*4;2;0;Create;True;0;0;False;0;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1516.068,252.0814;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1515.466,354.3778;Inherit;False;Constant;_3;/3;2;0;Create;True;0;0;False;0;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;10;-1365.861,251.3831;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1580.509,-6.276885;Inherit;False;Property;_PixelX;Pixel X;0;0;Create;True;0;0;False;0;0;53;0;320;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;4;-1102.994,-88.47025;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;12;-1215.057,49.0917;Inherit;False;Property;_Switchratio43alamano;Switch/ratio4/3-alamano;2;0;Create;True;0;0;False;0;0;1;1;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;1;-672.8674,-82.70202;Inherit;False;0;0;_MainTex;Shader;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCPixelate;3;-701.2987,0.6890877;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;2;-323.2587,-18.88267;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;145,2;Float;False;True;-1;2;ASEMaterialInspector;0;2;S_Pixelate;c71b220b631b6344493ea3cf87110c93;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;1;False;False;False;True;2;False;-1;False;False;True;2;False;-1;True;7;False;-1;False;True;0;False;0;False;False;False;False;False;False;False;False;False;False;True;2;0;;0;0;Standard;0;0;1;True;False;;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;10;0;7;0
WireConnection;10;1;11;0
WireConnection;12;1;5;0
WireConnection;12;0;10;0
WireConnection;3;0;4;0
WireConnection;3;1;12;0
WireConnection;3;2;6;0
WireConnection;2;0;1;0
WireConnection;2;1;3;0
WireConnection;0;0;2;0
ASEEND*/
//CHKSM=C12A08F1BC3CD1AA9D010A690A6A82D8EAEA55D2