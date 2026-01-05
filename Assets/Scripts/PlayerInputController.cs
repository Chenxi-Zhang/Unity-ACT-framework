using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public CameraManager cameraManager;

    private Actor controllingActor;
    public Actor ControllingActor => controllingActor;

    private PlayerInput playerInput;
    private InputSystem_Actions actions;

    Vector2 rawMove;
    float distance;
    bool attackPressed = false;
    bool defencePressed = false;

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

    public void SetControllingActor(Actor actor)
    {
        controllingActor = actor;
        cameraManager.SetFollow(actor);
    }

    void Update()
    {
        if (controllingActor == null)
        {
            return;
        }
        controllingActor.logicInput.InputMove(rawMove, distance);
        var interactChecker = controllingActor.interactChecker;
        var mainObj = UpdateInteractObjects(interactChecker.interactObjs);
        interactChecker.SetFocusedObject(mainObj);

        controllingActor.logicInput.InputButton(attackPressed ? InputType.AttackHold : InputType.AttackRelease);
        controllingActor.logicInput.InputButton(defencePressed ? InputType.DefenceHold : InputType.DefenceRelease);
    }

    List<InteractObject> validObjects = new();
    InteractObject UpdateInteractObjects(List<InteractObject> interactObjects)
    {
        var cam = Camera.main;
        validObjects.Clear();
        if (cam == null || interactObjects == null || interactObjects.Count == 0)
            return null;

        var camTransform = cam.transform;
        var camForward = camTransform.forward;
        var camPos = camTransform.position;

        foreach (var obj in interactObjects)
        {
            if (obj == null)
                continue;
            var screenPos = obj.indicator.screenPos;
            // 剔除屏幕后对象
            if (screenPos.z < 0)
            {
                obj.Hide();
                continue;
            }
            validObjects.Add(obj);
        }

        float minDist = float.MaxValue;
        InteractObject mainObj = null;
        foreach (var obj in validObjects)
        {
            var objPos = obj.transform.position;
            // 计算点到射线距离
            var toObj = objPos - camPos;
            var proj = Vector3.Project(toObj, camForward);
            var closestPoint = camPos + proj;
            float dist = Vector3.Distance(objPos, closestPoint);
            if (obj.IsInteractable(controllingActor) && dist < minDist)
            {
                minDist = dist;
                mainObj = obj;
            }
        }
        foreach (var obj in validObjects)
        {
            if (obj == mainObj)
                obj.SetDisplayState(DisplayState.Focus);
            else
                obj.SetDisplayState(DisplayState.Show);
        }
        return mainObj;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        rawMove = context.ReadValue<Vector2>();
        distance = rawMove.magnitude;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            attackPressed = true;
            controllingActor.logicInput.InputButton(InputType.Attack);
        }
        else if (context.canceled)
        {
            attackPressed = false;
            controllingActor.logicInput.InputButton(InputType.AttackRelease);
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            controllingActor.logicInput.InputButton(InputType.Interact);
        }
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

    public void OnLock(InputAction.CallbackContext context)
    {
        if (controllingActor == null)
            return;
        if (context.started)
        {
            DoClickLock();
        }
    }

    private void DoClickLock()
    {
        if (controllingActor.cameraStatus.IsLocking)
        {
            controllingActor.cameraStatus.UnlockTarget();
            return;
        }
        if (controllingActor.cameraStatus.TrySearchAndLock())
        {
            cameraManager.LockTo(controllingActor.cameraStatus.LockingTarget);
        }
        else
        {
            cameraManager.Recenter();
        }
    }

    public void OnDefence(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            defencePressed = true;
            controllingActor.logicInput.InputButton(InputType.Defence);
        }
        else if (context.canceled)
        {
            defencePressed = false;
            controllingActor.logicInput.InputButton(InputType.DefenceRelease);
        }
    }

}
