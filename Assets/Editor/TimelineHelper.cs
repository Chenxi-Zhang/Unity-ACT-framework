
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
}