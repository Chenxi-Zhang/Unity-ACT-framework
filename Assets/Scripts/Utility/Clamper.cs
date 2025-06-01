
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

public struct Vector3Clamper
{
    public float minX;
    public float maxX;

    public Vector3 minY;
    public Vector3 maxY;

    public Vector3Clamper(float minX, float maxX, Vector3 minY, Vector3 maxY)
    {
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;
    }

    public Vector3 Clamp(float x)
    {
        if (x >= maxX)
        {
            return maxY;
        }
        else if (x <= minX)
        {
            return minY;
        }
        else
        {
            float ratio = (x - minX) / (maxX - minX);
            return Vector3.Lerp(minY, maxY, ratio);
        }
    }

}