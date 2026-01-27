
using System;
using UnityEngine;
using UnityEngine.Playables;

public class SfxClip : ActionClipBase
{
    public SfxData sfxData;

    private SfxRuntimeData sfxRuntimeData;

    public override void OnActionPlay()
    {
        sfxRuntimeData = sfxData.CreateSfxFor(actor);
    }

    public override void OnActionPause()
    {
        sfxRuntimeData.Destroy();
    }

#if UNITY_EDITOR
    public override void OnPlayableDestroy(Playable playable)
    {
        if (!Application.isPlaying)
        {
            sfxRuntimeData?.Destroy();
            sfxRuntimeData = null;
        }
    }
#endif

}

public class SfxAsset : PlayableAsset
{
    public SfxData sfxData;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        if (sfxData == null)
        {
            return Playable.Create(graph);
        }
        var playable = ScriptPlayable<SfxClip>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.sfxData = sfxData;
        return playable;
    }
}