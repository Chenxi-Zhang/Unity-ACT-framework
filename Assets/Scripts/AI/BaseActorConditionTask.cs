#if NODECANVAS
using NodeCanvas.Framework;

public abstract class BaseActorConditionTask : ConditionTask<ActorAI>
{
    public Actor actor => agent.actor;
}
#endif