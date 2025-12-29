#if NODECANVAS
using NodeCanvas.Framework;

public abstract class BaseActorActionTask : ActionTask<ActorAI>
{
    public Actor actor => agent.actor;
}
#endif