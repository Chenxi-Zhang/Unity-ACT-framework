
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BuffData))]
public class BuffDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        property.serializedObject.Update();
        property.LayoutDrawRelative("buffId");
        var durationProp = property.FindPropertyRelative("duration");
        EditorGUILayout.PropertyField(durationProp);
        if (durationProp.floatValue > 0)
        {
            property.LayoutDrawRelative("canRefresh");
        }
        property.LayoutDrawRelative("addHpRatioImmediately");
        property.LayoutDrawRelative("addHpRatioDur");
        EditorGUILayout.Space(10);
        property.LayoutDrawRelative("sfx");
        property.LayoutDrawRelative("statusIcon");
        property.serializedObject.ApplyModifiedProperties();
        EditorGUI.EndProperty();
    }
}