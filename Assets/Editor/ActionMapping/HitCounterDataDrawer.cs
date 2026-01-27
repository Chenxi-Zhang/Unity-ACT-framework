
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HitCounterInfo))]
public class HitCounterDataDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 0;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, label, true);
        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            var ignoreHitProp = property.FindPropertyRelative("ignoreHit");
            EditorGUILayout.PropertyField(ignoreHitProp);
            if (!ignoreHitProp.boolValue)
            {
                EditorGUILayout.PropertyField(property.FindPropertyRelative("defenceData"));
                EditorGUILayout.PropertyField(property.FindPropertyRelative("shockTypeActionMapping"));
            }
            EditorGUI.indentLevel--;
        }
    }
}