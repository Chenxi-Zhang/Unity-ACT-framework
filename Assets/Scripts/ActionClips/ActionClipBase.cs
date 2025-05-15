
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
        actor = director.GetComponent<Actor>();
        if (actor == null)
            return;
        OnActionPlay();
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (!isPlaying)
            return;
        isPlaying = false;
        OnActionPause();
    }

    public abstract void OnActionPlay();
    public abstract void OnActionPause();
}