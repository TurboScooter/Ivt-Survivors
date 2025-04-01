using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.PackageManager;

namespace Quantum{
    using uiHelper = EditorUIHelpers;

    public class VisualizerInspector : ShaderGUI {

        const int SHADER_VERSION = 0x10001;
        
        public enum SupportedBlendOp{
            Add = UnityEngine.Rendering.BlendOp.Add,
            Subtract = UnityEngine.Rendering.BlendOp.Subtract,
            ReverseSubtract = UnityEngine.Rendering.BlendOp.ReverseSubtract,
            Min = UnityEngine.Rendering.BlendOp.Min,
            Max = UnityEngine.Rendering.BlendOp.Max
        }

        public enum SizeMode{
            Constant,
            Flicker,
            FlickerCurve,
            Lifetime,
            LifetimeCurve,
            Velocity,
            VelocityCurve
        }
        public enum SizeModeKeywords{
            SHAPE_SIZE_CONSTANT,
            SHAPE_SIZE_FLICKER,
            SHAPE_SIZE_FLICKER_CURVE,
            SHAPE_SIZE_LIFETIME,
            SHAPE_SIZE_LIFETIME_CURVE,
            SHAPE_SIZE_VELOCITY,
            SHAPE_SIZE_VELOCITY_CURVE
        }

        public enum ColorMode{
            Static,
            Pulse,
            Gradient,
            Direction,
            Velocity,
            Lifetime,
            Texture
        }
        public enum ColorModeKeywords{
            COLOR_MODE_STATIC,
            COLOR_MODE_PULSE,
            COLOR_MODE_GRADIENT,
            COLOR_MODE_DIRECTION,
            COLOR_MODE_VELOCITY,
            COLOR_MODE_LIFETIME,
            COLOR_MODE_TEXTURE
        }

        public enum AlphaFalloff{
            None,
            SquareRoot,
            HalfSine,
            Linear,
            Sine,
            Squared,
            Gradient
        }
        public enum AlphaFalloffKeywords{
            COLOR_FALLOFF_NONE,
            COLOR_FALLOFF_ROOT,
            COLOR_FALLOFF_HALFSINE,
            COLOR_FALLOFF_LINEAR,
            COLOR_FALLOFF_SINE,
            COLOR_FALLOFF_SQUARED,
            COLOR_FALLOFF_GRADIENT
        }

        public enum TexAnimMode{
            Off,
            On,
            Synched,
            FromLifetime
        }
        public enum TexAnimModeKeywords{
            COLOR_ANIM_TEX_OFF,
            COLOR_ANIM_TEX_ON,
            COLOR_ANIM_TEX_SYNCHED,
            COLOR_ANIM_TEX_LIFETIME
        }

        public enum ShapeMode{
            Point,
            Line,
            Triangle,
            Square,
            Circle
        }
        public enum ShapeModeKeywords{
            SHAPE_MODE_POINT,
            SHAPE_MODE_LINE,
            SHAPE_MODE_TRIANGLE,
            SHAPE_MODE_SQUARE,
            SHAPE_MODE_CIRCLE
        }

        public enum LineType{
            Absolute,
            Relative,
            Direction,
            Particle
        }
        public enum LineTypeKeywords{
            SHAPE_LINE_TYPE_ABSOLUTE,
            SHAPE_LINE_TYPE_RELATIVE,
            SHAPE_LINE_TYPE_DIRECTION,
            SHAPE_LINE_TYPE_PARTICLE
        }

        public enum LineOffsetMode{
            Static,
            Dynamic,
            Texture,
            Random
        }
        public enum LineOffsetModeKeywords{
            SHAPE_LINE_OFFSET_STATIC,
            SHAPE_LINE_OFFSET_DYNAMIC,
            SHAPE_LINE_OFFSET_TEXTURE,
            SHAPE_LINE_OFFSET_RANDOM
        }

        public enum RotSpaceName{
            World,
            Local,
            Screen,
            Direction
        }
        public enum RotSpaceKeywords{
            ROT_SPACE_WORLD,
            ROT_SPACE_LOCAL,
            ROT_SPACE_SCREEN,
            ROT_SPACE_DIR
        }

        public enum RotBaseMode{
            SingleValue,
            Range
        }
        public enum RotBaseModeKeywords{
            ROT_BASE_SINGLE,
            ROT_BASE_RANGE
        }

        public enum RotAnimMode{
            NoAnimation,
            OverTime,
            OverTimeWithRange,
            OscillateRange,
            LoopCurve,
            LifetimeRange,
            LifetimeCurve
        }
        public enum RotAnimModeKeywords{
            ROT_ANIM_NONE,
            ROT_OVER_TIME_SINGLE,
            ROT_OVER_TIME_RANGE,
            ROT_OSCILLATE_RANGE,
            ROT_LOOP_CURVE,
            ROT_LIFETIME_RANGE,
            ROT_LIFETIME_CURVE
        }

        static bool showBaseSettings = false;
        static bool showInputDataTexSettings = false;
        static bool showRenderSettings = false;
        static bool showShapeSettings = false;
        static bool showColorSettings = false;
        static bool showTextureSettings = false;
        static bool showRotationSettings = false;

        static int gradTexRes = 128;
        static String gradientPath = "Assets/Quantum/Particles/Resources/Gradients/";
        static String curvePath = "Assets/Quantum/Particles/Resources/Curves/";

        static AnimationCurve sizeCurve = new AnimationCurve();
        uiHelper.TextureLoader sizeTexLoader = new uiHelper.TextureLoader(curvePath, "_SizeCurve", gradTexRes, TextureFormat.RFloat, TextureWrapMode.Repeat);
        static uiHelper.DoubleCurve sizeCurve2D = new uiHelper.DoubleCurve();
        uiHelper.TextureLoader sizeTexLoader2D = new uiHelper.TextureLoader(curvePath, "_SizeCurve2D", gradTexRes, TextureFormat.RGFloat, TextureWrapMode.Repeat);
        static Gradient falloffGradient = new Gradient();
        uiHelper.TextureLoader falloffTexLoader = new uiHelper.TextureLoader(gradientPath, "_Falloff", gradTexRes, TextureFormat.Alpha8, TextureWrapMode.Repeat);
        static Gradient colorGradient = new Gradient();
        uiHelper.TextureLoader ColorTexLoader = new uiHelper.TextureLoader(gradientPath, "_ColorGradient", gradTexRes, TextureFormat.RGBA32, TextureWrapMode.Repeat);
        static uiHelper.TripleCurve rotationCurve = new uiHelper.TripleCurve();
        uiHelper.TextureLoader rotationTexLoader = new uiHelper.TextureLoader(curvePath, "_RotationCurve", gradTexRes, TextureFormat.RGBAFloat, TextureWrapMode.Repeat);
        
        public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] properties){
            uiHelper.createIconsHeader("Visualizer - " + uiHelper.shaderVersionToString(SHADER_VERSION));

            // if a version mismatch is detected, give a warning label
            if(FindProperty("_ShaderVersionVis", properties).intValue > SHADER_VERSION){
                EditorGUILayout.HelpBox("Shader Version Mismatch. This Material was created with a newer Shader Version. Please Update!", MessageType.Warning);

                // create a button to overwrite the version number
                if(GUILayout.Button("Ignore and Downgrade (might break settings)")){
                    FindProperty("_ShaderVersionVis", properties).intValue = SHADER_VERSION;
                }

                return;
            }else if(FindProperty("_ShaderVersionVis", properties).intValue < SHADER_VERSION){
                UpdateHelper.updateMaterial(materialEditor, properties, SHADER_VERSION);
            }

            /* ---------- BASE SETUP ---------- */
            uiHelper.createFoldoutBox(new GUIContent("Base Settings"), ref showBaseSettings, () => 
                baseSettings(materialEditor, properties)
            );
            
            /* ---------- RENDER SETUP ---------- */
            uiHelper.createFoldoutBox(new GUIContent("Render Settings"), ref showRenderSettings, () => 
                renderSettings(materialEditor, properties)
            );
            
            /* ---------- SHAPE & SIZE SETTINGS ---------- */
            uiHelper.createFoldoutBox(new GUIContent("Shape & Size Settings"), ref showShapeSettings, () => 
                shapeSettings(materialEditor, properties)
            );

            /* ---------- COLOR SETTINGS ---------- */
            uiHelper.createFoldoutBox(new GUIContent("Color Settings"), ref showColorSettings, () =>
                colorSettings(materialEditor, properties)
            );
            
            /* ---------- TEXTURE SETTINGS ---------- */
            uiHelper.createFoldoutCheckBox(new GUIContent("Texture Settings"), ref showTextureSettings, materialEditor, "COLOR_USE_TEXTURE", () => 
                textureSettings(materialEditor, properties)
            );
            
            /* ---------- ROTATION SETTINGS ---------- */
            uiHelper.createFoldoutCheckBox(new GUIContent("Rotation Settings"), ref showRotationSettings, materialEditor, "ROT_ON", () => 
                rotationSettings(materialEditor, properties)
            );
        }

        public void baseSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
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
            materialEditor.IntegerProperty(FindProperty("_SimCount", properties), "Simulator Count");
            materialEditor.FloatProperty(FindProperty("_SimSpeed", properties), "Simulation Speed");
            materialEditor.FloatProperty(FindProperty("_SimScale", properties), "Simulation Scale");
            materialEditor.ShaderProperty(FindProperty("_RandSeed", properties), "Random Seed");
        }

        public void renderSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
            uiHelper.createEnumPopup<SupportedBlendOp>(materialEditor, FindProperty("_BlendOp", properties), new GUIContent("BlendOp"));
            uiHelper.createEnumPopup<UnityEngine.Rendering.BlendMode>(materialEditor, FindProperty("_SrcBlend", properties), new GUIContent("SrcBlend"));
            uiHelper.createEnumPopup<UnityEngine.Rendering.BlendMode>(materialEditor, FindProperty("_DstBlend", properties), new GUIContent("DstBlend"));

            uiHelper.createEnumPopup<UnityEngine.Rendering.CompareFunction>(materialEditor, FindProperty("_ZTest", properties), new GUIContent("Depth Test"));
            materialEditor.ShaderProperty(FindProperty("_ZWrite", properties), "Depth Write");

            uiHelper.createKeywordCheckbox(materialEditor, "RENDER_AFTER_LIFETIME", new GUIContent("Render after Lifetime"));

            materialEditor.RenderQueueField();
        }

        public void shapeSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
            /* ---------- SHAPE MODE ---------- */
            int shapeMode = uiHelper.createKeywordPopup<ShapeMode, ShapeModeKeywords>(materialEditor, new GUIContent("Particle Shape"));

            /* ---------- LINE SETTINGS ---------- */
            if(shapeMode == (int)ShapeMode.Line){
                lineSettings(materialEditor, properties);
            } else {
                /* ---------- PARTICLE SIZE ---------- */
                if (shapeMode != (int)ShapeMode.Point){
                    bool sizes2DValues = uiHelper.createKeywordCheckbox(materialEditor, "SHAPE_SIZE_2D_VALUES", new GUIContent("2D Size Values"));
                    uiHelper.createKeywordSettings<SizeMode, SizeModeKeywords>(materialEditor, (mode) => {sizeModeSettings(materialEditor, properties, mode, sizes2DValues);});

                    uiHelper.createKeywordCheckbox(materialEditor, "SHAPE_SIZE_SCREEN_SPACE", new GUIContent("Screen Space Size"));
                }
            }
        }

        public void lineSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
            uiHelper.createLogSlider(materialEditor, FindProperty("_LineLengthLimit", properties), new GUIContent("Line Length Limit"));
            uiHelper.createKeywordCheckbox(materialEditor, "SHAPE_LINE_DRAW_PARTIAL", new GUIContent("Draw Partial Lines"));
            uiHelper.createKeywordCheckbox(materialEditor, "SHAPE_LINE_INVERT_DIR", new GUIContent("Invert Direction"));

            /* ---------- LINE TYPE ---------- */
            LineType lineType = (LineType)uiHelper.createKeywordPopup<LineType, LineTypeKeywords>(materialEditor, new GUIContent("Line Type"));
            switch(lineType){
                case LineType.Absolute:
                    uiHelper.createVector3Field(materialEditor, FindProperty("_LinePos", properties), new GUIContent("Line End Pos"));
                    uiHelper.createKeywordSwitch(materialEditor, "SHAPE_LINE_WORLD_SPACE", "SHAPE_LINE_LOCAL_SPACE", new GUIContent("Space"), new GUIContent("World"), new GUIContent("Local"), true);
                    break;
                case LineType.Relative:
                    uiHelper.createVector3Field(materialEditor, FindProperty("_LinePos", properties), new GUIContent("Line End Pos"));
                    uiHelper.createKeywordSwitch(materialEditor, "SHAPE_LINE_WORLD_SPACE", "SHAPE_LINE_LOCAL_SPACE", new GUIContent("Space"), new GUIContent("World"), new GUIContent("Local"), true);
                    break;
                case LineType.Direction:
                    uiHelper.createLogSlider(materialEditor, FindProperty("_LineLength", properties), new GUIContent("Line Length"));
                    uiHelper.createKeywordCheckbox(materialEditor, "SHAPE_LINE_VELOCITY_NORMALIZE", new GUIContent("Normalize"));
                    break;
                case LineType.Particle:
                    uiHelper.createKeywordSettings<LineOffsetMode, LineOffsetModeKeywords>(materialEditor, (mode) => {lineOffsetSettings(materialEditor, properties, mode);});
                    break;
                default: break;//nothing
            }
        }

        public void lineOffsetSettings(MaterialEditor materialEditor, MaterialProperty[] properties, int mode){
            switch((LineOffsetMode)mode){
                case LineOffsetMode.Static:
                    materialEditor.FloatProperty(FindProperty("_LineOffset", properties), "Offset");
                    break;
                case LineOffsetMode.Dynamic:
                    materialEditor.FloatProperty(FindProperty("_LineOffset", properties), "Offset Change Speed");
                    break;
                case LineOffsetMode.Texture:
                    materialEditor.TexturePropertySingleLine(new GUIContent("Offset Texture"), FindProperty("_LineOffTex", properties));
                    break;
                case LineOffsetMode.Random:
                    materialEditor.FloatProperty(FindProperty("_LineOffset", properties), "Offset Change Speed");
                    break;
                default: break;//nothing
            }
        }
        
        public void sizeModeSettings(MaterialEditor materialEditor, MaterialProperty[] properties, int mode, bool sizes2DValues){
            MaterialProperty minSize = FindProperty("_MinSize", properties);
            MaterialProperty minSize2D = FindProperty("_MinSize2D", properties);
            SizeMode sizeMode = (SizeMode)mode;

            if(sizeMode == SizeMode.Constant){
                if(sizes2DValues){
                    minSize2D.vectorValue = EditorGUILayout.Vector2Field("Particle Size", minSize2D.vectorValue);
                }else{
                    uiHelper.createLogSlider(materialEditor, minSize, new GUIContent("Particle Size"));
                }
                return;
            }

            if(sizeMode == SizeMode.FlickerCurve || sizeMode == SizeMode.LifetimeCurve || sizeMode == SizeMode.VelocityCurve){
                GUIContent curveLabel = new GUIContent(
                    sizeMode == SizeMode.FlickerCurve ? "Size over Time" :
                    sizeMode == SizeMode.LifetimeCurve ? "Size over Lifetime" :
                    sizeMode == SizeMode.VelocityCurve ? "Size from Speed" : "Unused");
                if(sizes2DValues){
                    uiHelper.createCurveTexture(materialEditor, FindProperty("_SizeCurveTex2D", properties), sizeCurve2D, new Rect(0,0,1,1), sizeTexLoader2D, curveLabel);
                }else{
                    uiHelper.createCurveTexture(materialEditor, FindProperty("_SizeCurveTex", properties), sizeCurve, new Rect(0,0,1,1), sizeTexLoader, curveLabel);
                }
            }

            MaterialProperty maxSize = FindProperty("_MaxSize", properties);
            MaterialProperty maxSize2D = FindProperty("_MaxSize2D", properties);
            GUIContent minSizeLabel = new GUIContent(
                sizeMode == SizeMode.Lifetime ? "Life Start Size" :
                sizeMode == SizeMode.Velocity ? "Low Speed Size" : "Min Size");
            GUIContent maxSizeLabel = new GUIContent(
                sizeMode == SizeMode.Lifetime ? "Life End Size" :
                sizeMode == SizeMode.Velocity ? "High Speed Size" : "Max Size");

            if(sizes2DValues){
                minSize2D.vectorValue = EditorGUILayout.Vector2Field(minSizeLabel, minSize2D.vectorValue);
                maxSize2D.vectorValue = EditorGUILayout.Vector2Field(maxSizeLabel, maxSize2D.vectorValue);
            }else{
                uiHelper.createLogSlider(materialEditor, minSize, minSizeLabel);
                uiHelper.createLogSlider(materialEditor, maxSize, maxSizeLabel);
            }

            if(sizeMode == SizeMode.Flicker || sizeMode == SizeMode.FlickerCurve){
                uiHelper.createLogSlider(materialEditor, FindProperty("_FlickerLength", properties), new GUIContent("Flicker Length"));
                uiHelper.createKeywordCheckbox(materialEditor, "SHAPE_SIZE_SYNC", new GUIContent("Synchronize"));
            }
            if(sizeMode == SizeMode.Velocity || sizeMode == SizeMode.VelocityCurve){
                uiHelper.createLogSlider(materialEditor, FindProperty("_SizeMaxSpeed", properties), new GUIContent("Max Speed"));
            }
            if(sizeMode == SizeMode.Lifetime || sizeMode == SizeMode.LifetimeCurve){
                uiHelper.createKeywordCheckbox(materialEditor, "SHAPE_SIZE_FACTOR_CLAMP", new GUIContent("Clamp Lifetime"));
            }
            if(sizeMode == SizeMode.Velocity || sizeMode == SizeMode.VelocityCurve){
                uiHelper.createKeywordCheckbox(materialEditor, "SHAPE_SIZE_FACTOR_CLAMP", new GUIContent("Clamp Speed"));
            }
        }

        public void colorSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
            /* ---------- COLOR MODE ---------- */
            uiHelper.createKeywordSettings<ColorMode, ColorModeKeywords>(materialEditor, (mode) => {colorModeSettings(materialEditor, properties, mode);});

            /* ---------- FALLOFF MODE ---------- */
            AlphaFalloff fMode = (AlphaFalloff)uiHelper.createKeywordPopup<AlphaFalloff, AlphaFalloffKeywords>(materialEditor, new GUIContent("Alpha Falloff"));
            if(fMode == AlphaFalloff.Gradient){
                uiHelper.createGradientTexture(materialEditor, FindProperty("_FalloffTex", properties), falloffGradient, falloffTexLoader, new GUIContent("Falloff Gradient"));
            }
        }

        public void colorModeSettings(MaterialEditor materialEditor, MaterialProperty[] properties, int mode){
            switch((ColorMode)mode){
                case ColorMode.Static:
                    materialEditor.ColorProperty(FindProperty("_Color1", properties), "Color");
                    break;
                case ColorMode.Pulse:
                    materialEditor.ColorProperty(FindProperty("_Color1", properties), "Color 1");
                    materialEditor.ColorProperty(FindProperty("_Color2", properties), "Color 2");
                    uiHelper.createLogSlider(materialEditor, FindProperty("_ColorCycleLength", properties), new GUIContent("Pulse Length"));
                    uiHelper.createKeywordCheckbox(materialEditor, "COLOR_ANIM_SYNC", new GUIContent("Synchronize"));
                    break;
                case ColorMode.Gradient:
                    uiHelper.createGradientTexture(materialEditor, FindProperty("_ColorGradTex", properties), colorGradient, ColorTexLoader, new GUIContent("Color over Time"));
                    uiHelper.createLogSlider(materialEditor, FindProperty("_ColorCycleLength", properties), new GUIContent("Cycle Length"));
                    uiHelper.createKeywordCheckbox(materialEditor, "COLOR_ANIM_SYNC", new GUIContent("Synchronize"));
                    break;
                case ColorMode.Direction:
                    materialEditor.ColorProperty(FindProperty("_Color0", properties), "Base Color");
                    materialEditor.ColorProperty(FindProperty("_Color1", properties), "X Axis");
                    materialEditor.ColorProperty(FindProperty("_Color2", properties), "Y Axis");
                    materialEditor.ColorProperty(FindProperty("_Color3", properties), "Z Axis");
                    bool normDir = uiHelper.createKeywordCheckbox(materialEditor, "COLOR_MODE_DIRECTION_NORMALIZE", new GUIContent("Normalize Direction"));
                    // start disable group
                    EditorGUI.BeginDisabledGroup(normDir);
                    uiHelper.createLogSlider(materialEditor, FindProperty("_ColorMaxSpeed", properties), new GUIContent("Max Speed"));
                    uiHelper.createKeywordCheckbox(materialEditor, "COLOR_FACTOR_CLAMP", new GUIContent("Clamp Speed"));
                    EditorGUI.EndDisabledGroup();
                    break;
                case ColorMode.Velocity:
                    uiHelper.createGradientTexture(materialEditor, FindProperty("_ColorGradTex", properties), colorGradient, ColorTexLoader, new GUIContent("Color from Speed"));
                    uiHelper.createLogSlider(materialEditor, FindProperty("_ColorMaxSpeed", properties), new GUIContent("Max Speed"));
                    uiHelper.createKeywordCheckbox(materialEditor, "COLOR_FACTOR_CLAMP", new GUIContent("Clamp Speed"));
                    break;
                case ColorMode.Lifetime:
                    uiHelper.createGradientTexture(materialEditor, FindProperty("_ColorGradTex", properties), colorGradient, ColorTexLoader, new GUIContent("Color over Lifetime"));
                    uiHelper.createKeywordCheckbox(materialEditor, "COLOR_FACTOR_CLAMP", new GUIContent("Clamp Lifetime"));
                    break;
                case ColorMode.Texture:
                    materialEditor.TexturePropertyWithHDRColor(new GUIContent("Colors"), FindProperty("_ColorTex", properties), FindProperty("_Color1", properties), true);
                    break;
                default: break;//nothing
            }
        }

        public void textureSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
            MaterialProperty particleTex = FindProperty("_ParticleTex", properties);
            materialEditor.TextureProperty(particleTex, "");

            MaterialProperty alphaCutoff = FindProperty("_TextureAlphaCutoff", properties);
            materialEditor.ShaderProperty(alphaCutoff, "Alpha Cutoff");

            TexAnimMode aMode = (TexAnimMode)uiHelper.createKeywordPopup<TexAnimMode, TexAnimModeKeywords>(materialEditor, new GUIContent("Animate"));
            if(aMode == TexAnimMode.On || aMode == TexAnimMode.Synched){
                uiHelper.createLogSlider(materialEditor, FindProperty("_TexAnimLength", properties), new GUIContent("Animation Length"));
            }

            if(aMode == TexAnimMode.FromLifetime){
                uiHelper.createKeywordCheckbox(materialEditor, "COLOR_ANIM_TEX_FACTOR_CLAMP", new GUIContent("Clamp Lifetime"));
            }
        }

        public void rotationSettings(MaterialEditor materialEditor, MaterialProperty[] properties){
            /* ---------- PARTICLE SIZE ---------- */
            uiHelper.createKeywordPopup<RotSpaceName, RotSpaceKeywords>(materialEditor, new GUIContent("Rotation Space"));

            uiHelper.createKeywordSettings<RotBaseMode, RotBaseModeKeywords>(materialEditor, (mode) => {rotationBaseSettings(materialEditor, properties, mode);});

            uiHelper.createKeywordSettings<RotAnimMode, RotAnimModeKeywords>(materialEditor, (mode) => {rotationAnimationSettings(materialEditor, properties, mode);});
        }

        public void rotationBaseSettings(MaterialEditor materialEditor, MaterialProperty[] properties, int mode){
            MaterialProperty baseRotMin =  FindProperty("_BaseRotMin", properties);
            MaterialProperty baseRotMax =  FindProperty("_BaseRotMax", properties);
            switch((RotBaseMode)mode){
                case RotBaseMode.SingleValue:
                    baseRotMin.vectorValue = EditorGUILayout.Vector3Field("Base Rotation", baseRotMin.vectorValue);
                    break;
                case RotBaseMode.Range:
                    baseRotMin.vectorValue = EditorGUILayout.Vector3Field("Min Base Rotation", baseRotMin.vectorValue);
                    baseRotMax.vectorValue = EditorGUILayout.Vector3Field("Max Base Rotation", baseRotMax.vectorValue);
                    uiHelper.createKeywordCheckbox(materialEditor, "ROT_BASE_SEP_AXES", new GUIContent("Separate Axes"));
                    break;
                default: break;//nothing
            }
        }

        public void rotationAnimationSettings(MaterialEditor materialEditor, MaterialProperty[] properties, int mode){
            MaterialProperty animRotMin =  FindProperty("_AnimRotMin", properties);
            MaterialProperty animRotMax =  FindProperty("_AnimRotMax", properties);
            MaterialProperty animRotLength =  FindProperty("_AnimRotLength", properties);
            MaterialProperty rotAnimCurveTex = FindProperty("_RotAnimCurveTex", properties);
            switch((RotAnimMode)mode){
                case RotAnimMode.NoAnimation:
                    EditorGUILayout.PrefixLabel("Animate Rotation");
                    break;
                case RotAnimMode.OverTime:
                    animRotMin.vectorValue = EditorGUILayout.Vector3Field("Rotation Speed", animRotMin.vectorValue);
                    break;
                case RotAnimMode.OverTimeWithRange:
                    animRotMin.vectorValue = EditorGUILayout.Vector3Field("Min Rotation Speed", animRotMin.vectorValue);
                    animRotMax.vectorValue = EditorGUILayout.Vector3Field("Max Rotation Speed", animRotMax.vectorValue);
                    uiHelper.createKeywordCheckbox(materialEditor, "ROT_ANIM_SEP_AXES", new GUIContent("Separate Axes"));
                    break;
                case RotAnimMode.OscillateRange:
                    animRotMin.vectorValue = EditorGUILayout.Vector3Field("Min Rotation Diff", animRotMin.vectorValue);
                    animRotMax.vectorValue = EditorGUILayout.Vector3Field("Max Rotation Diff", animRotMax.vectorValue);
                    uiHelper.createLogSlider(materialEditor, animRotLength, new GUIContent("Oscillation Length"));
                    uiHelper.createKeywordCheckbox(materialEditor, "ROT_ANIM_SYNC", new GUIContent("Synchronize"));
                    break;
                case RotAnimMode.LoopCurve:
                    uiHelper.createCurveTexture(materialEditor, rotAnimCurveTex, rotationCurve, new Rect(0,0,1,1), rotationTexLoader, new GUIContent("Rotation"));
                    uiHelper.createLogSlider(materialEditor, animRotLength, new GUIContent("Loop Length"));
                    uiHelper.createKeywordCheckbox(materialEditor, "ROT_ANIM_SYNC", new GUIContent("Synchronize"));
                    break;
                case RotAnimMode.LifetimeRange:
                    animRotMin.vectorValue = EditorGUILayout.Vector3Field("Start Rotation Diff", animRotMin.vectorValue);
                    animRotMax.vectorValue = EditorGUILayout.Vector3Field("End Rotation Diff", animRotMax.vectorValue);
                    uiHelper.createKeywordCheckbox(materialEditor, "ROT_ANIM_FACTOR_CLAMP", new GUIContent("Clamp Lifetime"));
                    break;
                case RotAnimMode.LifetimeCurve:
                    uiHelper.createCurveTexture(materialEditor, rotAnimCurveTex, rotationCurve, new Rect(0,0,1,1), rotationTexLoader, new GUIContent("Rotation"));
                    uiHelper.createKeywordCheckbox(materialEditor, "ROT_ANIM_FACTOR_CLAMP", new GUIContent("Clamp Lifetime"));
                    break;
                default: break;//nothing
            }
        }
    }
}