using UnityEngine;
using UnityEngine.Playables;

public class ActionPlayableDirector : MonoBehaviour
{
    public PlayableDirector playableDirector;
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
            PlayAction(playingAction);
        }
        else
        {
            if (playingAction.next != null)
                PlayAction(playingAction.next);
            else
                PlayAction(Idle);
        }
    }

    public void PlayAction(ActionTimelineAsset action)
    {
        actorMovement.ResetRotation();
        playableDirector.Play(action.TimelineAsset);
        playingAction = action;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
