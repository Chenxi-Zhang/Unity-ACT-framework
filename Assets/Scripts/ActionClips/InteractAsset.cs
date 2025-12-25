
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class InteractAsset : PlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<InteractClip>.Create(graph);
        return playable;
    }
}

class InteractClip : ActionClipBase
{
    public override void OnActionPause()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif
        actor.logicInput.UnregisterInputAction(InputType.Interact);
    }

    public override void OnActionPlay()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif
        actor.logicInput.RegisterInputAction(InputType.Interact, InputCallback);
    }

    private void InputCallback()
    {
        var focusedObj = actor.interactChecker.focusedObj;
        if (focusedObj != null)
        {
            focusedObj.ApplyInteract(actor);
        }
    }
}
