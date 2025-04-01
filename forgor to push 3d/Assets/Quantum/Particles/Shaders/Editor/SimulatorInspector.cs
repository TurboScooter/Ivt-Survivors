using UnityEngine;
using UnityEditor;
using System;

namespace Quantum{
    using uiHelper = EditorUIHelpers;

    public class SimulatorInspector : ShaderGUI {

        const int SHADER_VERSION = 0x10001;
        
        public enum AttrForceModeNames{
            Point,
            Shape,
            ShapeOffset,
            ShapeDynamicOffset,
            ShapeIndividualOffset
        }
        public enum AttrForceModeKeywords{
            ATTR_FORCE_POINT,
            ATTR_FORCE_SHAPE,
            ATTR_FORCE_SHAPE_OFFSET,
            ATTR_FORCE_SHAPE_DYNAMIC_OFFSET,
            ATTR_FORCE_SHAPE_INDIVIDUAL_OFFSET
        }

        public enum DirForceShapeNames{
            Uniform,
            Funnel,
            Coilgun
        }
        public enum DirForceShapeKeywords{
            DIR_FORCE_SHAPE_UNIFORM,
            DIR_FORCE_SHAPE_FUNNEL,
            DIR_FORCE_SHAPE_COILGUN
        }

        public enum DragForceModeNames{
            Position,
            Rotation,
            Both
        }
        public enum DragForceModeKeywords{
            DRAG_FORCE_POSITION,
            DRAG_FORCE_ROTATION,
            DRAG_FORCE_BOTH
        }

        public enum SpawnSourceModeNames{
            SingleTexture,
            MultipleSources
        }
        public enum SpawnSourceModeKeywords{
            SPAWN_SOURCE_SINGLE,
            SPAWN_SOURCE_MULTI
        }

        public enum SpawnPosModeNames{
            Keep,
            Texture,
            Point,
            Line,
            SimLine,
            Sphere,
            Box
        }
        public enum SpawnPosModeKeywords{
            SPAWN_POS_KEEP,
            SPAWN_POS_TEX,
            SPAWN_POS_POINT,
            SPAWN_POS_LINE,
            SPAWN_POS_SIMLINE,
            SPAWN_POS_SPHERE,
            SPAWN_POS_BOX
        }

        public enum SpawnVelModeNames{
            Keep,
            Texture,
            Value,
            Range,
            Sphere,
            Position
        }
        public enum SpawnVelModeKeywords{
            SPAWN_VEL_KEEP,
            SPAWN_VEL_TEX,
            SPAWN_VEL_VAL,
            SPAWN_VEL_RANGE,
            SPAWN_VEL_SPHERE,
            SPAWN_VEL_POSITION
        }

        public enum SpawnAgeModeNames{
            Keep,
            Texture,
            Value,
            Range
        }
        public enum SpawnAgeModeKeywords{
            SPAWN_AGE_KEEP,
            SPAWN_AGE_TEX,
            SPAWN_AGE_VAL,
            SPAWN_AGE_RANGE
        }

        public enum SpawnLifeModeNames{
            Keep,
            Texture,
            Value,
            Range
        }
        public enum SpawnLifeModeKeywords{
            SPAWN_LIFE_KEEP,
            SPAWN_LIFE_TEX,
            SPAWN_LIFE_VAL,
            SPAWN_LIFE_RANGE
        }

        public enum SpawnRateNames{
            All,
            Partial
        }
        public enum SpawnRateKeywords{
            SPAWN_RATE_ALL,
            SPAWN_RATE_PARTIAL
        }

        public enum SpawnRespawnConditionNames{
            Always,
            Conditions
        }
        public enum SpawnRespawnConditionKeywords{
            SPAWN_RESPAWN_ALWAYS,
            SPAWN_RESPAWN_CONDITIONS
        }

        public enum SpawnConditionPosNames{
            Off,
            Range,
            DistanceSmaller,
            DistanceLarger,
            DistanceRange
        }
        public enum SpawnConditionPosKeywords{
            SPAWN_CONDITION_POS_OFF,
            SPAWN_CONDITION_POS_RANGE,
            SPAWN_CONDITION_POS_DIST_SMALLER,
            SPAWN_CONDITION_POS_DIST_LARGER,
            SPAWN_CONDITION_POS_DIST_RANGE
        }

        public enum SpawnConditionVelNames{
            Off,
            Smaller,
            Larger,
            Range
        }
        public enum SpawnConditionVelKeywords{
            SPAWN_CONDITION_VEL_OFF,
            SPAWN_CONDITION_VEL_SMALLER,
            SPAWN_CONDITION_VEL_LARGER,
            SPAWN_CONDITION_VEL_RANGE
        }

        public enum SpawnConditionAgeNames{
            Off,
            Larger
        }
        public enum SpawnConditionAgeKeywords{
            SPAWN_CONDITION_AGE_OFF,
            SPAWN_CONDITION_AGE_LARGER
        }

        public enum SpawnAddModeNames{
            None,
            Position,
            Rotation,
            Both
        }
        public enum SpawnAddModeKeywords{
            SPAWN_ADD_NONE,
            SPAWN_ADD_POSITION,
            SPAWN_ADD_ROTATION,
            SPAWN_ADD_BOTH
        }

        public enum SpawnOffsetModeNames{
            None,
            Constant,
            Dynamic,
            Individual
        }
        public enum SpawnOffsetModeKeywords{
            SPAWN_OFFSET_NONE,
            SPAWN_OFFSET_CONSTANT,
            SPAWN_OFFSET_DYNAMIC,
            SPAWN_OFFSET_INDIVIDUAL
        }

        static bool showInputSettings = false;
        static bool showForceSettings = false;
        static bool showAttrForceSettings = false;
        static bool showDirForceSettings = false;
        static bool showRotForceSettings = false;
        static bool showTurbForceSettings = false;
        static bool showDragForceSettings = false;
        static bool showSpawnSettings = false;

        static bool showInputDataTexSettings = false;
        static bool showAttrShapeTexSettings = false;
        static bool showSpawnShapeTexSettings = false;
        static bool showSpawnPosTexSettings = false;
        static bool showSpawnVelTexSettings = false;

        static uiHelper.ForceField attractionForceField = new uiHelper.ForceField();
        static uiHelper.ForceField directionalForceField = new uiHelper.ForceField();
        static uiHelper.ForceField rotationalForceField = new uiHelper.ForceField();
        static uiHelper.ForceField turbulenceForceField = new uiHelper.ForceField();
        static uiHelper.ForceField dragForceField = new uiHelper.ForceField();

        public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] properties){
            uiHelper.createIconsHeader("Simulator - " + uiHelper.shaderVersionToString(SHADER_VERSION));

            // if a version mismatch is detected, give a warning label
            if(FindProperty("_ShaderVersionSim", properties).intValue > SHADER_VERSION){
                EditorGUILayout.HelpBox("Shader Version Mismatch. This Material was created with a newer Shader Version. Please Update!", MessageType.Warning);

                // create a button to overwrite the version number
                if(GUILayout.Button("Ignore and Downgrade (might break settings)")){
                    FindProperty("_ShaderVersionSim", properties).intValue = SHADER_VERSION;
                }

                return;
            }else if(FindProperty("_ShaderVersionSim", properties).intValue < SHADER_VERSION){
                UpdateHelper.updateMaterial(materialEditor, properties, SHADER_VERSION);
            }

            uiHelper.createFoldoutBox(new GUIContent("Base Settings"), ref showInputSettings, () => 
                baseSettings(materialEditor, properties)
            );
            uiHelper.createFoldoutBox(new GUIContent("Forces"), ref showForceSettings, () => {
                uiHelper.createFoldoutCheckBox(new GUIContent("Attraction"), ref showAttrForceSettings, materialEditor, "ATTR_FORCE_ON", () => 
                    attractionSettings(materialEditor, properties)
                );
                uiHelper.createFoldoutCheckBox(new GUIContent("Directional"), ref showDirForceSettings, materialEditor, "DIR_FORCE_ON", () => 
                    directionalSettings(materialEditor, properties)
                );
                uiHelper.createFoldoutCheckBox(new GUIContent("Rotational"), ref showRotForceSettings, materialEditor, "ROT_FORCE_ON", () => 
                    rotationalSettings(materialEditor, properties)
                );
                uiHelper.createFoldoutCheckBox(new GUIContent("Turbulence"), ref showTurbForceSettings, materialEditor, "TURB_FORCE_ON", () => 
                    turbulenceSettings(materialEditor, properties)
                );
                uiHelper.createFoldoutCheckBox(new GUIContent("Drag"), ref showDragForceSettings, materialEditor, "DRAG_FORCE_ON", () => 
                    dragSettings(materialEditor, properties)
                );
            });
            uiHelper.createFoldoutCheckBox(new GUIContent("Particle Spawning"), ref showSpawnSettings, materialEditor, "SPAWN_ON", () => 
                spawnSettings(materialEditor, properties)
            );
        }

/* ---------- BASE SETTINGS ---------- */
        public void baseSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
            // INPUT //
            uiHelper.createShapeTexture(new GUIContent("Input Texture"), ref showInputDataTexSettings, materialEditor, 
                FindProperty("_InputTex", properties),
                FindProperty("_InputTexPos", properties),
                FindProperty("_InputTexRot", properties),
                FindProperty("_InputTexRotMat1", properties),
                FindProperty("_InputTexRotMat2", properties),
                FindProperty("_InputTexRotMat3", properties),
                FindProperty("_InputTexScale", properties),
                "INPUT"
            );
            // SIMULATION //
            materialEditor.IntegerProperty(FindProperty("_SimID", properties), "Simulator ID");
            materialEditor.IntegerProperty(FindProperty("_SimCount", properties), "Simulator Count");
            materialEditor.FloatProperty(FindProperty("_SimSpeed", properties), "Simulation Speed");
            materialEditor.FloatProperty(FindProperty("_SimScale", properties), "Simulation Scale");
            // SPEED //
            materialEditor.RangeProperty(FindProperty("_SpeedLoss", properties), "Speed Loss");
            uiHelper.createLogSlider(materialEditor, FindProperty("_SpeedLimit", properties), new GUIContent("Speed Limit"));
        }

/* ---------- FORCES ---------- */

    /* ---------- ATTRACTION ---------- */
        public void attractionSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
            uiHelper.createKeywordSettings<AttrForceModeNames, AttrForceModeKeywords>(materialEditor, (mode) => {
                if((AttrForceModeNames)mode == AttrForceModeNames.Point){
                    uiHelper.createVector3Field(materialEditor, FindProperty("_AttrForceTexPos", properties), new GUIContent("Position"));
                }else{
                    uiHelper.createShapeTexture(new GUIContent("Attraction Shape"), ref showAttrShapeTexSettings, materialEditor, 
                        FindProperty("_AttrForceTex", properties),
                        FindProperty("_AttrForceTexPos", properties),
                        FindProperty("_AttrForceTexRot", properties),
                        FindProperty("_AttrForceTexRotMat1", properties),
                        FindProperty("_AttrForceTexRotMat2", properties),
                        FindProperty("_AttrForceTexRotMat3", properties),
                        FindProperty("_AttrForceTexScale", properties),
                        "ATTR_FORCE",
                        true
                    );
                    switch((AttrForceModeNames)mode){
                        case AttrForceModeNames.Point:
                        case AttrForceModeNames.Shape:
                            break;
                        case AttrForceModeNames.ShapeOffset:
                            materialEditor.FloatProperty(FindProperty("_AttrForceShapeOffset", properties), "Offset");
                            break;
                        case AttrForceModeNames.ShapeDynamicOffset:
                            materialEditor.FloatProperty(FindProperty("_AttrForceShapeOffset", properties), "Change Speed");
                            break;
                        case AttrForceModeNames.ShapeIndividualOffset:
                            materialEditor.TexturePropertySingleLine(new GUIContent("Offset Texture"), FindProperty("_AttrForceShapeOffsetTex", properties));
                            break;
                        default: break;//nothing
                    }
                }
            });

            bool affectPos = !uiHelper.createKeywordSwitch(materialEditor,
                "ATTR_FORCE_AFFECT_POSITION",
                "ATTR_FORCE_AFFECT_VELOCITY",
                new GUIContent("Affect"),
                new GUIContent("Position"),
                new GUIContent("Velocity"),
                true
            );

            if (affectPos){
                uiHelper.createKeywordCheckbox(materialEditor, "ATTR_FORCE_PREVENT_OVERSHOOT", new GUIContent("Prevent Overshoot"));
            }

            attractionForceField.drawGUI(materialEditor,
                FindProperty("_AttrForceFieldStrength", properties),
                FindProperty("_AttrForceFieldFalloff", properties),
                FindProperty("_AttrForceFieldOffset", properties),
                FindProperty("_AttrForceFieldLimit", properties),
                "ATTR",
                true
            );
        }

    /* ---------- DIRECTIONAL ---------- */
        public void directionalSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
            uiHelper.createKeywordSettings<DirForceShapeNames, DirForceShapeKeywords>(materialEditor, (shape) => {
                uiHelper.createVector3Field(materialEditor, FindProperty("_DirForceDirection", properties), new GUIContent("Direction"));
                switch((DirForceShapeNames)shape){
                    case DirForceShapeNames.Funnel:
                        uiHelper.createLogSlider(materialEditor, FindProperty("_DirForceWidth", properties), new GUIContent("Cone Width"));
                        materialEditor.RangeProperty(FindProperty("_DirForceFunnelInward", properties), "Inward Strength");
                        uiHelper.createLogSlider(materialEditor, FindProperty("_DirForceFunnelGradient", properties), new GUIContent("Gradient Length"));
                        break;
                    case DirForceShapeNames.Coilgun:
                        uiHelper.createLogSlider(materialEditor, FindProperty("_DirForceWidth", properties), new GUIContent("Cone Width"));
                        break;
                    default: break;//nothing
                }
            });
            uiHelper.createKeywordSwitch(materialEditor,
                "DIR_FORCE_WORLD_SPACE",
                "DIR_FORCE_LOCAL_SPACE",
                new GUIContent("Space"),
                new GUIContent("World"),
                new GUIContent("Local"),
                true
            );
            uiHelper.createKeywordSwitch(materialEditor,
                "DIR_FORCE_AFFECT_POSITION",
                "DIR_FORCE_AFFECT_VELOCITY",
                new GUIContent("Affect"),
                new GUIContent("Position"),
                new GUIContent("Velocity"),
                true
            );

            directionalForceField.drawGUI(materialEditor,
                FindProperty("_DirForceFieldStrength", properties),
                FindProperty("_DirForceFieldFalloff", properties),
                FindProperty("_DirForceFieldOffset", properties),
                FindProperty("_DirForceFieldLimit", properties),
                "DIR"
            );
        }

    /* ---------- ROTATIONAL ---------- */
        public void rotationalSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
            uiHelper.createVector3Field(materialEditor, FindProperty("_RotForceDirection", properties), new GUIContent("Rotation Axis"));
            materialEditor.RangeProperty(FindProperty("_RotForceAngle", properties), "Angle");
            materialEditor.RangeProperty(FindProperty("_RotForceDistFactor", properties), "Distance Factor");
            uiHelper.createKeywordSwitch(materialEditor,
                "ROT_FORCE_DIST_POINT",
                "ROT_FORCE_DIST_AXIS",
                new GUIContent("Distance from"),
                new GUIContent("Point"),
                new GUIContent("Axis"),
                true
            );
            uiHelper.createKeywordSwitch(materialEditor,
                "ROT_FORCE_WORLD_SPACE",
                "ROT_FORCE_LOCAL_SPACE",
                new GUIContent("Space"),
                new GUIContent("World"),
                new GUIContent("Local"),
                true
            );
            uiHelper.createKeywordSwitch(materialEditor,
                "ROT_FORCE_AFFECT_POSITION",
                "ROT_FORCE_AFFECT_VELOCITY",
                new GUIContent("Affect"),
                new GUIContent("Position"),
                new GUIContent("Velocity"),
                true
            );

            rotationalForceField.drawGUI(materialEditor,
                FindProperty("_RotForceFieldStrength", properties),
                FindProperty("_RotForceFieldFalloff", properties),
                FindProperty("_RotForceFieldOffset", properties),
                FindProperty("_RotForceFieldLimit", properties),
                "ROT"
            );
        }
    
    /* ---------- TURBULENCE ---------- */
        public void turbulenceSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
            uiHelper.createVector3Field(materialEditor, FindProperty("_TurbForceDensity", properties), new GUIContent("Density"));
            uiHelper.createVector3Field(materialEditor, FindProperty("_TurbForcePanning", properties), new GUIContent("Panning"));
            bool timeDependent = uiHelper.createKeywordCheckbox(materialEditor, "TURB_FORCE_TIME_DEPENDENT", new GUIContent("Time Dependent"));
            if(timeDependent){
                materialEditor.FloatProperty(FindProperty("_TurbForceTime", properties), "Change Speed");
            }
            uiHelper.createKeywordSwitch(materialEditor,
                "TURB_FORCE_WORLD_SPACE",
                "TURB_FORCE_LOCAL_SPACE",
                new GUIContent("Space"),
                new GUIContent("World"),
                new GUIContent("Local")
            );
            uiHelper.createKeywordSwitch(materialEditor,
                "TURB_FORCE_AFFECT_POSITION",
                "TURB_FORCE_AFFECT_VELOCITY",
                new GUIContent("Affect"),
                new GUIContent("Position"),
                new GUIContent("Velocity"),
                true
            );

            turbulenceForceField.drawGUI(materialEditor,
                FindProperty("_TurbForceFieldStrength", properties),
                FindProperty("_TurbForceFieldFalloff", properties),
                FindProperty("_TurbForceFieldOffset", properties),
                FindProperty("_TurbForceFieldLimit", properties),
                "TURB"
            );
        }

    /* ---------- DRAG ---------- */
        public void dragSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
            uiHelper.createKeywordPopup<DragForceModeNames, DragForceModeKeywords>(materialEditor, new GUIContent("Drag Mode"));
            uiHelper.createKeywordSwitch(materialEditor,
                "DRAG_FORCE_AFFECT_POSITION",
                "DRAG_FORCE_AFFECT_VELOCITY",
                new GUIContent("Affect"),
                new GUIContent("Position"),
                new GUIContent("Velocity"),
                true
            );

            dragForceField.drawGUI(materialEditor,
                FindProperty("_DragForceFieldStrength", properties),
                FindProperty("_DragForceFieldFalloff", properties),
                FindProperty("_DragForceFieldOffset", properties),
                FindProperty("_DragForceFieldLimit", properties),
                "DRAG"
            );
        }

/* ---------- SPAWN SETTINGS ---------- */
        public void spawnSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
            uiHelper.createKeywordSettings<SpawnSourceModeNames, SpawnSourceModeKeywords>(materialEditor, (mode) => {
                switch((SpawnSourceModeNames)mode){
                    case SpawnSourceModeNames.SingleTexture:
                        uiHelper.createShapeTexture(new GUIContent("Shape Texture"), ref showSpawnShapeTexSettings, materialEditor, 
                            FindProperty("_SpawnTex", properties),
                            FindProperty("_SpawnTexPos", properties),
                            FindProperty("_SpawnTexRot", properties),
                            FindProperty("_SpawnTexRotMat1", properties),
                            FindProperty("_SpawnTexRotMat2", properties),
                            FindProperty("_SpawnTexRotMat3", properties),
                            FindProperty("_SpawnTexScale", properties),
                            "SPAWN",
                            true
                        );
                        break;
                    case SpawnSourceModeNames.MultipleSources:
                        uiHelper.createKeywordSettings<SpawnPosModeNames, SpawnPosModeKeywords>(materialEditor, (mode) => {
                            switch((SpawnPosModeNames)mode){
                                case SpawnPosModeNames.Texture:
                                    uiHelper.createShapeTexture(new GUIContent("Position Texture"), ref showSpawnPosTexSettings, materialEditor, 
                                        FindProperty("_SpawnPosTex", properties),
                                        FindProperty("_SpawnPosTexPos", properties),
                                        FindProperty("_SpawnPosTexRot", properties),
                                        FindProperty("_SpawnPosTexRotMat1", properties),
                                        FindProperty("_SpawnPosTexRotMat2", properties),
                                        FindProperty("_SpawnPosTexRotMat3", properties),
                                        FindProperty("_SpawnPosTexScale", properties),
                                        "SPAWN_POS",
                                        true
                                    );
                                    break;
                                case SpawnPosModeNames.Point:
                                    uiHelper.createVector3Field(materialEditor, FindProperty("_SpawnPosPoint", properties), new GUIContent("Position"));
                                    break;
                                case SpawnPosModeNames.Line:
                                    uiHelper.createVector3Field(materialEditor, FindProperty("_SpawnPosPoint", properties), new GUIContent("Start Position"));
                                    uiHelper.createVector3Field(materialEditor, FindProperty("_SpawnPosPoint2", properties), new GUIContent("End Position"));
                                    break;
                                case SpawnPosModeNames.SimLine:
                                    uiHelper.createVector3Field(materialEditor, FindProperty("_SpawnPosPoint", properties), new GUIContent("Start Position"));
                                    materialEditor.IntegerProperty(FindProperty("_SpawnPosLineEndSimID", properties), "End Position Simulator");
                                    break;
                                case SpawnPosModeNames.Sphere:
                                    materialEditor.FloatProperty(FindProperty("_SpawnPosSphereRadius", properties), "Radius");
                                    uiHelper.createTransformVectorFields(materialEditor, 
                                        FindProperty("_SpawnPosTexPos", properties),
                                        FindProperty("_SpawnPosTexRot", properties),
                                        FindProperty("_SpawnPosTexRotMat1", properties),
                                        FindProperty("_SpawnPosTexRotMat2", properties),
                                        FindProperty("_SpawnPosTexRotMat3", properties),
                                        FindProperty("_SpawnPosTexScale", properties)
                                    );
                                    break;
                                case SpawnPosModeNames.Box:
                                    uiHelper.createTransformVectorFields(materialEditor, 
                                        FindProperty("_SpawnPosTexPos", properties),
                                        FindProperty("_SpawnPosTexRot", properties),
                                        FindProperty("_SpawnPosTexRotMat1", properties),
                                        FindProperty("_SpawnPosTexRotMat2", properties),
                                        FindProperty("_SpawnPosTexRotMat3", properties),
                                        FindProperty("_SpawnPosTexScale", properties)
                                    );
                                    break;
                                default:
                                    EditorGUILayout.LabelField(new GUIContent("Keep Position"));
                                    break;
                            }
                        });
                        // add horizontal line as separator
                        uiHelper.thinHorizontalLine();
                        uiHelper.createKeywordSettings<SpawnVelModeNames, SpawnVelModeKeywords>(materialEditor, (mode) => {
                            switch((SpawnVelModeNames)mode){
                                case SpawnVelModeNames.Texture:
                                    uiHelper.createShapeTexture(new GUIContent("Velocity Texture"), ref showSpawnVelTexSettings, materialEditor, 
                                        FindProperty("_SpawnVelTex", properties),
                                        null,
                                        FindProperty("_SpawnVelTexRot", properties),
                                        FindProperty("_SpawnVelTexRotMat1", properties),
                                        FindProperty("_SpawnVelTexRotMat2", properties),
                                        FindProperty("_SpawnVelTexRotMat3", properties),
                                        FindProperty("_SpawnVelTexScale", properties),
                                        "SPAWN_VEL",
                                        true
                                    );
                                    break;
                                case SpawnVelModeNames.Value:
                                    uiHelper.createVector3Field(materialEditor, FindProperty("_SpawnVelVal", properties), new GUIContent("Velocity"));
                                    uiHelper.createKeywordSwitch(materialEditor,
                                        "SPAWN_VEL_TEX_WORLD_SPACE",
                                        "SPAWN_VEL_TEX_LOCAL_SPACE",
                                        new GUIContent("Rotation Space"),
                                        new GUIContent("World"),
                                        new GUIContent("Local"),
                                        true);
                                    break;
                                case SpawnVelModeNames.Range:
                                    uiHelper.createVector3Field(materialEditor, FindProperty("_SpawnVelVal", properties), new GUIContent("Min Velocity"));
                                    uiHelper.createVector3Field(materialEditor, FindProperty("_SpawnVelVal2", properties), new GUIContent("Max Velocity"));
                                    uiHelper.createKeywordCheckbox(materialEditor, "SPAWN_VEL_SEP_AXES", new GUIContent("Separate Axes"));
                                    uiHelper.createKeywordSwitch(materialEditor,
                                        "SPAWN_VEL_TEX_WORLD_SPACE",
                                        "SPAWN_VEL_TEX_LOCAL_SPACE",
                                        new GUIContent("Rotation Space"),
                                        new GUIContent("World"),
                                        new GUIContent("Local"),
                                        true);
                                    break;
                                case SpawnVelModeNames.Sphere:
                                    materialEditor.FloatProperty(FindProperty("_SpawnVelSpeed", properties), "Speed");
                                    break;
                                case SpawnVelModeNames.Position:
                                    materialEditor.FloatProperty(FindProperty("_SpawnVelSpeed", properties), "Speed");
                                    materialEditor.FloatProperty(FindProperty("_SpawnVelCentralDist", properties), "Central Distance Factor");
                                    break;
                                default:
                                    EditorGUILayout.LabelField(new GUIContent("Keep Velocity"));
                                    break;
                            }
                        });
                        uiHelper.thinHorizontalLine();
                        uiHelper.createKeywordSettings<SpawnAgeModeNames, SpawnAgeModeKeywords>(materialEditor, (mode) => {
                            switch((SpawnAgeModeNames)mode){
                                case SpawnAgeModeNames.Texture:
                                    materialEditor.TexturePropertySingleLine(new GUIContent("Starting Age Texture"), FindProperty("_SpawnAgeTex", properties));
                                    break;
                                case SpawnAgeModeNames.Value:
                                    uiHelper.createLogSlider(materialEditor, FindProperty("_SpawnAgeVal", properties), new GUIContent("Starting Age"));
                                    break;
                                case SpawnAgeModeNames.Range:
                                    uiHelper.createLogSlider(materialEditor, FindProperty("_SpawnAgeVal", properties), new GUIContent("Min Starting Age"));
                                    uiHelper.createLogSlider(materialEditor, FindProperty("_SpawnAgeVal2", properties), new GUIContent("Max Starting Age"));
                                    break;
                                default:
                                    EditorGUILayout.LabelField(new GUIContent("Keep Age"));
                                    break;
                            }
                        });
                        uiHelper.thinHorizontalLine();
                        uiHelper.createKeywordSettings<SpawnLifeModeNames, SpawnLifeModeKeywords>(materialEditor, (mode) => {
                            switch((SpawnLifeModeNames)mode){
                                case SpawnLifeModeNames.Texture:
                                    materialEditor.TexturePropertySingleLine(new GUIContent("Lifetime Texture"), FindProperty("_SpawnLifeTex", properties));
                                    break;
                                case SpawnLifeModeNames.Value:
                                    uiHelper.createLogSlider(materialEditor, FindProperty("_SpawnLifeVal", properties), new GUIContent("Lifetime"));
                                    break;
                                case SpawnLifeModeNames.Range:
                                    uiHelper.createLogSlider(materialEditor, FindProperty("_SpawnLifeVal", properties), new GUIContent("Min Lifetime"));
                                    uiHelper.createLogSlider(materialEditor, FindProperty("_SpawnLifeVal2", properties), new GUIContent("Max Lifetime"));
                                    break;
                                default:
                                    EditorGUILayout.LabelField(new GUIContent("Keep Lifetime"));
                                    break;
                            }
                        });
                        uiHelper.thinHorizontalLine();
                        break;
                    default: break;//nothing
                }
            });

            // SPAWN RATE AND CONDITIONS //
            uiHelper.createKeywordSettings<SpawnRateNames, SpawnRateKeywords>(materialEditor, (mode) => {
                switch((SpawnRateNames)mode){
                    case SpawnRateNames.All:
                        EditorGUILayout.LabelField(new GUIContent("Check All Particles"));
                        break;
                    case SpawnRateNames.Partial:
                        materialEditor.FloatProperty(FindProperty("_SpawnPartialRate", properties), "Spawn Rate");
                        materialEditor.RangeProperty(FindProperty("_SpawnTimeErrorCompensation", properties), "Time Error Compensation");
                        break;
                    default: break;//nothing
                }
            });

            uiHelper.createKeywordSettings<SpawnRespawnConditionNames, SpawnRespawnConditionKeywords>(materialEditor, (mode) => {
                switch((SpawnRespawnConditionNames)mode){
                    case SpawnRespawnConditionNames.Always:
                        EditorGUILayout.LabelField(new GUIContent("Always Respawn"));
                        break;
                    case SpawnRespawnConditionNames.Conditions:
                        EditorGUILayout.LabelField(new GUIContent("Respawn Conditions:"));
                        uiHelper.createKeywordSettings<SpawnConditionPosNames, SpawnConditionPosKeywords>(materialEditor, (mode) => {
                            switch((SpawnConditionPosNames)mode){
                                case SpawnConditionPosNames.Off:
                                    EditorGUILayout.LabelField(new GUIContent("No Position Condition"));
                                    break;
                                case SpawnConditionPosNames.Range:
                                    uiHelper.createVector3Field(materialEditor, FindProperty("_SpawnConditionPosBelow", properties), new GUIContent("Position Below"));
                                    uiHelper.createVector3Field(materialEditor, FindProperty("_SpawnConditionPosAbove", properties), new GUIContent("Position Above"));
                                    break;
                                case SpawnConditionPosNames.DistanceSmaller:
                                    materialEditor.FloatProperty(FindProperty("_SpawnConditionDistBelow", properties), "Distance Below");
                                    break;
                                case SpawnConditionPosNames.DistanceLarger:
                                    materialEditor.FloatProperty(FindProperty("_SpawnConditionDistAbove", properties), "Distance Above");
                                    break;
                                case SpawnConditionPosNames.DistanceRange:
                                    materialEditor.FloatProperty(FindProperty("_SpawnConditionDistBelow", properties), "Distance Below");
                                    materialEditor.FloatProperty(FindProperty("_SpawnConditionDistAbove", properties), "Distance Above");
                                    break;
                                default: break;//nothing
                            }
                        });
                        uiHelper.createKeywordSettings<SpawnConditionVelNames, SpawnConditionVelKeywords>(materialEditor, (mode) => {
                            switch((SpawnConditionVelNames)mode){
                                case SpawnConditionVelNames.Off:
                                    EditorGUILayout.LabelField(new GUIContent("No Velocity Condition"));
                                    break;
                                case SpawnConditionVelNames.Smaller:
                                    materialEditor.FloatProperty(FindProperty("_SpawnConditionVelBelow", properties), "Velocity Below");
                                    break;
                                case SpawnConditionVelNames.Larger:
                                    materialEditor.FloatProperty(FindProperty("_SpawnConditionVelAbove", properties), "Velocity Above");
                                    break;
                                case SpawnConditionVelNames.Range:
                                    materialEditor.FloatProperty(FindProperty("_SpawnConditionVelBelow", properties), "Velocity Below");
                                    materialEditor.FloatProperty(FindProperty("_SpawnConditionVelAbove", properties), "Velocity Above");
                                    break;
                                default: break;//nothing
                            }
                        });
                        uiHelper.createKeywordSettings<SpawnConditionAgeNames, SpawnConditionAgeKeywords>(materialEditor, (mode) => {
                            switch((SpawnConditionAgeNames)mode){
                                case SpawnConditionAgeNames.Off:
                                    EditorGUILayout.LabelField(new GUIContent("No Age Condition"));
                                    break;
                                case SpawnConditionAgeNames.Larger:
                                    materialEditor.FloatProperty(FindProperty("_SpawnConditionAgeAbove", properties), "Age Above Lifetime");
                                    break;
                                default: break;//nothing
                            }
                        });
                        uiHelper.createKeywordCheckbox(materialEditor, "SPAWN_CONDITION_LIFETIME_INFINITE", new GUIContent("Infinite Lifetime"));
                        break;
                    default: break;//nothing
                }
            });

            // OFFSET AND FORCES //
            uiHelper.thinHorizontalLine();
            uiHelper.createKeywordSettings<SpawnOffsetModeNames, SpawnOffsetModeKeywords>(materialEditor, (mode) => {
                switch((SpawnOffsetModeNames)mode){
                    case SpawnOffsetModeNames.None:
                        EditorGUILayout.LabelField(new GUIContent("No Offset"));
                        break;
                    case SpawnOffsetModeNames.Constant:
                        materialEditor.FloatProperty(FindProperty("_SpawnOffset", properties), "Constant Offset");
                        break;
                    case SpawnOffsetModeNames.Dynamic:
                        materialEditor.FloatProperty(FindProperty("_SpawnOffset", properties), "Dynamic Offset");
                        break;
                    case SpawnOffsetModeNames.Individual:
                        materialEditor.TexturePropertySingleLine(new GUIContent("Offset Texture"), FindProperty("_SpawnOffsetTex", properties));
                        break;
                    default: break;//nothing
                }
            });
            int spawnAddMode = uiHelper.createKeywordPopup<SpawnAddModeNames, SpawnAddModeKeywords>(materialEditor, new GUIContent("Inherit Movement"));
            if(spawnAddMode != (int)SpawnAddModeKeywords.SPAWN_ADD_NONE){
                uiHelper.createLogSlider(materialEditor, FindProperty("_SpawnAddStrength", properties), new GUIContent("Strength"));
            }

            // NOISE //
            uiHelper.thinHorizontalLine();
            materialEditor.ShaderProperty(FindProperty("_RandSeed", properties), "Random Seed");
            uiHelper.createKeywordCheckbox(materialEditor, "SPAWN_NOISE_BETTER_QUALITY", new GUIContent("Better Quality Noise"));
            uiHelper.createKeywordCheckbox(materialEditor, "SPAWN_NOISE_TIME_DEPENDENT", new GUIContent("Time Dependent Noise"));
        }
    }
}