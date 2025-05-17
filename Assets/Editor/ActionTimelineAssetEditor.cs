
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

class ActionTimelineAssetEditor : Editor
{
    public static ActionTimelineAsset CreateActionTimelineAsset(string path)
    {
        var actionTimelineAsset = CreateInstance<ActionTimelineAsset>();
        AssetDatabase.CreateAsset(actionTimelineAsset, path);
        // 创建内置 TimelineAsset 作为子资源
        var timeline = CreateInstance<TimelineAsset>();
        timeline.editorSettings.frameRate = TimelineProjectSettings.instance.defaultFrameRate;
        timeline.name = "EmbeddedTimeline";
        actionTimelineAsset.SetTimelineAsset(timeline);
        AssetDatabase.AddObjectToAsset(timeline, actionTimelineAsset);
        AssetDatabase.SaveAssets();
        return actionTimelineAsset;
    }


    internal class DoCreateTimeline : UnityEditor.ProjectWindowCallback.EndNameEditAction
    {
        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var customTimeline = CreateActionTimelineAsset(pathName);
            ProjectWindowUtil.ShowCreatedAsset(customTimeline);
        }
    }

    [MenuItem("Assets/Create/Timeline/ActionTimeline", false, -124)]
    public static void CreateNewTimeline()
    {
        var icon = EditorGUIUtility.IconContent("TimelineAsset Icon").image as Texture2D;
        ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, CreateInstance<DoCreateTimeline>(), "New Timeline.asset", icon, null);
    }

    // 双击资源时打开Timeline窗口
    [OnOpenAsset(1)]
    public static bool OnOpenCustomTimeline(int instanceID, int line)
    {
        ActionTimelineAsset customTimeline = EditorUtility.InstanceIDToObject(instanceID) as ActionTimelineAsset;
        if (customTimeline != null)
        {
            OpenTimelineWindow(customTimeline);
            return true; // 表示我们已处理此资源
        }
        return false;
    }

    private static void OpenTimelineWindow(ActionTimelineAsset asset)
    {
        // 获取内部TimelineAsset
        TimelineAsset timelineAsset = asset.TimelineAsset;
        TimelineHelper.SelectDirector(timelineAsset);
        // 打开Timeline窗口并加载此资源
        var window = TimelineEditor.GetOrCreateWindow();
        window.SetTimeline(timelineAsset);
        window.Focus();
    }
}