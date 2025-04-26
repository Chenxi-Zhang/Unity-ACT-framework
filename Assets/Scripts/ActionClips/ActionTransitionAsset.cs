using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ActionTransitionAsset : PlayableAsset
{
    public InputType inputType;
    public TimelineAsset action;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ActionTransitionClip>.Create(graph);
        ActionTransitionClip clip = playable.GetBehaviour();
        clip.action = action;
        clip.inputType = inputType;
        return playable;
    }
}

public class ActionTransitionClip : PlayableBehaviour
{
    public InputType inputType;
    public TimelineAsset action;

    Actor actor;

    bool isPlaying = false;
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (isPlaying)
            return;
        isPlaying = true;
        actor = info.output.GetUserData() as Actor;
        if (actor == null)
            return;
        actor.logicInput.RegisterInputAction(inputType, OnInputTriggered);
    }

    private void OnInputTriggered()
    {
        actor.actionPlayableDirector.PlayAction(action);
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (!isPlaying)
            return;
        isPlaying = false;
        actor.logicInput.UnregisterInputAction(inputType);
    }
}