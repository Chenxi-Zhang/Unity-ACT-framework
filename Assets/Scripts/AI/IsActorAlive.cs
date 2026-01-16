#if NODECANVAS
using ParadoxNotion.Design;

[Name("Is Actor Alive")]
[Category("ActorAI")]
[Description("AI是否还或者")]
public class IsActorAlive : BaseActorConditionTask
{

    protected override bool OnCheck()
    {
        return actor.data.IsAlive;
    }
}
#endif