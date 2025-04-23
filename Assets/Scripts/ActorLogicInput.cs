using UnityEngine;

public class ActorLogicInput : MonoBehaviour
{
    public Actor actor;

    public void InputMove(Vector3 direction)
    {
        actor.movement.UpdateTurn(direction);
    }

}
