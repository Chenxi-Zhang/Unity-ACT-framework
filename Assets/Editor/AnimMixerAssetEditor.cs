
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine.Timeline;

[CustomEditor(typeof(AnimMixerAsset))]
public class AnimMixerAssetInspector : Editor
{
    private SerializedProperty clipProperty;
    private SerializedProperty layerMixerProperty;
    private SerializedProperty weightProperty;
    private SerializedProperty maskProperty;
    private SerializedProperty isAdditiveProperty;

    private void OnEnable()
    {
        clipProperty = serializedObject.FindProperty("clip");
        layerMixerProperty = serializedObject.FindProperty("layerMixer");
        weightProperty = serializedObject.FindProperty("weight");
        maskProperty = serializedObject.FindProperty("mask");
        isAdditiveProperty = serializedObject.FindProperty("isAdditive");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(clipProperty);

        // 只有当 layerMixer 为 true 时才显示 weight 和 mask
        if (layerMixerProperty.boolValue)
        {
            EditorGUILayout.LabelField("Layer Mixer Enabled", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(weightProperty);
            EditorGUILayout.PropertyField(maskProperty);
            EditorGUILayout.PropertyField(isAdditiveProperty);
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomTimelineEditor(typeof(AnimMixerAsset))]
public class AnimMixerAssetEditor : ClipEditor
{
    public override void OnCreate(TimelineClip clip, TrackAsset track, TimelineClip clonedFrom)
    {
        var asset = clip.asset as AnimMixerAsset;
        if (asset == null)
            return;
        var timeline = TimelineHelper.GetTimelineAssetFromTrack(track);
        if (timeline == null)
            return;
        var actionTimeline = TimelineHelper.GetParentActionTimelineAsset(timeline);
        if (actionTimeline == null)
            return;
        asset.layerMixer = actionTimeline.isSub;
    }
}