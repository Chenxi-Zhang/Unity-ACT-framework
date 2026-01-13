
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public abstract class BaseItemMappingEditor : Editor
{

    public void DrawItemMappingList<K, V>(BaseItemMapping<K, V> mapping)
    {
        // 显示标题
        EditorGUILayout.LabelField($"{typeof(K).Name} - {typeof(V).Name} Mappings", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 使用 SerializedProperty 来遍历 list
        var listProperty = serializedObject.FindProperty("list");
        if (listProperty != null && listProperty.isArray)
        {
            // 记录需要删除的索引
            var indexToRemove = -1;

            for (int i = 0; i < listProperty.arraySize; i++)
            {
                var element = listProperty.GetArrayElementAtIndex(i);
                var keyProperty = element.FindPropertyRelative("key");
                var valueProperty = element.FindPropertyRelative("value");

                if (keyProperty != null && valueProperty != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.PropertyField(valueProperty, new GUIContent(GetKeyLabel(keyProperty)));
                    EditorGUILayout.EndVertical();
                    // 删除按钮
                    if (GUILayout.Button("Remove", GUILayout.Width(80)))
                    {
                        indexToRemove = i;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }

            // 在循环外执行删除
            if (indexToRemove >= 0)
            {
                listProperty.DeleteArrayElementAtIndex(indexToRemove);
            }
        }

        EditorGUILayout.Space();
        // 添加按钮
        if (GUILayout.Button("Add Mapping", GUILayout.Height(30)))
        {
            ShowAddMenu(mapping);
        }
    }

    protected abstract IEnumerable<K> GetAllKeys<K>();

    public void ShowAddMenu<K, V>(BaseItemMapping<K, V> mapping)
    {
        // 获取当前字典
        var dict = GetDictionary(mapping);
        if (dict == null) return;

        // 创建 GenericMenu
        var menu = new GenericMenu();

        // 遍历所有Key
        foreach (K inputType in GetAllKeys<K>())
        {
            // 跳过已经存在的键
            if (dict.ContainsKey(inputType))
                continue;
            // 添加菜单项
            var capturedType = inputType;
            menu.AddItem(new GUIContent(inputType.ToString()), false, () =>
            {
                // 创建新的 KeyToValue 实例
                var keyToValue = new BaseItemMapping<K, V>.K2V
                {
                    key = capturedType,
                    value = default
                };

                dict[capturedType] = keyToValue;
                EditorUtility.SetDirty(target);
            });
        }

        menu.ShowAsContext();
    }

    public static Dictionary<K, BaseItemMapping<K, V>.K2V> GetDictionary<K, V>(BaseItemMapping<K, V> mapping)
    {
        var field = typeof(BaseItemMapping<K, V>).GetField("dict", BindingFlags.NonPublic | BindingFlags.Instance);
        return field?.GetValue(mapping) as Dictionary<K, BaseItemMapping<K, V>.K2V>;
    }

    protected virtual string GetKeyLabel(SerializedProperty property) => property.propertyType switch
    {
        SerializedPropertyType.Enum => property.enumDisplayNames[property.enumValueIndex],
        SerializedPropertyType.Integer => property.intValue.ToString(),
        SerializedPropertyType.String => property.stringValue,
        SerializedPropertyType.Float => property.floatValue.ToString(),
        _ => property.boxedValue?.ToString() ?? "null"
    };

}