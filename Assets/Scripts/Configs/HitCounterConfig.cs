
using System;
using UnityEngine;

public class HitCounterConfig : ScriptableObject
{
    public static HitCounterConfig Instance => SingletonHolder.Instance.hitCounterConfig;

    [SerializeField]
    private HitCounterType[] _data;

    public HitCounterType[] Data => _data;

    public int GetHitCounterTypeIndex(HitCounterType hitCounterType)
    {
        for (int i = 0; i < _data.Length; i++)
        {
            if (_data[i].name == hitCounterType.name)
            {
                return i;
            }
        }
        return -1;
    }

}

[Serializable]
public struct HitCounterType
{
    public string name;
}
