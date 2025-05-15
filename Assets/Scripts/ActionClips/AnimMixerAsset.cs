
using UnityEngine;
using UnityEngine.Playables;

class AnimMixerAsset : PlayableAsset
{
    public AnimationClip clip;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AnimMixerClip>.Create(graph);
        AnimMixerClip mixerClip = playable.GetBehaviour();
        mixerClip.clip = clip;
        return playable;
    }
}

class AnimMixerClip : ActionClipBase
{
    public AnimationClip clip;

    public override void OnActionPause()
    {
        actor.animationSimpleBlender.RemoveClip(clip);
    }

    public override void OnActionPlay()
    {
        actor.animationSimpleBlender.AddClip(clip);
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (actor == null)
            return;
        actor.animationSimpleBlender.SetTime(clip, (float)playable.GetTime());
    }
}