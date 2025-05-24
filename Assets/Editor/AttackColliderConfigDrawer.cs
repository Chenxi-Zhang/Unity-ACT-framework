using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AttackColliderConfig))]
class AttackColliderConfigDrawer : PropertyDrawer
{
    // Calculate property height based on expansion state
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return EditorGUIUtility.singleLineHeight;

        // Basic height for foldout
        float height = EditorGUIUtility.singleLineHeight;

        // Add height for binding fields
        height += EditorGUIUtility.singleLineHeight * 3; // Root, Target, Path fields

        // Count other properties
        SerializedProperty iterator = property.Copy();
        int initialDepth = iterator.depth;
        bool enterChildren = true;

        while (iterator.NextVisible(enterChildren))
        {
            enterChildren = false;

            // 添加深度检查，确保不会计算下一个元素的高度
            if (iterator.depth <= initialDepth)
                break;

            if (iterator.name == "path") continue; // Skip path field
            height += EditorGUI.GetPropertyHeight(iterator, true) + EditorGUIUtility.standardVerticalSpacing;
        }

        // 添加最后一个元素的额外间距
        height += EditorGUIUtility.standardVerticalSpacing * 2;

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var atkColl = SceneObjectTool.GetComponentInSceneOrPrefabStage<ActorAttackColliderManager>();
        var title = "No binding";
        var pathProp = property.FindPropertyRelative("path");
        GameObject root = null;
        GameObject currentObj = null;
        if (atkColl)
        {
            root = atkColl.gameObject;
            if (!string.IsNullOrEmpty(pathProp.stringValue))
            {
                var t = root.transform;
                var segments = pathProp.stringValue.Split('/');
                foreach (var seg in segments)
                {
                    t = t.Find(seg);
                    if (t == null) break;
                }
                if (t != null) currentObj = t.gameObject;
            }
            title = $"Binding: {atkColl.name}";
            if (currentObj != null)
            {
                title += $" => {currentObj.name}";
            }
            else
            {
                title += " (No binding)";
            }
        }
        label.text = title;

        // Foldout header
        Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

        if (!property.isExpanded)
        {
            EditorGUI.EndProperty();
            return;
        }

        // Calculate rects
        float y = position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        float fieldHeight = EditorGUIUtility.singleLineHeight;
        Rect rect = new Rect(position.x, y, position.width, fieldHeight);

        EditorGUI.indentLevel++;

        if (atkColl == null)
        {
            // Warning message
            rect.height = fieldHeight * 2;
            EditorGUI.HelpBox(rect, "Open the Actor prefab!", MessageType.Warning);
            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
            return;
        }

        EditorGUI.BeginDisabledGroup(true);
        // Draw binding root field
        EditorGUI.ObjectField(rect, "Binding Root", root, typeof(GameObject), true);
        rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;
        // Draw path field
        EditorGUI.PropertyField(rect, pathProp);
        rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;
        EditorGUI.EndDisabledGroup();

        // Draw binding target field
        GameObject newObj = (GameObject)EditorGUI.ObjectField(rect, "Binding to", currentObj, typeof(GameObject), true);
        if (newObj != currentObj && newObj != null && root != null && newObj.transform.IsChildOf(root.transform))
        {
            pathProp.stringValue = GetRelativePath(root.transform, newObj.transform);
        }
        rect.y += fieldHeight + EditorGUIUtility.standardVerticalSpacing;

        // Draw other fields
        SerializedProperty iterator = property.Copy();
        int initialDepth = iterator.depth;
        bool enterChildren = true;

        while (iterator.NextVisible(enterChildren))
        {
            enterChildren = false;

            // Only process properties that are children of the current property
            // Stop when we hit the next element at the same depth
            if (iterator.depth <= initialDepth)
                break;

            // Skip path property since we're handling it separately
            if (iterator.name == "path")
                continue;

            float height = EditorGUI.GetPropertyHeight(iterator, true);
            rect.height = height;
            EditorGUI.PropertyField(rect, iterator, true);
            rect.y += height + EditorGUIUtility.standardVerticalSpacing;
        }

        EditorGUI.indentLevel--;
        EditorGUI.EndProperty();
    }

    // Helper to get hierarchy path
    private string GetRelativePath(Transform root, Transform target)
    {
        if (target == root) return "";
        var stack = new System.Collections.Generic.Stack<string>();
        var t = target;
        while (t != null && t != root)
        {
            stack.Push(t.name);
            t = t.parent;
        }
        return string.Join("/", stack);
    }
}