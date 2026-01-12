
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ShockTypeActionMapping))]
public class ShockTypeActionMappingEditor : BaseItemMappingEditor
{

    void OnEnable()
    {
        ValidateMapping((ShockTypeActionMapping)target);
    }

    private void ValidateMapping(ShockTypeActionMapping target)
    {
        var dict = GetDictionary(target);
        if (dict == null)
            return;

        // 1. 获取所有应该存在的 Key
        var allKeys = new HashSet<ShockType>();
        foreach (var key in GetAllKeys<ShockType>())
        {
            allKeys.Add(key);
        }

        // 2. 删除多余的（dict 中有，但 allKeys 没有的）
        var keysToRemove = new List<ShockType>();
        foreach (var key in dict.Keys)
        {
            if (!allKeys.Contains(key))
            {
                keysToRemove.Add(key);
            }
        }
        foreach (var key in keysToRemove)
        {
            dict.Remove(key);
        }

        // 3. 添加缺失的（allKeys 有，但 dict 中没有的）
        foreach (var key in allKeys)
        {
            if (!dict.ContainsKey(key))
            {
                dict[key] = new BaseItemMapping<ShockType, ActionTimelineAsset>.K2V
                {
                    key = key,
                    value = null
                };
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawAllKeysItem((ShockTypeActionMapping)target);
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    protected override IEnumerable<K> GetAllKeys<K>()
    {
        if (typeof(K) == typeof(ShockType))
        {
            foreach (ShockType shockType in ShockConfig.Instance.Data)
            {
                yield return (K)(object)shockType;
            }
        }
    }

}