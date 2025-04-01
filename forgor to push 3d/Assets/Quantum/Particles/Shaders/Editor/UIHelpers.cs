using UnityEngine;
using UnityEditor;
using System;

namespace Quantum{
    public static class EditorUIHelpers{
        //TODO: add change check to all ui elements

        // static float for indenting the ui
        public static float indentSize = 15.0f;
        public static float valueFieldSize = 50.0f;
        public static float valueFieldSpace = 5.0f;

        public static String shaderVersionToString(int version){
            // the version is encoded in the following way:
            // the last to bytes of the integer are the patch version
            // the next two bytes are the minor version
            // the first two bytes are the major version
            int major = version >> 16;
            int minor = (version >> 8) & 0xFF;
            int patch = version & 0xFF;
            return major + "." + minor + "." + patch;
        }

        public static void createIconsHeader(String subtitle = ""){
            // create a horizontal layout group containing clickable icons which will open the corresponding documentation page
            // the images used for the icons are in a folder called "Icons" in the same directory as this script
            EditorGUILayout.BeginHorizontal();

            // make sure the text is horizontally and vertically centered
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 20;
            style.alignment = TextAnchor.MiddleCenter;
            GUIStyle styleSub = new GUIStyle(GUI.skin.label);
            styleSub.fontSize = 16;
            styleSub.alignment = TextAnchor.MiddleCenter;
            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField("Quantum Particles", style);
            EditorGUILayout.LabelField(subtitle, styleSub);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndVertical();
            GUILayout.FlexibleSpace();

            // make sure the icons are centered
            if(GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Quantum/Particles/Shaders/Editor/Icons/docs.png"), GUIStyle.none, GUILayout.Width(32), GUILayout.Height(32))){
                Application.OpenURL("https://link250.github.io/QuantumDocs/");
            }
            GUILayout.Space(8);
            if(GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Quantum/Particles/Shaders/Editor/Icons/discord.png"), GUIStyle.none, GUILayout.Width(32), GUILayout.Height(32))){
                Application.OpenURL("https://discord.gg/Va5VPev");
            }
            GUILayout.Space(8);
            if(GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Quantum/Particles/Shaders/Editor/Icons/youtube.png"), GUIStyle.none, GUILayout.Width(32), GUILayout.Height(32))){
                Application.OpenURL("https://www.youtube.com/@quantumlot");
            }
            GUILayout.Space(8);
            if(GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Quantum/Particles/Shaders/Editor/Icons/patreon.png"), GUIStyle.none, GUILayout.Width(32), GUILayout.Height(32))){
                Application.OpenURL("https://www.patreon.com/quantumlot");
            }
            GUILayout.Space(8);
            if(GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Quantum/Particles/Shaders/Editor/Icons/github.png"), GUIStyle.none, GUILayout.Width(32), GUILayout.Height(32))){
                Application.OpenURL("https://github.com/Link250");
            }
            EditorGUILayout.EndHorizontal();
        }

        /* 
         * this class will convert a float to a log scale
         * the log scale has up to three segments depending on the input values
         * the mid value is used to split the scale into multiple segments and should be set to the smallest non-zero value
         * every value between -mid and mid will be in the middle segment and will be scaled linearly
         * every value smaller than -mid will be in the left segment and will be scaled logarithmically
         * every value larger than mid will be in the right segment and will be scaled logarithmically
         * 
         * in the slider the mid value will always be mapped to 1 and -1 respectively
         * therefore the linear segment will always be between 0 and 1
         * and the logarithmic segments will be above and below 1 and -1
         * this means that negative logarithmic values will be shifted outwards from the mid point
         */
        private class SignedLogConverter{
            private float _mid;
            private float _logBase;
            private float _logMid;

            public SignedLogConverter(float mid = 0.01f, float logBase = 10.0f){
                _mid = mid;
                _logBase = logBase;
                _logMid = Mathf.Log(_mid, _logBase);
            }

            public float toScale(float f){
                if(Mathf.Abs(f) > _mid)
                    return Mathf.Sign(f) * (Mathf.Log(Mathf.Abs(f), _logBase) - _logMid + 1);
                else
                    return f / _mid;
            }

            public float fromScale(float f){
                if(Mathf.Abs(f) > 1.0f)
                    return Mathf.Sign(f) * Mathf.Pow(_logBase, Mathf.Abs(f) + _logMid - 1);
                else
                    return f * _mid;
            }
        }

        /* ---------- Misc. helper functions ---------- */
        public static void horizontalLineLabel(GUIContent description){
            EditorGUILayout.PrefixLabel(description);
            float offset = EditorGUIUtility.singleLineHeight / 2 + 2;
            GUILayout.Space(- offset);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.Height(offset));
        }

        public static void thinHorizontalLine(){
            GUILayout.Space(2 - EditorGUIUtility.singleLineHeight / 2);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider, GUILayout.Height(2 + EditorGUIUtility.singleLineHeight / 2));
        }

        /* ---------- Float helper functions ---------- */
        public static void createLogSlider(MaterialEditor materialEditor, MaterialProperty prop, GUIContent description, float mid = 0.01f, params GUILayoutOption[] options){
            // placeholder to get the rect
            EditorGUILayout.LabelField("");
            Rect rect = EditorGUI.PrefixLabel (GUILayoutUtility.GetLastRect(), GUIUtility.GetControlID (FocusType.Passive), description);
            // rect for slider
            Rect sliderRect = new Rect(rect.x, rect.y, rect.width - valueFieldSize - valueFieldSpace, rect.height);
            // rect for value field
            Rect fieldRect = new Rect(rect.x + rect.width - valueFieldSize, rect.y, valueFieldSize, rect.height);

            // create converter
            SignedLogConverter converter = new SignedLogConverter(mid);

            // Calculate slider values
            float sliderMin = converter.toScale(prop.rangeLimits.x);
            float sliderMax = converter.toScale(prop.rangeLimits.y);

            // draw vertical lines behind the slider area for every integer value
            for(int i = (int)sliderMin; i <= (int)sliderMax; i++){
                float x = sliderRect.x + valueFieldSpace + (sliderRect.width - 2 * valueFieldSpace) * (i - sliderMin) / (sliderMax - sliderMin);
                // make the line darker for values from -1 to 1
                if(i >= -1 && i <= 1) Handles.color = new Color(0.7f, 0.3f, 0.3f);
                else Handles.color = new Color(0.5f, 0.5f, 0.5f);
                // draw a line with thickness 2
                Handles.DrawAAPolyLine(2.0f, new Vector3(x, sliderRect.yMin + sliderRect.height / 5), new Vector3(x, sliderRect.yMax - sliderRect.height / 5));
            }

            // Draw slider
            EditorGUI.BeginChangeCheck();
            float sliderVal = GUI.HorizontalSlider(sliderRect, converter.toScale(prop.floatValue), sliderMin, sliderMax);
            
            // Convert slider value back to linear scale
            float value = converter.fromScale(sliderVal);

            // add float field
            value = EditorGUI.FloatField(fieldRect, value);
            if (EditorGUI.EndChangeCheck()){
                materialEditor.RegisterPropertyChangeUndo("Undo Float");
                // update the property with respect to the range limits
                prop.floatValue = Mathf.Clamp(value, prop.rangeLimits.x, prop.rangeLimits.y);
            }
        }

        /* ---------- Vector helper functions ---------- */
        public static void createVector3Field(MaterialEditor materialEditor, MaterialProperty prop, GUIContent description, params GUILayoutOption[] options){
            Vector4 vec = prop.vectorValue;
            EditorGUI.BeginChangeCheck();
            vec = EditorGUILayout.Vector3Field(description, vec, options);
            if (EditorGUI.EndChangeCheck()){
                materialEditor.RegisterPropertyChangeUndo("Undo Vector3");
                prop.vectorValue = vec;
            }
        }

        public static void createVector3RotationField(MaterialEditor materialEditor, MaterialProperty rotProp, MaterialProperty mat1Prop, MaterialProperty mat2Prop, MaterialProperty mat3Prop, GUIContent description, bool invert = false, params GUILayoutOption[] options){
            Vector4 vec = rotProp.vectorValue;
            EditorGUI.BeginChangeCheck();
            vec = EditorGUILayout.Vector3Field(description, vec, options);
            if (EditorGUI.EndChangeCheck()){
                materialEditor.RegisterPropertyChangeUndo("Undo Vector3");
                rotProp.vectorValue = vec;
                foreach(Material mat in materialEditor.targets){
                    // turn the vector into a rotation matrix and set it as the property
                    Matrix4x4 mat4 = Matrix4x4.Rotate(Quaternion.Euler(vec)).transpose;
                    if(invert) mat4 = mat4.inverse;
                    mat1Prop.vectorValue = mat4.GetColumn(0);
                    mat2Prop.vectorValue = mat4.GetColumn(1);
                    mat3Prop.vectorValue = mat4.GetColumn(2);
                }
            }
        }

        public static void createTransformVectorFields(MaterialEditor materialEditor, MaterialProperty posProp, MaterialProperty rotProp, MaterialProperty rotMat1Prop, MaterialProperty rotMat2Prop, MaterialProperty rotMat3Prop, MaterialProperty scaleProp){
            if(posProp != null) createVector3Field(materialEditor, posProp, new GUIContent("Position"));
            createVector3RotationField(materialEditor, rotProp, rotMat1Prop, rotMat2Prop, rotMat3Prop, new GUIContent("Rotation"));
            createVector3Field(materialEditor, scaleProp, new GUIContent("Scale"));
        }

        /* ---------- Keyword helper functions ---------- */
        public static void setMaterialKeyword(MaterialEditor materialEditor, String keyword, bool active){
            foreach(Material mat in materialEditor.targets){
                if(active) mat.EnableKeyword(keyword);
                else mat.DisableKeyword(keyword);
            }
        }

        public static void setMaterialEnumKeyword<TKeyEnum>(MaterialEditor materialEditor, int activeKeyword){
            foreach(Material mat in materialEditor.targets){
                int index = 0;
                foreach(TKeyEnum keyword in Enum.GetValues(typeof(TKeyEnum))){
                    if(index++ == activeKeyword) mat.EnableKeyword(keyword.ToString());
                    else mat.DisableKeyword(keyword.ToString());
                }
            }
        }

        public static int getMaterialEnumKeyword<TKeyEnum>(MaterialEditor materialEditor){
            Material mat = (Material)materialEditor.target;
            foreach(TKeyEnum keyword in Enum.GetValues(typeof(TKeyEnum))){
                if(mat.IsKeywordEnabled(keyword.ToString())) return (int)(object)keyword;
            }
            // enable the first keyword if none is active
            mat.EnableKeyword(Enum.GetNames(typeof(TKeyEnum))[0]);
            return 0;
        }

        public static bool getMaterialKeywordPair(MaterialEditor materialEditor, String keywordOff, String keywordOn, bool defaultOn = false){
            Material mat = (Material)materialEditor.target;
            if(mat.IsKeywordEnabled(keywordOff)) return false;
            if(mat.IsKeywordEnabled(keywordOn)) return true;
            // if neither keyword is active, return the default value
            // but also set the default value as the active keyword
            mat.EnableKeyword(defaultOn ? keywordOn : keywordOff);
            return defaultOn;
        }

        /* ---------- Keyword UI Elements ---------- */
        public static string[] addWhitespaces(string[] input){
            // create a new array with the same size as the input
            string[] output = new string[input.Length];
            // iterate over the input array and copy the strings, but add a whitespace before every capital letter
            // only the first letter can be copied without a whitespace
            for(int i = 0; i < input.Length; i++){
                output[i] = input[i][0].ToString();
                for(int j = 1; j < input[i].Length; j++){
                    if(char.IsUpper(input[i][j])) output[i] += " ";
                    output[i] += input[i][j];
                }
            }
            return output;
        }

        public static int createKeywordPopup<TNameEnum, TKeyEnum>(MaterialEditor materialEditor, GUIContent description, params GUILayoutOption[] options){
            int mode = getMaterialEnumKeyword<TKeyEnum>(materialEditor);

            EditorGUI.BeginChangeCheck();
            mode = EditorGUILayout.Popup(description, mode, addWhitespaces(Enum.GetNames(typeof(TNameEnum))), options);
            if (EditorGUI.EndChangeCheck()){
                materialEditor.RegisterPropertyChangeUndo("Undo Keyword Popup");
                setMaterialEnumKeyword<TKeyEnum>(materialEditor, mode);
            }

            return mode;
        }

        public static bool createKeywordCheckbox(MaterialEditor materialEditor, String keyword, GUIContent description, params GUILayoutOption[] options){
            bool isActive = (materialEditor.target as Material).IsKeywordEnabled(keyword);
            EditorGUI.BeginChangeCheck();
            isActive = EditorGUILayout.Toggle(description, isActive, options);
            if (EditorGUI.EndChangeCheck()){
                setMaterialKeyword(materialEditor, keyword, isActive);
            }
            return isActive;
        }

        public static void createKeywordSettings<TNameEnum, TKeyEnum>(MaterialEditor materialEditor, Action<int> drawContent){
            EditorGUILayout.BeginHorizontal(GUIStyle.none);
                EditorGUILayout.BeginVertical(GUIStyle.none);
                    drawContent(getMaterialEnumKeyword<TKeyEnum>(materialEditor));
                EditorGUILayout.EndVertical();
                createKeywordPopup<TNameEnum, TKeyEnum>(materialEditor, GUIContent.none, GUILayout.Width(20));
            EditorGUILayout.EndHorizontal();
        }

        // simple function for testing styles
        // will just place a BeginHorizontal with a label and the given string as style
        public static void createStyleTest(String style){
            EditorGUILayout.BeginHorizontal(style);
                EditorGUILayout.LabelField("TEST");
            EditorGUILayout.EndHorizontal();
        }

        // small helper function to create a button that can look like it's pressed
        // takes a string and a bool as input
        private static bool createButton(GUIContent content, bool isActive){
            GUIStyle pressedStyle = new GUIStyle("MiniTextField");
            GUIStyle releasedStyle = new GUIStyle("miniButton");
            pressedStyle.alignment = releasedStyle.alignment;
            pressedStyle.padding = releasedStyle.padding;
            pressedStyle.margin = releasedStyle.margin;
            if(isActive) return GUILayout.Button(content, pressedStyle, GUILayout.ExpandWidth(true));
            return GUILayout.Button(content, releasedStyle, GUILayout.ExpandWidth(true));
        }

        public static bool createKeywordSwitch(MaterialEditor materialEditor, String keywordLeft, String keywordRight, GUIContent description, GUIContent descLeft, GUIContent descRight, bool defaultOn = false, params GUILayoutOption[] options){
            bool isActive = getMaterialKeywordPair(materialEditor, keywordLeft, keywordRight, defaultOn);
            EditorGUILayout.BeginHorizontal(GUIStyle.none);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PrefixLabel(description);
            EditorGUILayout.BeginHorizontal();
                bool leftClick = createButton(descLeft, !isActive);
                bool rightClick = createButton(descRight, isActive);
                isActive = isActive ? !leftClick : rightClick;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            if (EditorGUI.EndChangeCheck()){
                setMaterialKeyword(materialEditor, keywordLeft, !isActive);
                setMaterialKeyword(materialEditor, keywordRight, isActive);
            }
            return isActive;
        }

        /* ---------- Enum UI Elements ---------- */
        public static int createEnumPopup<TEnum>(MaterialEditor materialEditor, MaterialProperty prop, GUIContent description, params GUILayoutOption[] options){
            EditorGUI.BeginChangeCheck();
            int mode = EditorGUILayout.Popup(description, (int)prop.floatValue, Enum.GetNames(typeof(TEnum)), options);
            if (EditorGUI.EndChangeCheck()){
                materialEditor.RegisterPropertyChangeUndo("Undo Enum");
                prop.floatValue = mode;
            }
            return mode;
        }

        /* ---------- Foldout UI Elements ---------- */
        public static void createFoldoutBox(GUIContent foldoutContent, ref bool isOpen, Action drawContent){
            EditorGUILayout.BeginHorizontal("box");
                GUILayout.Space(indentSize);
                isOpen = EditorGUILayout.Foldout(isOpen, foldoutContent, true, new GUIStyle("Foldout") { overflow = new RectOffset(-14, 0, 0, 0) });
            EditorGUILayout.EndHorizontal();

            if(isOpen){
                EditorGUILayout.BeginHorizontal(GUIStyle.none);
                GUILayout.Space(indentSize);
                EditorGUILayout.BeginVertical(GUIStyle.none);
                drawContent();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
        }

        public static void createFoldoutCheckBox(GUIContent foldoutContent, ref bool isOpen, MaterialEditor materialEditor, String keyword, Action drawContent){
            bool isActive;
            EditorGUILayout.BeginHorizontal("box");
                GUILayout.Space(indentSize);
                isOpen = EditorGUILayout.Foldout(isOpen, foldoutContent, true);
                GUILayout.FlexibleSpace();
                isActive = createKeywordCheckbox(materialEditor, keyword, GUIContent.none, GUILayout.Width(15));
            EditorGUILayout.EndHorizontal();

            if(isOpen){
                // disable ui when the keyword is not active
                EditorGUI.BeginDisabledGroup(!isActive);
                EditorGUILayout.BeginHorizontal(GUIStyle.none);
                GUILayout.Space(indentSize);
                EditorGUILayout.BeginVertical(GUIStyle.none);
                drawContent();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                EditorGUI.EndDisabledGroup();
            }
        }

        /* ---------- Texture helper functions ---------- */
        public static void applyColorsFromGradient(Gradient gradient, Texture2D tex){
            int res = tex.width;
            Color[] colors = new Color[res];
            for (int x = 0; x < res; x++){
                colors[x] = gradient.Evaluate(((float)x)/(float)(res-1));
            }
            tex.SetPixels(0, 0, res, 1, colors);
            tex.Apply();
        }

        public static void applyColorsFromCurve(AnimationCurve curve, Texture2D tex){
            int res = tex.width;
            Color[] colors = new Color[res];
            for (int x = 0; x < res; x++){
                colors[x] = new Color(curve.Evaluate(((float)x)/(float)(res-1)), 0.0f, 0.0f);
            }
            tex.SetPixels(0, 0, res, 1, colors);
            tex.Apply();
        }

        public class TripleCurve{
            public AnimationCurve x = new AnimationCurve();
            public AnimationCurve y = new AnimationCurve();
            public AnimationCurve z = new AnimationCurve();
        };

        public static void applyColorsFromCurve(DoubleCurve curve, Texture2D tex){
            int res = tex.width;
            Color[] colors = new Color[res];
            for (int x = 0; x < res; x++){
                float sample = ((float)x)/(float)(res-1);
                colors[x] = new Color(curve.x.Evaluate(sample), curve.y.Evaluate(sample), 0.0f);
            }
            tex.SetPixels(0, 0, res, 1, colors);
            tex.Apply();
        }

        public class DoubleCurve{
            public AnimationCurve x = new AnimationCurve();
            public AnimationCurve y = new AnimationCurve();
        };

        public static void applyColorsFromCurve(TripleCurve curve, Texture2D tex){
            int res = tex.width;
            Color[] colors = new Color[res];
            for (int x = 0; x < res; x++){
                float sample = ((float)x)/(float)(res-1);
                colors[x] = new Color(curve.x.Evaluate(sample), curve.y.Evaluate(sample), curve.z.Evaluate(sample));
            }
            tex.SetPixels(0, 0, res, 1, colors);
            tex.Apply();
        }

        public class TextureLoader{
            String path;
            int materialID;
            String postfix;
            int res;
            TextureFormat format;
            TextureWrapMode wrapMode;
            Texture2D tex;

            public TextureLoader(String path, String postfix, int res, TextureFormat format, TextureWrapMode wrapMode){
                this.path = path;
                this.materialID = 0;
                this.postfix = postfix;
                this.res = res;
                this.format = format;
                this.wrapMode = wrapMode;
                this.tex = null;
            }

            public Texture2D loadOrCreate(int id){
                if(tex == null || materialID != id){
                    this.materialID = id;
                    String fullPath = path+materialID+postfix+".asset";
                    if(AssetDatabase.FindAssets(materialID+postfix, new String[] {path}).Length == 0){
                        tex = new Texture2D(res, 1, format, true);
                        tex.wrapMode = wrapMode;
                        // create path if it does not exist
                        if(!System.IO.Directory.Exists(path)){
                            System.IO.Directory.CreateDirectory(path);
                        }
                        AssetDatabase.CreateAsset(tex, fullPath);
                        // Debug.Log("new created");
                    }else{
                        tex = AssetDatabase.LoadAssetAtPath<Texture2D>(fullPath);
                        // Debug.Log("tex found");
                    }
                }
                return tex;
            }
        }

        /* ---------- Texture UI Elements ---------- */
        public static void createGradientTexture(MaterialEditor materialEditor, MaterialProperty texProp, Gradient gradient, TextureLoader texLoader, GUIContent description){
            EditorGUILayout.BeginHorizontal(GUIStyle.none);

            EditorGUILayout.PrefixLabel("placeholder");
            materialEditor.TexturePropertyMiniThumbnail(GUILayoutUtility.GetLastRect(), texProp, description.text, description.tooltip);
            EditorGUI.BeginChangeCheck();
            gradient = EditorGUILayout.GradientField(GUIContent.none, gradient, GUILayout.MinWidth(15));
            if (EditorGUI.EndChangeCheck()){
                Texture2D tex = texLoader.loadOrCreate(((Material)materialEditor.target).GetInstanceID());
                applyColorsFromGradient(gradient, tex);
                texProp.textureValue = tex;
            }

            EditorGUILayout.EndHorizontal();
        }

        public static void createCurveTexture(MaterialEditor materialEditor, MaterialProperty texProp, AnimationCurve curve, Rect ranges, TextureLoader texLoader, GUIContent description){
            EditorGUILayout.BeginHorizontal(GUIStyle.none);

            EditorGUILayout.PrefixLabel("placeholder");
            materialEditor.TexturePropertyMiniThumbnail(GUILayoutUtility.GetLastRect(), texProp, description.text, description.tooltip);
            EditorGUI.BeginChangeCheck();
            curve = EditorGUILayout.CurveField(GUIContent.none, curve, Color.red, ranges, GUILayout.MinWidth(15));
            if (EditorGUI.EndChangeCheck()){
                Texture2D tex = texLoader.loadOrCreate(((Material)materialEditor.target).GetInstanceID());
                applyColorsFromCurve(curve, tex);
                texProp.textureValue = tex;
            }

            EditorGUILayout.EndHorizontal();
        }

        public static void createCurveTexture(MaterialEditor materialEditor, MaterialProperty texProp, DoubleCurve curve, Rect ranges, TextureLoader texLoader, GUIContent description){
            EditorGUILayout.BeginHorizontal(GUIStyle.none);

            EditorGUILayout.PrefixLabel("placeholder");
            materialEditor.TexturePropertyMiniThumbnail(GUILayoutUtility.GetLastRect(), texProp, description.text, description.tooltip);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("X", GUILayout.Width(10));
            curve.x = EditorGUILayout.CurveField(GUIContent.none, curve.x, Color.red, ranges, GUILayout.MinWidth(15));
            EditorGUILayout.LabelField("Y", GUILayout.Width(10));
            curve.y = EditorGUILayout.CurveField(GUIContent.none, curve.y, Color.green, ranges, GUILayout.MinWidth(15));
            if (EditorGUI.EndChangeCheck()){
                Texture2D tex = texLoader.loadOrCreate(((Material)materialEditor.target).GetInstanceID());
                applyColorsFromCurve(curve, tex);
                texProp.textureValue = tex;
            }

            EditorGUILayout.EndHorizontal();
        }

        public static void createCurveTexture(MaterialEditor materialEditor, MaterialProperty texProp, TripleCurve curve, Rect ranges, TextureLoader texLoader, GUIContent description){
            EditorGUILayout.BeginHorizontal(GUIStyle.none);

            EditorGUILayout.PrefixLabel("placeholder");
            materialEditor.TexturePropertyMiniThumbnail(GUILayoutUtility.GetLastRect(), texProp, description.text, description.tooltip);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField("X", GUILayout.Width(10));
            curve.x = EditorGUILayout.CurveField(GUIContent.none, curve.x, Color.red, ranges, GUILayout.MinWidth(15));
            EditorGUILayout.LabelField("Y", GUILayout.Width(10));
            curve.y = EditorGUILayout.CurveField(GUIContent.none, curve.y, Color.green, ranges, GUILayout.MinWidth(15));
            EditorGUILayout.LabelField("Z", GUILayout.Width(10));
            curve.z = EditorGUILayout.CurveField(GUIContent.none, curve.z, Color.blue, ranges, GUILayout.MinWidth(15));
            if (EditorGUI.EndChangeCheck()){
                Texture2D tex = texLoader.loadOrCreate(((Material)materialEditor.target).GetInstanceID());
                applyColorsFromCurve(curve, tex);
                texProp.textureValue = tex;
            }

            EditorGUILayout.EndHorizontal();
        }

        public static void createShapeTexture(GUIContent content, ref bool isOpen, MaterialEditor materialEditor, MaterialProperty texProp, MaterialProperty posProp, MaterialProperty rotProp, MaterialProperty rotMat1Prop, MaterialProperty rotMat2Prop, MaterialProperty rotMat3Prop, MaterialProperty scaleProp, String keywordPrefix, bool defaultSpaceLocal = false){
            EditorGUILayout.BeginHorizontal(GUIStyle.none);
                GUILayout.Space(indentSize);
                materialEditor.TexturePropertySingleLine(content, texProp);
                int particleCount = texProp.textureValue != null ? (texProp.textureValue.width * texProp.textureValue.height / 2) : 0;
                GUI.Label(GUILayoutUtility.GetLastRect(), new GUIContent($"Particle Count: {particleCount}"), new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight });
                isOpen = EditorGUI.Foldout(GUILayoutUtility.GetLastRect(), isOpen, GUIContent.none, true);
            EditorGUILayout.EndHorizontal();

            if(isOpen){
                EditorGUILayout.BeginHorizontal(GUIStyle.none);
                GUILayout.Space(indentSize);
                EditorGUILayout.BeginVertical(GUIStyle.none);
                // create a disabled ui if the world space keyword is active
                EditorGUI.BeginDisabledGroup(!createKeywordSwitch(materialEditor,
                    keywordPrefix + "_TEX_WORLD_SPACE",
                    keywordPrefix + "_TEX_LOCAL_SPACE",
                    new GUIContent("Space"),
                    new GUIContent("World"),
                    new GUIContent("Local"),
                    defaultSpaceLocal));
                createTransformVectorFields(materialEditor, posProp, rotProp, rotMat1Prop, rotMat2Prop, rotMat3Prop, scaleProp);
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
        }

        /* ---------- Graph UI Elements ---------- */

        public class ForceField{
            private float mouseDownX = 1.0f;
            private bool isOpen = false;

            public void drawGUI(MaterialEditor materialEditor, MaterialProperty strength, MaterialProperty falloff, MaterialProperty offset, MaterialProperty forceLimit, String keywordPrefix, bool defaultBehaviorDynamic = false){
                EditorGUILayout.BeginHorizontal(GUIStyle.none);
                    GUILayout.Space(indentSize);
                    createLogSlider(materialEditor, strength, new GUIContent("Field Strength"));
                    isOpen = EditorGUI.Foldout(GUILayoutUtility.GetLastRect(), isOpen, GUIContent.none, true);
                EditorGUILayout.EndHorizontal();

                if(isOpen){
                    EditorGUILayout.BeginHorizontal(GUIStyle.none);
                    GUILayout.Space(indentSize);
                    EditorGUILayout.BeginVertical(GUIStyle.none);
                    bool isDynamic = createKeywordSwitch(materialEditor,
                        keywordPrefix + "_FORCE_FIELD_UNIFORM",
                        keywordPrefix + "_FORCE_FIELD_DYNAMIC",
                        new GUIContent("Field Behavior"),
                        new GUIContent("Uniform"),
                        new GUIContent("Dynamic"),
                        defaultBehaviorDynamic);
                    EditorGUI.BeginDisabledGroup(!isDynamic);
                    createLogSlider(materialEditor, falloff, new GUIContent("Strength Falloff"));
                    materialEditor.RangeProperty(offset, "Distance Offset");
                    createLogSlider(materialEditor, forceLimit, new GUIContent("Force Limit"), 0.001f);
                    if(isDynamic)drawGraph(materialEditor, strength.floatValue, falloff.floatValue, offset.floatValue, forceLimit.floatValue);
                    EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.EndHorizontal();
                }
            }

            // convert a field coordinate to a ui coordinate
            // the ui should always show the y axis from -maxForce to maxForce
            // use the maxForce of this field to calculate the zoom
            // make sure to keep the aspect ratio of the field flip the y axis for the ui
            private static Vector2 fieldToUICoord(Vector2 fieldCoord, Rect uiRect, float forceLimit){
                float zoom = 1.0f;
                if(forceLimit > 0.0f){
                    zoom = uiRect.height / (2.0f * forceLimit);
                }
                return new Vector2(
                    uiRect.xMin + fieldCoord.x * zoom,
                    (uiRect.yMin + uiRect.yMax) / 2.0f - (fieldCoord.y * zoom)
                );
            }

            // get the size of the field for the given ui rect
            private static Vector2 getFieldSize(Rect uiRect, float forceLimit){
                float zoom = 1.0f;
                if(forceLimit > 0.0f){
                    zoom = uiRect.height / (2.0f * forceLimit);
                }
                return new Vector2(
                    uiRect.width / zoom,
                    2.0f * forceLimit * zoom
                );
            }

            // get nearest step size for the forceLimit
            // the step size will be calculated by first getting the nearest power of 10 to the forceLimit
            // with this we can normalize the forceLimit to a value between 1 and 10
            // for values between 1 and 2 we use 1 as step size
            // for values between 2 and 5 we use 2 as step size
            // for values between 5 and 10 we use 5 as step size
            private static float getStepSize(float forceLimit){
                float power = Mathf.Pow(10.0f, Mathf.Floor(Mathf.Log10(forceLimit)));
                float normalizedSize = forceLimit / power;
                if(normalizedSize < 2.0f) return power;
                if(normalizedSize < 5.0f) return 2.0f * power;
                return 5.0f * power;
            }

            private static void placeText(Vector2 pos, String text, GUIStyle style, int xBound = 0){
                // get size of the text with the used style
                Vector2 size = style.CalcSize(new GUIContent(text));
                // create a rect with the size of the text and place it at the given position
                // if the xBound is set to a negative value, the text will left bound, if it is set to a positive value, the text will right bound, if it is set to zero, the text will be centered
                Rect rect = new Rect(pos.x - (xBound < 0 ? size.x : (xBound > 0 ? 0 : size.x / 2.0f)), pos.y - size.y / 2.0f, size.x, size.y);
                // draw the text
                EditorGUI.LabelField(rect, text, style);
            }

            private float fieldStrengthAt(float x, float strength, float falloff, float offset, float forceLimit){
                return Mathf.Min(Mathf.Max(Mathf.Pow(Mathf.Max(x + offset, 0.00001f), falloff) * strength, -forceLimit), forceLimit);
            }

            private void drawGraph(MaterialEditor materialEditor, float strength, float falloff, float offset, float forceLimit){
                // add a little space
                GUILayout.Space(2.0f);
                var fullRect = GUILayoutUtility.GetRect(100, 120);
                // split rect vertically 
                Vector2Int descriptionSize = Vector2Int.RoundToInt(EditorStyles.whiteMiniLabel.CalcSize(new GUIContent("-00")));
                var graphRect = new Rect(fullRect.xMin + descriptionSize.x, fullRect.yMin + descriptionSize.y, fullRect.width - descriptionSize.x, fullRect.height - descriptionSize.y*2);
                placeText(new Vector2(fullRect.x + descriptionSize.x, fullRect.y + descriptionSize.y / 2.0f), "Force over Distance", EditorStyles.whiteMiniLabel, 1);

                // return if the graph rect is too small
                if(graphRect.width < 1 || graphRect.height < 1) return;

                // reset handle color
                Handles.color = Color.white;
                // draw background box
                Handles.DrawSolidRectangleWithOutline(graphRect, new Color(0.2f, 0.2f, 0.2f), Color.black);
                Vector2 fieldSize = getFieldSize(graphRect, forceLimit);

                // early return if the force limit is zero
                if(forceLimit == 0.0f) return;

                // get the step size for the field
                float stepSize = getStepSize(forceLimit);
                Handles.color = new Color(0.1f, 0.1f, 0.1f);

                // draw vertical lines for each full unit of the field
                // walk from zero to the max value of the field in steps of the step size
                for(float i = 0.0f; i <= fieldSize.x; i += stepSize){
                    float x = fieldToUICoord(new Vector2(i, 0.0f), graphRect, forceLimit).x;
                    placeText(new Vector2(x, graphRect.yMax + descriptionSize.y / 2.0f), i.ToString("0.##"), EditorStyles.whiteMiniLabel);
                    Handles.DrawLine(new Vector3(x, graphRect.yMin), new Vector3(x, graphRect.yMax));
                }

                // draw horizontal lines for each full unit of the field
                // walk from the most negative multiple of the step size to the most positive multiple of the step size
                float stepStart = Mathf.Ceil(-forceLimit / stepSize) * stepSize;
                for(float i = stepStart; i <= forceLimit; i += stepSize){
                    float y = fieldToUICoord(new Vector2(0.0f, i), graphRect, forceLimit).y;
                    placeText(new Vector2(graphRect.xMin - descriptionSize.x / 2.0f, y), i.ToString("0.##"), EditorStyles.whiteMiniLabel);
                    Handles.DrawLine(new Vector3(graphRect.xMin, y), new Vector3(graphRect.xMax, y));
                }

                // draw field strength line as a graph of the field strength with the equation y = min(max(pow(x + UNITY_HALF_MIN + offset, falloff) * strength, -maxForce), maxForce)
                Handles.color = Color.white;
                var linePoints = new Vector3[(int)graphRect.width];
                for(int i = 0; i < linePoints.Length; i++){
                    float x = (float)i * fieldSize.x / (float)linePoints.Length;
                    linePoints[i] = fieldToUICoord(new Vector2(x, fieldStrengthAt(x, strength, falloff, offset, forceLimit)), graphRect, forceLimit);
                }
                // draw the actual line showing the force of the field in relation to the distance
                Handles.DrawAAPolyLine(2.0f, linePoints);

                // detect mouse press on the graph
                if(Event.current.type == EventType.MouseDown && graphRect.Contains(Event.current.mousePosition)){
                    // calculate the field coordinate x of the mouse position and store it
                    this.mouseDownX = (Event.current.mousePosition.x - graphRect.xMin) / graphRect.width * fieldSize.x;;
                }

                // draw a cross at the last mouse position and show the field strength above the top right of the graph
                float fieldX = this.mouseDownX;
                float fieldY = fieldStrengthAt(fieldX, strength, falloff, offset, forceLimit);
                Vector2 mousePos = fieldToUICoord(new Vector2(fieldX, fieldY), graphRect, forceLimit);
                Handles.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
                Handles.DrawLine(new Vector3(mousePos.x, graphRect.yMin), new Vector3(mousePos.x, graphRect.yMax));
                Handles.DrawLine(new Vector3(graphRect.xMin, mousePos.y), new Vector3(graphRect.xMax, mousePos.y));

                // show a label with the field strength and distance at the mouse position
                string text = "D=" + fieldX.ToString("0.###") + "\tF=" + fieldY.ToString("0.####");
                // show the label above the top right of the graph
                placeText(new Vector2(fullRect.x + fullRect.width, fullRect.y + descriptionSize.y / 2.0f), text, EditorStyles.whiteMiniLabel, -1);
            }
        }
    }
}