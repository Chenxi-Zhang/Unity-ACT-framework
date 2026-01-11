
using UnityEngine;
using UnityEngine.Playables;

public class AnimMixerAsset : PlayableAsset
{
    public AnimationClip clip;

    public bool layerMixer = false;
    [Range(0f, 1f)]
    public float weight = 1f;
    public AvatarMask mask;
    public bool isAdditive = false;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AnimMixerClip>.Create(graph);
        AnimMixerClip mixerClip = playable.GetBehaviour();
        mixerClip.clip = clip;
        mixerClip.layerMixer = layerMixer;
        mixerClip.mask = mask;
        mixerClip.isAdditive = isAdditive;
        mixerClip.weight = weight;
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

    public bool layerMixer = false;
    public float weight;
    public AvatarMask mask;
    public bool isAdditive;

#if UNITY_EDITOR
    public override void OnPlayableDestroy(Playable playable)
    {
        if (layerMixer)
            return;
        if (actor)
        {
            actor.movement.ResetTransform();
        }
    }
#endif

    public override void OnActionPause()
    {
        if (layerMixer)
            actor.animationSimpleBlender.RemoveLayerClip(clip);
        else
            actor.animationSimpleBlender.RemoveClip(clip);
    }

    public override void OnActionPlay()
    {
        if (layerMixer)
            actor.animationSimpleBlender.AddLayerClip(clip, mask, isAdditive, weight);
        else
            actor.animationSimpleBlender.AddClip(clip);
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        if (actor == null)
            return;
        if (layerMixer)
            actor.animationSimpleBlender.SetLayerTime(clip, (float)playable.GetTime());
        else
            actor.animationSimpleBlender.SetTime(clip, (float)playable.GetTime());
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            actor.animationSimpleBlender.DoUpdate(Time.deltaTime);
        }
#endif
    }
}