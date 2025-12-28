
using UnityEngine;
using UnityEngine.Playables;

class StrafeMoveClip : ActionClipBase
{
    public override void OnActionPause()
    {
        actor.logicInput.canStrafe.RemoveStatus();
    }

    public override void OnActionPlay()
    {
        actor.logicInput.canStrafe.AddStatus();
    }

}

public class StrafeMoveAsset : PlayableAsset
{

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<StrafeMoveClip>.Create(graph);
        var behaviour = playable.GetBehaviour();
        return playable;
    }
}