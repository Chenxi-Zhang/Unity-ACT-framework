
using UnityEditor;
using UnityEngine;

public class EditorHelper {

    public static string GetScriptableObjectDirectory(GameObject scriptableObject) {
        string path = AssetDatabase.GetAssetPath(scriptableObject);
        string folder = System.IO.Path.GetDirectoryName(path);
        return folder;
    }

    public static bool IsSelectionObjectScriptableObject<T>() where T : ScriptableObject {
        var select = Selection.activeObject;
        if (!select)
        {
            return false;
        }
        return select.GetType() == typeof(T);
    }

    public static bool IsSelectionObjectMonoScript() {
        var select = Selection.activeObject;
        if (!select)
        {
            return false;
        }
        Debug.Log($"select:{select.name}, select.GetType():{select.GetType().Name}");
        if (!(select.GetType() == typeof(MonoScript)))
        {
            return false;
        }
        return true;
    }

    public static bool IsSelectionObjectMonoScript<T>() {
        if (!IsSelectionObjectMonoScript())
        {
            return false;
        }
        var select = Selection.activeObject as MonoScript;
        var cls = select.GetClass();
        if (cls == null)
        {
            ///如果这个文件没有class或者由多个class，都返回空
            return false;
        }
        return cls == typeof(T);
    }

    public static bool IsSelectionObjectSubclassOf<T>() {
        if (!IsSelectionObjectMonoScript())
        {
            return false;
        }
        var select = Selection.activeObject as MonoScript;
        var cls = select.GetClass();
        if (cls == null)
        {
            ///如果这个文件没有class或者由多个class，都返回空
            return false;
        }
        return cls == typeof(T) || cls.IsSubclassOf(typeof(T));
    }

}