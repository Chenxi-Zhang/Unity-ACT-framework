
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShockConfig : ScriptableObject
{
    public static ShockConfig Instance => SingletonHolder.Instance.shockConfig;

    [SerializeField]
    private ShockType[] _data;

    public ShockType[] Data => _data;

    // 运行时缓存
    private Dictionary<string, int> _runtimeCache;

    private void OnEnable()
    {
        BuildRuntimeCache();
    }

    private void BuildRuntimeCache()
    {
        _runtimeCache = new Dictionary<string, int>(_data.Length);
        for (int i = 0; i < _data.Length; i++)
        {
            _runtimeCache[_data[i].name] = i;
        }
    }


#if UNITY_EDITOR
    public int GetTypeIndexInEditor(ShockType type)
    {
        for (int i = 0; i < _data.Length; i++)
        {
            if (_data[i].name == type.name)
            {
                return i;
            }
        }
        return -1;
    }
#endif

    public int GetTypeIndex(ShockType type)
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return GetTypeIndexInEditor(type);
        }
#endif
        if (_runtimeCache == null)
        {
            BuildRuntimeCache();
        }
        return _runtimeCache.TryGetValue(type.name, out int index) ? index : -1;
    }

    public int Compare(ShockType a, ShockType b)
    {
        return GetTypeIndex(a).CompareTo(GetTypeIndex(b));
    }

}

[Serializable]
public struct ShockType
{
    public string name;

    public override string ToString()
    {
        return name;
    }
}
