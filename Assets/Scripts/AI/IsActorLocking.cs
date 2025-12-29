#if NODECANVAS
using ParadoxNotion.Design;

[Name("Is Actor Locking")]
[Category("ActorAI")]
[Description("AI是否锁定敌人")]
public class IsActorLocking : BaseActorConditionTask
{

    protected override bool OnCheck()
    {
        return actor.cameraStatus.IsLocking;
    }
}
#endif