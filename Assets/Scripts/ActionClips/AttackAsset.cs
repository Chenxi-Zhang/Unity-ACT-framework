
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class AttackColliderConfig
{
    // 预制件的路径
    public string path;
    public Vector3 position;
    public Quaternion rotation;
    public float height = 1;
    public float radius = 0.3f;
    public bool isDisabled;
}

public class AttackAsset : PlayableAsset
{
    public List<AttackColliderConfig> configs = new();
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AttackClip>.Create(graph);
        var behaviour = playable.GetBehaviour();
        behaviour.configs = configs;
        return playable;
    }
}

class AttackClip : ActionClipBase
{
    public List<AttackColliderConfig> configs;

#if UNITY_EDITOR
    public override void OnPlayableDestroy(Playable playable)
    {
        if (actor == null)
            return;
        if (!Application.isPlaying)
        {
            ActorAttackColliderManager actorAttackCollider = actor.attackColliderManager;
            if (actorAttackCollider)
            {
                actorAttackCollider.DisposeColliders();
            }
        }
    }
#endif

    public override void OnActionPlay()
    {
        var attackColliderManager = actor.attackColliderManager;
        attackColliderManager.EnableColliders(configs);
    }

    public override void OnActionPause()
    {
        actor.attackColliderManager.DisableAllColliders();
    }
}
