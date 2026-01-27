
using System;
using UnityEngine;
using UnityEngine.Playables;

public class SfxClip : ActionClipBase
{
    public GameObject sfxPrefab;
    public float destroyDelay;
    [Tooltip("If true, the SFX will be attached to the binding object. If false, it keeps world position.")]
    public bool isAttaching;
    public BindingInfo bindingInfo;

    private GameObject sfxObj;

    public override void OnActionPlay()
    {
        var bindingResult = bindingInfo.GetBindingTransform(actor.transform);
        var bindingTrans = bindingResult.target;
        var spawnPosition = bindingResult.worldPosition;
        var spawnRotation = bindingResult.worldRotation;
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            sfxObj = GameObject.Instantiate(sfxPrefab, spawnPosition, spawnRotation, bindingTrans);
            return;
        }
#endif
        if (isAttaching)
        {
            sfxObj = GameObject.Instantiate(sfxPrefab, spawnPosition, spawnRotation, bindingTrans);
        }
        else
        {
            sfxObj = GameObject.Instantiate(sfxPrefab, spawnPosition, spawnRotation);
        }
    }

    public override void OnActionPause()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            if (sfxObj != null)
            {
                GameObject.DestroyImmediate(sfxObj);
                sfxObj = null;
            }
            return;
        }
#endif
        if (sfxObj != null)
        {
            if (sfxObj.TryGetComponent<BaseSfxContainer>(out var sfxContainer))
            {
                sfxContainer.Destroy(destroyDelay);
            }
            else
            {
                GameObject.Destroy(sfxObj, destroyDelay);
            }
            sfxObj = null;
        }
    }

#if UNITY_EDITOR
    public override void OnPlayableDestroy(Playable playable)
    {
        if (!Application.isPlaying)
        {
            if (sfxObj != null)
            {
                GameObject.DestroyImmediate(sfxObj);
                sfxObj = null;
            }
            return;
        }
    }
#endif

}

public class SfxAsset : PlayableAsset
{
    public GameObject sfxPrefab;
    public float destroyDelay = 5f;
    public bool isAttaching;
    public BindingInfo bindingInfo = BindingInfo.Default;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        if (sfxPrefab == null)
        {
            return Playable.Create(graph);
        }
        var playable = ScriptPlayable<SfxClip>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.sfxPrefab = sfxPrefab;
        behaviour.destroyDelay = destroyDelay;
        behaviour.isAttaching = isAttaching;
        behaviour.bindingInfo = bindingInfo;
        return playable;
    }
}