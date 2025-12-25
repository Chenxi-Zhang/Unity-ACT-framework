using UnityEngine;
using UnityEngine.Playables;

public class ActionPlayableDirector : BasePlayableDirector
{
    public ActionTimelineAsset Idle;
    public ActorMovement actorMovement;

    private ActionTimelineAsset playingAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playableDirector.extrapolationMode = DirectorWrapMode.None;
        // playableDirector.stopped += OnPlayableDirectorStopped;
        playableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
        PlayAction(Idle);
    }

    private void OnPlayableDirectorStopped(double initialTime = 0f)
    {
        if (playingAction.loop)
        {
            PlayAction(playingAction, initialTime);
        }
        else
        {
            if (playingAction.next != null)
                PlayAction(playingAction.next, initialTime);
            else
                PlayAction(Idle, initialTime);
        }
    }

    public override void PlayAction(ActionTimelineAsset action, double initialTime = 0)
    {
        actorMovement.ResetRotation();
        playableDirector.Play(action.TimelineAsset);
        playableDirector.time = 0;
        playingAction = action;
        DoUpdate((float)initialTime);
    }

    public void DoUpdate(float deltaTime)
    {
        var time = playableDirector.time + deltaTime;
        if (time >= playableDirector.duration)
        {
            OnPlayableDirectorStopped(time - playableDirector.duration);
        }
        else
        {
            playableDirector.time = time;
            playableDirector.Evaluate();
        }
    }
}
