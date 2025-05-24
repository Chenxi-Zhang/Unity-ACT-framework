using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Actor : MonoBehaviour
{
    public ActorLogicInput logicInput;
    public ActorMovement movement;
    public ActionPlayableDirector actionPlayableDirector;
    public AnimationSimpleBlender animationSimpleBlender;
    public ActorAttackColliderManager attackColliderManager;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
