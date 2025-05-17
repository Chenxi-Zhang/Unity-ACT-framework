
using System.IO;
using UnityEngine;

static class PathUtility
{

    public static bool TryGetAssetPath(string fullpath, out string assetPath)
    {
        assetPath = null;
        if (fullpath == null)
        {
            return false;
        }
        fullpath = fullpath.Replace("\\", "/");
        if (fullpath.StartsWith("Assets/"))
        {
            assetPath = fullpath;
            return true;
        }
        if (fullpath.StartsWith(Application.dataPath))
        {
            assetPath = ("Assets" + fullpath.Substring(Application.dataPath.Length)).Replace("\\", "/");
            return true;
        }
        return false;
    }

    public static bool TryGetAbsolutePath(string assetPath, out string fullpath)
    {
        fullpath = null;
        if (string.IsNullOrEmpty(assetPath))
            return false;
        assetPath = assetPath.Replace("\\", "/");
        if (assetPath.StartsWith(Application.dataPath))
        {
            fullpath = assetPath;
            return true;
        }
        if (!assetPath.StartsWith("Assets/"))
            return false;
        fullpath = System.IO.Path.Combine(Application.dataPath, assetPath.Substring("Assets/".Length)).Replace("\\", "/");
        return true;
    }

    // Add this method to your class
    public static string SanitizeFileNamePart(string name)
    {
        // Array of chars that aren't allowed in file names
        char[] invalidChars = Path.GetInvalidFileNameChars();
        // Replace invalid chars with underscores
        string sanitized = name;
        foreach (char c in invalidChars)
        {
            sanitized = sanitized.Replace(c, '_');
        }
        // Additional replacements for characters that may cause issues
        sanitized = sanitized.Replace(':', '_')
                            .Replace('/', '_')
                            .Replace('\\', '_')
                            .Replace('*', '_')
                            .Replace('?', '_')
                            .Replace('"', '_')
                            .Replace('<', '_')
                            .Replace('>', '_')
                            .Replace('|', '_');
        // Trim spaces at beginning and end
        sanitized = sanitized.Trim();
        // Ensure the name isn't empty after sanitization
        if (string.IsNullOrEmpty(sanitized))
        {
            sanitized = "Unnamed";
        }
        return sanitized;
    }

}