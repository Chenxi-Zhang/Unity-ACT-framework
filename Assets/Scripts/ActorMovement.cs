using UnityEngine;

public class ActorMovement : MonoBehaviour
{
    public Actor actor;

    Quaternion rotation = Quaternion.identity;

    // 由输入调用的旋转
    public void UpdateTurn(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            // Smoothly rotate towards the target direction
            rotation = Quaternion.RotateTowards(
                rotation,
                targetRotation,
                GetTurnSpeed() * Time.deltaTime
            );
        }
    }

    float GetTurnSpeed()
    {
        return 360f * 2; //每秒旋转角度
    }

    void Update() {
        actor.transform.rotation = rotation;
    }
}
