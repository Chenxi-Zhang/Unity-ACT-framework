
using UnityEngine;

public class ActorBeHit : MonoBehaviour, IHittable
{
    public Actor actor;

    public void BeHit(ActorAttacker attacker)
    {
        if (attacker.actor == actor)
            // don't hit self
            return;
        Debug.Log($"{attacker.gameObject.name} Hits {actor.gameObject.name}");
    }

}
