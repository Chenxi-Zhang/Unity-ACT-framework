

using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public ActorAttacker attacker;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ActorBeHit>(out var hitbox)) {
            attacker.TryHit(hitbox);
        }
    }
}