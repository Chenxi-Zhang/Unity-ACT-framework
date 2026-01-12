
using System;
using UnityEngine;

public class ShockConfig : ScriptableObject
{
    public static ShockConfig Instance => SingletonHolder.Instance.shockConfig;

    [SerializeField]
    private ShockType[] _data;

    public ShockType[] Data => _data;

    public int GetHitCounterTypeIndex(ShockType ShockType)
    {
        for (int i = 0; i < _data.Length; i++)
        {
            if (_data[i].name == ShockType.name)
            {
                return i;
            }
        }
        return -1;
    }

}

[Serializable]
public struct ShockType
{
    public string name;
}
