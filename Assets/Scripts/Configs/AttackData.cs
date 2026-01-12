
using System;

[Serializable]
public class AttackData
{
    public AttackBeCounterData[] hitCounters;
    public ShockType shockType;
}

[Serializable]
public class AttackBeCounterData
{
    public HitCounterType hitCounter;
    public ActionTimelineAsset beCounterAction;
}
