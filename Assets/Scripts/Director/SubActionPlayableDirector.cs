
using System;
using UnityEngine;
using UnityEngine.Playables;

public class SubActionPlayableDirector : BasePlayableDirector
{

    private ActionTimelineAsset playingAction;
    public ActionTimelineAsset PlayingAction => playingAction;

    void Start()
    {
        playableDirector.extrapolationMode = DirectorWrapMode.None;
        playableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
    }

    public override void PlayAction(ActionTimelineAsset action, double initialTime = 0)
    {
        Debug.Log($"Sub Play Action: {action.name}");
        playableDirector.Play(action.TimelineAsset);
        playableDirector.time = 0;
        playingAction = action;
        DoUpdate((float)initialTime);
    }

    public void StopAction()
    {
        playableDirector.Stop();
        playingAction = null;
    }

    public void DoUpdate(float deltaTime)
    {
        if (playingAction == null)
            return;
        var time = playableDirector.time + deltaTime;
        if (time >= playableDirector.duration)
        {
            StopAction();
        }
        else
        {
            playableDirector.time = time;
            playableDirector.Evaluate();
        }
    }

}