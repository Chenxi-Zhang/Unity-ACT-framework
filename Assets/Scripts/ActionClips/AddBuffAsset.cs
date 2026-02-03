
using UnityEngine;
using UnityEngine.Playables;

class AddBuffClip : ActionClipBase
{
    public BuffData buffData;
    public override void OnActionPause()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
        if (buffData.IsStatic)
        {
            actor.buffs.RemoveBuff(buffData);
        }
    }

    public override void OnActionPlay()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
        actor.buffs.AddBuff(buffData);
    }

}

public class AddBuffAsset : PlayableAsset
{
    [BuffId]
    public string buffId;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        if (!BuffConfig.Instance.TryGetValue(buffId, out var buffData))
        {
            return Playable.Create(graph);
        }
        var playable = ScriptPlayable<AddBuffClip>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.buffData = buffData;
        return playable;
    }
}
