using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public CinemachineCamera vcam;

    private Actor controllingActor;

    private PlayerInput playerInput;
    private InputSystem_Actions actions;

    Vector2 rawMove;
    float distance;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        actions = new InputSystem_Actions();
        playerInput.actions = actions.asset;
        playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
        actions.Player.SetCallbacks(this);
    }

    void OnEnable()
    {
        actions.Enable();
    }
    void OnDisable()
    {
        actions.Disable();
    }
    void OnDestroy()
    {
        actions.Dispose();
    }

    void Start()
    {
        SetControllingActor(FindFirstObjectByType<Actor>());
    }

    public void SetControllingActor(Actor actor)
    {
        controllingActor = actor;
        vcam.Follow = actor.transform;
    }

    void Update()
    {
        if (controllingActor == null)
        {
            return;
        }
        controllingActor.logicInput.InputMove(ConvertFromCameraLocalToWorld(rawMove), distance);
    }

    private Vector3 ConvertFromCameraLocalToWorld(Vector2 move)
    {
        var cam = Camera.main;
        var movement = move.magnitude;
        if (cam != null && movement > 0.1f)
        {
            // Convert the input vector to world space using the camera's transform
            var direction = cam.transform.TransformDirection(new Vector3(rawMove.x, 0, rawMove.y));
            // Flatten the worldMove vector to ignore vertical movement
            direction.y = 0;
            direction.Normalize();
            return direction;
        }
        return Vector3.zero;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        rawMove = context.ReadValue<Vector2>();
        distance = rawMove.magnitude;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnNext(InputAction.CallbackContext context)
    {
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
    }
}
