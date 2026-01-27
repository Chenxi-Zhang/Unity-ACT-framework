
using System;

[Serializable]
public class AttackData
{
    public AttackBeCounterData[] hitCounters;
    public ShockType shockType;
    public float damageMultiplier = 1f;
}

[Serializable]
public class AttackBeCounterData
{
    public HitCounterType hitCounter;
    public ActionTimelineAsset beCounterAction;
}
