using UnityEngine;
using UnityEngine.Playables;

public class ActionTransitionAsset : PlayableAsset
{
    public EnumByName<InputType> inputType;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ActionTransitionClip>.Create(graph);
        ActionTransitionClip clip = playable.GetBehaviour();
        clip.inputType = inputType;
        return playable;
    }
}

public class ActionTransitionClip : ActionClipBase
{
    public InputType inputType;

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
        actor.actionPlayableDirector.TriggerInputType(inputType);
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