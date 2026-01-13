
using System;
using System.Collections.Generic;

[Serializable]
public abstract class BaseItemMappingManager<T, K, V> : IItemMapping<K, V> where T : IItemMapping<K, V>
{
    public T defaultMapping;
    private List<T> mappings = new();
    public List<T> Mappings => mappings;

    public bool TryGetValue(K key, out V value)
    {
        for (int i = mappings.Count - 1; i >= 0; i--)
        {
            var itemMapping = mappings[i];
            if (itemMapping.TryGetValue(key, out value))
            {
                return true;
            }
        }
        if (defaultMapping == null)
        {
            value = default;
            return false;
        }
        return defaultMapping.TryGetValue(key, out value);
    }

    public void AddMapping(T mapping)
    {
        mappings.Add(mapping);
    }

    public void RemoveMapping(T mapping)
    {
        int index = mappings.LastIndexOf(mapping);
        if (index >= 0)
        {
            mappings.RemoveAt(index);
        }
    }
}