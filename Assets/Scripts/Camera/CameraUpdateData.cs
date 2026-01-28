
using System;
using UnityEngine;

[Serializable]
public struct CameraUpdateData
{
    public float blendTime;
    public bool changeCameraDistance;
    public float cameraDistance;

    private CameraBlendState blendState;

    public void BlendTo(CameraUpdateData toData)
    {
        blendState = new CameraBlendState
        {
            from = this,
            to = toData,
            duration = toData.blendTime,
            elapsed = 0f
        };
    }

    public void Update(float deltaTime)
    {
        if (blendState == null)
            return;
        if (blendState.IsDone())
            return;
        blendState.Update(deltaTime);
        var current = blendState.GetCurrentData();
        cameraDistance = current.cameraDistance;
    }

    class CameraBlendState
    {
        public CameraUpdateData from;
        public CameraUpdateData to;
        public float duration;
        public float elapsed;

        public void Update(float deltaTime)
        {
            elapsed += deltaTime;
        }

        public bool IsDone()
        {
            return elapsed >= duration;
        }

        public CameraUpdateData GetCurrentData()
        {
            float t = Mathf.Clamp01(elapsed / duration);
            CameraUpdateData result = new();
            // 插值相机距离
            result.cameraDistance = to.changeCameraDistance ? 
                Mathf.Lerp(from.cameraDistance, to.cameraDistance, t) :
                from.cameraDistance;
            return result;
        }
    }
}