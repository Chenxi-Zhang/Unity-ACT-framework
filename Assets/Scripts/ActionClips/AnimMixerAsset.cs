
using UnityEngine;
using UnityEngine.Playables;

public class AnimMixerAsset : PlayableAsset
{
    public AnimationClip clip;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AnimMixerClip>.Create(graph);
        AnimMixerClip mixerClip = playable.GetBehaviour();
        mixerClip.clip = clip;
#if UNITY_EDITOR
        // 编辑器里，记录起始位置
        var actor = owner.GetComponent<Actor>();
        if (actor)
        {
            actor.movement.StartRecordMovement();
        }
#endif
        return playable;
    }
}

public class AnimMixerClip : ActionClipBase
{
    public AnimationClip clip;

#if UNITY_EDITOR
    public override void OnPlayableDestroy(Playable playable)
    {
        if (actor)
        {
            actor.movement.ResetTransform();
        }
    }
#endif

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