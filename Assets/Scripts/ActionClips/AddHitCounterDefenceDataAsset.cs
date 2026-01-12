
using UnityEngine;
using UnityEngine.Playables;

public class AddHitCounterDefenceDataClip : ActionClipBase
{
    public HitCounterMapping mapping;
    public override void OnActionPause()
    {
        actor.beHit.hitCounterMappingManager.RemoveMapping(mapping);
    }

    public override void OnActionPlay()
    {
        actor.beHit.hitCounterMappingManager.AddMapping(mapping);
    }
}

public class AddHitCounterDefenceDataAsset : PlayableAsset
{
    public HitCounterMapping mapping;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        if (mapping == null)
        {
            return Playable.Create(graph);
        }
        var playable = ScriptPlayable<AddHitCounterDefenceDataClip>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.mapping = mapping;
        return playable;
    }

}
