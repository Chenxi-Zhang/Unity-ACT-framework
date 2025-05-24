
using UnityEngine;

public static class GameObjectTool
{
    public static GameObject CreateChild(this GameObject parent, string name = "NewChild") {
        var child = new GameObject(name);
        child.transform.SetParent(parent.transform, false);
        return child;
    }
}