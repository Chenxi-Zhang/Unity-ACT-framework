#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

// 附加在模型上，Director会传递给timeline用于启用colliders
public partial class ActorAttackColliderManager : MonoBehaviour
{
    public partial class AttackColliderHolder
    {
        partial void AddRecorderInEditor(CapsuleCollider collider, AttackColliderConfig config)
        {
            // 编辑器状态，collider变化会同步给timelineAsset
            if (!Application.isPlaying)
            {
                if (!collider.gameObject.TryGetComponent<ColliderChangeRecorder>(out var syncer)) {
                    syncer = collider.gameObject.AddComponent<ColliderChangeRecorder>();
                }
                syncer.config = config;
            }
        }
    }

    void OnDrawGizmos()
    {
        var aac = Selection.activeObject as GameObject;
        // 选中模型时不绘制，由Editor绘制
        if (aac && aac.GetComponentInChildren<ActorAttackColliderManager>())
        {
            return;
        }
        foreach (var kvp in colliderHolders)
        {
            var colliderObj = kvp.Value;
            DrawCollidersGizmo(colliderObj);
        }
    }

    void DrawCollidersGizmo(AttackColliderHolder colliderObj)
    {
        var enabledColliders = colliderObj.enabledColliders;
        if (enabledColliders == null) return;

        foreach (var col in enabledColliders)
        {
            if (col == null || !col.enabled) continue;
            DrawCapsuleGizmo(col);
        }
    }

    void DrawCapsuleGizmo(CapsuleCollider capsule)
    {
        var transform = capsule.transform;
        Gizmos.color = Color.cyan;
        // Get capsule parameters
        Vector3 center = transform.position;
        float radius = capsule.radius * Mathf.Abs(transform.lossyScale.x);
        float height = Mathf.Max(0, capsule.height * Mathf.Abs(transform.lossyScale.y) - (2 * radius));

        // Determine direction of the capsule
        Vector3 up = transform.up;
        if (capsule.direction == 0) up = transform.right; // X-axis
        else if (capsule.direction == 2) up = transform.forward; // Z-axis

        // Draw spheres at the ends
        Vector3 topSphere = center + (up * height / 2);
        Vector3 bottomSphere = center - (up * height / 2);
        Gizmos.DrawWireSphere(topSphere, radius);
        Gizmos.DrawWireSphere(bottomSphere, radius);
        // Draw a cylinder between the spheres
        if (up != transform.up)
        {
            Gizmos.DrawLine(topSphere + transform.up * radius, bottomSphere + transform.up * radius);
            Gizmos.DrawLine(topSphere - transform.up * radius, bottomSphere - transform.up * radius);
        }
        if (up != transform.right)
        {
            Gizmos.DrawLine(topSphere + transform.right * radius, bottomSphere + transform.right * radius);
            Gizmos.DrawLine(topSphere - transform.right * radius, bottomSphere - transform.right * radius);
        }
        if (up != transform.forward)
        {
            Gizmos.DrawLine(topSphere + transform.forward * radius, bottomSphere + transform.forward * radius);
            Gizmos.DrawLine(topSphere - transform.forward * radius, bottomSphere - transform.forward * radius);
        }
    }

}

public class ColliderChangeRecorder : MonoBehaviour
{
    public AttackColliderConfig config;
}

#endif