
using UnityEngine;
using UnityEngine.Playables;

public class BeHitShockChangeClip : ActionClipBase
{
    public ShockTypeActionMapping shockTypeActionMapping;

    public override void OnActionPause()
    {
        actor.beHit.shockTypeActionMappingManager.RemoveMapping(shockTypeActionMapping);
    }

    public override void OnActionPlay()
    {
        actor.beHit.shockTypeActionMappingManager.AddMapping(shockTypeActionMapping);
    }
}

public class BeHitShockChangeAsset : PlayableAsset
{
    public ShockTypeActionMapping shockTypeActionMapping;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        if (shockTypeActionMapping == null)
        {
            Debug.LogError("ShockTypeActionMapping is null");
            return Playable.Create(graph);
        }
        var playable = ScriptPlayable<BeHitShockChangeClip>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.shockTypeActionMapping = shockTypeActionMapping;
        return playable;
    }

}
