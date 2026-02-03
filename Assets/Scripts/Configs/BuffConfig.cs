
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffConfig : ScriptableObject, ISerializationCallbackReceiver
{
    public static BuffConfig Instance => SingletonHolder.Instance.buffConfig;

    public BuffData[] buffDatas;
    private Dictionary<string, BuffData> buffDataDict = new();

    public bool TryGetValue(string buffId, out BuffData buffData)
    {
        if (string.IsNullOrEmpty(buffId))
        {
            buffData = null;
            return false;
        }
        return buffDataDict.TryGetValue(buffId, out buffData);
    }

    public void OnBeforeSerialize()
    {
    }

    public void OnAfterDeserialize()
    {
        buffDataDict.Clear();
        foreach (var buffData in buffDatas)
        {
            buffDataDict[buffData.buffId] = buffData;
        }
    }
}

public class BuffIdAttribute : Attribute
{
}
