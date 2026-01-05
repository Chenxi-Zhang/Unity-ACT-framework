
using System;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HitCounterType))]
public class HitCounterTypePropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var nameProp = property.FindPropertyRelative("name");
        bool isEditable = IsInHitCounterConfig(property);

        if (isEditable)
        {
            EditorGUI.BeginChangeCheck();
            string oldVal = nameProp.stringValue;
            string newVal = EditorGUI.TextField(position, label, oldVal);
            if (EditorGUI.EndChangeCheck())
            {
                nameProp.stringValue = newVal;
                nameProp.serializedObject.ApplyModifiedProperties();
            }
        }
        else
        {
            var options = GetHitCounterOptions();
            int currentIndex = Array.IndexOf(options, nameProp.stringValue);
            if (currentIndex < 0)
                currentIndex = -1;

            Rect popupPosition = EditorGUI.PrefixLabel(position, label);
            EditorGUI.BeginChangeCheck();
            int newIndex = EditorGUI.Popup(popupPosition, currentIndex, options);
            if (EditorGUI.EndChangeCheck())
            {
                nameProp.stringValue = options[newIndex];
                nameProp.serializedObject.ApplyModifiedProperties();
            }
        }

        EditorGUI.EndProperty();
    }

    private bool IsInHitCounterConfig(SerializedProperty property)
    {
        var target = property.serializedObject.targetObject;
        if (target is HitCounterConfig)
        {
            return true;
        }
        return false;
    }

    private string[] GetHitCounterOptions()
    {
        if (HitCounterConfig.Instance != null && HitCounterConfig.Instance.Data != null)
        {
            var names = new string[HitCounterConfig.Instance.Data.Length];
            for (int i = 0; i < HitCounterConfig.Instance.Data.Length; i++)
            {
                names[i] = HitCounterConfig.Instance.Data[i].name;
            }
            return names;
        }
        return new string[0];
    }
}