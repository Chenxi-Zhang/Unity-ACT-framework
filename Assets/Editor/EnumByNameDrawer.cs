#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumByName<>), true)]
public class EnumByNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get the generic type argument (the enum type)
        var enumType = fieldInfo.FieldType.GetGenericArguments()[0];

        // Get the string property
        var stringProp = property.FindPropertyRelative("value");

        // Parse current value
        Enum.TryParse(enumType, stringProp.stringValue, out object currentEnum);

        // Draw popup with null check
        var newEnum = EditorGUI.EnumPopup(position, label, (Enum)currentEnum ?? GetDefaultEnumValue(enumType));

        // Save back as string
        stringProp.stringValue = newEnum.ToString();
    }

    private Enum GetDefaultEnumValue(Type enumType)
    {
        // Get the first enum value (default value)
        var enumValues = Enum.GetValues(enumType);
        return enumValues.Length > 0 ? (Enum)enumValues.GetValue(0) : default(Enum);
    }
}
#endif
