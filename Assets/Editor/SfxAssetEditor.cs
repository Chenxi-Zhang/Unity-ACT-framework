
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SfxAsset))]
public class SfxAssetEditor : Editor
{
    SfxAsset sfxAsset;
    Actor actor;
    BindingInfoEditorHelper bindingInfoHelper;

    SerializedProperty prefabProp;
    SerializedProperty destroyDelayProp;
    SerializedProperty isAttachingProp;

    void OnEnable()
    {
        SceneView.duringSceneGui -= OnDuringSceneGUI;
        SceneView.duringSceneGui += OnDuringSceneGUI;
        sfxAsset = target as SfxAsset;
        actor = SceneObjectTool.GetComponentInPrefabStage<Actor>();
        if (sfxAsset == null || actor == null)
            return;

        prefabProp = serializedObject.FindProperty("sfxPrefab");
        destroyDelayProp = serializedObject.FindProperty("destroyDelay");
        isAttachingProp = serializedObject.FindProperty("isAttaching");
        var bindingInfoProp = serializedObject.FindProperty("bindingInfo");
        bindingInfoHelper = new BindingInfoEditorHelper(actor.transform, bindingInfoProp);
    }

    void OnDisable()
    {
        SceneView.duringSceneGui -= OnDuringSceneGUI;
        sfxAsset = null;
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
        if (sfxAsset == null || actor == null)
            return;
        serializedObject.Update();
        bindingInfoHelper.OnDuringSceneGUI(view, sfxAsset.bindingInfo);
        serializedObject.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        if (sfxAsset == null || actor == null)
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