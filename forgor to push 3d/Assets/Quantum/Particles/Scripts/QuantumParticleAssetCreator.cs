#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace Quantum{
    public class QuantumParticleAssetCreator : EditorWindow{

        // section foldouts
        bool foldoutMesh = false;
        bool foldoutShapeTexture = false;
        bool foldoutRenderTexture = false;
        bool foldoutOffsetTexture = false;
        // scroll position
        private Vector2 scrollPosition = Vector2.zero;

        // path to save assets
        static string path = "Assets/Quantum/Particles/Resources/";
        static string meshSubPath = "Meshes/";
        static string renderSubPath = "RenderTextures/";
        static string shapeSubPath = "Shapes/";
        static string offsetSubPath = "Offsets/";

        // texture resolution
        static int resolution = 512;

        // mesh creation
        static float bounds = 10;
        static string particleMeshName = "NewParticleMesh";
        static string simulatorMeshName = "NewSimulatorMesh";

        // render texture creation
        static string renderTextureName = "NewRenderTexture";

        // shape texture creation
        static string shapeTextureName = "NewShapeTexture";
        static float radius = 0.5f;
        static int detail = 10;
        static bool colorEncoding;
        static Texture2D inputTex;
        static Mesh inputMesh;

        // offset texture creation
        static string offsetTextureName = "NewOffsetTexture";
        
        [MenuItem("Tools/Quantum/Particles")]
        private static void Init(){
            GetWindow<QuantumParticleAssetCreator>("Quantum Particle Asset Creator");
        }

        protected virtual void OnGUI(){

            // scroll view
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            // path to save assets
            path = EditorGUILayout.TextField("Particle Resources Path", path);
            // give a warning if the path doesn't exist
            if(!System.IO.Directory.Exists(path)){
                EditorGUILayout.HelpBox("Path does not exist", MessageType.Warning);
            }

            // texture resolution

            // create an integer field for the resolution but with button right and left to increase or decrease the value by multiples of 2
            EditorGUILayout.BeginHorizontal();
            resolution = EditorGUILayout.IntField("Resolution", resolution);
            if(GUILayout.Button("<<", GUILayout.Width(30))){
                int log2 = (int)Mathf.Ceil(Mathf.Log(resolution, 2));
                if(log2 > 2) log2--;
                resolution = (int)Mathf.Pow(2, log2);
            }
            if(GUILayout.Button(">>", GUILayout.Width(30))){
                int log2 = (int)Mathf.Log(resolution, 2);
                if(log2 < 13) log2++;
                resolution = (int)Mathf.Pow(2, log2);
            }
            EditorGUILayout.EndHorizontal();

            // show particle count
            EditorGUILayout.LabelField("Particle Count: " + resolution * resolution / 2);

            if(resolution <= 4){
                EditorGUILayout.HelpBox("A resolution this low could break some features", MessageType.Warning);
            }
            if(resolution >= 2048){
                EditorGUILayout.HelpBox("Performance Warning !", MessageType.Warning);
            }

            // horizontal line
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            // section for mesh creation
            foldoutMesh = EditorGUILayout.Foldout(foldoutMesh, "Meshes", true);
            if (foldoutMesh){
                MeshSection();
            }

            // horizontal line
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            // section for Render Textures
            foldoutRenderTexture = EditorGUILayout.Foldout(foldoutRenderTexture, "Render Textures", true);
            if (foldoutRenderTexture){
                RenderTextureSection();
            }

            // horizontal line
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            // section for Shape Textures
            foldoutShapeTexture = EditorGUILayout.Foldout(foldoutShapeTexture, "Shape Textures", true);
            if (foldoutShapeTexture){
                ShapeTextureSection();
            }
            
            // horizontal line
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            // section for Offset Textures
            foldoutOffsetTexture = EditorGUILayout.Foldout(foldoutOffsetTexture, "Offset Textures", true);
            if (foldoutOffsetTexture){
                OffsetTextureSection();
            }

            EditorGUILayout.EndScrollView();
        }

        private void saveAsset(UnityEngine.Object asset, string subPath, string name){
            // create entire path if it doesn't exist
            if(!System.IO.Directory.Exists(path + subPath)){
                System.IO.Directory.CreateDirectory(path + subPath);
            }
            AssetDatabase.CreateAsset(asset, path + subPath + name + ".asset");
        }

        private void MeshSection(){
            GUILayout.Label ("\nParticle Mesh", EditorStyles.boldLabel);

            bounds = EditorGUILayout.FloatField ("Bounds", bounds);
            particleMeshName = EditorGUILayout.TextField ("Mesh Name", particleMeshName);

            if (GUILayout.Button("Create")){
                int size = resolution * resolution / 2;
                // big thanks to Nave for this useful tiny script
                Mesh mesh = new Mesh();
                mesh.vertices = new Vector3[] {new Vector3(0, 0, 0)};
                mesh.SetIndices(new int[size], MeshTopology.Points, 0);
                mesh.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(bounds, bounds, bounds));

                saveAsset(mesh, meshSubPath, particleMeshName);
            }

            GUILayout.Label ("\nSimulator Mesh", EditorStyles.boldLabel);
            simulatorMeshName = EditorGUILayout.TextField ("Mesh Name", simulatorMeshName);
            if (GUILayout.Button("Create")){
                // big thanks to Nave for this useful tiny script
                Mesh mesh = new Mesh();
                mesh.vertices = new Vector3[] {new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0)};
                mesh.uv = new Vector2[] {new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1)};
                mesh.triangles =  new int[] {0, 1, 2, 2, 1, 3};
                mesh.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(0, 0, 0));

                saveAsset(mesh, meshSubPath, simulatorMeshName);
            }

        }

        private void RenderTextureSection(){
            GUILayout.Label ("\nData Texture Pair", EditorStyles.boldLabel);

            renderTextureName = EditorGUILayout.TextField ("Texture Name", renderTextureName);

            if (GUILayout.Button("Create")){
                RenderTexture texture0 = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
                texture0.wrapMode = TextureWrapMode.Repeat;
                texture0.filterMode = FilterMode.Point;
                texture0.useMipMap = false;
                texture0.anisoLevel = 0;
                texture0.antiAliasing = 1;

                RenderTexture texture1 = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
                texture1.wrapMode = TextureWrapMode.Repeat;
                texture1.filterMode = FilterMode.Point;
                texture1.useMipMap = false;
                texture1.anisoLevel = 0;
                texture1.antiAliasing = 1;

                saveAsset(texture0, renderSubPath, renderTextureName + " 0");
                saveAsset(texture1, renderSubPath, renderTextureName + " 1");
            }
        }

        private void ShapeTextureSection(){
            GUILayout.Label ("");
            shapeTextureName = EditorGUILayout.TextField ("Texture Name", shapeTextureName);

            GUILayout.Label ("\nSphere Shape", EditorStyles.boldLabel);
            radius = EditorGUILayout.FloatField ("Radius", radius);
            if (GUILayout.Button("Create")){
                var texture = new Texture2D(resolution, resolution, TextureFormat.RGBAFloat, false, true);
                texture.filterMode = FilterMode.Point;

                for(int y = 0; y < resolution; y++){
                    for(int x = 0; x < resolution; x++){
                        if(y < resolution/2){
                            Vector3 pos = UnityEngine.Random.insideUnitSphere * radius;
                            texture.SetPixel(x, y, new Color(pos.x, pos.y, pos.z, UnityEngine.Random.Range(0.0f, 1.0f)));
                        }else{
                            texture.SetPixel(x, y, new Color(0.0f, 0.0f, 0.0f, 1.0f));
                        }
                    }
                }

                texture.Apply();
                saveAsset(texture, shapeSubPath, shapeTextureName);
            }

            GUILayout.Label ("\nShape from Image", EditorStyles.boldLabel);
            inputTex = (Texture2D)EditorGUILayout.ObjectField(inputTex, typeof(Texture2D), true);

            bool validImage = inputTex != null;
            if(validImage){
                string path = AssetDatabase.GetAssetPath(inputTex);
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                if(textureImporter != null && !textureImporter.isReadable){
                    // give a warning if the image is not readable
                    EditorGUILayout.HelpBox("Image is not readable, please enable Read/Write in the import settings", MessageType.Warning);
                    validImage = false;
                }
            }

            colorEncoding = EditorGUILayout.Toggle ("Encode Colors", colorEncoding);
            EditorGUI.BeginDisabledGroup(colorEncoding);
            detail = EditorGUILayout.IntField ("Detail", detail);
            // warning if the detail is too high
            if(detail >= 100){
                if(detail >= 1000){
                    EditorGUILayout.HelpBox("Detail is very high, this could take a very long time to create", MessageType.Warning);
                }else{
                    EditorGUILayout.HelpBox("A higher Detail will take longer to create", MessageType.Warning);
                }
            }
            EditorGUI.EndDisabledGroup();
            
            // create disabled button if the image is not valid
            EditorGUI.BeginDisabledGroup(!validImage);
            if (GUILayout.Button("Create")){
                if(colorEncoding){
                    saveAsset(CreateShapeTextureImageColored(inputTex, resolution), shapeSubPath, shapeTextureName);
                }else{
                    saveAsset(CreateShapeTextureImage(inputTex, resolution, detail), shapeSubPath, shapeTextureName);
                }
            }
            EditorGUI.EndDisabledGroup();

            /*-----MODEL CREATION-----*/
            GUILayout.Label ("\nShape from Model", EditorStyles.boldLabel);
            inputMesh = (Mesh)EditorGUILayout.ObjectField(inputMesh, typeof(Mesh), true);

            bool validModel = inputMesh != null;
            if(validModel){
                string path = AssetDatabase.GetAssetPath(inputMesh);
                ModelImporter modelImporter = AssetImporter.GetAtPath(path) as ModelImporter;
                if(modelImporter != null && !modelImporter.isReadable){
                    // give a warning if read/write is not enabled for the mesh
                    EditorGUILayout.HelpBox("Mesh is not readable, please enable Read/Write in the import settings", MessageType.Warning);
                    validModel = false;
                }
            }

            // create disabled button if the mesh is not valid
            EditorGUI.BeginDisabledGroup(!validModel);
            if (GUILayout.Button("Create")){
                saveAsset(CreateShapeModelTexture(inputMesh, resolution), shapeSubPath, shapeTextureName);
            }
            EditorGUI.EndDisabledGroup();
        }

        static Texture2D CreateShapeTextureImage(Texture2D source, int resolution, int maxTries){
            var texture = new Texture2D(resolution, resolution, TextureFormat.RGBAFloat, false, true);
            texture.filterMode = FilterMode.Point;
            float w = source.width, h = source.height;
            if(w > h){
                h /= w;
                w = 1.0f;
            }else{
                w /= h;
                h = 1.0f;
            }
            int tries = 0;

            for(int y = 0; y < resolution/2; y++){
                for(int x = 0; x < resolution; x++){
                    Vector3 pos = new Vector3(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
                    if(source.GetPixel((int)(pos.x*source.width), (int)(pos.y*source.height)).grayscale > pos.z || tries > maxTries){
                        texture.SetPixel(x, y, new Color((pos.x-0.5f)*w, (pos.y-0.5f)*h, 0.0f, UnityEngine.Random.Range(0.0f, 1.0f)));
                        tries = 0;
                    }else{
                        x--;
                        tries++;
                    }
                }
            }

            for(int y = resolution/2; y < resolution; y++){
                for(int x = 0; x < resolution; x++){
                    texture.SetPixel(x, y, new Color(0.0f, 0.0f, 0.0f, 1.0f));
                }
            }

            texture.Apply();
            return texture;
        }

        static Texture2D CreateShapeTextureImageColored(Texture2D source, int resolution){
            var texture = new Texture2D(resolution, resolution, TextureFormat.RGBAFloat, false, true);
            texture.filterMode = FilterMode.Point;
            float w = source.width, h = source.height;
            if(w > h){
                h /= w;
                w = 1.0f;
            }else{
                w /= h;
                h = 1.0f;
            }
            for(int y = 0; y < resolution/2; y++){
                for(int x = 0; x < resolution; x++){
                    texture.SetPixel(x, y, new Color((((float)x)/resolution-0.5f)*w, (((float)y)/(resolution/2)-0.5f)*h, 0.0f, UnityEngine.Random.Range(0.0f, 1.0f)));
                    texture.SetPixel(x, y+resolution/2, new Color(0.0f, 0.0f, 0.0f, 1.0f));
                }
            }

            texture.Apply();
            return texture;
        }

        static Texture2D CreateShapeModelTexture(Mesh source, int resolution){
            Texture2D texture = new Texture2D(resolution, resolution, TextureFormat.RGBAFloat, false, true);
            texture.filterMode = FilterMode.Point;

            int[] tris = source.triangles;
            Vector3[] verts = source.vertices;
            for(int y = 0; y < resolution/2; y++){
                for(int x = 0; x < resolution; x++){
                    int primID = (int)((tris.Length / 3) * ((y*resolution+x) / (float)(resolution*resolution/2)));
                    Vector3[] vertices = new Vector3[]{verts[tris[primID*3]], verts[tris[primID*3+1]], verts[tris[primID*3+2]]};
                    Vector2 rand = new Vector2(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
                    if(rand.x + rand.y > 1.0f){
                        rand = new Vector2(1.0f, 1.0f) - rand;
                    }
                    Vector3 pos = vertices[0] + (vertices[1] - vertices[0])*rand.x + (vertices[2] - vertices[0])*rand.y;
                    texture.SetPixel(x, y, new Color(pos.x, pos.y, pos.z, UnityEngine.Random.Range(0.0f, 1.0f)));
                }
            }

            for(int y = resolution/2; y < resolution; y++){
                for(int x = 0; x < resolution; x++){
                    texture.SetPixel(x, y, new Color(0.0f, 0.0f, 0.0f, 1.0f));
                }
            }

            texture.Apply();
            return texture;
        }

        private void OffsetTextureSection(){
            GUILayout.Label ("\nTree Structure", EditorStyles.boldLabel);

            offsetTextureName = EditorGUILayout.TextField ("Texture Name", offsetTextureName);

            if (GUILayout.Button("Create")){
                var texture = new Texture2D(resolution, resolution, TextureFormat.RFloat, false, true);
                texture.filterMode = FilterMode.Point;

                for(int y = 0; y < resolution; y++){
                    for(int x = 0; x < resolution; x++){
                        Vector2Int offset = new Vector2Int((resolution / 2) - x, (resolution / 4) - y % (resolution / 2));
                        if(offset.x == 0 && offset.y == 0)
                            offset = new Vector2Int(0, 0);
                        if(offset.x == 0)
                            offset = new Vector2Int(0, Math.Sign(offset.y));
                        else if(offset.y == 0)
                            offset = new Vector2Int(Math.Sign(offset.x), 0);
                        else {
                            var xAxis = UnityEngine.Random.Range(0, Math.Abs(offset.x) + Math.Abs(offset.y)) < Math.Abs(offset.x);
                            offset = new Vector2Int(xAxis ? Math.Sign(offset.x) : 0, !xAxis ? Math.Sign(offset.y) : 0);
                        }
                        texture.SetPixel(x, y, new Color(offset.x + offset.y * resolution, 0.0f, 0.0f, 0.0f));
                    }
                }

                texture.Apply();
                saveAsset(texture, offsetSubPath, offsetTextureName);
            }
        }
    }
}
#endif