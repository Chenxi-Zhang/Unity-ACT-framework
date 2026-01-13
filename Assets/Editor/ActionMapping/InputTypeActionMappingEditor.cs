
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(InputTypeActionMapping))]
public class InputTypeActionMappingEditor : BaseItemMappingEditor
{
    private SerializedProperty idleProperty;
    private SerializedProperty strafeProperty;

    private void OnEnable()
    {
        idleProperty = serializedObject.FindProperty("Idle");
        strafeProperty = serializedObject.FindProperty("Strafe");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var mapping = (InputTypeActionMapping)target;
        // 显示 Idle 字段
        EditorGUILayout.PropertyField(idleProperty);
        EditorGUILayout.PropertyField(strafeProperty);
        EditorGUILayout.Space();
        // 显示标题
        DrawItemMappingList(mapping);
        serializedObject.ApplyModifiedProperties();
        // 标记资产为脏，确保修改被保存
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    static HashSet<InputType> ignoredInputTypes = new()
    {
        InputType.None,
        // 添加其他需要忽略的 InputType
        InputType.ForceActionStrafe,

        InputType.ForceActionAI,
        InputType.ForceActionBeCounter,
        InputType.ForceBeHit,

    };

    protected override IEnumerable<K> GetAllKeys<K>()
    {
        if (typeof(K) == typeof(EnumByName<InputType>))
        {
            foreach (InputType inputType in Enum.GetValues(typeof(InputType)))
            {
                if (ignoredInputTypes.Contains(inputType))
                {
                    continue;
                }
                yield return (K)(object)new EnumByName<InputType> { Value = inputType };
            }
        }
    }

}