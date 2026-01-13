
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class BaseItemMapping<K, V> : ScriptableObject, IItemMapping<K, V>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<K2V> list = new();
    private Dictionary<K, K2V> dict = new();

    [Serializable]
    public class K2V
    {
        public K key;
        public V value;
    }

    public void OnBeforeSerialize()
    {
        if (list == null)
            list = new List<K2V>();
        list.Clear();
        list.AddRange(dict.Values);
        list.Sort(Compare);
    }

    protected virtual int CompareKey(K a, K b)
    {
        return a.ToString().CompareTo(b.ToString());
    }

    private int Compare(K2V a, K2V b)
    {
        return CompareKey(a.key, b.key);
    }

    public void OnAfterDeserialize()
    {
        dict.Clear();
        if (list == null)
            return;
        foreach (var item in list)
        {
            dict[item.key] = item;
        }
    }

    public bool TryGetValue(K key, out V value)
    {
        if (dict.TryGetValue(key, out var k2v) && k2v.value != null)
        {
            value = k2v.value;
            return true;
        }
        value = default;
        return false;
    }
}