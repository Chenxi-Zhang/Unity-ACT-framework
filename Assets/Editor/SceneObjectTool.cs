
using UnityEditor.SceneManagement;
using UnityEngine;

static class SceneObjectTool
{

    public static T GetComponentInSceneOrPrefabStage<T>() where T : Component
    {
        var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
        if (prefabStage != null)
        {
            foreach (var go in prefabStage.scene.GetRootGameObjects())
            {
                var director = go.GetComponentInChildren<T>(true);
                if (director != null)
                    return director;
            }
        }
        else
        {
            return Object.FindAnyObjectByType<T>();
        }
        return null;
    }

}