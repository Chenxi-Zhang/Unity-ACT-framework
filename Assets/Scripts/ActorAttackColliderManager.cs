
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class ActorAttackColliderManager : MonoBehaviour
{
    public Actor actor;
    public ActorAttacker attacker;

    [NonSerialized]
    public readonly Dictionary<string, AttackColliderHolder> colliderHolders = new();

    public partial class AttackColliderHolder
    {
        public List<CapsuleCollider> enabledColliders;

        GameObject holder;
        List<CapsuleCollider> reuseColliders;
        ActorAttacker attacker;

        public AttackColliderHolder(GameObject parent, ActorAttacker attacker)
        {
            this.attacker = attacker;
            holder = parent.CreateChild("ColliderObject");
            holder.hideFlags = HideFlags.DontSave;
            enabledColliders = new();
            reuseColliders = new();
        }

        CapsuleCollider CreateCapsuleCollider()
        {
            var go = holder.CreateChild($"Collider{enabledColliders.Count + reuseColliders.Count}");
            go.layer = attacker.attackLayer;

            var collider = go.AddComponent<CapsuleCollider>();
            collider.direction = 0;
            collider.isTrigger = true;
            var attackCollider = go.AddComponent<AttackTrigger>();
            attackCollider.attacker = attacker;
            return collider;
        }

        public void EnableCollider(AttackColliderConfig config)
        {
            CapsuleCollider collider;
            if (reuseColliders.Count > 0)
            {
                collider = reuseColliders[reuseColliders.Count - 1];
                reuseColliders.RemoveAt(reuseColliders.Count - 1);
                collider.gameObject.SetActive(true);
            }
            else
            {
                collider = CreateCapsuleCollider();
            }
            AddRecorderInEditor(collider, config);
            collider.transform.SetLocalPositionAndRotation(config.position, config.rotation);
            collider.height = config.height;
            collider.radius = config.radius;
            collider.enabled = !config.isDisabled;
            enabledColliders.Add(collider);
        }

        public void DisableAllCollider()
        {
            for (int i = 0; i < enabledColliders.Count; i++)
            {
                var collider = enabledColliders[i];
                collider.gameObject.SetActive(false);
                reuseColliders.Add(collider);
            }
            enabledColliders.Clear();
        }

        partial void AddRecorderInEditor(CapsuleCollider collider, AttackColliderConfig config);

#if UNITY_EDITOR
        public void Dispose()
        {
            DestroyImmediate(holder);
        }
#endif

    }

    AttackColliderHolder CreateColliderHolder(string path)
    {
        var childTrans = transform.Find(path);
        if (!childTrans)
        {
            return null;
        }
        var go = childTrans.gameObject;
        var colliderObj = new AttackColliderHolder(go, attacker);
        return colliderObj;
    }

    void EnableCollider(AttackColliderConfig config)
    {
        var path = config.path;
        if (!colliderHolders.TryGetValue(path, out var colliderHolder))
        {
            colliderHolder = CreateColliderHolder(path);
            if (colliderHolder == null)
            {
                Debug.Log($"GameObject is null at path: {path}");
                return;
            }
            colliderHolders.Add(path, colliderHolder);
        }
        colliderHolder.EnableCollider(config);
    }

    public void EnableColliders(List<AttackColliderConfig> configs, AttackData attackData, WeaponState weaponState)
    {
        if (configs == null || configs.Count == 0)
            return;
        attacker.attackData = attackData;
        attacker.weaponState = weaponState;
        for (int i = 0; i < configs.Count; i++)
        {
            EnableCollider(configs[i]);
        }
    }

    public void DisableAllColliders()
    {
        foreach (var kvp in colliderHolders)
        {
            var colliderObj = kvp.Value;
            colliderObj.DisableAllCollider();
        }
        attacker.ClearHitboxes();
    }

#if UNITY_EDITOR
    public void DisposeColliders()
    {
        foreach (var kvp in colliderHolders)
        {
            var colliderObj = kvp.Value;
            colliderObj.Dispose();
        }
        colliderHolders.Clear();
        attacker.ClearHitboxes();
    }
#endif

}