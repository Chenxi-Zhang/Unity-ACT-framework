
using System;
using UnityEngine;
using UnityEngine.Playables;

class DisableTurnClip : ActionClipBase
{

    public override void OnActionPlay()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
        actor.movement.TurnDisabled.AddStatus();
    }

    public override void OnActionPause()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
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