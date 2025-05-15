
using UnityEngine;

public struct Clamper
{
    public float minX;
    public float maxX;

    public float minXValue;
    public float maxXValue;

    public Clamper(float minX, float maxX, float minXValue, float maxXValue)
    {
        this.minX = minX;
        this.minXValue = minXValue;
        this.maxX = maxX;
        this.maxXValue = maxXValue;
    }

    public float Clamp(float x)
    {
        if (x >= maxX)
        {
            return maxXValue;
        }
        else if (x <= minX)
        {
            return minXValue;
        }
        else
        {
            float ratio = (x - minX) / (maxX - minX);
            return Mathf.Lerp(minXValue, maxXValue, ratio);
        }
    }
}