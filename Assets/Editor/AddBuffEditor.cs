
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AddBuffAsset))]
public class AddBuffEditor : Editor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var buffIdProp = serializedObject.FindProperty("buffId");
        if (buffIdProp.propertyType != SerializedPropertyType.String)
        {
            EditorGUILayout.LabelField("[BuffId]", "Must be a string type.");
            return;
        }
        GUILayout.BeginHorizontal();
        var position = EditorGUI.PrefixLabel(EditorGUILayout.GetControlRect(), new GUIContent("BuffId"));
        if (GUI.Button(position, buffIdProp.stringValue, EditorStyles.popup))
        {
            var buffIds = BuffConfig.Instance.buffDatas.Select(b => b.buffId).ToList();
            SearchableDropdown dropdown = new (buffIds, (selected) =>
            {
                buffIdProp.stringValue = selected;
                serializedObject.ApplyModifiedProperties();
            });
            PopupWindow.Show(position, dropdown);
        }
        GUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }
}