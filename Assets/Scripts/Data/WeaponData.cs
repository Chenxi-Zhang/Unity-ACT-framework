
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponData
{
    public float baseDamage;
    public List<CounterData> counters;

    public float GetCounterDefRatio(HitCounterType counterType)
    {
        foreach (var counter in counters)
        {
            if (counter.counterType.name == counterType.name)
            {
                return counter.counterDefRatio;
            }
        }
        return 0f;
    }

}

[Serializable]
public class CounterData
{
    public HitCounterType counterType;
    public float counterDamage;
    [Range(0, 1)]
    public float counterDefRatio;
}
