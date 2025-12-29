
using System;
using UnityEngine;

public class ActorAI : MonoBehaviour
{
    public Actor actor;

    public void MovingTo(Vector3 targetPosition)
    {
        var direction = targetPosition - actor.transform.position;
        direction.y = 0;
        direction.Normalize();
        actor.logicInput.DoInputMoveWorldDir(direction);
    }

    public void StopMoving()
    {
        actor.logicInput.DoInputMoveCancel();
    }
}