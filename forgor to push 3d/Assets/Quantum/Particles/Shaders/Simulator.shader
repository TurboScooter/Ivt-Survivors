Shader "Quantum/Particles/Simulator" {
	Properties {
		_ShaderVersionSim ("Shader Version", Integer) = -1
		// ============================== BASE SETTINGS ============================== //
		
		// SHAPE TEXTURE //
		_InputTex ("Input Data", 2D) = "white" {}
		_InputTexPos ("Input Position", Vector) = (0, 0, 0, 0)
		_InputTexRot ("Input Rotation", Vector) = (0, 0, 0, 0)
		[HideInInspector] _InputTexRotMat1 ("Input Rotation Matrix 1", Vector) = (1, 0, 0, 0)
		[HideInInspector] _InputTexRotMat2 ("Input Rotation Matrix 2", Vector) = (0, 1, 0, 0)
		[HideInInspector] _InputTexRotMat3 ("Input Rotation Matrix 3", Vector) = (0, 0, 1, 0)
		_InputTexScale ("Input Scale", Vector) = (1, 1, 1, 1)

		// SIMULATION //
		_SimID ("Simulator ID", Integer) = 0.0
		_SimCount ("Simulator Count", Integer) = 2.0
		_SimSpeed ("Simulation Speed", Range(0.0, 1000.0)) = 1.0
		_SimScale ("Simulation Scale", Range(0.00001, 1000.0)) = 1.0

		// SPEED //
		_SpeedLoss ("Speed Loss", Range(0.0, 1.0)) = 0.5
		_SpeedLimit ("Speed Limit", Range(0.001, 1000.0)) = 1.0

		// ============================== FORCES ============================== //

		// ------------------------------ ATTRACTION ------------------------------ //
		// SHAPE TEXTURE //
		_AttrForceTex ("Shape Texture", 2D) = "Black" {}
		_AttrForceTexPos ("Attraction Position", Vector) = (0, 0, 0, 0)
		_AttrForceTexRot ("Attraction Rotation", Vector) = (0, 0, 0, 0)
		[HideInInspector] _AttrForceTexRotMat1 ("Attraction Rotation Matrix 1", Vector) = (1, 0, 0, 0)
		[HideInInspector] _AttrForceTexRotMat2 ("Attraction Rotation Matrix 2", Vector) = (0, 1, 0, 0)
		[HideInInspector] _AttrForceTexRotMat3 ("Attraction Rotation Matrix 3", Vector) = (0, 0, 1, 0)
		_AttrForceTexScale ("Attraction Scale", Vector) = (1, 1, 1, 1)

		_AttrForceShapeTex ("Shape Texture", 2D) = "Black" {}
		_AttrForceShapeOffset ("Offset", Float) = 1.0
		_AttrForceShapeOffsetTex ("Offset Texture", 2D) = "Black" {}

		// FORCEFIELD //
		_AttrForceFieldStrength ("Field Strength", Range(-100.0, 100.0)) = 0.02
		_AttrForceFieldFalloff ("Strength Falloff", Range(-100.0, 100.0)) = -2.0
		_AttrForceFieldOffset ("Distance Offset", Range(-1.0, 1.0)) = 0.0
		_AttrForceFieldLimit ("Force Limit", Range(0.001, 1000.0)) = 1.0

		// ------------------------------ DIRECTIONAL ------------------------------ //
		_DirForceDirection ("Direction", Vector) = (0, 0, 0, 0)
		_DirForceWidth ("Cone Width", Range(0.0, 100.0)) = 0.0
		_DirForceFunnelInward ("Inward Strength", Range(0.0, 1.0)) = 1.0
		_DirForceFunnelGradient ("Gradient Length", Range(0.0001, 100.0)) = 1.0

		// FORCEFIELD //
		_DirForceFieldStrength ("Field Strength", Range(-100.0, 100.0)) = 0.02
		_DirForceFieldFalloff ("Strength Falloff", Range(-100.0, 100.0)) = 0.0
		_DirForceFieldOffset ("Distance Offset", Range(-1.0, 1.0)) = 0.0
		_DirForceFieldLimit ("Force Limit", Range(0.001, 1000.0)) = 1.0

		// ------------------------------ ROTATIONAL ------------------------------ //
		_RotForceDirection ("Rotation Axis", Vector) = (0, 0, 0, 0)
		_RotForceAngle ("Rotation Angle", Range(-1.0, 1.0)) = 0.0
		_RotForceDistFactor ("Distance Factor", Range(0.0, 1.0)) = 0.0

		// FORCEFIELD //
		_RotForceFieldStrength ("Field Strength", Range(-100.0, 100.0)) = 0.02
		_RotForceFieldFalloff ("Strength Falloff", Range(-100.0, 100.0)) = 0.0
		_RotForceFieldOffset ("Distance Offset", Range(-1.0, 1.0)) = 0.0
		_RotForceFieldLimit ("Force Limit", Range(0.001, 1000.0)) = 1.0

		// ------------------------------ TURBULENCE ------------------------------ //
		_TurbForceDensity ("Noise Density", Vector) = (1, 1, 1, 0)
		_TurbForcePanning ("Panning", Vector) = (0, 0, 0, 0)
		_TurbForceTime ("Change Speed", Float) = 1.0

		// FORCEFIELD //
		_TurbForceFieldStrength ("Field Strength", Range(-100.0, 100.0)) = 0.02
		_TurbForceFieldFalloff ("Strength Falloff", Range(-100.0, 100.0)) = 0.0
		_TurbForceFieldOffset ("Distance Offset", Range(-1.0, 1.0)) = 0.0
		_TurbForceFieldLimit ("Force Limit", Range(0.001, 1000.0)) = 1.0

		// ------------------------------ DRAG ------------------------------ //
		// FORCEFIELD //
		_DragForceFieldStrength ("Field Strength", Range(-100.0, 100.0)) = 0.02
		_DragForceFieldFalloff ("Strength Falloff", Range(-100.0, 100.0)) = 0.0
		_DragForceFieldOffset ("Distance Offset", Range(-1.0, 1.0)) = 0.0
		_DragForceFieldLimit ("Force Limit", Range(0.001, 1000.0)) = 1.0


		// ============================== SPAWNER SETTINGS ============================== //
		// ------------------------------ SINGLE TEXTURE ------------------------------ //
		// SHAPE TEXTURE //
		_SpawnTex ("Spawn Shape", 2D) = "Black" {}
		_SpawnTexPos ("Shape Position", Vector) = (0, 0, 0, 0)
		_SpawnTexRot ("Shape Rotation", Vector) = (0, 0, 0, 0)
		[HideInInspector] _SpawnTexRotMat1 ("Shape Rotation Matrix 1", Vector) = (1, 0, 0, 0)
		[HideInInspector] _SpawnTexRotMat2 ("Shape Rotation Matrix 2", Vector) = (0, 1, 0, 0)
		[HideInInspector] _SpawnTexRotMat3 ("Shape Rotation Matrix 3", Vector) = (0, 0, 1, 0)
		_SpawnTexScale ("Shape Scale", Vector) = (1, 1, 1, 1)

		// ------------------------------ POSITION ------------------------------ //
		// SHAPE TEXTURE //
		_SpawnPosTex ("Spawn Position", 2D) = "Black" {}
		_SpawnPosTexPos ("Spawn Position Position", Vector) = (0, 0, 0, 0)
		_SpawnPosTexRot ("Spawn Position Rotation", Vector) = (0, 0, 0, 0)
		[HideInInspector] _SpawnPosTexRotMat1 ("Spawn Position Rotation Matrix 1", Vector) = (1, 0, 0, 0)
		[HideInInspector] _SpawnPosTexRotMat2 ("Spawn Position Rotation Matrix 2", Vector) = (0, 1, 0, 0)
		[HideInInspector] _SpawnPosTexRotMat3 ("Spawn Position Rotation Matrix 3", Vector) = (0, 0, 1, 0)
		_SpawnPosTexScale ("Spawn Position Scale", Vector) = (1, 1, 1, 1)

		_SpawnPosPoint ("Spawn Position", Vector) = (0, 0, 0, 0)
		_SpawnPosPoint2 ("Spawn Line End", Vector) = (0, 0, 0, 0)
		_SpawnPosLineEndSimID ("Spawn Line End Simulator ID", Integer) = 0.0
		_SpawnPosSphereRadius ("Spawn Sphere Radius", Float) = 1.0

		// ------------------------------ VELOCITY ------------------------------ //
		// SHAPE TEXTURE //
		_SpawnVelTex ("Spawn Velocity", 2D) = "Black" {}
		_SpawnVelTexRot ("Spawn Velocity Rotation", Vector) = (0, 0, 0, 0)
		[HideInInspector] _SpawnVelTexRotMat1 ("Spawn Velocity Rotation Matrix 1", Vector) = (1, 0, 0, 0)
		[HideInInspector] _SpawnVelTexRotMat2 ("Spawn Velocity Rotation Matrix 2", Vector) = (0, 1, 0, 0)
		[HideInInspector] _SpawnVelTexRotMat3 ("Spawn Velocity Rotation Matrix 3", Vector) = (0, 0, 1, 0)
		_SpawnVelTexScale ("Spawn Velocity Scale", Vector) = (1, 1, 1, 1)

		_SpawnVelVal ("Spawn Velocity", Vector) = (0, 0, 0, 0)
		_SpawnVelVal2 ("Spawn Velocity 2", Vector) = (0, 0, 0, 0)
		_SpawnVelSpeed ("Spawn Velocity Speed", Float) = 1.0
		_SpawnVelCentralDist ("Spawn Velocity Central Distance", Float) = 1.0
		
		// ------------------------------ STARTING AGE ------------------------------ //
		_SpawnAgeTex ("Spawn Position", 2D) = "Black" {}
		_SpawnAgeVal ("Spawn Age", Range(0.0, 1000.0)) = 0.0
		_SpawnAgeVal2 ("Spawn Age 2", Range(0.0, 1000.0)) = 1.0

		// ------------------------------ LIFETIME ------------------------------ //
		_SpawnLifeTex ("Spawn Position", 2D) = "Black" {}
		_SpawnLifeVal ("Spawn Lifetime", Range(0.001, 1000.0)) = 1.0
		_SpawnLifeVal2 ("Spawn Lifetime 2", Range(0.001, 1000.0)) = 2.0

		// ------------------------------ RESPAWN ------------------------------ //
		// PARTIAL RESPAWN //
		_SpawnPartialRate ("Spawn Partial Rate", Float) = 1.0
		_SpawnTimeErrorCompensation ("Spawn Time Error Compensation", Range(0.0, 1.0)) = 0.5
		// RESPAWN CONDITIONS //
		_SpawnConditionPosBelow ("Spawn Condition Position Below", Vector) = (-1, -1, -1, 0)
		_SpawnConditionPosAbove ("Spawn Condition Position Above", Vector) = (1, 1, 1, 0)
		_SpawnConditionDistBelow ("Spawn Condition Distance Below", Float) = 0.0
		_SpawnConditionDistAbove ("Spawn Condition Distance Above", Float) = 1.0
		_SpawnConditionVelBelow ("Spawn Condition Velocity Below", Float) = 0.0
		_SpawnConditionVelAbove ("Spawn Condition Velocity Above", Float) = 1.0
		_SpawnConditionAgeAbove ("Spawn Condition Age Above", Float) = 1.0

		// ------------------------------ OFFSETS ------------------------------ //
		_SpawnOffset ("Spawn Texture Offset", Float) = 1.0
		_SpawnOffsetTex ("Spawn Texture Offset Texture", 2D) = "Black" {}

		// ------------------------------ MOVEMENT ------------------------------ //
		_SpawnAddStrength ("Movement Add Strength", Range(0.01, 100.0)) = 1.0

		// ------------------------------ NOISE ------------------------------ //
		_RandSeed ("Random Seed", Float) = 31193
	}
	SubShader {
		Tags { "Lightmode" = "Forwardbase"}
		LOD 100

		Pass {
			Tags { "Lightmode" = "Forwardbase"}
			Cull Off
			CGPROGRAM

			// ============================== BASE SETTINGS ============================== //
			#pragma shader_feature_local INPUT_TEX_WORLD_SPACE INPUT_TEX_LOCAL_SPACE

			// ============================== FORCES ============================== //

			// ------------------------------ ATTRACTION ------------------------------ //
			#pragma shader_feature_local ATTR_FORCE_ON
			#pragma shader_feature_local ATTR_FORCE_POINT ATTR_FORCE_SHAPE ATTR_FORCE_SHAPE_OFFSET ATTR_FORCE_SHAPE_DYNAMIC_OFFSET ATTR_FORCE_SHAPE_INDIVIDUAL_OFFSET
			#pragma shader_feature_local ATTR_FORCE_TEX_WORLD_SPACE ATTR_FORCE_TEX_LOCAL_SPACE
			#pragma shader_feature_local ATTR_FORCE_AFFECT_POSITION ATTR_FORCE_AFFECT_VELOCITY
			#pragma shader_feature_local ATTR_FORCE_PREVENT_OVERSHOOT
			#pragma shader_feature_local ATTR_FORCE_FIELD_UNIFORM ATTR_FORCE_FIELD_DYNAMIC

			// ------------------------------ DIRECTIONAL ------------------------------ //
			#pragma shader_feature_local DIR_FORCE_ON
			#pragma shader_feature_local DIR_FORCE_SHAPE_UNIFORM DIR_FORCE_SHAPE_FUNNEL DIR_FORCE_SHAPE_COILGUN
			#pragma shader_feature_local DIR_FORCE_WORLD_SPACE DIR_FORCE_LOCAL_SPACE
			#pragma shader_feature_local DIR_FORCE_AFFECT_POSITION DIR_FORCE_AFFECT_VELOCITY
			#pragma shader_feature_local DIR_FORCE_FIELD_UNIFORM DIR_FORCE_FIELD_DYNAMIC

			// ------------------------------ ROTATIONAL ------------------------------ //
			#pragma shader_feature_local ROT_FORCE_ON
			#pragma shader_feature_local ROT_FORCE_DIST_POINT ROT_FORCE_DIST_AXIS
			#pragma shader_feature_local ROT_FORCE_WORLD_SPACE ROT_FORCE_LOCAL_SPACE
			#pragma shader_feature_local ROT_FORCE_AFFECT_POSITION ROT_FORCE_AFFECT_VELOCITY
			#pragma shader_feature_local ROT_FORCE_FIELD_UNIFORM ROT_FORCE_FIELD_DYNAMIC

			// ------------------------------ TURBULENCE ------------------------------ //
			#pragma shader_feature_local TURB_FORCE_ON
			#pragma shader_feature_local TURB_FORCE_TIME_DEPENDENT
			#pragma shader_feature_local TURB_FORCE_WORLD_SPACE TURB_FORCE_LOCAL_SPACE
			#pragma shader_feature_local TURB_FORCE_AFFECT_POSITION TURB_FORCE_AFFECT_VELOCITY
			#pragma shader_feature_local TURB_FORCE_FIELD_UNIFORM TURB_FORCE_FIELD_DYNAMIC

			// ------------------------------ DRAG ------------------------------ //
			#pragma shader_feature_local DRAG_FORCE_ON
			#pragma shader_feature_local DRAG_FORCE_POSITION DRAG_FORCE_ROTATION DRAG_FORCE_BOTH
			#pragma shader_feature_local DRAG_FORCE_AFFECT_POSITION DRAG_FORCE_AFFECT_VELOCITY
			#pragma shader_feature_local DRAG_FORCE_FIELD_UNIFORM DRAG_FORCE_FIELD_DYNAMIC


			// ============================== SPAWN SETTINGS ============================== //
			#pragma shader_feature_local SPAWN_ON
			#pragma shader_feature_local SPAWN_SOURCE_SINGLE SPAWN_SOURCE_MULTI

			#pragma shader_feature_local SPAWN_TEX_WORLD_SPACE SPAWN_TEX_LOCAL_SPACE
			#pragma shader_feature_local SPAWN_POS_KEEP SPAWN_POS_TEX SPAWN_POS_POINT SPAWN_POS_LINE SPAWN_POS_SIMLINE SPAWN_POS_SPHERE SPAWN_POS_BOX
			#pragma shader_feature_local SPAWN_POS_TEX_WORLD_SPACE SPAWN_POS_TEX_LOCAL_SPACE
			#pragma shader_feature_local SPAWN_VEL_KEEP SPAWN_VEL_TEX SPAWN_VEL_VAL SPAWN_VEL_RANGE SPAWN_VEL_SPHERE SPAWN_VEL_POSITION
			#pragma shader_feature_local SPAWN_VEL_TEX_WORLD_SPACE SPAWN_VEL_TEX_LOCAL_SPACE
			#pragma shader_feature_local SPAWN_VEL_SEP_AXES
			#pragma shader_feature_local SPAWN_AGE_KEEP SPAWN_AGE_TEX SPAWN_AGE_VAL SPAWN_AGE_RANGE
			#pragma shader_feature_local SPAWN_LIFE_KEEP SPAWN_LIFE_TEX SPAWN_LIFE_VAL SPAWN_LIFE_RANGE

			#pragma shader_feature_local SPAWN_RATE_ALL SPAWN_RATE_PARTIAL
			#pragma shader_feature_local SPAWN_RESPAWN_CONDITIONS SPAWN_RESPAWN_ALWAYS
			#pragma shader_feature_local SPAWN_CONDITION_POS_OFF SPAWN_CONDITION_POS_RANGE SPAWN_CONDITION_POS_DIST_LARGER SPAWN_CONDITION_POS_DIST_SMALLER SPAWN_CONDITION_POS_DIST_RANGE
			#pragma shader_feature_local SPAWN_CONDITION_VEL_OFF SPAWN_CONDITION_VEL_LARGER SPAWN_CONDITION_VEL_SMALLER SPAWN_CONDITION_VEL_RANGE
			#pragma shader_feature_local SPAWN_CONDITION_AGE_OFF SPAWN_CONDITION_AGE_LARGER
			#pragma shader_feature_local SPAWN_CONDITION_LIFETIME_INFINITE

			#pragma shader_feature_local SPAWN_OFFSET_NONE SPAWN_OFFSET_CONSTANT SPAWN_OFFSET_DYNAMIC SPAWN_OFFSET_INDIVIDUAL
			#pragma shader_feature_local SPAWN_ADD_NONE SPAWN_ADD_POSITION SPAWN_ADD_ROTATION SPAWN_ADD_BOTH

			#pragma shader_feature_local SPAWN_NOISE_BETTER_QUALITY
			#pragma shader_feature_local SPAWN_NOISE_TIME_DEPENDENT

			#pragma target 5.0
			#pragma vertex vert
			#pragma fragment frag

			#include "Lib/QuantumParticles.cginc"
			#include "Lib/QuaternionMath.cginc"
			#include "Lib/QuantumNoise.cginc"
			#include "Lib/BitangentNoise.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 simRot : TEXCOORD1;
				float4 rotDiff : TEXCOORD2;
				float4 simPos : TEXCOORD5;
				float4 posDiff : TEXCOORD6;
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
			uniform uint _SimID;
			// uniform uint _SimCount; // already defined in QuantumParticles.cginc
			// uniform float _SimSpeed; // already defined in QuantumParticles.cginc
			// uniform float _SimScale; // already defined in QuantumParticles.cginc

			// SPEED //
			uniform float _SpeedLoss;
			uniform float _SpeedLimit;

			// ============================== FORCES ============================== //

			// ------------------------------ ATTRACTION ------------------------------ //
			#if defined(ATTR_FORCE_ON)
				#if defined(ATTR_FORCE_POINT)
					uniform float3 _AttrForceTexPos;
				#else
					// SHAPE TEXTURE //
					uniform sampler2D_float _AttrForceTex;
					#if defined(ATTR_FORCE_TEX_LOCAL_SPACE)
						uniform float3 _AttrForceTexPos;
						uniform float3 _AttrForceTexRotMat1;
						uniform float3 _AttrForceTexRotMat2;
						uniform float3 _AttrForceTexRotMat3;
						uniform float3 _AttrForceTexScale;
					#endif
					#if defined(ATTR_FORCE_SHAPE_OFFSET) || defined(ATTR_FORCE_SHAPE_DYNAMIC_OFFSET)
						uniform float _AttrForceShapeOffset;
					#elif defined(ATTR_FORCE_SHAPE_INDIVIDUAL_OFFSET)
						uniform sampler2D_float _AttrForceShapeOffsetTex;
					#endif
				#endif
				// FORCEFIELD //
				uniform float _AttrForceFieldStrength;
				#if defined(ATTR_FORCE_FIELD_DYNAMIC)
					uniform float _AttrForceFieldFalloff;
					uniform float _AttrForceFieldOffset;
					uniform float _AttrForceFieldLimit;
				#endif
			#endif

			// ------------------------------ DIRECTIONAL ------------------------------ //
			#if defined(DIR_FORCE_ON)
				uniform float3 _DirForceDirection;
				#if defined(DIR_FORCE_SHAPE_FUNNEL) || defined(DIR_FORCE_SHAPE_COILGUN)
					uniform float _DirForceWidth;
				#endif
				#if defined(DIR_FORCE_SHAPE_FUNNEL)
					uniform float _DirForceFunnelInward;
					uniform float _DirForceFunnelGradient;
				#endif
				// FORCEFIELD //
				uniform float _DirForceFieldStrength;
				#if defined(DIR_FORCE_FIELD_DYNAMIC)
					uniform float _DirForceFieldFalloff;
					uniform float _DirForceFieldOffset;
					uniform float _DirForceFieldLimit;
				#endif
			#endif

			// ------------------------------ ROTATIONAL ------------------------------ //
			#if defined(ROT_FORCE_ON)
				uniform float3 _RotForceDirection;
				uniform float _RotForceAngle;
				uniform float _RotForceDistFactor;
				// FORCEFIELD //
				uniform float _RotForceFieldStrength;
				#if defined(ROT_FORCE_FIELD_DYNAMIC)
					uniform float _RotForceFieldFalloff;
					uniform float _RotForceFieldOffset;
					uniform float _RotForceFieldLimit;
				#endif
			#endif

			// ------------------------------ TURBULENCE ------------------------------ //
			#if defined(TURB_FORCE_ON)
				uniform float3 _TurbForceDensity;
				uniform float3 _TurbForcePanning;
				#if defined(TURB_FORCE_TIME_DEPENDENT)
					uniform float _TurbForceTime;
				#endif
				// FORCEFIELD //
				uniform float _TurbForceFieldStrength;
				#if defined(TURB_FORCE_FIELD_DYNAMIC)
					uniform float _TurbForceFieldFalloff;
					uniform float _TurbForceFieldOffset;
					uniform float _TurbForceFieldLimit;
				#endif
			#endif
			
			// ------------------------------ DRAG ------------------------------ //
			#if defined(DRAG_FORCE_ON)
				// FORCEFIELD //
				uniform float _DragForceFieldStrength;
				#if defined(DRAG_FORCE_FIELD_DYNAMIC)
					uniform float _DragForceFieldFalloff;
					uniform float _DragForceFieldOffset;
					uniform float _DragForceFieldLimit;
				#endif
			#endif

			// ============================== SPAWN SETTINGS ============================== //
			#if defined(SPAWN_ON)
				// ------------------------------ SINGLE TEXTURE ------------------------------ //
				#if defined(SPAWN_SOURCE_SINGLE)
					// SHAPE TEXTURE //
					uniform sampler2D_float _SpawnTex;
					#if defined(SPAWN_TEX_LOCAL_SPACE)
						uniform float3 _SpawnTexPos;
						uniform float3 _SpawnTexRotMat1;
						uniform float3 _SpawnTexRotMat2;
						uniform float3 _SpawnTexRotMat3;
						uniform float3 _SpawnTexScale;
					#endif
				#elif defined(SPAWN_SOURCE_MULTI)
					// ------------------------------ POSITION ------------------------------ //
					#if defined(SPAWN_POS_TEX)
						// SHAPE TEXTURE //
						uniform sampler2D_float _SpawnPosTex;
						#if defined(SPAWN_POS_TEX_LOCAL_SPACE)
							uniform float3 _SpawnPosTexPos;
							uniform float3 _SpawnPosTexRotMat1;
							uniform float3 _SpawnPosTexRotMat2;
							uniform float3 _SpawnPosTexRotMat3;
							uniform float3 _SpawnPosTexScale;
						#endif
					#elif defined(SPAWN_POS_POINT)
						uniform float3 _SpawnPosPoint;
					#elif defined(SPAWN_POS_LINE)
						uniform float3 _SpawnPosPoint;
						uniform float3 _SpawnPosPoint2;
					#elif defined(SPAWN_POS_SIMLINE)
						uniform float3 _SpawnPosPoint;
						uniform float _SpawnPosLineEndSimID;
					#elif defined(SPAWN_POS_SPHERE)
						uniform float _SpawnPosSphereRadius;
						uniform float3 _SpawnPosTexPos;
						uniform float3 _SpawnPosTexRotMat1;
						uniform float3 _SpawnPosTexRotMat2;
						uniform float3 _SpawnPosTexRotMat3;
						uniform float3 _SpawnPosTexScale;
					#elif defined(SPAWN_POS_BOX)
						uniform float3 _SpawnPosTexPos;
						uniform float3 _SpawnPosTexRotMat1;
						uniform float3 _SpawnPosTexRotMat2;
						uniform float3 _SpawnPosTexRotMat3;
						uniform float3 _SpawnPosTexScale;
					#endif
					// ------------------------------ VELOCITY ------------------------------ //
					#if defined(SPAWN_VEL_TEX)
						// SHAPE TEXTURE //
						uniform sampler2D_float _SpawnVelTex;
						#if defined(SPAWN_VEL_TEX_LOCAL_SPACE)
							uniform float3 _SpawnVelTexRotMat1;
							uniform float3 _SpawnVelTexRotMat2;
							uniform float3 _SpawnVelTexRotMat3;
							uniform float3 _SpawnVelTexScale;
						#endif
					#elif defined(SPAWN_VEL_VAL)
						uniform float3 _SpawnVelVal;
					#elif defined(SPAWN_VEL_RANGE)
						uniform float3 _SpawnVelVal;
						uniform float3 _SpawnVelVal2;
					#elif defined(SPAWN_VEL_SPHERE)
						uniform float _SpawnVelSpeed;
					#elif defined(SPAWN_VEL_POSITION)
						uniform float _SpawnVelSpeed;
						uniform float _SpawnVelCentralDist;
					#endif
					// ------------------------------ STARTING AGE ------------------------------ //
					#if defined(SPAWN_AGE_TEX)
						uniform sampler2D_float _SpawnAgeTex;
					#elif defined(SPAWN_AGE_VAL)
						uniform float _SpawnAgeVal;
					#elif defined(SPAWN_AGE_RANGE)
						uniform float _SpawnAgeVal;
						uniform float _SpawnAgeVal2;
					#endif
					// ------------------------------ LIFETIME ------------------------------ //
					#if defined(SPAWN_LIFE_TEX)
						uniform sampler2D_float _SpawnLifeTex;
					#elif defined(SPAWN_LIFE_VAL)
						uniform float _SpawnLifeVal;
					#elif defined(SPAWN_LIFE_RANGE)
						uniform float _SpawnLifeVal;
						uniform float _SpawnLifeVal2;
					#endif
				#endif
				// ------------------------------ RESPAWN ------------------------------ //
				// PARTIAL RESPAWN //
				#if defined(SPAWN_RATE_PARTIAL)
					uniform float _SpawnPartialRate;
					uniform float _SpawnTimeErrorCompensation;
				#endif
				// RESPAWN CONDITIONS //
				#if defined(SPAWN_CONDITION_POS_RANGE)
					uniform float3 _SpawnConditionPosBelow;
					uniform float3 _SpawnConditionPosAbove;
				#else
					#if defined(SPAWN_CONDITION_POS_DIST_SMALLER) || defined(SPAWN_CONDITION_POS_DIST_RANGE)
						uniform float _SpawnConditionDistBelow;
					#endif
					#if defined(SPAWN_CONDITION_POS_DIST_LARGER) || defined(SPAWN_CONDITION_POS_DIST_RANGE)
						uniform float _SpawnConditionDistAbove;
					#endif
				#endif
				#if defined(SPAWN_CONDITION_VEL_SMALLER) || defined(SPAWN_CONDITION_VEL_RANGE)
					uniform float _SpawnConditionVelBelow;
				#endif
				#if defined(SPAWN_CONDITION_VEL_LARGER) || defined(SPAWN_CONDITION_VEL_RANGE)
					uniform float _SpawnConditionVelAbove;
				#endif
				#if defined(SPAWN_CONDITION_AGE_LARGER)
					uniform float _SpawnConditionAgeAbove;
				#endif
				// ------------------------------ OFFSETS ------------------------------ //
				#if defined(SPAWN_OFFSET_CONSTANT) || defined(SPAWN_OFFSET_DYNAMIC)
					uniform float _SpawnOffset;
				#elif defined(SPAWN_OFFSET_INDIVIDUAL)
					uniform sampler2D_float _SpawnOffsetTex;
					uniform float4 _SpawnOffsetTex_TexelSize;
				#endif
				// ------------------------------ MOVEMENT ------------------------------ //
				#if !defined(SPAWN_ADD_NONE)
					uniform float _SpawnAddStrength;
				#endif
				// ------------------------------ NOISE ------------------------------ //
				uniform float _RandSeed;
				#if defined(SPAWN_NOISE_TIME_DEPENDENT)
					#if defined(SPAWN_NOISE_BETTER_QUALITY)
						#define SPAWN_NOISE_1 q_rand_1tp
						#define SPAWN_NOISE_2 q_rand_2tp
						#define SPAWN_NOISE_3 q_rand_3tp
						#define SPAWN_NOISE_4 q_rand_4tp
					#else
						#define SPAWN_NOISE_1 q_rand_1t
						#define SPAWN_NOISE_2 q_rand_2t
						#define SPAWN_NOISE_3 q_rand_3t
						#define SPAWN_NOISE_4 q_rand_4t
					#endif
				#else
					#if defined(SPAWN_NOISE_BETTER_QUALITY)
						#define SPAWN_NOISE_1 q_rand_1p
						#define SPAWN_NOISE_2 q_rand_2p
						#define SPAWN_NOISE_3 q_rand_3p
						#define SPAWN_NOISE_4 q_rand_4p
					#else
						#define SPAWN_NOISE_1 q_rand_1
						#define SPAWN_NOISE_2 q_rand_2
						#define SPAWN_NOISE_3 q_rand_3
						#define SPAWN_NOISE_4 q_rand_4
					#endif
				#endif
			#endif

			v2f vert (appdata v) {
				v2f o;
				if(distance(mul(unity_ObjectToWorld, v.vertex).xyz, _WorldSpaceCameraPos) < 0.001 && uint((_ProjectionParams.z - 0.01) * 1000) == _SimID){
					o.vertex = float4((v.uv.x - 0.5) * (_ScreenParams.y / _ScreenParams.x) * 2.0, (v.uv.y - 0.5) * -2.0, 0.5, 1.0);
				}else{
					o.vertex = 0.0;
				}
				float4 oldPos = sampleSimPos(_InputTex, _InputTex_TexelSize, _SimID);
				float4 oldRot = sampleSimRot(_InputTex, _InputTex_TexelSize, _SimID);
				o.uv = v.uv;
				o.simRot = quatFromMatrix(unscaledMatrix(float3x3(unity_ObjectToWorld[0].xyz, unity_ObjectToWorld[1].xyz, unity_ObjectToWorld[2].xyz)));
				o.rotDiff = q_unify(q_diff(oldRot, o.simRot));
				o.simPos = getTranslation(unity_ObjectToWorld);
				o.posDiff = o.simPos - oldPos;
				return o;
			}

			// Calculates the attraction force between two points based on their distance and other parameters.
			inline float3 getForceFieldStrength(float3 particlePos, float3 gravityPos, float offset, float falloff, float strength, float maxAttraction){
				return min(max(safePow(worldToSimScale(distance(gravityPos, particlePos)) + offset, falloff) * strength, -maxAttraction), maxAttraction);
			}

			#if defined(ATTR_FORCE_ON)
			inline float3 getAttractionForce(v2f i, uint particleID, uint maxID, float3 position){
				#if defined(ATTR_FORCE_POINT)
					float3 attractorPos = i.simPos + _AttrForceTexPos;
				#else
					#if defined(ATTR_FORCE_SHAPE)
						const float2 sampleUV = float2(i.uv.xy);
					#elif defined(ATTR_FORCE_SHAPE_OFFSET)
						const float2 sampleUV = offsetParticleUV(particleID, _AttrForceShapeOffset, maxID, _InputTex_TexelSize);
					#elif defined(ATTR_FORCE_SHAPE_DYNAMIC_OFFSET)
						const float2 sampleUV = offsetParticleUV(particleID, _AttrForceShapeOffset * adjustedTime(), maxID, _InputTex_TexelSize);
					#elif defined(ATTR_FORCE_SHAPE_INDIVIDUAL_OFFSET)
						const float2 sampleUV = offsetParticleUV(particleID, sampleOffsetTex(_AttrForceShapeOffsetTex, i.uv), maxID, _InputTex_TexelSize);
					#endif
					#if defined(ATTR_FORCE_TEX_WORLD_SPACE)
						float4 attractorPos = samplePosGlobal(_AttrForceTex, sampleUV);
					#elif defined(ATTR_FORCE_TEX_LOCAL_SPACE)
						float3x3 attrForceTexRotMat = float3x3(_AttrForceTexRotMat1, _AttrForceTexRotMat2, _AttrForceTexRotMat3);
						float4 attractorPos = samplePosLocal(_AttrForceTex, sampleUV, _AttrForceTexPos, attrForceTexRotMat, _AttrForceTexScale);
					#endif
				#endif

				const float3 direction = safeNormalize(attractorPos - position);

				#if defined(ATTR_FORCE_FIELD_UNIFORM)
					const float3 attrForce = direction * _AttrForceFieldStrength;
				#elif defined(ATTR_FORCE_FIELD_DYNAMIC)
					const float3 attrForce = direction * getForceFieldStrength(position, attractorPos, _AttrForceFieldOffset, _AttrForceFieldFalloff, _AttrForceFieldStrength, _AttrForceFieldLimit);
				#endif

				#if defined(ATTR_FORCE_AFFECT_POSITION) && defined(ATTR_FORCE_PREVENT_OVERSHOOT)
					return safeNormalize(attrForce) * worldToSimScale(min(simToWorldScale(length(attrForce)) * adjustedDeltaTime(), length(position - attractorPos))) / adjustedDeltaTime();
				#else
					return attrForce;
				#endif
			}
			#endif

			#if defined(DIR_FORCE_ON)

			#if defined(DIR_FORCE_SHAPE_FUNNEL)
			inline float3 getFunnelFieldDir(float3 start, float3 dir, float3 pos){
				// get closest point on line
				float3 linePoint = start + dir * dot(pos - start, dir);
				// get vector from point to line
				float3 lineVec = linePoint - pos;
				// get factor for funnel direction
				float dirFactor = clamp(worldToSimScale(length(lineVec)) - _DirForceWidth, 0.0, _DirForceFunnelInward * _DirForceFunnelGradient) / _DirForceFunnelGradient;
				// get direction of force
				return lerp(dir, safeNormalize(lineVec), dirFactor);
			}
			#endif
			
			#if defined(DIR_FORCE_SHAPE_COILGUN)
			inline float3 getCoilgunFieldDir(float3 start, float3 dir, float3 pos){
				// get closest point on line
				float t = dot(pos - start, dir);
				float3 pointOnLine = start + dir * t;
				// get vector parallel to line
				float3 vecOnLine = pointOnLine - start;
				// get vector from line to point
				float3 vecToPoint = pos - pointOnLine;

				// calculate the Y coordinate for the circle calculations
				float lineDist = worldToSimScale(length(vecToPoint)) - _DirForceWidth;
				// calculate the X coordinate for the circle calculations
				float pointDist = worldToSimScale(length(vecOnLine)) * sign(t);
				
				// for calculating the radius of the circle, we need to make sure that the denominator is never 0
				float ry = max(lineDist, UNITY_HALF_MIN);
				// calculate the radius of the circle
				float r = ((pointDist * pointDist) + (ry * ry)) / (2.0 * ry);
			
				// calculate relative position to a circle with its center at (0, r)
				float2 rel = safeNormalize(float2(pointDist, lineDist - r));
				// calculate angle factor of the force
				// float angle = frac(1. - (atan2(rel.x, rel.y) / UNITY_TWO_PI) + 1. + _DirForceWidthFactor);
				// calculate the direction of the force by rotating the relative position by 90°
				return safeNormalize(safeNormalize(vecToPoint) * rel.x - safeNormalize(dir) * rel.y);
			}
			#endif

			inline float3 getDirectionalForce(v2f i, float3 position){
				#if defined(DIR_FORCE_LOCAL_SPACE)
					float3 direction = rotate_vector(safeNormalize(_DirForceDirection), i.simRot);
				#elif defined(DIR_FORCE_WORLD_SPACE)
					float3 direction = safeNormalize(_DirForceDirection);
				#endif


				#if defined(DIR_FORCE_SHAPE_UNIFORM)
					// do nothing
				#elif defined(DIR_FORCE_SHAPE_FUNNEL)
					direction = getFunnelFieldDir(i.simPos, direction, position);
				#elif defined(DIR_FORCE_SHAPE_COILGUN)
					direction = getCoilgunFieldDir(i.simPos, direction, position);
				#endif

				#if defined(DIR_FORCE_FIELD_UNIFORM)
					return direction * _DirForceFieldStrength;
				#elif defined(DIR_FORCE_FIELD_DYNAMIC)
					return direction * getForceFieldStrength(position, i.simPos, _DirForceFieldOffset, _DirForceFieldFalloff, _DirForceFieldStrength, _DirForceFieldLimit);
				#endif
			}
			#endif

			#if defined(ROT_FORCE_ON)
			inline float3 getRotationalForce(v2f i, float3 position){
				const float3 axisOfRotation = safeNormalize(_RotForceDirection);
				#if defined(ROT_FORCE_DIST_AXIS)
					// get closest point on line
					const float t = dot(position - i.simPos.xyz, axisOfRotation);
					const float3 refPoint = i.simPos.xyz + axisOfRotation * t;
				#else
					const float3 refPoint = i.simPos;
				#endif
				float3 relPos = position.xyz - refPoint;
				// calculate quaternion for the axis of rotation
				const float halfAngle = _RotForceAngle * UNITY_HALF_PI;
				float4 rotation = float4(axisOfRotation.x * sin(halfAngle),
    									 axisOfRotation.y * sin(halfAngle),
										 axisOfRotation.z * sin(halfAngle),
										 cos(halfAngle));
				// calculate position where particle should be after applying simulator rotation
				const float3 targetPos = rotate_vector(relPos.xyz, rotation);
				// calculate simulator angular velocity
				const float3 direction = safeNormalize(targetPos - relPos.xyz) * lerp(1.0, worldToSimScale(length(relPos)), _RotForceDistFactor);

				#if defined(ROT_FORCE_FIELD_UNIFORM)
					return direction * _RotForceFieldStrength;
				#elif defined(ROT_FORCE_FIELD_DYNAMIC)
					return direction * getForceFieldStrength(position, refPoint, _RotForceFieldOffset, _RotForceFieldFalloff, _RotForceFieldStrength, _RotForceFieldLimit);
				#endif
			}
			#endif

			#if defined(TURB_FORCE_ON)
			inline float3 getTurbulenceForce(v2f i, float3 position){

				#if defined(TURB_FORCE_LOCAL_SPACE)
					float3 samplePos = mul(unity_WorldToObject, float4(position, 1.0)).xyz;
				#elif defined(TURB_FORCE_WORLD_SPACE)
					float3 samplePos = position;
				#endif

				// keep position within bounds
				samplePos = (worldToSimScale(samplePos * _TurbForceDensity) + _TurbForcePanning * adjustedTime()) % 1000.0;

				#if defined(TURB_FORCE_TIME_DEPENDENT)
					// time will go back and forth between 0 and 10000
					const float pingPongTime = abs(frac(adjustedTime() * _TurbForceTime / 2000.0) - 0.5) * 2000.0;
					const float3 direction = safeNormalize(BitangentNoise4D(float4(samplePos, pingPongTime)));
				#else
					const float3 direction = safeNormalize(BitangentNoise3D(samplePos));
				#endif

				#if defined(TURB_FORCE_FIELD_UNIFORM)
					return direction * _TurbForceFieldStrength;
				#elif defined(TURB_FORCE_FIELD_DYNAMIC)
					return direction * getForceFieldStrength(position, i.simPos, _TurbForceFieldOffset, _TurbForceFieldFalloff, _TurbForceFieldStrength, _TurbForceFieldLimit);
				#endif
			}
			#endif

			#if defined(DRAG_FORCE_ON)
			inline float3 getDragForce(v2f i, float3 position){
				float3 direction = float3(0.0, 0.0, 0.0);

				#if defined(DRAG_FORCE_POSITION) || defined(DRAG_FORCE_BOTH)
					// calculate simulator velocity
					direction += i.posDiff.xyz;
				#endif
				#if defined(DRAG_FORCE_ROTATION) || defined(DRAG_FORCE_BOTH)
					float3 relPos = position.xyz - (i.simPos.xyz - i.posDiff.xyz);
					// calculate position where particle should be after applying simulator rotation
					float3 targetPos = rotate_vector(relPos.xyz, i.rotDiff);
					// calculate simulator angular velocity
					direction += (targetPos - relPos.xyz);
				#endif

				direction = worldToSimScale(direction);

				#if defined(DRAG_FORCE_FIELD_UNIFORM)
					return direction * _DragForceFieldStrength / adjustedDeltaTime();
				#elif defined(DRAG_FORCE_FIELD_DYNAMIC)
					return direction * getForceFieldStrength(position, i.simPos, _DragForceFieldOffset, _DragForceFieldFalloff, _DragForceFieldStrength, _DragForceFieldLimit) / adjustedDeltaTime();
				#endif
			}
			#endif

			#if defined(SPAWN_ON)

			bool shouldRespawn(float4 position, float4 velocity, float normID){
				#if defined(SPAWN_RATE_PARTIAL)
					bool respawn = frac(normID - frac(adjustedTime() * _SpawnPartialRate) + 1.0) <= (1.0 + _SpawnTimeErrorCompensation) * adjustedSmoothDeltaTime() * _SpawnPartialRate;
				#else
					bool respawn = true;
				#endif

				#if defined(SPAWN_RESPAWN_CONDITIONS)
					bool anyConditionMet = false;

					#if !defined(SPAWN_CONDITION_POS_OFF)
						float3 localPos = worldToSimScale(mul(unity_WorldToObject, float4(position.xyz, 1.0)).xyz);
					#endif
					#if defined(SPAWN_CONDITION_POS_RANGE)
						anyConditionMet = anyConditionMet
						 | ((localPos.x <= min(_SpawnConditionPosBelow.x, _SpawnConditionPosAbove.x) | localPos.x >= max(_SpawnConditionPosBelow.x, _SpawnConditionPosAbove.x)) ^ (_SpawnConditionPosBelow.x >= _SpawnConditionPosAbove.x))
						 | ((localPos.y <= min(_SpawnConditionPosBelow.y, _SpawnConditionPosAbove.y) | localPos.y >= max(_SpawnConditionPosBelow.y, _SpawnConditionPosAbove.y)) ^ (_SpawnConditionPosBelow.y >= _SpawnConditionPosAbove.y))
						 | ((localPos.z <= min(_SpawnConditionPosBelow.z, _SpawnConditionPosAbove.z) | localPos.z >= max(_SpawnConditionPosBelow.z, _SpawnConditionPosAbove.z)) ^ (_SpawnConditionPosBelow.z >= _SpawnConditionPosAbove.z));
					#elif defined(SPAWN_CONDITION_POS_DIST_SMALLER)
						anyConditionMet = anyConditionMet | length(localPos) < _SpawnConditionDistBelow;
					#elif defined(SPAWN_CONDITION_POS_DIST_LARGER)
						anyConditionMet = anyConditionMet | length(localPos) > _SpawnConditionDistAbove;
					#elif defined(SPAWN_CONDITION_POS_DIST_RANGE)
						anyConditionMet = anyConditionMet | ((length(localPos) <= min(_SpawnConditionDistBelow, _SpawnConditionDistAbove) | length(localPos) >= max(_SpawnConditionDistBelow, _SpawnConditionDistAbove)) ^ (_SpawnConditionDistBelow >= _SpawnConditionDistAbove));
					#endif

					#if defined(SPAWN_CONDITION_VEL_SMALLER)
						anyConditionMet = anyConditionMet | length(velocity.xyz) < _SpawnConditionVelBelow;
					#elif defined(SPAWN_CONDITION_VEL_LARGER)
						anyConditionMet = anyConditionMet | length(velocity.xyz) > _SpawnConditionVelAbove;
					#elif defined(SPAWN_CONDITION_VEL_RANGE)
						anyConditionMet = anyConditionMet | ((length(velocity.xyz) <= min(_SpawnConditionVelBelow, _SpawnConditionVelAbove) | length(velocity.xyz) >= max(_SpawnConditionVelBelow, _SpawnConditionVelAbove)) ^ (_SpawnConditionVelBelow >= _SpawnConditionVelAbove));
					#endif

					#if defined(SPAWN_CONDITION_AGE_LARGER)
						anyConditionMet = anyConditionMet | position.w >= _SpawnConditionAgeAbove;
					#endif

					#if defined(SPAWN_CONDITION_LIFETIME_INFINITE)
						anyConditionMet = anyConditionMet | velocity.w <= 0.0;
					#endif

					respawn = respawn & anyConditionMet;
				#endif

				return respawn;
			}

			inline float2 getSpawnSampleUV(uint particleID, uint maxID, float2 uv){
				#if defined(SPAWN_OFFSET_NONE)
					return uv;
				#elif defined(SPAWN_OFFSET_CONSTANT)
					return offsetParticleUV(particleID, _SpawnOffset, maxID, _InputTex_TexelSize);
				#elif defined(SPAWN_OFFSET_DYNAMIC)
					return offsetParticleUV(particleID, _SpawnOffset * adjustedTime(), maxID, _InputTex_TexelSize);
				#elif defined(SPAWN_OFFSET_INDIVIDUAL)
					return offsetParticleUV(particleID, sampleOffsetTex(_SpawnOffsetTex, uv), maxID, _InputTex_TexelSize);
				#endif
			}

			#if defined(SPAWN_SOURCE_MULTI)

			#if !defined(SPAWN_POS_KEEP)
			inline float3 getSpawnPoint(float2 sampleUV, inout float seed, float normID, v2f i){
				#if defined(SPAWN_POS_TEX)
					#if defined(SPAWN_POS_TEX_WORLD_SPACE)
						float3 spawnPoint = samplePosGlobal(_SpawnPosTex, sampleUV).xyz;
					#elif defined(SPAWN_POS_TEX_LOCAL_SPACE)
						float3x3 spawnPosTexRotMat = float3x3(_SpawnPosTexRotMat1, _SpawnPosTexRotMat2, _SpawnPosTexRotMat3);
						float3 spawnPoint = samplePosLocal(_SpawnPosTex, sampleUV, _SpawnPosTexPos, spawnPosTexRotMat, _SpawnPosTexScale).xyz;
					#endif
				#elif defined(SPAWN_POS_POINT)
					float3 spawnPoint = rotate_vector(simToWorldScale(_SpawnPosPoint), i.simRot) + i.simPos.xyz;
				#elif defined(SPAWN_POS_LINE)
					float3 lineStartPos = rotate_vector(simToWorldScale(_SpawnPosPoint), i.simRot) + i.simPos.xyz;
					float3 lineEndPos = rotate_vector(simToWorldScale(_SpawnPosPoint2), i.simRot) + i.simPos.xyz;
					float3 spawnPoint = lerp(lineStartPos, lineEndPos, SPAWN_NOISE_1(seed, normID));
				#elif defined(SPAWN_POS_SIMLINE)
					float3 lineStartPos = rotate_vector(simToWorldScale(_SpawnPosPoint), i.simRot) + i.simPos.xyz;
					#if defined(INPUT_TEX_WORLD_SPACE)
						float4 lineEndPos = samplePosGlobal(_InputTex, uvFromSimID(_SpawnPosLineEndSimID, _InputTex_TexelSize));
					#elif defined(INPUT_TEX_LOCAL_SPACE)
						float3x3 inputTexRotMat = float3x3(_InputTexRotMat1, _InputTexRotMat2, _InputTexRotMat3);
						float4 lineEndPos = samplePosLocal(_InputTex, uvFromSimID(_SpawnPosLineEndSimID, _InputTex_TexelSize), _InputTexPos, inputTexRotMat, _InputTexScale);
					#endif
					float3 spawnPoint = lerp(lineStartPos, lineEndPos, SPAWN_NOISE_1(seed, normID));
				#elif defined(SPAWN_POS_SPHERE)
					float3 randVals = SPAWN_NOISE_3(seed, normID);
					randVals.x *= 6.28318530718 * 1.0f;
					randVals.y = acos(randVals.y * 2.0 - 1.0);
					randVals.z = sqrt(randVals.z) * _SpawnPosSphereRadius;
					float2 xyPlane = float2(cos(randVals.x), sin(randVals.x));
					float3 spherePos = float3(xyPlane * sin(randVals.y), cos(randVals.y)) * randVals.z;
					float3x3 spawnPosTexRotMat = float3x3(_SpawnPosTexRotMat1, _SpawnPosTexRotMat2, _SpawnPosTexRotMat3);
					float3 spawnPoint = rotate_vector(mul(spawnPosTexRotMat, simToWorldScale(spherePos.xyz) * _SpawnPosTexScale) + simToWorldScale(_SpawnPosTexPos), i.simRot) + i.simPos.xyz;
				#elif defined(SPAWN_POS_BOX)
					float3 boxPos = SPAWN_NOISE_3(seed, normID) * 2.0 - 1.0;
					float3x3 spawnPosTexRotMat = float3x3(_SpawnPosTexRotMat1, _SpawnPosTexRotMat2, _SpawnPosTexRotMat3);
					float3 spawnPoint = rotate_vector(mul(spawnPosTexRotMat, simToWorldScale(boxPos.xyz) * _SpawnPosTexScale) + simToWorldScale(_SpawnPosTexPos), i.simRot) + i.simPos.xyz;
				#endif
				return spawnPoint;
			}
			#endif

			#if !defined(SPAWN_AGE_KEEP)
			float getSpawnStartAge(float2 uv, inout float seed, float normID, float lifetime) {
				#if defined(SPAWN_AGE_TEX)
					return samplePosGlobal(_SpawnAgeTex, uv).w;
				#elif defined(SPAWN_AGE_VAL)
					return _SpawnAgeVal * lifetime;
				#elif defined(SPAWN_AGE_RANGE)
					return lerp(_SpawnAgeVal, _SpawnAgeVal2, SPAWN_NOISE_1(seed, normID)) * lifetime;
				#endif
			}
			#endif

			#if !defined(SPAWN_VEL_KEEP)
			float3 getSpawnVelocity(float2 uv, inout float seed, float normID, float3 position, v2f i) {
				#if defined(SPAWN_VEL_TEX)
					#if defined(SPAWN_VEL_TEX_WORLD_SPACE)
						float3 spawnVelocity = samplePosGlobal(_SpawnVelTex, uv).xyz;
					#elif defined(SPAWN_VEL_TEX_LOCAL_SPACE)
						float3x3 spawnVelTexRotMat = float3x3(_SpawnVelTexRotMat1, _SpawnVelTexRotMat2, _SpawnVelTexRotMat3);
						float3 spawnVelocity = sampleVelLocal(_SpawnVelTex, uv, spawnVelTexRotMat, _SpawnVelTexScale).xyz;
					#endif
				#elif defined(SPAWN_VEL_VAL)
					#if defined(SPAWN_VEL_TEX_WORLD_SPACE)
						float3 spawnVelocity = _SpawnVelVal;
					#elif defined(SPAWN_VEL_TEX_LOCAL_SPACE)
						float3 spawnVelocity = rotate_vector(_SpawnVelVal, i.simRot);
					#endif
				#elif defined(SPAWN_VEL_RANGE)
					#if defined(SPAWN_VEL_SEP_AXES)
						float3 spawnVelocity = lerp(_SpawnVelVal, _SpawnVelVal2, SPAWN_NOISE_3(seed, normID));
					#else
						float3 spawnVelocity = lerp(_SpawnVelVal, _SpawnVelVal2, SPAWN_NOISE_1(seed, normID));
					#endif
					#if defined(SPAWN_VEL_TEX_LOCAL_SPACE)
						spawnVelocity = rotate_vector(spawnVelocity, i.simRot);
					#endif
				#elif defined(SPAWN_VEL_SPHERE)
					float2 randVals = SPAWN_NOISE_2(seed, normID);
					randVals.x *= 6.28318530718 * 1.0f;
					randVals.y = acos(randVals.y * 2.0 - 1.0);
					float2 xyPlane = float2(cos(randVals.x), sin(randVals.x));
					float3 spawnVelocity = float3(xyPlane * sin(randVals.y), cos(randVals.y)) * _SpawnVelSpeed;
				#elif defined(SPAWN_VEL_POSITION)
					float3 direction = worldToSimScale(position - i.simPos.xyz);
					float3 spawnVelocity = safeNormalize(direction) * safePow(length(direction), _SpawnVelCentralDist) * _SpawnVelSpeed;
				#endif
				return spawnVelocity;
			}
			#endif

			#if !defined(SPAWN_LIFE_KEEP)
			float getSpawnLifetime(float2 uv, inout float seed, float normID) {
				#if defined(SPAWN_LIFE_TEX)
					return samplePosGlobal(_SpawnLifeTex, uv).w;
				#elif defined(SPAWN_LIFE_VAL)
					return 1.0 / _SpawnLifeVal;
				#elif defined(SPAWN_LIFE_RANGE)
					return 1.0 / lerp(_SpawnLifeVal, _SpawnLifeVal2, SPAWN_NOISE_1(seed, normID));
				#endif
			}
			#endif

			#endif

			void setSpawnValues(inout float4 position, inout float4 velocity, float normID, float2 sampleUV, v2f i){
				float seed = _RandSeed;

				#if defined(SPAWN_SOURCE_SINGLE)
					#if defined(SPAWN_TEX_WORLD_SPACE)
						position = samplePosGlobal(_SpawnTex, sampleUV);
						velocity = sampleVelGlobal(_SpawnTex, sampleUV);
					#elif defined(SPAWN_TEX_LOCAL_SPACE)
						float3x3 spawnTexRotMat = float3x3(_SpawnTexRotMat1, _SpawnTexRotMat2, _SpawnTexRotMat3);
						position = samplePosLocal(_SpawnTex, sampleUV, _SpawnTexPos, spawnTexRotMat, _SpawnTexScale);
						velocity = sampleVelLocal(_SpawnTex, sampleUV, spawnTexRotMat, _SpawnTexScale);
					#endif
				#else
					#if !defined(SPAWN_POS_KEEP)
						position.xyz = getSpawnPoint(sampleUV, seed, normID, i);
					#endif
					#if !defined(SPAWN_VEL_KEEP)
						velocity.xyz = getSpawnVelocity(sampleUV, seed, normID, position.xyz, i);
					#endif
					#if !defined(SPAWN_LIFE_KEEP)
						velocity.w = getSpawnLifetime(sampleUV, seed, normID);
					#endif
					#if !defined(SPAWN_AGE_KEEP)
						position.w = getSpawnStartAge(sampleUV, seed, normID, velocity.w);
					#endif
				#endif
				
				#if !defined(SPAWN_ADD_NONE)
					float3 addVel = float3(0.0, 0.0, 0.0);
					#if defined(SPAWN_ADD_POSITION) || defined(SPAWN_ADD_BOTH)
						// calculate simulator velocity
						addVel += i.posDiff.xyz;
					#endif
					#if defined(SPAWN_ADD_ROTATION) || defined(SPAWN_ADD_BOTH)
						float3 relPos = position.xyz - i.simPos;
						// calculate position where particle should be after applying simulator rotation
						float3 targetPos = rotate_vector(relPos.xyz, i.rotDiff);
						// calculate simulator angular velocity
						addVel += (targetPos - relPos.xyz);
					#endif
					velocity.xyz += _SpawnAddStrength * worldToSimScale(addVel) / adjustedDeltaTime();
				#endif
			}

			#endif

			inline float3 updateSpeed(float3 current, float3 added){
				current = current*pow(1.0-_SpeedLoss,adjustedDeltaTime() / _SimCount) + added * adjustedDeltaTime();
				return safeNormalize(current) * min(length(current), _SpeedLimit);
			}

			float4 frag (v2f i) : SV_Target {
				const uint maxID = maxIDFromTexelSize(_InputTex_TexelSize);
				const uint simID = simIDFromUV(i.uv, _InputTex_TexelSize);
				const uint particleID = simID - _SimCount;
				const float normID = float(particleID) / float(maxID);

				// STORE SIMULATOR DATA
				if(simID < _SimCount){
					// GAMEOBJECT POSITION + GRAB / ROTATION
					if(simID == _SimID){
						// only return position/rotation if it's the current sim
						return (i.uv.y < 0.5) ? float4(i.simPos.xyz, 1.0) : i.simRot;
					}else{
						// otherwise leave the value untouched
						return tex2Dlod(_InputTex, float4(i.uv.xy, 0.0, 0.0));
					}
				}


				// get current position and velocity
				#if defined(INPUT_TEX_WORLD_SPACE)
					float4 position = samplePosGlobal(_InputTex, i.uv.xy);
					float4 velocity = sampleVelGlobal(_InputTex, i.uv.xy);
				#elif defined(INPUT_TEX_LOCAL_SPACE)
					float3x3 inputTexRotMat = float3x3(_InputTexRotMat1, _InputTexRotMat2, _InputTexRotMat3);
					float4 position = samplePosLocal(_InputTex, i.uv.xy, _InputTexPos, inputTexRotMat, _InputTexScale);
					float4 velocity = sampleVelLocal(_InputTex, i.uv.xy, inputTexRotMat, _InputTexScale);
				#endif


				#if defined(SPAWN_ON)
				// respawn particle if the conditions are met
				if(shouldRespawn(position, velocity, normID)){
					setSpawnValues(position, velocity, normID, getSpawnSampleUV(particleID, maxID, i.uv.xy), i);
				}
				#endif


				if(i.uv.y < 0.5){
					// UPDATE POSITION
					float3 forceDelta = float3(0.0f, 0.0f, 0.0f);

					#if defined(ATTR_FORCE_ON) && defined(ATTR_FORCE_AFFECT_POSITION)
						forceDelta += getAttractionForce(i, particleID, maxID, position.xyz);
					#endif

					#if defined(DIR_FORCE_ON) && defined(DIR_FORCE_AFFECT_POSITION)
						forceDelta += getDirectionalForce(i, position.xyz);
					#endif

					#if defined(ROT_FORCE_ON) && defined(ROT_FORCE_AFFECT_POSITION)
						forceDelta += getRotationalForce(i, position.xyz);
					#endif

					#if defined(TURB_FORCE_ON) && defined(TURB_FORCE_AFFECT_POSITION)
						forceDelta += getTurbulenceForce(i, position.xyz);
					#endif

					#if defined(DRAG_FORCE_ON) && defined(DRAG_FORCE_AFFECT_POSITION)
						forceDelta += getDragForce(i, position.xyz);
					#endif

					position.xyz += simToWorldScale(forceDelta) * adjustedDeltaTime();
					position += float4(simToWorldScale(velocity.xyz), velocity.w) * adjustedDeltaTime() / _SimCount;
					return position;
				}else{
					// UPDATE VELOCITY
					float3 velocityDelta = float3(0.0f, 0.0f, 0.0f);

					#if defined(ATTR_FORCE_ON) && defined(ATTR_FORCE_AFFECT_VELOCITY)
						velocityDelta += getAttractionForce(i, particleID, maxID, position.xyz);
					#endif

					#if defined(DIR_FORCE_ON) && defined(DIR_FORCE_AFFECT_VELOCITY)
						velocityDelta += getDirectionalForce(i, position.xyz);
					#endif

					#if defined(ROT_FORCE_ON) && defined(ROT_FORCE_AFFECT_VELOCITY)
						velocityDelta += getRotationalForce(i, position.xyz);
					#endif

					#if defined(TURB_FORCE_ON) && defined(TURB_FORCE_AFFECT_VELOCITY)
						velocityDelta += getTurbulenceForce(i, position.xyz);
					#endif

					#if defined(DRAG_FORCE_ON) && defined(DRAG_FORCE_AFFECT_VELOCITY)
						velocityDelta += getDragForce(i, position.xyz);
					#endif

					return float4(updateSpeed(velocity.xyz, velocityDelta), velocity.w);
				}
			}
			ENDCG
		}
	}
	CustomEditor "Quantum.SimulatorInspector"
}
