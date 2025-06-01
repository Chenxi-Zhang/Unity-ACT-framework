
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class ItemPlayableDirector : BasePlayableDirector
{
    private ActionTimelineAsset playingAction;

    void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
        playableDirector.playableAsset = null;
        playableDirector.extrapolationMode = DirectorWrapMode.None;
        // playableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
    }

    void Start()
    {
        playableDirector.stopped -= OnDirectorStopped;
        playableDirector.stopped += OnDirectorStopped;
    }

    public override void PlayAction(ActionTimelineAsset action)
    {
        playingAction = action;
        playableDirector.Play(action.TimelineAsset, DirectorWrapMode.None);
    }

    void OnDirectorStopped(PlayableDirector director)
    {
        if (playingAction.loop)
        {
            PlayAction(playingAction);
            return;
        }
        if (playingAction.next)
        {
            PlayAction(playingAction.next);
            return;
        }
    }
}