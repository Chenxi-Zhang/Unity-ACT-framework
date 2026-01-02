#if NODECANVAS
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

[Name("Move to target")]
[Category("ActorAI")]
[Description("移动到锁定目标指定距离内")]
public class MoveToTarget : BaseActorActionTask
{
    [RequiredField]
    public BBParameter<float> distance;

    private float GetDistanceToLockingTarget()
    {
        var targetPosition = actor.cameraStatus.LockingTarget.transform.position;
        return Vector3.Distance(actor.transform.position, targetPosition);
    }

    protected override void OnStop()
    {
        actor.actorAI.StopMoving();
    }

    protected override void OnUpdate()
    {
        if (!actor.cameraStatus.IsLocking)
        {
            EndAction(false);
            return;
        }
        float currentDistance = GetDistanceToLockingTarget();
        if (currentDistance < distance.value)
        {
            EndAction(true);
            return;
        }
        actor.actorAI.MovingTo(actor.cameraStatus.LockingTarget.transform.position);
    }
}
#endif