
using UnityEditor.Timeline;
using UnityEngine.Timeline;

[CustomTimelineEditor(typeof(ActionTransitionTrack))]
class ActionTransitionTrackEditor : TrackEditor
{
    public override void OnCreate(TrackAsset track, TrackAsset copiedFrom)
    {
        if (copiedFrom)
        {
            return;
        }
        if (track is ActionTransitionTrack actionTransitionTrack)
        {
            var duration = track.timelineAsset.duration;
            var clip = actionTransitionTrack.CreateDefaultClip();
            clip.duration = duration;
        }
    }
}

[CustomTimelineEditor(typeof(ActionTransitionAsset))]
class ActionTransitionClipEditor : ClipEditor
{

    public override void OnClipChanged(TimelineClip clip)
    {
        var asset = clip.asset as ActionTransitionAsset;
        if (asset == null)
            return;
        var inputType = asset.inputType;
        var action = asset.action;
        var nextActionName = action != null ? action.name : "无";
        var name = $"{inputType}->{nextActionName}";
        clip.displayName = name;
    }

}