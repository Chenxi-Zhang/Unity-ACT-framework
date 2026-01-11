
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

class TimelineHelper
{

    // 双击资源时打开Timeline窗口。优先级高于TimelineWindow
    [OnOpenAsset(0)]
    public static bool OnDoubleClick(int instanceID, int line)
    {
        var asset = EditorUtility.InstanceIDToObject(instanceID) as TimelineAsset;
        if (asset == null)
            return false;
        SelectDirector(asset);
        // true表示我们已处理此资源，这里返回false，让TimelineWindow继续处理
        return false;
    }

    public static void SelectDirector(TimelineAsset asset)
    {
        var director = FindPlayableDirector();
        if (director != null)
        {
            Selection.activeObject = director.gameObject;
            EditorGUIUtility.PingObject(director.gameObject);
            director.playableAsset = asset;
        }
    }

    private static PlayableDirector FindPlayableDirector()
    {
        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null)
        {
            foreach (var go in prefabStage.scene.GetRootGameObjects())
            {
                var director = go.GetComponentInChildren<PlayableDirector>(true);
                if (director != null)
                    return director;
            }
        }
        else
        {
            return Object.FindAnyObjectByType<PlayableDirector>();
        }
        return null;
    }

    public static TimelineAsset GetTimelineAssetFromClip(TimelineClip clip)
    {
        if (clip == null) return null;
        TrackAsset track = clip.GetParentTrack();
        if (track == null) return null;
        return track.timelineAsset;
    }

    public static TimelineAsset GetTimelineAssetFromTrack(TrackAsset track)
    {
        if (track == null) return null;
        return track.timelineAsset;
    }

    public static ActionTimelineAsset GetParentActionTimelineAsset(TimelineAsset timelineAsset)
    {
        if (timelineAsset == null) return null;

        // 获取 TimelineAsset 的路径
        string assetPath = AssetDatabase.GetAssetPath(timelineAsset);
        if (string.IsNullOrEmpty(assetPath)) return null;

        // 加载该路径下的所有资源
        var allAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

        // 查找 ActionTimelineAsset 类型的资源（主资源）
        foreach (var asset in allAssets)
        {
            if (asset is ActionTimelineAsset actionTimeline)
            {
                return actionTimeline;
            }
        }

        return null;
    }

}