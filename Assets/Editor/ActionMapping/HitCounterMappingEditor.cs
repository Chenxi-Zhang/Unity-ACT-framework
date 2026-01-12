
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HitCounterMapping))]
public class HitCounterMappingEditor : BaseItemMappingEditor
{

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawItemMappingList((HitCounterMapping)target);
        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    protected override IEnumerable<K> GetAllKeys<K>()
    {
        if (typeof(K) == typeof(HitCounterType))
        {
            foreach (HitCounterType hitCounterType in HitCounterConfig.Instance.Data)
            {
                yield return (K)(object)hitCounterType;
            }
        }
    }

}