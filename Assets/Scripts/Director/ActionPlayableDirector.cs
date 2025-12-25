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
        playableDirector.stopped += OnPlayableDirectorStopped;
        PlayAction(Idle);
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        if (playingAction.loop)
        {
            // PlayAction(playingAction);
        }
        else
        {
            if (playingAction.next != null)
                PlayAction(playingAction.next);
            else
                PlayAction(Idle);
        }
    }

    public override void PlayAction(ActionTimelineAsset action)
    {
        actorMovement.ResetRotation();
        playableDirector.Play(action.TimelineAsset, action.loop ? DirectorWrapMode.Loop : DirectorWrapMode.None);
        playingAction = action;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
