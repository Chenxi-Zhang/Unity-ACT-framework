
using System;

[Serializable]
public class BuffData
{

    public string buffId;
    public float duration;
    public bool IsStatic => duration <= 0f;
    public bool canRefresh;

    public float addHpRatioImmediately;
    public float addHpRatioDur;

}