
using UnityEngine;
using UnityEngine.Playables;

public class DisableTurningToLockClip : ActionClipBase
{
    public override void OnActionPause()
    {
        actor.logicInput.disableTurningToLock.RemoveStatus();
    }

    public override void OnActionPlay()
    {
        actor.logicInput.disableTurningToLock.AddStatus();
    }
}

public class DisableTurningToLockAsset : PlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DisableTurningToLockClip>.Create(graph);
        return playable;
    }
}