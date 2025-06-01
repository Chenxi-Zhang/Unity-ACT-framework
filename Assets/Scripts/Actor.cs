using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Actor : MonoBehaviour
{
    public ActorLogicInput logicInput;
    public ActorMovement movement;
    public ActionPlayableDirector actionPlayableDirector;
    public AnimationSimpleBlender animationSimpleBlender;
    public ActorAttackColliderManager attackColliderManager;
    public ActorInteractChecker interactiveChecker;

    private CharacterController _characterController;
    public CharacterController characterController
    {
        get
        {
            if (_characterController == null)
            {
                _characterController = GetComponent<CharacterController>();
            }
            return _characterController;
        }
    }

}
