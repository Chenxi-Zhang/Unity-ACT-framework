#if NODECANVAS
using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

[Name("Move to Position")]
[Category("ActorAI")]
[Description("移动到目的地")]
public class MoveToPosition : BaseActorActionTask
{
    [RequiredField]
    public BBParameter<Vector3> position;

    protected override string info
    {
        get
        {
            return $"Move to {position}";
        }
    }

    protected override void OnStop()
    {
        actor.actorAI.StopMoving();
    }

    protected override void OnUpdate()
    {
        float currentDistance = GetDistanceToPosition();
        if (currentDistance < 0.1f)
        {
            EndAction(true);
            return;
        }
        actor.actorAI.MovingTo(position.value);
    }

    private float GetDistanceToPosition()
    {
        return Vector3.Distance(
            actor.transform.position,
            position.value
        );
    }
}
#endif