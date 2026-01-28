
using UnityEngine;
using UnityEngine.Playables;

public class CameraControllClip : ActionClipBase
{
    public bool isOverride = false;
    public bool isCameraUpdating = false;
    public CameraUpdateData cameraUpdateData;
    public bool impulse = false;

    public override void OnActionPlay()
    {
        if (isOverride)
        {
            actor.cameraStatus.isOverriding.AddStatus();
        }
        else if (isCameraUpdating)
        {
            actor.cameraStatus.UpdateCameraData(cameraUpdateData);
        }
        if (impulse)
            actor.cameraStatus.isImpulse = true;
    }

    public override void OnActionPause()
    {
        if (isOverride)
        {
            actor.cameraStatus.isOverriding.RemoveStatus();
        }
        else if (isCameraUpdating)
        {
            actor.cameraStatus.ResetCameraData();
        }
    }
}

public class CameraControllAsset : PlayableAsset
{
    public bool isOverride = false;
    public bool isCameraUpdating = false;
    public CameraUpdateData cameraUpdateData;
    public bool impulse = false;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<CameraControllClip>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.isOverride = isOverride;
        behaviour.isCameraUpdating = isCameraUpdating;
        behaviour.cameraUpdateData = cameraUpdateData;
        behaviour.impulse = impulse;
        return playable;
    }

}