
using UnityEngine;

//初始化1个值和改变速度，每次更新时传入目标和deltaTime，返回缓动后的值
public struct FloatEaseTool
{
    private float value;
    private float speed;

    public FloatEaseTool(float value, float speed)
    {
        this.value = value;
        this.speed = speed;
    }

    public void SetValue(float value)
    {
        this.value = value;
    }

    public float GetEaseValue(float target, float deltaTime)
    {
        value = Mathf.MoveTowards(value, target, speed * deltaTime);
        return value;
    }

}