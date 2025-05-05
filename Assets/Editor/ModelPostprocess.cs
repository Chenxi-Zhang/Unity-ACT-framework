
using UnityEditor;
using UnityEngine;

class ModelPostprocess : AssetPostprocessor
{

    void OnPostprocessModel(GameObject gameObject)
    {
        var importer = assetImporter as ModelImporter;
        if (importer == null)
            return;
        if (importer.animationType != ModelImporterAnimationType.Human)
            return;
        // Get all animation clips for this model
        var clips = importer.clipAnimations;
        if (clips == null || clips.Length == 0)
        {
            // If no custom clips, use defaultClips
            clips = importer.defaultClipAnimations;
        }
        // Set Root Transform Rotation > Based Upon (at Start) to Original for all clips
        // 我的想法是，尽可能按照模型动作数据进行旋转，未来再看是否需要可配置
        foreach (var clip in clips)
        {
            clip.keepOriginalOrientation = true;
        }
        importer.clipAnimations = clips; // Apply changes
    }
}