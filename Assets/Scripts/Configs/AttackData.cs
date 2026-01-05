
using System;

[Serializable]
public class AttackData
{
    public AttackBeCounterData[] hitCounters;
}

[Serializable]
public class AttackBeCounterData
{
    public HitCounterType hitCounter;
    public ActionTimelineAsset beCounterAction;
}
