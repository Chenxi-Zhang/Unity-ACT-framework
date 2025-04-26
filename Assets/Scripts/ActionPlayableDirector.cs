using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ActionPlayableDirector : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public TimelineAsset Idle;

    private TimelineAsset playingAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playableDirector.extrapolationMode = DirectorWrapMode.None;
        playableDirector.stopped += OnPlayableDirectorStopped;
        playableDirector.Play(Idle);
    }

    private void OnPlayableDirectorStopped(PlayableDirector director)
    {
        playableDirector.Play(Idle);
    }

    public void PlayAction(TimelineAsset action)
    {
        playableDirector.Play(action);
        playingAction = action;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
