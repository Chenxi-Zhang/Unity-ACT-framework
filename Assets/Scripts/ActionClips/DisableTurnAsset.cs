
using System;
using UnityEngine;
using UnityEngine.Playables;

class DisableTurnClip : ActionClipBase
{

    public override void OnActionPlay()
    {
        actor.movement.TurnDisabled.AddStatus();
    }

    public override void OnActionPause()
    {
        actor.movement.TurnDisabled.RemoveStatus();
    }
}

class DisableTurnAsset : PlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DisableTurnClip>.Create(graph);
        return playable;
    }
}