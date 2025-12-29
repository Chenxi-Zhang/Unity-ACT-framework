#if NODECANVAS
using System;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

[Name("Search Enemy")]
[Category("ActorAI")]
[Description("搜索范围内敌人")]
public class SearchEnemy : BaseActorActionTask
{
    [RequiredField]
    public BBParameter<float> searchRadius = 5f;
    [RequiredField]
    public BBParameter<float> fieldOfViewAngle = 90f;
    [RequiredField]
    public BBParameter<LayerMask> layerMask;

    protected override void OnUpdate()
    {
        if (actor.cameraStatus.IsLocking)
        {
            EndAction(true);
            return;
        }
        var enemies = SearchEnemies(searchRadius.value, fieldOfViewAngle.value);
        if (enemies != null)
        {
            actor.cameraStatus.SetTarget(enemies);
            EndAction(true);
            return;
        }
    }

    private Actor SearchEnemies(float searchRadius, float fieldOfViewAngle)
    {
        actor.cameraStatus.CollectTargetInView(
            actor.transform.position,
            actor.transform.forward,
            searchRadius,
            fieldOfViewAngle,
            layerMask.value
        );
        return actor.cameraStatus.FindClosestTarget();
    }

}
#endif