
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    public GameObject target;
    public BasePlayableDirector targetDirector;

    public ActionTimelineAsset targetAction;
    public ActionTimelineAsset actorAction;

    public Vector3 position;
    public float speed = 0;

    protected Vector3 screenPos;

    public void ApplyInteract(Actor actor)
    {
        var pos = target.transform.TransformPoint(position);
        actor.movement.ForceSetPosition(pos, speed);
        actor.movement.ForceSetTurn((target.transform.position - pos).normalized);
        targetDirector.PlayAction(targetAction);
        actor.actionPlayableDirector.PlayAction(actorAction);
    }

    void OnDrawGizmosSelected()
    {
        if (target == null)
            return;

        // 将本地坐标转换为世界坐标
        Vector3 worldPosition = target.transform.TransformPoint(position);

        // 设置绘制颜色
        Gizmos.color = Color.green;

        // 绘制一个小球体表示位置
        Gizmos.DrawSphere(worldPosition, 0.1f);

        // 绘制一条连接线，从target到位置点
        Gizmos.DrawLine(target.transform.position, worldPosition);

        // // 绘制方向指示（用于表示-position.normalized的朝向）
        // Gizmos.color = Color.blue;
        Vector3 direction = -position.normalized;
        Vector3 worldDirection = target.transform.TransformDirection(direction);
        Gizmos.DrawRay(worldPosition, worldDirection * 0.5f);
    }

}
