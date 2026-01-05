using System.IO;
using UnityEditor;
using UnityEngine;

public static class CreateScriptableObject
{

    [MenuItem("Assets/Tools/ScriptableObject/Create", true)]
    public static bool CheckSelectionAsScriptableObject() {
        return EditorHelper.IsSelectionObjectSubclassOf<ScriptableObject>();
    }

    [MenuItem("Assets/Tools/ScriptableObject/Create")]
    public static void CreateSelectionAsScriptableObject() {
        var select = Selection.activeObject as MonoScript;
        var type = select.GetClass();
        var asset = ScriptableObject.CreateInstance(type);

        var path = AssetDatabase.GetAssetPath(select);
        string dirpath = Path.GetDirectoryName(path);
        string fileName = Path.GetFileNameWithoutExtension(path);
        string assetPath = Path.Combine(dirpath, fileName + ".asset");
        AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath(assetPath));
    }

}