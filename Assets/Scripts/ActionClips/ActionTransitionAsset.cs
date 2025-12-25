using UnityEngine;
using UnityEngine.Playables;

public class ActionTransitionAsset : PlayableAsset
{
    public EnumByName<InputType> inputType;
    public ActionTimelineAsset action;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ActionTransitionClip>.Create(graph);
        ActionTransitionClip clip = playable.GetBehaviour();
        clip.action = action;
        clip.inputType = inputType;
        return playable;
    }
}

public class ActionTransitionClip : ActionClipBase
{
    public InputType inputType;
    public ActionTimelineAsset action;

    public override void OnActionPlay()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif
        actor.logicInput.RegisterInputAction(inputType, OnInputTriggered);
    }

    void OnInputTriggered()
    {
        actor.actionPlayableDirector.PlayAction(action);
    }

    public override void OnActionPause()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif
        actor.logicInput.UnregisterInputAction(inputType);
    }
}