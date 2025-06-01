
using System;
using UnityEngine;
using UnityEngine.Playables;

class DisableTurnClip : ActionClipBase
{

    public override void OnActionPlay()
    {
        actor.movement.AddDisableTurn();
    }

    public override void OnActionPause()
    {
        actor.movement.RemoveDisableTurn();
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