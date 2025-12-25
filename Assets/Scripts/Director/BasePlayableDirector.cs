
using UnityEngine;
using UnityEngine.Playables;

public abstract class BasePlayableDirector : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public abstract void PlayAction(ActionTimelineAsset action, double initialTime = 0);
}