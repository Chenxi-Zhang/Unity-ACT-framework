
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(DefenceData))]
public class DefenceDataDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var shockData = ShockConfig.Instance.Data;
        if (shockData == null) return EditorGUIUtility.singleLineHeight;

        return (shockData.Length + 1) * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var actionsProp = property.FindPropertyRelative("responseActions");
        var shockData = ShockConfig.Instance.Data;

        if (shockData == null)
        {
            EditorGUI.LabelField(position, "ShockConfig not found");
            EditorGUI.EndProperty();
            return;
        }

        // 标题行
        Rect labelRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.LabelField(labelRect, label);
        position.y += EditorGUIUtility.singleLineHeight;

        // 确保数组大小匹配
        if (actionsProp.arraySize != shockData.Length)
        {
            actionsProp.arraySize = shockData.Length;
        }

        EditorGUI.indentLevel++;
        // 为每个 ShockType 显示一行
        for (int i = 0; i < shockData.Length; i++)
        {
            Rect rowRect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var element = actionsProp.GetArrayElementAtIndex(i);
            var shockTypeProp = element.FindPropertyRelative("shockType");
            var actionProp = element.FindPropertyRelative("actionTimelineAsset");
            // 更新 shockType 值
            shockTypeProp.FindPropertyRelative("name").stringValue = shockData[i].name;
            // 左侧：ShockType 名称（只读）
            var restRect = EditorGUI.PrefixLabel(rowRect, new GUIContent(shockData[i].name));
            EditorGUI.PropertyField(restRect, actionProp, GUIContent.none);
            position.y += EditorGUIUtility.singleLineHeight;
        }
        EditorGUI.indentLevel--;

        EditorGUI.EndProperty();
    }
}