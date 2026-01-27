
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SfxData))]
public class SfxDataEditor : Editor
{
    SfxData sfxData;
    Actor actor;
    BindingInfoEditorHelper bindingInfoHelper;

    SerializedProperty prefabProp;
    SerializedProperty destroyDelayProp;
    SerializedProperty isAttachingProp;

    void OnEnable()
    {
        SceneView.duringSceneGui -= OnDuringSceneGUI;
        SceneView.duringSceneGui += OnDuringSceneGUI;
        sfxData = target as SfxData;
        actor = SceneObjectTool.GetComponentInPrefabStage<Actor>();
        if (sfxData == null)
            return;
        prefabProp = serializedObject.FindProperty("sfxPrefab");
        destroyDelayProp = serializedObject.FindProperty("destroyDelay");
        isAttachingProp = serializedObject.FindProperty("isAttaching");
        if (actor == null)
            return;
        var bindingInfoProp = serializedObject.FindProperty("bindingInfo");
        bindingInfoHelper = new BindingInfoEditorHelper(actor.transform, bindingInfoProp);
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnDuringSceneGUI;
        sfxData = null;
        actor = null;
        bindingInfoHelper = default;
        prefabProp = null;
        destroyDelayProp = null;
        isAttachingProp = null;
    }

    private void OnDuringSceneGUI(SceneView view)
    {
        if (Application.isPlaying)
            return;
        if (sfxData == null || actor == null)
            return;
        serializedObject.Update();
        bindingInfoHelper.OnDuringSceneGUI(view, sfxData.bindingInfo);
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        if (sfxData == null)
            return;
        serializedObject.Update();
        EditorGUILayout.PropertyField(prefabProp);
        EditorGUILayout.PropertyField(destroyDelayProp);
        EditorGUILayout.PropertyField(isAttachingProp);
        if (actor == null)
        {
            EditorGUILayout.HelpBox("No Actor found in the current Prefab Stage.", MessageType.Warning);
            serializedObject.ApplyModifiedProperties();
            return;
        }
        bindingInfoHelper.DrawInspectorGUI();
        serializedObject.ApplyModifiedProperties();
    }

}