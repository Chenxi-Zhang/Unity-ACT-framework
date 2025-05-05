
using UnityEngine;
using UnityEngine.Timeline;

public class ActionTimelineAsset : ScriptableObject
{

    public bool loop;
    public ActionTimelineAsset next;

    [SerializeField, HideInInspector]
    private TimelineAsset _timelineAsset;
    public TimelineAsset TimelineAsset => _timelineAsset;

    public void SetTimelineAsset(TimelineAsset timelineAsset)
    {
        _timelineAsset = timelineAsset;
    }
}