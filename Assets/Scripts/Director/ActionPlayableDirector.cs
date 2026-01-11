using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ActionPlayableDirector : BasePlayableDirector
{
    public Actor actor;
    public ActionTimelineAsset Idle;
    public ActorMovement actorMovement;

    private ActionTimelineAsset playingAction;
    public ActionTimelineAsset PlayingAction => playingAction;

    public Transform subActionRoot;
    private List<SubActionPlayableDirector> subActionDirectors = new();

    private static UnityEngine.Object _actorBindingObj;
    public static UnityEngine.Object ActorBindingObj
    {
        get
        {
            if (_actorBindingObj == null)
            {
                _actorBindingObj = ScriptableObject.CreateInstance<ScriptableObject>();
                _actorBindingObj.hideFlags = HideFlags.HideAndDontSave;
            }
            return _actorBindingObj;
        }
    }

    public event Action onActionDone;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playableDirector.extrapolationMode = DirectorWrapMode.None;
        // playableDirector.stopped += OnPlayableDirectorStopped;
        playableDirector.timeUpdateMode = DirectorUpdateMode.Manual;
        playableDirector.SetGenericBinding(ActorBindingObj, actor);
        DoPlayAction(Idle);
    }

    private void OnPlayableDirectorStopped(double initialTime = 0f)
    {
        if (playingAction.loop)
        {
            DoPlayAction(playingAction, initialTime);
        }
        else
        {
            if (playingAction.next != null)
                DoPlayAction(playingAction.next, initialTime);
            else
                DoPlayAction(Idle, initialTime);
        }
    }

    public override void PlayAction(ActionTimelineAsset action, double initialTime = 0)
    {
        if (action.isSub)
        {
            DoPlaySubAction(action, initialTime);
        }
        else
        {
            DoPlayAction(action, initialTime);
        }
    }

    private void DoPlayAction(ActionTimelineAsset action, double initialTime = 0)
    {
        onActionDone?.Invoke();
        onActionDone = null;
        actorMovement.ResetRotation();
        playableDirector.Play(action.TimelineAsset);
        playableDirector.time = 0;
        playingAction = action;
        DoUpdate((float)initialTime);
    }

    private void DoPlaySubAction(ActionTimelineAsset action, double initialTime = 0)
    {
        if (TryGetSubActionPlaying(action, out _))
            return;
        var director = GetFreeSubActionDirector();
        director.PlayAction(action, initialTime);
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
        foreach (var item in subActionDirectors)
        {
            item.DoUpdate(deltaTime);
        }
    }

    private void DoStopSubAction(ActionTimelineAsset action)
    {
        if (TryGetSubActionPlaying(action, out var playingDirector))
        {
            playingDirector.StopAction();
        }
    }

    private SubActionPlayableDirector GetFreeSubActionDirector()
    {
        foreach (var director in subActionDirectors)
        {
            if (director.PlayingAction == null)
                return director;
        }
        var go = new GameObject($"SubDirector{subActionDirectors.Count}", typeof(PlayableDirector));
        go.transform.SetParent(subActionRoot);
        var newDirector = go.AddComponent<SubActionPlayableDirector>();
        newDirector.playableDirector = go.GetComponent<PlayableDirector>();
        subActionDirectors.Add(newDirector);
        newDirector.playableDirector.SetGenericBinding(ActorBindingObj, actor);
        return newDirector;
    }

    private bool TryGetSubActionPlaying(ActionTimelineAsset action, out SubActionPlayableDirector playingDirector)
    {
        foreach (var director in subActionDirectors)
        {
            if (director.PlayingAction == action)
            {
                playingDirector = director;
                return true;
            }
        }
        playingDirector = null;
        return false;
    }
}
