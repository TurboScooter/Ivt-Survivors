Shader "Quantum/Particles/Visualizer"{
	Properties{
		_ShaderVersionVis ("Shader Version", Integer) = -1
		// ============================== BASE SETTINGS ============================== //

		// SHAPE TEXTURE //
		_InputTex ("Input Data", 2D) = "Black" {}
		_InputTexPos ("Input Position", Vector) = (0, 0, 0, 0)
		_InputTexRot ("Input Rotation", Vector) = (0, 0, 0, 0)
		[HideInInspector] _InputTexRotMat1 ("Input Rotation Matrix 1", Vector) = (1, 0, 0, 0)
		[HideInInspector] _InputTexRotMat2 ("Input Rotation Matrix 2", Vector) = (0, 1, 0, 0)
		[HideInInspector] _InputTexRotMat3 ("Input Rotation Matrix 3", Vector) = (0, 0, 1, 0)
		_InputTexScale ("Input Scale", Vector) = (1, 1, 1, 1)

		// SIMULATION //
		_SimCount ("Simulator Count", Integer) = 2.0
		_SimSpeed ("Simulation Speed", Range(0.0, 1000.0)) = 1.0
		_SimScale ("Simulation Scale", Range(0.00001, 1000.0)) = 1.0
		_RandSeed ("Random Seed", Float) = 29399

		// ============================== RENDER SETTINGS ============================== //

		_BlendOp ("BlendOp", Float) = 0.0
		_SrcBlend ("SrcBlend", Float) = 5.0
		_DstBlend ("DstBlend", Float) = 1.0
		_ZTest ("ZTest", Float) = 4
		[Toggle]_ZWrite ("ZWrite", Int) = 0.0

		// ============================== COLOR & TEXTURE SETTINGS ============================== //

		// COLOR MODE //
		[HDR]_Color0 ("Color3", Color) = (0.1, 0.1, 0.1, 1.0)
		[HDR]_Color1 ("Color1", Color) = (0.1, 0.1, 1.0, 1.0)
		[HDR]_Color2 ("Color2", Color) = (0.1, 1.0, 0.1, 1.0)
		[HDR]_Color3 ("Color3", Color) = (1.0, 0.1, 0.1, 1.0)
		_ColorCycleLength ("Cycle Length", Range(0.001, 1000.0)) = 1.0
		_ColorMaxSpeed ("Max Speed", Range(0.001, 1000.0)) = 0.003
		[NoScaleOffset]_ColorGradTex ("Gradient Texture", 2D) = "White" {}
		[NoScaleOffset]_ColorTex ("Color Texture", 2D) = "White" {}
		_TexAnimLength ("Animation Length", Range(0.001, 1000.0)) = 1.0
		// FALLOFF MODE //
		[NoScaleOffset] _FalloffTex ("Falloff Texture", 2D) = "White" {}
		// TEXTURE //
		_ParticleTex ("Particle Texture", 2D) = "White" {}
		_TextureAlphaCutoff ("Texture Alpha Cutoff", Range(0.0, 1.0)) = 0.1

		// ============================== SHAPE & SIZE SETTINGS ============================== //

		// BASE SIZE //
		_MinSize ("Min Size", Range(0.0, 1)) = 0.001
		_MaxSize ("Max Size", Range(0.0, 1)) = 0.001
		_MinSize2D ("Min Size", Vector) = (0.001, 0.001, 0, 0)
		_MaxSize2D ("Max Size", Vector) = (0.001, 0.001, 0, 0)
		// SIZE MODE SETTINGS //
		_FlickerLength ("Flicker Length", Range(0.001, 1000.0)) = 1.0
		_SizeMaxSpeed ("Max Speed", Range(0.001, 1000.0)) = 0.003
		[NoScaleOffset]_SizeCurveTex ("Size Curve", 2D) = "Black" {}
		[NoScaleOffset]_SizeCurveTex2D ("Size Curve", 2D) = "Black" {}
		// LINE SHAPE SETTINGS //
		_LineLengthLimit ("Line Length Limit", Range(0.0, 100.0)) = 0.1
		_LinePos ("Line Position", Vector) = (0, 0, 0, 0)
		_LineLength ("Line Length", Range(0.0, 100)) = 1.0
		_LineOffset ("Line Offset", Float) = 1.0
		[NoScaleOffset]_LineOffTex ("Line Connection Offsets", 2D) = "Black" {}

		// ============================== ROTATION SETTINGS ============================== //

		_BaseRotMin ("BaAnimRotion Min", Vector) = (0.0, 0.0, 0.0, 0.0)
		_BaseRotMax ("Base Rotation Max", Vector) = (0.0, 0.0, 0.0, 0.0)
		_AnimRotMin ("Rotation Min", Vector) = (0.0, 0.0, 0.0, 0.0)
		_AnimRotMax ("Rotation Max", Vector) = (0.0, 0.0, 0.0, 0.0)
		_AnimRotLength ("Rotation Animation Length", Range(0.001, 1000.0)) = 1.0
		[NoScaleOffset]_RotAnimCurveTex ("Rotation Curve", 2D) = "Black" {}
	}
	SubShader{
		Tags { "Lightmode" = "Forwardbase" "Queue" = "Transparent+1" "RenderType" = "Transparent" }
		LOD 100

		Pass{
			Name "VisualizerMainPass"
			Tags { "Lightmode" = "Forwardbase" "Queue" = "Transparent+1" "RenderType" = "Transparent" }

			Cull Off
			BlendOp [_BlendOp]
			Blend [_SrcBlend] [_DstBlend]
			ZTest [_ZTest]
			ZWrite [_ZWrite]

			CGPROGRAM
			// ============================== BASE SETTINGS ============================== //
			#pragma shader_feature_local INPUT_TEX_WORLD_SPACE INPUT_TEX_LOCAL_SPACE

			// ============================== COLOR & TEXTURE SETTINGS ============================== //
			#pragma shader_feature_local COLOR_MODE_STATIC COLOR_MODE_PULSE COLOR_MODE_GRADIENT COLOR_MODE_DIRECTION COLOR_MODE_VELOCITY COLOR_MODE_LIFETIME COLOR_MODE_TEXTURE
			#pragma shader_feature_local COLOR_MODE_DIRECTION_NORMALIZE
			#pragma shader_feature_local COLOR_ANIM_SYNC
			#pragma shader_feature_local COLOR_FACTOR_CLAMP
			#pragma shader_feature_local COLOR_FALLOFF_NONE COLOR_FALLOFF_ROOT COLOR_FALLOFF_LINEAR COLOR_FALLOFF_SQUARED COLOR_FALLOFF_HALFSINE COLOR_FALLOFF_SINE COLOR_FALLOFF_GRADIENT
			#pragma shader_feature_local COLOR_USE_TEXTURE
			#pragma shader_feature_local COLOR_ANIM_TEX_OFF COLOR_ANIM_TEX_ON COLOR_ANIM_TEX_SYNCHED COLOR_ANIM_TEX_LIFETIME
			#pragma shader_feature_local COLOR_ANIM_TEX_FACTOR_CLAMP
			#pragma shader_feature_local RENDER_AFTER_LIFETIME

			// ============================== SHAPE & SIZE SETTINGS ============================== //
			#pragma shader_feature_local SHAPE_MODE_POINT SHAPE_MODE_LINE SHAPE_MODE_TRIANGLE SHAPE_MODE_SQUARE SHAPE_MODE_CIRCLE
			#pragma shader_feature_local SHAPE_LINE_DRAW_PARTIAL
			#pragma shader_feature_local SHAPE_LINE_TYPE_ABSOLUTE SHAPE_LINE_TYPE_RELATIVE SHAPE_LINE_TYPE_DIRECTION SHAPE_LINE_TYPE_PARTICLE
			#pragma shader_feature_local SHAPE_LINE_INVERT_DIR
			#pragma shader_feature_local SHAPE_LINE_WORLD_SPACE SHAPE_LINE_LOCAL_SPACE
			#pragma shader_feature_local SHAPE_LINE_VELOCITY_NORMALIZE
			#pragma shader_feature_local SHAPE_LINE_OFFSET_STATIC SHAPE_LINE_OFFSET_DYNAMIC SHAPE_LINE_OFFSET_TEXTURE SHAPE_LINE_OFFSET_RANDOM
			#pragma shader_feature_local SHAPE_SIZE_2D_VALUES
			#pragma shader_feature_local SHAPE_SIZE_CONSTANT SHAPE_SIZE_FLICKER SHAPE_SIZE_FLICKER_CURVE SHAPE_SIZE_LIFETIME SHAPE_SIZE_LIFETIME_CURVE SHAPE_SIZE_VELOCITY SHAPE_SIZE_VELOCITY_CURVE
			#pragma shader_feature_local SHAPE_SIZE_SYNC
			#pragma shader_feature_local SHAPE_SIZE_FACTOR_CLAMP
			#pragma shader_feature_local SHAPE_SIZE_SCREEN_SPACE

			// ============================== ROTATION SETTINGS ============================== //
			#pragma shader_feature_local ROT_ON
			#pragma shader_feature_local ROT_SPACE_WORLD ROT_SPACE_LOCAL ROT_SPACE_SCREEN ROT_SPACE_DIR
			#pragma shader_feature_local ROT_BASE_SINGLE ROT_BASE_RANGE
			#pragma shader_feature_local ROT_BASE_SEP_AXES
			#pragma shader_feature_local ROT_ANIM_NONE ROT_OVER_TIME_SINGLE ROT_OVER_TIME_RANGE ROT_OSCILLATE_RANGE ROT_LOOP_CURVE ROT_LIFETIME_RANGE ROT_LIFETIME_CURVE
			#pragma shader_feature_local ROT_ANIM_SEP_AXES
			#pragma shader_feature_local ROT_ANIM_SYNC
			#pragma shader_feature_local ROT_ANIM_FACTOR_CLAMP

			#pragma target 5.0
			#pragma vertex vert
			#pragma geometry geom
			#pragma fragment frag

			#include "Lib/QuantumParticles.cginc"
			#include "Lib/QuantumNoise.cginc"

			struct appdata{
				float4 pos : POSITION;
			};

			struct v2g{
				float4 pos : POSITION;
			};

			struct g2f{
				float4 pos : SV_POSITION;
				float4 col : COLOR;
				#if defined(COLOR_USE_TEXTURE) && !defined(SHAPE_MODE_POINT) && !defined(SHAPE_MODE_LINE)
					float3 uvPos : TEXCOORD0;
				#else
					float2 uvPos : TEXCOORD0;
				#endif
			};

			// ============================== BASE SETTINGS ============================== //
			// SHAPE TEXTURE //
			uniform sampler2D_float _InputTex;
			uniform float4 _InputTex_TexelSize;
			#if defined(INPUT_TEX_LOCAL_SPACE)
				uniform float3 _InputTexPos;
				uniform float3 _InputTexRotMat1;
				uniform float3 _InputTexRotMat2;
				uniform float3 _InputTexRotMat3;
				uniform float3 _InputTexScale;
			#endif
			// SIMULATION //
			// uniform uint _SimCount; // already defined in QuantumParticles.cginc
			// uniform float _SimSpeed; // already defined in QuantumParticles.cginc
			// uniform float _SimScale; // already defined in QuantumParticles.cginc
			uniform float _RandSeed;

			// ============================== COLOR & TEXTURE SETTINGS ============================== //
			// COLOR MODE //
			#if defined(COLOR_MODE_STATIC)
				uniform float4 _Color1;

			#elif defined(COLOR_MODE_PULSE)
				uniform float4 _Color1;
				uniform float4 _Color2;
				uniform float _ColorCycleLength;

			#elif defined(COLOR_MODE_GRADIENT)
				uniform sampler2D _ColorGradTex;
				uniform float _ColorCycleLength;

			#elif defined(COLOR_MODE_DIRECTION)
				uniform float4 _Color0;
				uniform float4 _Color1;
				uniform float4 _Color2;
				uniform float4 _Color3;
				#if !defined(COLOR_MODE_DIRECTION_NORMALIZE)
					uniform float _ColorMaxSpeed;
				#endif
				
			#elif defined(COLOR_MODE_VELOCITY)
				uniform sampler2D _ColorGradTex;
				uniform float _ColorMaxSpeed;

			#elif defined(COLOR_MODE_LIFETIME)
				uniform sampler2D _ColorGradTex;

			#elif defined(COLOR_MODE_TEXTURE)
				uniform float4 _Color1;
				uniform sampler2D _ColorTex;
			#endif

			// FALLOFF MODE //
			#if defined(COLOR_FALLOFF_GRADIENT)
				uniform sampler2D _FalloffTex;
			#endif

			// TEXTURE //
			#if defined(COLOR_USE_TEXTURE)
				uniform sampler2D _ParticleTex;
				uniform float4 _ParticleTex_ST;
				uniform float _TextureAlphaCutoff;
			#endif
			#if defined(COLOR_ANIM_TEX_ON) || defined(COLOR_ANIM_TEX_SYNCHED)
				uniform float _TexAnimLength;
			#endif

			// ============================== SHAPE & SIZE SETTINGS ============================== //
			#if defined(SHAPE_MODE_LINE)
				// LINE SHAPE SETTINGS //
				uniform float _LineLengthLimit;
				#if defined(SHAPE_LINE_TYPE_ABSOLUTE) || defined(SHAPE_LINE_TYPE_RELATIVE)
					uniform float3 _LinePos;
				#elif defined(SHAPE_LINE_TYPE_DIRECTION)
					uniform float _LineLength;
				#elif defined(SHAPE_LINE_TYPE_PARTICLE)
					#if defined(SHAPE_LINE_OFFSET_STATIC) || defined(SHAPE_LINE_OFFSET_DYNAMIC) || defined(SHAPE_LINE_OFFSET_RANDOM)
						uniform float _LineOffset;
					#elif defined(SHAPE_LINE_OFFSET_TEXTURE)
						uniform sampler2D _LineOffTex;
					#endif
				#endif
			#elif !defined(SHAPE_MODE_POINT)
				#define SQRT3 1.7320508075688772935274463415059
				#define SQRT3_HALF 0.86602540378443864676372317075294
				#if defined(SHAPE_SIZE_2D_VALUES)
					#define SIZE_TYPE float2
					#define _MinSizeType _MinSize2D
					#define _MaxSizeType _MaxSize2D
					#define _SizeCurveTexType _SizeCurveTex2D
				#else
					#define SIZE_TYPE float
					#define _MinSizeType _MinSize
					#define _MaxSizeType _MaxSize
					#define _SizeCurveTexType _SizeCurveTex
				#endif

				// BASE SIZE //
				uniform SIZE_TYPE _MinSizeType;

				// SIZE MODE SETTINGS //
				#if !defined(SHAPE_SIZE_CONSTANT)
					uniform SIZE_TYPE _MaxSizeType;

					#if defined(SHAPE_SIZE_FLICKER) || defined(SHAPE_SIZE_FLICKER_CURVE)
						uniform float _FlickerLength;
					#endif
		
					#if defined(SHAPE_SIZE_FLICKER_CURVE) || defined(SHAPE_SIZE_LIFETIME_CURVE) || defined(SHAPE_SIZE_VELOCITY_CURVE)
						uniform sampler2D _SizeCurveTexType;
					#endif
		
					#if defined(SHAPE_SIZE_VELOCITY) || defined(SHAPE_SIZE_VELOCITY_CURVE)
						uniform float _SizeMaxSpeed;
					#endif
				#endif
			#endif

			// ============================== ROTATION SETTINGS ============================== //
			#if defined(ROT_ON) && !defined(SHAPE_MODE_POINT) && !defined(SHAPE_MODE_LINE)
				uniform float3 _BaseRotMin;
				#if defined(ROT_BASE_RANGE)
					uniform float3 _BaseRotMax;
				#endif
				#if defined(ROT_OVER_TIME_SINGLE)
					uniform float3 _AnimRotMin;

				#elif defined(ROT_OVER_TIME_RANGE)
					uniform float3 _AnimRotMin;
					uniform float3 _AnimRotMax;

				#elif defined(ROT_OSCILLATE_RANGE)
					uniform float3 _AnimRotMin;
					uniform float3 _AnimRotMax;
					uniform float _AnimRotLength;

				#elif defined(ROT_LOOP_CURVE)
					uniform float _AnimRotLength;
					uniform sampler2D _RotAnimCurveTex;

				#elif defined(ROT_LIFETIME_RANGE)
					uniform float3 _AnimRotMin;
					uniform float3 _AnimRotMax;

				#elif defined(ROT_LIFETIME_CURVE)
					uniform sampler2D _RotAnimCurveTex;
				#endif
			#endif

			v2g vert (appdata v){
				v2g o;
				o.pos = v.pos;
				return o;
			}

			float4 getParticleColor(inout float seed, float normID, float2 uv, float3 vel, float age){
				#if defined(COLOR_MODE_STATIC)
					return _Color1;

				#elif defined(COLOR_MODE_PULSE)
					#if defined(COLOR_ANIM_SYNC)
						return lerp(_Color1, _Color2, sin(adjustedTime() / _ColorCycleLength * UNITY_TWO_PI) * 0.5 + 0.5);
					#else
						return lerp(_Color1, _Color2, sin((adjustedTime() / _ColorCycleLength + q_rand_1(seed, normID)) * UNITY_TWO_PI) * 0.5 + 0.5);
					#endif

				#elif defined(COLOR_MODE_GRADIENT)
					#if defined(COLOR_ANIM_SYNC)
						float s = frac(adjustedTime() / _ColorCycleLength);
					#else
						float s = frac(adjustedTime() / _ColorCycleLength + q_rand_1(seed, normID));
					#endif
					return tex2Dlod(_ColorGradTex, float4(s, q_rand_1(seed, normID), 0.0, 0.0));

				#elif defined(COLOR_MODE_DIRECTION)
					const float3 v = safeNormalize(abs(vel.xyz));
					const float4 c = (v.x * _Color1 + v.y * _Color2 + v.z * _Color3);
					#if defined(COLOR_MODE_DIRECTION_NORMALIZE)
						return lerp(_Color0, c, length(v));
					#else
						#if defined(COLOR_FACTOR_CLAMP)
							return lerp(_Color0, c, min(length(vel.xyz) / _ColorMaxSpeed, 1.0));
						#else
							const float f = length(vel.xyz) / _ColorMaxSpeed;
							return c * f + _Color0 * max(1.0 - f, 0.0);
						#endif
					#endif

				#elif defined(COLOR_MODE_VELOCITY)
					#if defined(COLOR_FACTOR_CLAMP)
						return tex2Dlod(_ColorGradTex, float4(textureClamp(length(vel.xyz) / _ColorMaxSpeed), q_rand_1(seed, normID), 0.0, 0.0));
					#else
						return tex2Dlod(_ColorGradTex, float4(length(vel.xyz) / _ColorMaxSpeed, q_rand_1(seed, normID), 0.0, 0.0));
					#endif

				#elif defined(COLOR_MODE_LIFETIME)
					#if defined(COLOR_FACTOR_CLAMP)
						return tex2Dlod(_ColorGradTex, float4(textureClamp(age), q_rand_1(seed, normID), 0.0, 0.0));
					#else
						return tex2Dlod(_ColorGradTex, float4(age, q_rand_1(seed, normID), 0.0, 0.0));
					#endif

				#elif defined(COLOR_MODE_TEXTURE)
					return _Color1 * tex2Dlod(_ColorTex, float4(uv.x, uv.y * 2.0, 0.0, 0.0));
				#endif
			}

			#if !defined(SHAPE_MODE_POINT) && !defined(SHAPE_MODE_LINE)
			SIZE_TYPE getParticleSize(inout float seed, float normID, float3 pos, float3 vel, float age){
				#if defined(SHAPE_SIZE_CONSTANT)
					SIZE_TYPE size = _MinSizeType;
				#else
					#if defined(SHAPE_SIZE_FLICKER)
						#if defined(SHAPE_SIZE_SYNC)
							const float sizeFactor = sin(adjustedTime() / _FlickerLength * UNITY_TWO_PI) * 0.5 + 0.5;
						#else
							const float sizeFactor = sin((adjustedTime() / _FlickerLength + q_rand_1(seed, normID)) * UNITY_TWO_PI) * 0.5 + 0.5;
						#endif

					#elif defined(SHAPE_SIZE_FLICKER_CURVE)
						#if defined(SHAPE_SIZE_SYNC)
							const SIZE_TYPE sizeFactor = tex2Dlod(_SizeCurveTexType, float4((adjustedTime() / _FlickerLength) % 1.0, 0.5, 0.0, 0.0));
						#else
							const SIZE_TYPE sizeFactor = tex2Dlod(_SizeCurveTexType, float4((adjustedTime() / _FlickerLength + q_rand_1(seed, normID)) % 1.0, 0.5, 0.0, 0.0));
						#endif

					#elif defined(SHAPE_SIZE_LIFETIME)
						#if defined(SHAPE_SIZE_FACTOR_CLAMP)
							const float sizeFactor = clamp(age, 0.0f, 1.0f);
						#else
							const float sizeFactor = age;
						#endif
						
					#elif defined(SHAPE_SIZE_LIFETIME_CURVE)
						#if defined(SHAPE_SIZE_FACTOR_CLAMP)
							const SIZE_TYPE sizeFactor = tex2Dlod(_SizeCurveTexType, float4(textureClamp(age), 0.5, 0.0, 0.0));
						#else
							const SIZE_TYPE sizeFactor = tex2Dlod(_SizeCurveTexType, float4(age, 0.5, 0.0, 0.0));
						#endif

					#elif defined(SHAPE_SIZE_VELOCITY)
						#if defined(SHAPE_SIZE_FACTOR_CLAMP)
							const float sizeFactor = clamp(length(vel.xyz) / _SizeMaxSpeed, 0.0f, 1.0f);
						#else
							const float sizeFactor = length(vel.xyz) / _SizeMaxSpeed;
						#endif

					#elif defined(SHAPE_SIZE_VELOCITY_CURVE)
						#if defined(SHAPE_SIZE_FACTOR_CLAMP)
							const SIZE_TYPE sizeFactor = tex2Dlod(_SizeCurveTexType, float4(textureClamp(length(vel.xyz) / _SizeMaxSpeed), 0.5, 0.0, 0.0));
						#else
							const SIZE_TYPE sizeFactor = tex2Dlod(_SizeCurveTexType, float4(length(vel.xyz) / _SizeMaxSpeed, 0.5, 0.0, 0.0));
						#endif
					#endif

					SIZE_TYPE size = lerp(_MinSizeType, _MaxSizeType, sizeFactor);
				#endif

				#if defined(SHAPE_SIZE_SCREEN_SPACE)
					size *= distance(pos.xyz, _WorldSpaceCameraPos.xyz);
				#else
					size = simToWorldScale(size);
				#endif

				return size;
			}
			#endif

			#if defined(ROT_ON) && !defined(SHAPE_MODE_POINT) && !defined(SHAPE_MODE_LINE)
			float3x3 getParticleRotation(inout float seed, float normID, float age){
				//base rotation
				#if defined(ROT_BASE_RANGE)
					#if defined(ROT_BASE_SEP_AXES)
						float3 rot = lerp(_BaseRotMin, _BaseRotMax, q_rand_3(seed, normID));
					#else
						float3 rot = lerp(_BaseRotMin, _BaseRotMax, q_rand_1(seed, normID));
					#endif
					#else
					float3 rot = _BaseRotMin;
				#endif

				//animated rotation
				#if defined(ROT_OVER_TIME_SINGLE)
					rot += _AnimRotMin * adjustedTime();

				#elif defined(ROT_OVER_TIME_RANGE)
					#if defined(ROT_ANIM_SEP_AXES)
						rot += lerp(_AnimRotMin, _AnimRotMax, q_rand_3(seed, normID)) * adjustedTime();
					#else
						rot += lerp(_AnimRotMin, _AnimRotMax, q_rand_1(seed, normID)) * adjustedTime();
					#endif

				#elif defined(ROT_OSCILLATE_RANGE)
					#if defined(ROT_ANIM_SYNC)
						rot += lerp(_AnimRotMin, _AnimRotMax, sin(adjustedTime() / _AnimRotLength * UNITY_TWO_PI) * 0.5 + 0.5);
					#else
						rot += lerp(_AnimRotMin, _AnimRotMax, sin((adjustedTime() / _AnimRotLength + q_rand_1(seed, normID)) * UNITY_TWO_PI) * 0.5 + 0.5);
					#endif

				#elif defined(ROT_LOOP_CURVE)
					#if defined(ROT_ANIM_SYNC)
						rot += tex2Dlod(_RotAnimCurveTex, float4((adjustedTime() / _AnimRotLength) % 1.0, 0.5, 0.0, 0.0)).xyz;
					#else
						rot += tex2Dlod(_RotAnimCurveTex, float4((adjustedTime() / _AnimRotLength + q_rand_1(seed, normID)) % 1.0, 0.5, 0.0, 0.0)).xyz;
					#endif

				#elif defined(ROT_LIFETIME_RANGE)
					#if defined(ROT_ANIM_FACTOR_CLAMP)
						rot += lerp(_AnimRotMin, _AnimRotMax, clamp(age, 0.0, 1.0));
					#else
						rot += lerp(_AnimRotMin, _AnimRotMax, age);
					#endif

				#elif defined(ROT_LIFETIME_CURVE)
					#if defined(ROT_ANIM_FACTOR_CLAMP)
						rot += tex2Dlod(_RotAnimCurveTex, float4(textureClamp(age), 0.5, 0.0, 0.0)).xyz;
					#else
						rot += tex2Dlod(_RotAnimCurveTex, float4(age, 0.5, 0.0, 0.0)).xyz;
					#endif
				#endif

				rot = frac(rot) * UNITY_TWO_PI;
				//calculate rotation matrix
				float3x3 rotX = float3x3(1, 0, 0, 0, cos(rot.x), -sin(rot.x), 0, sin(rot.x), cos(rot.x));
				float3x3 rotY = float3x3(cos(rot.y), 0, sin(rot.y), 0, 1, 0, -sin(rot.y), 0, cos(rot.y));
				float3x3 rotZ = float3x3(cos(rot.z), -sin(rot.z), 0, sin(rot.z), cos(rot.z), 0, 0, 0, 1);
				return mul(rotX, mul(rotY, rotZ));
			}
			#endif

			#if defined(SHAPE_MODE_POINT)
			[maxvertexcount(1)]
            void geom(point v2g IN[1], uint primID : SV_PrimitiveID, inout PointStream<g2f> pointstream){
			#elif defined(SHAPE_MODE_LINE)
			[maxvertexcount(2)]
			void geom(point v2g IN[1], uint primID : SV_PrimitiveID, inout LineStream<g2f> linestream){
			#else
			[maxvertexcount(3)]
            void geom(point v2g IN[1], uint primID : SV_PrimitiveID, inout TriangleStream<g2f> tristream){
			#endif
                g2f o;

				// skip if particle data is used for a simulator
				if(primID < _SimCount) return;

				// GET NECESSARY DATA
				float seed = _RandSeed;
				const uint maxID = maxIDFromTexelSize(_InputTex_TexelSize);
				const uint particleID = primID - _SimCount;
				const float normID = float(particleID) / float(maxID);
				const float2 uv = uvFromSimID(primID, _InputTex_TexelSize);

				// INPUT SETUP
				#if defined(INPUT_TEX_WORLD_SPACE)
					const float4 pos = samplePosGlobal(_InputTex, uv.xy);
					const float4 vel = sampleVelGlobal(_InputTex, uv.xy);
				#elif defined(INPUT_TEX_LOCAL_SPACE)
					const float3x3 inputTexRotMat = float3x3(_InputTexRotMat1, _InputTexRotMat2, _InputTexRotMat3);
					const float4 pos = samplePosLocal(_InputTex, uv.xy, _InputTexPos, inputTexRotMat, _InputTexScale);
					const float4 vel = sampleVelLocal(_InputTex, uv.xy, inputTexRotMat, _InputTexScale);
				#endif

				// GET SECOND POSITION FOR LINES
				#if defined(SHAPE_MODE_LINE)
					#if defined(SHAPE_LINE_TYPE_ABSOLUTE)
						#if defined(SHAPE_LINE_WORLD_SPACE)
							const float4 pos2 = float4(_LinePos, 1.0);
						#elif defined(SHAPE_LINE_LOCAL_SPACE)
							const float4 pos2 = unscaledMul(unity_ObjectToWorld, float4(simToWorldScale(_LinePos), 1.0));
						#endif
					#elif defined(SHAPE_LINE_TYPE_RELATIVE)
						#if defined(SHAPE_LINE_WORLD_SPACE)
							const float4 pos2 = pos + float4(simToWorldScale(_LinePos), 0.0);
						#elif defined(SHAPE_LINE_LOCAL_SPACE)
							const float4 pos2 = pos + float4(unscaledMul(unity_ObjectToWorld, simToWorldScale(_LinePos)), 0.0);
						#endif
					#elif defined(SHAPE_LINE_TYPE_DIRECTION)
						#if defined(SHAPE_LINE_VELOCITY_NORMALIZE)
							const float4 pos2 = pos + float4(simToWorldScale(safeNormalize(vel.xyz)) * _LineLength, 0.0);
						#else
							const float4 pos2 = pos + float4(simToWorldScale(vel.xyz) * _LineLength, 0.0);
						#endif
					#elif defined(SHAPE_LINE_TYPE_PARTICLE)
						// GET SECOND PARTICLE
						#if defined(SHAPE_LINE_OFFSET_STATIC)
							const int IDoffset = _LineOffset;
						#elif defined(SHAPE_LINE_OFFSET_DYNAMIC)
							const int IDoffset = _LineOffset * adjustedTime();
						#elif defined(SHAPE_LINE_OFFSET_TEXTURE)
							const int IDoffset = sampleOffsetTex(_LineOffTex, uv.xy);
						#elif defined(SHAPE_LINE_OFFSET_RANDOM)
							float offsetSeed = _RandSeed-100;
							const float sampleID = q_rand_1(offsetSeed, floor(normID + _LineOffset * adjustedTime()) / maxID);
							const int IDoffset = q_rand_1(offsetSeed, sampleID) * maxID;
						#endif

						const uint particleID2 = offsetParticleID(particleID, IDoffset, maxID);
						const float normID2 = float(particleID2) / float(maxID);
						const float2 uv2 = uvFromParticleID(particleID2, _InputTex_TexelSize);

						// INPUT SETUP
						#if defined(INPUT_TEX_WORLD_SPACE)
							const float4 pos2 = samplePosGlobal(_InputTex, uv2.xy);
							const float4 vel2 = sampleVelGlobal(_InputTex, uv2.xy);
						#elif defined(INPUT_TEX_LOCAL_SPACE)
							const float4 pos2 = samplePosLocal(_InputTex, uv2.xy, _InputTexPos, inputTexRotMat, _InputTexScale);
							const float4 vel2 = sampleVelLocal(_InputTex, uv2.xy, inputTexRotMat, _InputTexScale);
						#endif
						const float age2 = pos2.w;
					#endif
				#endif


				const float age = pos.w;
				#if !defined(RENDER_AFTER_LIFETIME)
					#if (defined(SHAPE_MODE_LINE) && defined(SHAPE_LINE_TYPE_PARTICLE))
						if(age >= 1.0 || age2 >= 1.0) return;
					#else
						if(age >= 1.0) return;
					#endif
				#endif

				// COLOR SETTINGS
				#if defined(SHAPE_MODE_LINE) & defined(SHAPE_LINE_TYPE_PARTICLE)
					float col2Seed = seed;
					const float4 col2 = getParticleColor(col2Seed, normID2, uv2, vel2, age2);
				#endif

				const float4 col = getParticleColor(seed, normID, uv, vel, age);

				#if defined(SHAPE_MODE_LINE) & !defined(SHAPE_LINE_TYPE_PARTICLE)
					const float4 col2 = col;
				#endif

				// SHAPE SETTINGS
				#if defined(SHAPE_MODE_POINT)
					// CREATE POINT
					o.uvPos = float2(0.0, 0.0);
					o.pos = mul(UNITY_MATRIX_VP, float4(pos.xyz, 1.0f));
					o.col = col;
					pointstream.Append(o);
					pointstream.RestartStrip();
				#elif defined(SHAPE_MODE_LINE)
					#if !defined(SHAPE_LINE_DRAW_PARTIAL)
						if(distance(pos.xyz, pos2.xyz) > simToWorldScale(_LineLengthLimit)) return;
					#endif
					// CREATE LINE

					// FIRST VERTEX
					o.uvPos = float2(1.0, 0.0);
					o.pos = mul(UNITY_MATRIX_VP, float4(pos.xyz, 1.0));
					o.col = col;
					linestream.Append(o);

					// SECOND VERTEX
					o.uvPos = float2(0.0, 0.0);
					#if defined(SHAPE_LINE_INVERT_DIR)
						o.pos = mul(UNITY_MATRIX_VP, float4(pos.xyz - safeNormalize(pos2.xyz - pos.xyz) * (min(distance(pos.xyz, pos2.xyz), simToWorldScale(_LineLengthLimit))), 1.0));
					#else
						o.pos = mul(UNITY_MATRIX_VP, float4(pos.xyz + safeNormalize(pos2.xyz - pos.xyz) * (min(distance(pos.xyz, pos2.xyz), simToWorldScale(_LineLengthLimit))), 1.0));
					#endif
					o.col = col2;
					linestream.Append(o);

					// FINISH IT UP
					linestream.RestartStrip();
				#else
					#if defined(SHAPE_MODE_TRIANGLE)
						float2 vertices[3] = {float2(-SQRT3_HALF, -0.5), float2(0.0, 1.0), float2(SQRT3_HALF, -0.5)};

					#elif defined(SHAPE_MODE_SQUARE)
						float2 vertices[3] = {float2(-(SQRT3_HALF + 1.5), -(SQRT3_HALF + 0.5)), float2(0.0, SQRT3 + 1.0), float2(SQRT3_HALF + 1.5, -(SQRT3_HALF + 0.5))};

					#elif defined(SHAPE_MODE_CIRCLE)
						float2 vertices[3] = {float2(-SQRT3, -1.0), float2(0.0, 2.0), float2(SQRT3, -1.0)};
					#endif

					// SIZE SETTINGS
					SIZE_TYPE size = getParticleSize(seed, normID, pos, vel, age);

					// ROTATION SETTINGS
					#if defined(ROT_ON)
						float3x3 rot = getParticleRotation(seed, normID, age);
						#if defined(ROT_SPACE_DIR)
							// if the velocity is zero, we use the camera up vector as the forward direction
							float3 safeVel = lerp(vel.xyz, mul(unity_CameraToWorld, float4(0.0, 1.0, 0.0, 0.0)).xyz, (1.0 - step(0.001, length(vel.xyz))));
							float3 sideDir = safeNormalize(cross(safeVel, pos.xyz - _WorldSpaceCameraPos.xyz));
							float3 upDir = safeNormalize(safeVel);
							float3 forwardDir = safeNormalize(cross(sideDir, upDir));
							float3x3 rotDir = transpose(float3x3(sideDir, upDir, forwardDir));
						#endif
					#endif

					// SPRITE SETTINGS
					#if defined(COLOR_USE_TEXTURE)
						#if defined(COLOR_ANIM_TEX_OFF)
							float spriteFactor = q_rand_1(seed, normID);

						#elif defined(COLOR_ANIM_TEX_ON)
							float spriteFactor = q_rand_1(seed, normID) + adjustedTime() / _TexAnimLength;

						#elif defined(COLOR_ANIM_TEX_SYNCHED)
							float spriteFactor = adjustedTime() / _TexAnimLength;

						#elif defined(COLOR_ANIM_TEX_LIFETIME)
							#if defined(COLOR_ANIM_TEX_FACTOR_CLAMP)
								float spriteFactor = textureClamp(age);
							#else
								float spriteFactor = age;
							#endif
						#endif

						const int spriteCount = int(_ParticleTex_ST.x) * int(_ParticleTex_ST.y);
						const float sprite_offset = posMod(int(spriteFactor * float(spriteCount)), spriteCount) + 0.5;
					#endif

					// CREATE TRIANGLE
					for(uint vIndex = 0; vIndex < vertices.Length; vIndex++){
						#if defined(COLOR_USE_TEXTURE)
							o.uvPos = float3(vertices[vIndex], sprite_offset);
						#else
							o.uvPos = vertices[vIndex];
						#endif
						#if defined(ROT_ON)
							#if defined(ROT_SPACE_WORLD)
								o.pos = mul(UNITY_MATRIX_VP, float4(pos.xyz, 1.0f) + float4(mul(rot, float3(o.uvPos.xy * size, 0.0)), 0.0));
							#elif defined(ROT_SPACE_LOCAL)
								o.pos = mul(UNITY_MATRIX_VP, float4(pos.xyz, 1.0f) + float4(unscaledMul(unity_ObjectToWorld, mul(rot, float3(o.uvPos.xy * size, 0.0))), 0.0));
							#elif defined(ROT_SPACE_SCREEN)
								o.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_V, float4(pos.xyz, 1.0f)) + float4(mul(rot, float3(o.uvPos.xy * size, 0.0)), 0.0));
							#elif defined(ROT_SPACE_DIR)
								o.pos = mul(UNITY_MATRIX_VP, float4(pos.xyz, 1.0f) + float4(mul(rotDir, mul(rot, float3(o.uvPos.xy * size, 0.0))), 0.0));
							#endif
						#else
							o.pos = mul(UNITY_MATRIX_P, mul(UNITY_MATRIX_V, float4(pos.xyz, 1.0f)) + float4(o.uvPos.xy * size, 0.0, 0.0));
						#endif

						o.col = col;

						tristream.Append(o);
					}
					tristream.RestartStrip();
				#endif
            }

			fixed4 frag (g2f i) : SV_Target{
				// APPLY COLOR
				float4 color = i.col;

				// APPLY ALPHA FALLOFF & SHAPE CUTOFF
				#if !defined(SHAPE_MODE_POINT)
					#if defined(SHAPE_MODE_LINE) && !defined(COLOR_FALLOFF_NONE)
						float edgeDist = i.uvPos.x;
					#elif defined(SHAPE_MODE_TRIANGLE) && !defined(COLOR_FALLOFF_NONE)
						// taken from: https://www.shadertoy.com/view/Xl2yDW (MIT License applies)
						float2 p = i.uvPos.xy;
						p.x = abs(p.x) - SQRT3_HALF;
						p.y += 0.5;
						if(p.x + SQRT3 * p.y > 0.0) p = float2(p.x - SQRT3 * p.y, -SQRT3 * p.x - p.y) / 2.0;
						p.x -= clamp(p.x, -2.0, 0.0);
						float edgeDist = length(p) * sign(p.y) * 2.0;
					#elif defined(SHAPE_MODE_SQUARE)
						float2 q = abs(i.uvPos.xy) - 1.0;
						float edgeDist = -min(max(q.x, q.y), 0.0) -length(max(q, 0.0));
						clip(edgeDist);
					#elif defined(SHAPE_MODE_CIRCLE)
						float edgeDist = 1.0 - length(i.uvPos.xy);
						clip(edgeDist);
					#endif

					#if !defined(COLOR_FALLOFF_NONE)
						#if defined(COLOR_FALLOFF_ROOT)
							color.a *= sqrt(edgeDist);

						#elif defined(COLOR_FALLOFF_HALFSINE)
							color.a *= sin(edgeDist * UNITY_HALF_PI);

						#elif defined(COLOR_FALLOFF_LINEAR)
							color.a *= edgeDist;

						#elif defined(COLOR_FALLOFF_SINE)
							color.a *= (sin((edgeDist-0.5) * UNITY_PI) + 1.0) / 2.0;

						#elif defined(COLOR_FALLOFF_SQUARED)
							color.a *= edgeDist*edgeDist;

						#elif defined(COLOR_FALLOFF_GRADIENT)
							color.a *= tex2D(_FalloffTex, float2(1.0 - edgeDist, 0.5)).a;
						#endif
					#endif
				#endif

				// APPLY SHAPE
				#if defined(COLOR_USE_TEXTURE) && !defined(SHAPE_MODE_POINT) && !defined(SHAPE_MODE_LINE)
					float2 texUV = i.uvPos.xy / 2.0 + 0.5 + _ParticleTex_ST.zw;
					uint2 tiling = uint2(_ParticleTex_ST.xy);
					uint spriteIndex = uint(i.uvPos.z);
					texUV = (texUV + float2(spriteIndex % tiling.x, spriteIndex / tiling.x)) / tiling;
					float4 texColor = tex2D(_ParticleTex, texUV);
					clip(texColor.a - _TextureAlphaCutoff);
					color *= texColor;
				#endif

				return color;
			}
			ENDCG
		}
	}
	CustomEditor "Quantum.VisualizerInspector"
}
