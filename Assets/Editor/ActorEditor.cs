using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Actor))]
public class ActorEditor : Editor
{

    void OnSceneGUI()
    {
        DrawAttackColliders();
    }

    private void DrawAttackColliders()
    {
        if (Application.isPlaying)
            return;
        var actor = (Actor)target;
        var actorCollider = actor.attackColliderManager;
        var colliderHolders = actorCollider.colliderHolders;
        foreach (var obj in colliderHolders.Values)
        {
            var enabledColliders = obj.enabledColliders;
            foreach (CapsuleCollider col in enabledColliders)
            {
                if (col == null || !col.enabled)
                    continue;
                EditorGUI.BeginChangeCheck();
                // Draw Unity's built-in collider handle
                CapsuleColliderEditorHandle(col);
                if (EditorGUI.EndChangeCheck())
                {
                    var changeRecorder = col.gameObject.GetComponent<ColliderChangeRecorder>();
                    if (changeRecorder)
                    {
                        // 记录Undo
                        Undo.RecordObject(changeRecorder, "Modify CapsuleCollider");
                        var config = changeRecorder.config;
                        config.height = col.height;
                        config.radius = col.radius;
                        config.position = col.transform.localPosition;
                        config.rotation = col.transform.localRotation;
                        EditorUtility.SetDirty(changeRecorder);
                    }
                }
            }
        }
    }

    // 利用UnityEditor.Handles模拟CapsuleCollider的编辑
    void CapsuleColliderEditorHandle(CapsuleCollider col)
    {
        Transform t = col.transform;
        Vector3 center = t.position;
        Quaternion rot = t.rotation;
        float radius = col.radius * Mathf.Abs(t.lossyScale.x);
        float height = Mathf.Max(col.height * Mathf.Abs(t.lossyScale.y), radius * 2);
        // 目前默认使用X方向
        if (col.direction != 0)
            col.direction = 0;
        if (Tools.current == Tool.Move)
        {
            // Draw a handle for the center
            center = Handles.PositionHandle(center, rot);
            if (!Mathf.Approximately(Vector3.Distance(t.position, center), 0f))
            {
                t.position = center;
            }
        }
        else if (Tools.current == Tool.Rotate)
        {
            rot = Handles.RotationHandle(rot, center);
            if (!Mathf.Approximately(Quaternion.Angle(t.rotation, rot), 0f))
            {
                t.rotation = rot;
            }
        }

        // Draw a handle for the height
        Vector3 up = t.right;
        Vector3 top = center + up * (height / 2 - radius);
        Vector3 bottom = center - up * (height / 2 - radius);

        float newHeight = Handles.ScaleSlider(height, top, up, rot, 1, 0.1f);
        if (!Mathf.Approximately(newHeight, height))
            col.height = newHeight / Mathf.Abs(t.lossyScale.y);

        // Draw a handle for the radius
        float newRadius = Handles.RadiusHandle(rot, top, radius);
        if (!Mathf.Approximately(newRadius, radius))
            col.radius = newRadius / Mathf.Abs(t.lossyScale.x);
    }
}