using UnityEditor;
using UnityEngine;

namespace Quantum{

public static class UpdateHelper
{
    public static void updateMaterial(MaterialEditor materialEditor, MaterialProperty[] properties, int targetVersion){
        // iterate through all materials of the editor
        foreach (Material material in materialEditor.targets){
            // check the shader name to determine the type of material
            string shaderName = material.shader.name;
            if (shaderName.Contains("Visualizer")){
                // get version from material
                int currentVersion = material.GetInteger("_ShaderVersionVis");
                UpdateVisualizerMaterial(material, currentVersion, targetVersion);
            } else if (shaderName.Contains("Simulator")){
                // get version from material
                int currentVersion = material.GetInteger("_ShaderVersionSim");
                UpdateSimulatorMaterial(material, currentVersion, targetVersion);
            } else {
                Debug.LogError("Unknown shader type: " + shaderName);
            }
        }
    }

    private static void UpdateVisualizerMaterial(Material material, int currentVersion, int targetVersion){
        // this will be implemented with more features in the future
        // for now we just set the default values
        if (currentVersion < 0){
            setVisualizerDefaults(material, targetVersion);
            return;
        } else {
            material.SetInteger("_ShaderVersionVis", targetVersion);
        }
    }

    private static void setVisualizerDefaults(Material material, int targetVersion){
        // set version to target version
        material.SetInteger("_ShaderVersionVis", targetVersion);
        // set all default keywords
        material.EnableKeyword("INPUT_TEX_WORLD_SPACE");
        material.EnableKeyword("COLOR_MODE_STATIC");
        material.EnableKeyword("COLOR_FACTOR_CLAMP");
        material.EnableKeyword("COLOR_FALLOFF_NONE");
        material.EnableKeyword("COLOR_ANIM_TEX_OFF");
        material.EnableKeyword("COLOR_ANIM_TEX_FACTOR_CLAMP");
        material.EnableKeyword("RENDER_AFTER_LIFETIME");
        material.EnableKeyword("SHAPE_MODE_CIRCLE");
        material.EnableKeyword("SHAPE_LINE_DRAW_PARTIAL");
        material.EnableKeyword("SHAPE_LINE_TYPE_RELATIVE");
        material.EnableKeyword("SHAPE_LINE_WORLD_SPACE");
        material.EnableKeyword("SHAPE_LINE_OFFSET_DYNAMIC");
        material.EnableKeyword("SHAPE_SIZE_CONSTANT");
        material.EnableKeyword("SHAPE_SIZE_FACTOR_CLAMP");
        material.EnableKeyword("ROT_SPACE_WORLD");
        material.EnableKeyword("ROT_BASE_SINGLE");
        material.EnableKeyword("ROT_ANIM_NONE");
        material.EnableKeyword("ROT_ANIM_FACTOR_CLAMP");
    }

    private static void UpdateSimulatorMaterial(Material material, int currentVersion, int targetVersion){
        // this will be implemented with more features in the future
        // for now we just set the default values
        if (currentVersion < 0){
            setSimulatorDefaults(material, targetVersion);
            return;
        } else {
            material.SetInteger("_ShaderVersionSim", targetVersion);
        }
    }

    private static void setSimulatorDefaults(Material material, int targetVersion){
        // set version to target version
        material.SetInteger("_ShaderVersionSim", targetVersion);
        // set all default keywords
        material.EnableKeyword("INPUT_TEX_WORLD_SPACE");
        material.EnableKeyword("ATTR_FORCE_POINT");
        material.EnableKeyword("ATTR_FORCE_TEX_LOCAL_SPACE");
        material.EnableKeyword("ATTR_FORCE_AFFECT_VELOCITY");
        material.EnableKeyword("ATTR_FORCE_PREVENT_OVERSHOOT");
        material.EnableKeyword("ATTR_FORCE_FIELD_DYNAMIC");
        material.EnableKeyword("DIR_FORCE_SHAPE_UNIFORM");
        material.EnableKeyword("DIR_FORCE_WORLD_SPACE");
        material.EnableKeyword("DIR_FORCE_AFFECT_VELOCITY");
        material.EnableKeyword("DIR_FORCE_FIELD_UNIFORM");
        material.EnableKeyword("ROT_FORCE_DIST_AXIS");
        material.EnableKeyword("ROT_FORCE_LOCAL_SPACE");
        material.EnableKeyword("ROT_FORCE_AFFECT_VELOCITY");
        material.EnableKeyword("ROT_FORCE_FIELD_UNIFORM");
        material.EnableKeyword("TURB_FORCE_TIME_DEPENDENT");
        material.EnableKeyword("TURB_FORCE_WORLD_SPACE");
        material.EnableKeyword("TURB_FORCE_AFFECT_VELOCITY");
        material.EnableKeyword("TURB_FORCE_FIELD_UNIFORM");
        material.EnableKeyword("DRAG_FORCE_POSITION");
        material.EnableKeyword("DRAG_FORCE_AFFECT_VELOCITY");
        material.EnableKeyword("DRAG_FORCE_FIELD_DYNAMIC");
        material.EnableKeyword("SPAWN_SOURCE_SINGLE");
        material.EnableKeyword("SPAWN_TEX_LOCAL_SPACE");
        material.EnableKeyword("SPAWN_POS_SPHERE");
        material.EnableKeyword("SPAWN_POS_TEX_LOCAL_SPACE");
        material.EnableKeyword("SPAWN_VEL_VAL");
        material.EnableKeyword("SPAWN_VEL_TEX_LOCAL_SPACE");
        material.EnableKeyword("SPAWN_AGE_RANGE");
        material.EnableKeyword("SPAWN_LIFE_RANGE");
        material.EnableKeyword("SPAWN_RATE_ALL");
        material.EnableKeyword("SPAWN_RESPAWN_CONDITIONS");
        material.EnableKeyword("SPAWN_CONDITION_POS_OFF");
        material.EnableKeyword("SPAWN_CONDITION_VEL_OFF");
        material.EnableKeyword("SPAWN_CONDITION_AGE_LARGER");
        material.EnableKeyword("SPAWN_CONDITION_LIFETIME_INFINITE");
        material.EnableKeyword("SPAWN_OFFSET_NONE");
        material.EnableKeyword("SPAWN_ADD_NONE");
    }
}
}