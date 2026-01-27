
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BindingInfo
{
    public string bindingPath;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;

    public static BindingInfo Default => new()
    {
        bindingPath = string.Empty,
        positionOffset = Vector3.zero,
        rotationOffset = Quaternion.identity
    };

    public BindingResult GetBindingTransform(Transform root)
    {
        var a = Quaternion.Normalize(default);
        var bindingTrans = GetChildByPath(root, bindingPath);
        Vector3 worldPosition = bindingTrans.TransformPoint(positionOffset);
        Quaternion worldRotation = bindingTrans.rotation * rotationOffset.normalized;
        return new BindingResult
        {
            target = bindingTrans,
            worldPosition = worldPosition,
            worldRotation = worldRotation
        };
    }

    public static Transform GetChildByPath(Transform parent, string path)
    {
        if (string.IsNullOrEmpty(path))
            return parent;
        var segments = path.Split('/');
        Transform current = parent.transform;
        foreach (var segment in segments)
        {
            current = current.Find(segment);
            if (current == null)
            {
                return parent;
            }
        }
        return current;
    }

    public static string GetPathOfChild(Transform transform, Transform child)
    {
        if (transform == child)
        {
            return string.Empty;
        }
        var pathSegments = new List<string>();
        Transform current = child;
        while (current != transform)
        {
            pathSegments.Add(current.name);
            current = current.parent;
            if (current == null)
            {
                return string.Empty;
            }
        }
        pathSegments.Reverse();
        return string.Join("/", pathSegments);
    }

}

public struct BindingResult
{
    public Transform target;
    public Vector3 worldPosition;
    public Quaternion worldRotation;
}