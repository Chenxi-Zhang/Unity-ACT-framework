using UnityEngine;
using UnityEditor;
using UnityEngine.Timeline;
using System.IO;
using System;

public class ActionTimelineAutoCreator
{
    [MenuItem("Assets/Tools/ActionTimeline/Gen Timeline(Anims->Actions)", true)]
    static bool CheckCreateAllTimeline()
    {
        foreach (var obj in Selection.objects)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer != null)
            {
                return true;
            }
        }
        return false;
    }

    [MenuItem("Assets/Tools/ActionTimeline/Gen Timeline(Anims->Actions)", false, 1000)]
    public static void CreateAllTimeline()
    {
        var basePath = AssetDatabase.GetAssetPath(Selection.activeObject);
        var baseFolder = Path.GetDirectoryName(basePath).Replace("\\", "/");
        // Popup a file selection dialog for the user to pick a model file
        string actionsFolder = EditorUtility.OpenFolderPanel("Select action folder", baseFolder, "");
        if (!PathUtility.TryGetAssetPath(actionsFolder, out var assetFolder))
        {
            Debug.LogWarning($"Not asset folder: {actionsFolder}");
            return;
        }
        foreach (var obj in Selection.objects)
        {
            var path = AssetDatabase.GetAssetPath(obj);
            var importer = AssetImporter.GetAtPath(path) as ModelImporter;
            if (importer == null)
            {
                continue;
            }
            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
            if (clip == null || clip.name.StartsWith("__preview__"))
                continue;
            string timelinePath = Path.Combine(assetFolder, PathUtility.SanitizeFileNamePart($"{clip.name}_{obj.name}.asset"));
            if (File.Exists(timelinePath))
            {
                Debug.LogWarning($"Timeline exist: {timelinePath}, skip");
                continue;
            }
            CreateActionTimelineFromClip(timelinePath, clip);
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/Tools/ActionTimeline/Gen Timeline(Clip->Action)", true)]
    public static bool CheckCreateTimelineFromAnimationClip()
    {
        var obj = Selection.activeObject as AnimationClip;
        if (obj == null)
        {
            return false;
        }
        return true;
    }

    [MenuItem("Assets/Tools/ActionTimeline/Gen Timeline(Clip->Action)", false, 1010)]
    public static void CreateTimelineFromAnimationClip()
    {
        // Get the selected animation clip
        var clip = Selection.activeObject as AnimationClip;
        if (clip == null)
            return;
        // Get folder of the current selection
        var basePath = AssetDatabase.GetAssetPath(Selection.activeObject);
        var baseFolder = Path.GetDirectoryName(basePath).Replace("\\", "/");

        // Generate a default file name based on the clip name
        string sanitizedName = PathUtility.SanitizeFileNamePart(clip.name);
        string defaultFileName = $"{sanitizedName}.asset";

        // Show save file dialog to let user choose the exact file path
        string savePath = EditorUtility.SaveFilePanel(
            "Save Timeline Asset",
            baseFolder,
            defaultFileName,
            "asset");
        // Handle cancel button press
        if (string.IsNullOrEmpty(savePath))
            return;
        // Convert from absolute path to Unity asset path
        if (!PathUtility.TryGetAssetPath(savePath, out var timelinePath))
        {
            Debug.LogWarning($"Invalid asset path: {savePath}");
            return;
        }
        // Check if file already exists
        if (File.Exists(timelinePath))
        {
            if (!EditorUtility.DisplayDialog("File Exists",
                $"The file {Path.GetFileName(timelinePath)} already exists. Do you want to overwrite it?",
                "Overwrite", "Cancel"))
            {
                return;
            }
        }

        // Create the timeline asset
        CreateActionTimelineFromClip(timelinePath, clip);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // Select and ping the created asset
        var asset = AssetDatabase.LoadAssetAtPath<ActionTimelineAsset>(timelinePath);
        if (asset != null)
        {
            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }
    }

    static void CreateActionTimelineFromClip(string timelinePath, AnimationClip animClip)
    {
        if (animClip.legacy)
        {
            Debug.LogError($"Legacy Animation Clips are not supported: {timelinePath}");
            return;
        }
        var customTimelineAsset = ActionTimelineAssetEditor.CreateActionTimelineAsset(timelinePath);
        var timelineAsset = customTimelineAsset.TimelineAsset;
        var animTrack = timelineAsset.CreateTrack<AnimMixerTrack>(null, "Anim Mix Track");
        var timelineClip = animTrack.CreateClip<AnimMixerAsset>();
        double clipLength = GetAnimationClipLength(animClip);

        var mixerAsset = timelineClip.asset as AnimMixerAsset;
        mixerAsset.clip = animClip;
        mixerAsset.name = animClip.name;
        if (!double.IsInfinity(clipLength) && clipLength >= 1.0 / 60.0 && clipLength < 1000000.0)
        {
            timelineClip.duration = clipLength;
        }
        timelineClip.displayName = animClip.name;

        timelineAsset.durationMode = TimelineAsset.DurationMode.FixedLength;
        timelineAsset.fixedDuration = clipLength;
    }

    // fixes rounding errors from using single precision for length
    public static double GetAnimationClipLength(AnimationClip clip)
    {
        if (clip == null || clip.empty)
            return 0;

        double length = clip.length;
        if (clip.frameRate > 0)
        {
            double frames = Mathf.Round(clip.length * clip.frameRate);
            length = frames / clip.frameRate;
        }
        return length;
    }

}