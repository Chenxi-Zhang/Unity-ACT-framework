
using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorCameraStatus : MonoBehaviour
{
    public Actor actor;
    public Transform cameraTarget;
    public float maxLockDistance = 20f;
    // 可锁定目标的层
    public LayerMask targetableLayers = -1; // 默认所有层
    [Range(0, 180)]
    public float lockFieldOfView = 90f;

    public Actor LockingTarget { get; private set; }
    public bool IsLocking => LockingTarget != null;

    // 存储候选目标
    private List<Actor> targetCandidates = new();

    void CollectTargetCandidates()
    {
        // 确保有搜索原点
        Transform searchOrigin = Camera.main.transform;
        // 创建搜索方向 - 使用相机或角色的前方
        Vector3 searchDirection = searchOrigin.forward;
        // 查找范围内的所有碰撞体
        int cnt = Physics.OverlapSphereNonAlloc(
            searchOrigin.position,
            maxLockDistance,
            PhysicsHelper.colliders,
            targetableLayers
        );
        targetCandidates.Clear();
        // 筛选出符合条件的目标
        for (int i = 0; i < cnt; i++)
        {
            Collider collider = PhysicsHelper.colliders[i];
            GameObject potentialTarget = collider.gameObject;
            var actor = potentialTarget.GetComponent<Actor>();
            if (actor == null)
                continue;
            if (actor == LockingTarget)
                continue; // 忽略当前跟随目标
            if (actor == this.actor)
                continue; // 忽略自己
            // 计算目标方向
            Vector3 directionToTarget = (potentialTarget.transform.position - searchOrigin.position).normalized;
            // 检查目标是否在视野范围内
            float angle = Vector3.Angle(searchDirection, directionToTarget);
            if (angle > lockFieldOfView * 0.5f)
                continue;
            targetCandidates.Add(actor);
        }
    }

    // 选择最近的目标
    Actor FindClosestTarget()
    {
        Actor bestTarget = null;
        float bestSqrDist = float.MaxValue;
        foreach (var target in targetCandidates)
        {
            var screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
            // 如果目标在相机后面，跳过它（WorldToScreenPoint z值为负表示在相机后方）
            if (screenPos.z < 0)
                continue;
            var sqrDist = screenPos.x * screenPos.x + screenPos.y * screenPos.y;
            if (sqrDist < bestSqrDist)
            {
                bestTarget = target;
                bestSqrDist = sqrDist;
            }
        }
        return bestTarget;
    }

    Actor FindAnotherTarget(Vector2 direction)
    {
        if (LockingTarget == null || direction == Vector2.zero)
            return null;
        Actor bestTarget = null;
        float bestMatchValue = -1f;
        Vector2 lockingScreenPos = Camera.main.WorldToScreenPoint(LockingTarget.transform.position);
        direction.Normalize();
        foreach (var target in targetCandidates)
        {
            var screenPos = Camera.main.WorldToScreenPoint(target.transform.position);
            if (screenPos.z < 0)
                continue;
            Vector2 screenPos2 = new(screenPos.x, screenPos.y);
            Vector2 directionToTarget = screenPos2 - lockingScreenPos;
            if (directionToTarget.sqrMagnitude < 0.001f)
                continue; // 忽略太近的目标
            directionToTarget.Normalize();
            // 计算方向匹配度（点积越接近1，方向越匹配）
            float dot = Vector2.Dot(direction, directionToTarget);
            if (dot <= 0)
                continue; // 忽略反向目标
            float screenDistance = Vector2.Distance(lockingScreenPos, screenPos2);
            float distanceFactor = 1f - (screenDistance / (Screen.width / 2f));
            // 方向匹配度和距离因素的加权组合
            // 可以调整权重比例
            float matchValue = (dot * 0.3f) + (distanceFactor * 0.7f);
            // 更新最佳匹配
            if (matchValue > bestMatchValue)
            {
                bestTarget = target;
                bestMatchValue = matchValue;
            }
        }
        return bestTarget;
    }

    public bool TrySearchAndLock()
    {
        CollectTargetCandidates();
        var target = FindClosestTarget();
        if (target != null)
        {
            LockingTarget = target;
            Debug.Log($"Locked onto target: {LockingTarget.name}");
            return true;
        }
        return false;
    }

    public bool TryChangeLockTarget(Vector2 direction)
    {
        CollectTargetCandidates();
        var target = FindAnotherTarget(direction);
        if (target != null)
        {
            LockingTarget = target;
            Debug.Log($"Locked onto target: {LockingTarget.name}");
            return true;
        }
        return false;
    }

    public void UnlockTarget()
    {
        LockingTarget = null;
    }
}