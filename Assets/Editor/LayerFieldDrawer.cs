#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(LayerFieldAttribute))]
public class LayerFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 确保属性是整数类型
        if (property.propertyType == SerializedPropertyType.Integer)
        {
            // 使用内置的LayerField绘制器来显示层选择下拉菜单
            EditorGUI.BeginChangeCheck();
            int layer = EditorGUI.LayerField(position, label, property.intValue);
            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = layer;
            }
        }
        else
        {
            // 如果不是整数类型，显示错误
            EditorGUI.LabelField(position, label.text, "LayerField attribute can only be used with int fields");
        }
    }
}
#endif