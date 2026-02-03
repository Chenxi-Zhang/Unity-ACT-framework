
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(BuffIdAttribute))]
public class BuffIdDropdown : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.LabelField(position, label.text, "Use BuffIdDropdown with string.");
            return;
        }

        if (GUI.Button(position, label.text + ": " + property.stringValue, EditorStyles.popup))
        {
            var buffIds = BuffConfig.Instance.buffDatas.Select(b => b.buffId).ToList();
            SearchableDropdown dropdown = new SearchableDropdown(buffIds, (selected) =>
            {
                property.stringValue = selected;
                property.serializedObject.ApplyModifiedProperties();
            });
            PopupWindow.Show(position, dropdown);
        }
    }
}