
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "HitCounterMapping", menuName = "Configs/HitCounterMapping")]
public class HitCounterMapping : BaseItemMapping<HitCounterType, HitCounterData>
{
    protected override int CompareKey(HitCounterType a, HitCounterType b)
    {
        return HitCounterConfig.Instance.Compare(a, b);
    }
}

[Serializable]
public class HitCounterData
{
    public bool ignoreHit = false;
    public DefenceData defenceData;
    public ShockTypeActionMapping shockTypeActionMapping;
}