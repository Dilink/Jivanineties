// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "S_Principal"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_Asset_color("Asset_color", Color) = (0.1415984,0.4056604,0.2571256,0.345098)
		_Zone_Shadow("Zone_Shadow", Float) = 0.14
		_MainColor("Main Color", Color) = (0.3921569,0.3921569,0.3921569,1)
		_SpecularColor("Specular Color", Color) = (0.3921569,0.3921569,0.3921569,1)
		_Shininess("Shininess", Range( 0.01 , 1)) = 0.1
		_DRIED("DRIED", Float) = 0
		_Albedo("Albedo", 2D) = "white" {}
		_VERTEX_ANIM_POWER("VERTEX_ANIM_POWER", Float) = 2.05
		[Toggle(_DISAPEAR_ON)] _Disapear("Disapear", Float) = 0
		_Noise_vertex_animation("Noise_vertex_animation", 2D) = "white" {}
		[Toggle(_VERTEX_ANIMATION_ON)] _Vertex_animation("Vertex_animation", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _VERTEX_ANIMATION_ON
		#pragma shader_feature_local _DISAPEAR_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		struct SurfaceOutputCustomLightingCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			Input SurfInput;
			UnityGIInput GIData;
		};

		uniform sampler2D _Noise_vertex_animation;
		uniform float _VERTEX_ANIM_POWER;
		uniform float _DRIED;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float4 _Asset_color;
		uniform float4 _SpecularColor;
		uniform float _Shininess;
		uniform float4 _MainColor;
		uniform float _Zone_Shadow;
		uniform float _Cutoff = 0.5;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_vertexNormal = v.normal.xyz;
			float mulTime59 = _Time.y * 0.09;
			float2 panner55 = ( mulTime59 * float2( 1,0 ) + v.texcoord.xy);
			#ifdef _VERTEX_ANIMATION_ON
				float4 staticSwitch60 = ( float4( ase_vertexNormal , 0.0 ) * ( ( v.color.r * pow( tex2Dlod( _Noise_vertex_animation, float4( panner55, 0, 0.0) ) , 4.54 ) ) * _VERTEX_ANIM_POWER ) );
			#else
				float4 staticSwitch60 = float4( 0,0,0,0 );
			#endif
			v.vertex.xyz += staticSwitch60.rgb;
		}

		inline half4 LightingStandardCustomLighting( inout SurfaceOutputCustomLightingCustom s, half3 viewDir, UnityGI gi )
		{
			UnityGIInput data = s.GIData;
			Input i = s.SurfInput;
			half4 c = 0;
			#ifdef UNITY_PASS_FORWARDBASE
			float ase_lightAtten = data.atten;
			if( _LightColor0.a == 0)
			ase_lightAtten = 0;
			#else
			float3 ase_lightAttenRGB = gi.light.color / ( ( _LightColor0.rgb ) + 0.000001 );
			float ase_lightAtten = max( max( ase_lightAttenRGB.r, ase_lightAttenRGB.g ), ase_lightAttenRGB.b );
			#endif
			#if defined(HANDLE_SHADOWS_BLENDING_IN_GI)
			half bakedAtten = UnitySampleBakedOcclusion(data.lightmapUV.xy, data.worldPos);
			float zDist = dot(_WorldSpaceCameraPos - data.worldPos, UNITY_MATRIX_V[2].xyz);
			float fadeDist = UnityComputeShadowFadeDistance(data.worldPos, zDist);
			ase_lightAtten = UnityMixRealtimeAndBakedShadows(data.atten, bakedAtten, UnityComputeShadowFade(fadeDist));
			#endif
			float ifLocalVar77 = 0;
			if( _DRIED > 0.5 )
				ifLocalVar77 = 0.0;
			else if( _DRIED == 0.5 )
				ifLocalVar77 = 0.0;
			else if( _DRIED < 0.5 )
				ifLocalVar77 = 1.0;
			#ifdef _DISAPEAR_ON
				float staticSwitch83 = ifLocalVar77;
			#else
				float staticSwitch83 = 1.0;
			#endif
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode71 = tex2D( _Albedo, uv_Albedo );
			float4 color96 = IsGammaSpace() ? float4(0.4433962,0.4433962,0.4433962,0) : float4(0.1653383,0.1653383,0.1653383,0);
			float3 desaturateInitialColor42 = tex2DNode71.rgb;
			float desaturateDot42 = dot( desaturateInitialColor42, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar42 = lerp( desaturateInitialColor42, desaturateDot42.xxx, 1.3 );
			float4 lerpResult43 = lerp( tex2DNode71 , ( tex2DNode71 * ( color96 * float4( desaturateVar42 , 0.0 ) ) ) , _DRIED);
			float4 color37 = IsGammaSpace() ? float4(1,1,1,0) : float4(1,1,1,0);
			float4 lerpResult36 = lerp( _Asset_color , color37 , _Asset_color.a);
			float4 temp_output_34_0 = ( lerpResult43 * lerpResult36 );
			float4 color33 = IsGammaSpace() ? float4(0.5573158,0.6601495,0.9528302,0) : float4(0.2709788,0.3933205,0.8960326,0);
			float4 blendOpSrc31 = temp_output_34_0;
			float4 blendOpDest31 = color33;
			float4 temp_output_43_0_g1 = _SpecularColor;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aseld
			float3 ase_worldlightDir = 0;
			#else //aseld
			float3 ase_worldlightDir = normalize( UnityWorldSpaceLightDir( ase_worldPos ) );
			#endif //aseld
			float3 normalizeResult4_g2 = normalize( ( ase_worldViewDir + ase_worldlightDir ) );
			float3 normalizeResult64_g1 = normalize( (WorldNormalVector( i , float3(0,0,1) )) );
			float dotResult19_g1 = dot( normalizeResult4_g2 , normalizeResult64_g1 );
			#if defined(LIGHTMAP_ON) && ( UNITY_VERSION < 560 || ( defined(LIGHTMAP_SHADOW_MIXING) && !defined(SHADOWS_SHADOWMASK) && defined(SHADOWS_SCREEN) ) )//aselc
			float4 ase_lightColor = 0;
			#else //aselc
			float4 ase_lightColor = _LightColor0;
			#endif //aselc
			float4 temp_output_40_0_g1 = ( ase_lightColor * ase_lightAtten );
			float dotResult14_g1 = dot( normalizeResult64_g1 , ase_worldlightDir );
			UnityGI gi34_g1 = gi;
			float3 diffNorm34_g1 = normalizeResult64_g1;
			gi34_g1 = UnityGI_Base( data, 1, diffNorm34_g1 );
			float3 indirectDiffuse34_g1 = gi34_g1.indirect.diffuse + diffNorm34_g1 * 0.0001;
			float4 temp_output_42_0_g1 = _MainColor;
			float temp_output_3_0_g4 = ( ( ( float4( (temp_output_43_0_g1).rgb , 0.0 ) * (temp_output_43_0_g1).a * pow( max( dotResult19_g1 , 0.0 ) , ( _Shininess * 128.0 ) ) * temp_output_40_0_g1 ) + ( ( ( temp_output_40_0_g1 * max( dotResult14_g1 , 0.0 ) ) + float4( indirectDiffuse34_g1 , 0.0 ) ) * float4( (temp_output_42_0_g1).rgb , 0.0 ) ) ).r - _Zone_Shadow );
			float4 lerpResult18 = lerp( temp_output_34_0 , ( saturate( ( blendOpSrc31 * blendOpDest31 ) )) , ( ( 1.0 - saturate( ( temp_output_3_0_g4 / fwidth( temp_output_3_0_g4 ) ) ) ) * 0.84 ));
			c.rgb = lerpResult18.rgb;
			c.a = 1;
			clip( staticSwitch83 - _Cutoff );
			return c;
		}

		inline void LightingStandardCustomLighting_GI( inout SurfaceOutputCustomLightingCustom s, UnityGIInput data, inout UnityGI gi )
		{
			s.GIData = data;
		}

		void surf( Input i , inout SurfaceOutputCustomLightingCustom o )
		{
			o.SurfInput = i;
			o.Normal = float3(0,0,1);
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustomLighting keepalpha fullforwardshadows vertex:vertexDataFunc 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputCustomLightingCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputCustomLightingCustom, o )
				surf( surfIN, o );
				UnityGI gi;
				UNITY_INITIALIZE_OUTPUT( UnityGI, gi );
				o.Alpha = LightingStandardCustomLighting( o, worldViewDir, gi ).a;
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18000
1706.667;69.33334;1920;1019;-114.7394;71.10651;1;True;True
Node;AmplifyShaderEditor.CommentaryNode;66;-68.32642,361.2559;Inherit;False;1261.136;730.558;Vertex_displacement;10;52;50;51;58;56;59;55;88;90;91;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;86;-2000.643,-613.4634;Inherit;False;743.1896;477.761;Desat;4;96;42;45;95;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;56;-18.32641,676.814;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;59;-18.32641,980.814;Inherit;False;1;0;FLOAT;0.09;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;58;13.67348,820.814;Inherit;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;False;0;1,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;71;-2019.261,-997.3484;Inherit;True;Property;_Albedo;Albedo;8;0;Create;True;0;0;False;0;-1;None;1959e52e8f749e9409e8eaf9a74c77a6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;-2008.163,-497.6941;Inherit;False;Constant;_Float2;Float 2;4;0;Create;True;0;0;False;0;1.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DesaturateOpNode;42;-1887.985,-547.3037;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;96;-1845.592,-317.992;Inherit;False;Constant;_Color1;Color 1;9;0;Create;True;0;0;False;0;0.4433962,0.4433962,0.4433962,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;55;237.6734,676.814;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;51;422.4301,605.5706;Inherit;True;Property;_Noise_vertex_animation;Noise_vertex_animation;11;0;Create;True;0;0;False;0;-1;None;805af41b6e803c748bc9986722ba9df2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-1625.473,-420.0551;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;65;-1861.802,136.4657;Inherit;False;893.6163;430;Shadow;6;5;12;13;23;24;25;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;35;-1187.584,-241.4385;Inherit;False;Property;_Asset_color;Asset_color;1;0;Create;True;0;0;False;0;0.1415984,0.4056604,0.2571256,0.345098;1,1,1,0.345098;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;12;-1723.802,186.4657;Inherit;False;Property;_Zone_Shadow;Zone_Shadow;2;0;Create;True;0;0;False;0;0.14;0.14;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;-1492.454,-563.4634;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1170.906,-345.1395;Inherit;False;Property;_DRIED;DRIED;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;50;495.8916,411.2559;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;90;704.4076,638.9055;Inherit;True;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;4.54;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;37;-1190.764,-61.4014;Inherit;False;Constant;_white;white;4;0;Create;True;0;0;False;0;1,1,1,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;5;-1811.802,288.4658;Inherit;True;Blinn-Phong Light;3;;1;cf814dba44d007a4e958d2ddd5813da6;0;3;42;COLOR;0,0,0,0;False;52;FLOAT3;0,0,0;False;43;COLOR;0,0,0,0;False;2;COLOR;0;FLOAT;57
Node;AmplifyShaderEditor.RangedFloatNode;91;879.4076,941.9055;Inherit;False;Property;_VERTEX_ANIM_POWER;VERTEX_ANIM_POWER;9;0;Create;True;0;0;False;0;2.05;2.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;87;490.9969,-503.7245;Inherit;False;862.8405;657.0056;Opacity;7;79;80;77;81;82;84;83;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;36;-871.6752,-196.0525;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;732.8679,463.0075;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;13;-1530.803,226.4657;Inherit;True;Step Antialiasing;-1;;4;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;43;-1018.566,-533.7715;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;79;550.6479,-453.7245;Inherit;False;Constant;_Float0;Float 0;9;0;Create;True;0;0;False;0;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;550.6479,-375.7245;Inherit;False;Constant;_Float3;Float 3;9;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;103;853.7394,326.8935;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;23;-1292.019,225.4886;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;33;-723.5873,477.8809;Inherit;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;False;0;0.5573158,0.6601495,0.9528302,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;82;573.6394,-123.3696;Inherit;False;Constant;_Float5;Float 5;7;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;990.5164,509.0354;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;81;540.9969,-231.8382;Inherit;False;Constant;_Float4;Float 4;7;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-421.101,-223.8259;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-1298.201,318.4194;Inherit;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;False;0;0.84;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ConditionalIfNode;77;896.1199,-309.48;Inherit;False;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;1230.006,461.1317;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BlendOpsNode;31;-424.1657,454.1834;Inherit;True;Multiply;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;64;-290.3391,-933.7127;Inherit;False;506.9594;275.2195;Degrade_bas/haut;2;61;63;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;25;-1130.186,243.145;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;959.3746,37.28106;Inherit;False;Constant;_Float6;Float 6;8;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendOpsNode;63;-24.41844,-852.1284;Inherit;False;ColorBurn;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;60;1478.751,427.9958;Inherit;False;Property;_Vertex_animation;Vertex_animation;12;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;83;1127.837,-53.47196;Inherit;False;Property;_Disapear;Disapear;10;0;Create;True;0;0;False;0;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;18;-154.1884,133.3344;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PosVertexDataNode;61;-258.5707,-853.5116;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1855.66,-108.078;Float;False;True;-1;2;ASEMaterialInspector;0;0;CustomLighting;S_Principal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;42;0;71;0
WireConnection;42;1;49;0
WireConnection;55;0;56;0
WireConnection;55;2;58;0
WireConnection;55;1;59;0
WireConnection;51;1;55;0
WireConnection;95;0;96;0
WireConnection;95;1;42;0
WireConnection;45;0;71;0
WireConnection;45;1;95;0
WireConnection;90;0;51;0
WireConnection;36;0;35;0
WireConnection;36;1;37;0
WireConnection;36;2;35;4
WireConnection;52;0;50;1
WireConnection;52;1;90;0
WireConnection;13;1;12;0
WireConnection;13;2;5;0
WireConnection;43;0;71;0
WireConnection;43;1;45;0
WireConnection;43;2;44;0
WireConnection;23;0;13;0
WireConnection;88;0;52;0
WireConnection;88;1;91;0
WireConnection;34;0;43;0
WireConnection;34;1;36;0
WireConnection;77;0;44;0
WireConnection;77;1;79;0
WireConnection;77;2;80;0
WireConnection;77;3;81;0
WireConnection;77;4;82;0
WireConnection;93;0;103;0
WireConnection;93;1;88;0
WireConnection;31;0;34;0
WireConnection;31;1;33;0
WireConnection;25;0;23;0
WireConnection;25;1;24;0
WireConnection;63;2;61;2
WireConnection;60;0;93;0
WireConnection;83;1;84;0
WireConnection;83;0;77;0
WireConnection;18;0;34;0
WireConnection;18;1;31;0
WireConnection;18;2;25;0
WireConnection;0;10;83;0
WireConnection;0;13;18;0
WireConnection;0;11;60;0
ASEEND*/
//CHKSM=571D005D2627B0CA46427F501BEED60315AC5F97