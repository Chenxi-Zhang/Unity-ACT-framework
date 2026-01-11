
using UnityEngine;
using UnityEngine.Playables;

public abstract class ActionClipBase : PlayableBehaviour
{

    protected Actor actor;

    bool isPlaying = false;
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (isPlaying)
            return;
        isPlaying = true;
        var director = playable.GetGraph().GetResolver() as PlayableDirector;
        if (director == null)
            return;
        actor = director.GetGenericBinding(ActionPlayableDirector.ActorBindingObj) as Actor;
#if UNITY_EDITOR
        if (!Application.isPlaying)
            actor = director.GetComponent<Actor>();
#endif
        if (actor == null)
            return;
        OnActionPlay();
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (!isPlaying)
            return;
        isPlaying = false;
        if (actor == null)
            return;
        OnActionPause();
    }

    public abstract void OnActionPlay();
    public abstract void OnActionPause();
}