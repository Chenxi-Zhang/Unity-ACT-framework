
using UnityEngine;
using UnityEngine.Playables;

public class AddHitCounterDefenceDataClip : ActionClipBase
{
    public HitCounterDefenceData data;
    public override void OnActionPause()
    {
        actor.beHit.hitCounters.Remove(data);
    }

    public override void OnActionPlay()
    {
        actor.beHit.hitCounters.Add(data);
    }
}

public class AddHitCounterDefenceDataAsset : PlayableAsset
{
    public HitCounterDefenceData data;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AddHitCounterDefenceDataClip>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.data = data;
        return playable;
    }
}
