
using UnityEditor;
using UnityEngine;

public static class SerializedPropertyExtension
{
    public static void LayoutDrawRelative(this SerializedProperty property, string relativePath)
    {
        var prop = property.FindPropertyRelative(relativePath);
        if (prop != null)
        {
            EditorGUILayout.PropertyField(prop);
        }
    }

    public static void DrawRelative(this SerializedProperty property, string relativePath,
        Rect position, GUIContent label, bool includeChildren = true)
    {
        var prop = property.FindPropertyRelative(relativePath);
        if (prop != null)
        {
            EditorGUI.PropertyField(position, prop, label, includeChildren);
        }
    }

}