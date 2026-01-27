
using UnityEditor;
using UnityEngine;

public struct BindingInfoEditorHelper
{
    Transform root;
    SerializedProperty bindingInfoProp;
    SerializedProperty bindingPathProp;
    SerializedProperty positionOffsetProp;
    SerializedProperty rotationOffsetProp;

    public BindingInfoEditorHelper(Transform root, SerializedProperty bindingInfoProp)
    {
        this.root = root;
        this.bindingInfoProp = bindingInfoProp;
        bindingPathProp = bindingInfoProp.FindPropertyRelative("bindingPath");
        positionOffsetProp = bindingInfoProp.FindPropertyRelative("positionOffset");
        rotationOffsetProp = bindingInfoProp.FindPropertyRelative("rotationOffset");
    }

    public void OnDuringSceneGUI(SceneView view, BindingInfo bindingInfo)
    {
        var bindingResult = bindingInfo.GetBindingTransform(root);
        var bindingObj = bindingResult.target;
        Handles.color = Color.cyan;

        // 计算世界坐标（考虑父对象的旋转）
        Vector3 worldPosition = bindingResult.worldPosition;
        Quaternion worldRotation = bindingResult.worldRotation;

        if (Tools.current == Tool.Move)
        {
            EditorGUI.BeginChangeCheck();
            var rot = Quaternion.identity;
            if (Tools.pivotRotation == PivotRotation.Local)
            {
                rot = worldRotation;
            }
            var newPosition = Handles.PositionHandle(worldPosition, rot);

            if (EditorGUI.EndChangeCheck())
            {
                // 将世界坐标转换回 local 坐标
                positionOffsetProp.vector3Value = bindingObj.InverseTransformPoint(newPosition);
            }
        }
        else if (Tools.current == Tool.Rotate)
        {
            EditorGUI.BeginChangeCheck();
            var newRotation = Handles.RotationHandle(worldRotation, worldPosition);

            if (EditorGUI.EndChangeCheck())
            {
                // 将世界旋转转换回 local 旋转
                rotationOffsetProp.quaternionValue = Quaternion.Inverse(bindingObj.rotation) * newRotation;
            }
        }
    }

    public void DrawInspectorGUI()
    {
        using (new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.ObjectField("Binding Root", root, typeof(Transform), true);
        }
        var bindingTrans = BindingInfo.GetChildByPath(root, bindingPathProp.stringValue);
        using (new EditorGUI.IndentLevelScope())
        {
            EditorGUI.BeginChangeCheck();
            var newBindingTrans = EditorGUILayout.ObjectField(
                "Binding Object",
                bindingTrans != null ? bindingTrans.gameObject : null,
                typeof(Transform),
                true
            ) as Transform;
            if (EditorGUI.EndChangeCheck())
            {
                if (newBindingTrans == null)
                {
                    bindingPathProp.stringValue = string.Empty;
                }
                else
                {
                    bindingPathProp.stringValue = BindingInfo.GetPathOfChild(root, newBindingTrans);
                }
            }

            EditorGUILayout.PropertyField(positionOffsetProp);
            EditorGUILayout.PropertyField(rotationOffsetProp);
        }
    }

}