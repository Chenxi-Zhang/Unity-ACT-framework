using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorLogicInput : MonoBehaviour
{
    public Actor actor;
    private bool IsLocking => actor.actorCameraStatus.IsLocking;
    private Actor LockingTarget => actor.actorCameraStatus.LockingTarget;

    Dictionary<InputType, Action> inputActions = new();
    Dictionary<InputType, Action> inputThisFrame = new();

    public float maxBodyTwistAngle = 45f;
    public float turnBackAngle = 150f;
    public float stopTurnAngle = 5f;

    public BoolStatus disableTurningToLock = new();

    private Vector3 inputMoveDirection = Vector3.zero;

    public void RegisterInputAction(InputType inputType, Action action)
    {
        if (inputActions.ContainsKey(inputType))
        {
            throw new Exception($"Input action {inputType} already registered.");
        }
        inputActions[inputType] = action;
    }

    public void UnregisterInputAction(InputType inputType)
    {
        inputActions.Remove(inputType);
    }

    public void InputMove(Vector3 direction, float distance)
    {
        inputMoveDirection = direction;
        if (distance > 0.1f)
        {
            TryAddInput(InputType.Move);
        }
        else
        {
            TryAddInput(InputType.MoveCancel);
        }
    }

    public void InputButton(InputType inputType)
    {
        TryAddInput(inputType);
    }

    private void TryAddInput(InputType inputType)
    {
        if (inputActions.TryGetValue(inputType, out var action))
        {
            inputThisFrame[inputType] = action;
        }
    }

    void Update()
    {
        if (IsLocking)
        {
            TryTurnToLockingTarget();
        }
        else
        {
            actor.movement.UpdateTurn(inputMoveDirection);
        }
        inputMoveDirection = Vector3.zero;
    }

    private void TryTurnToLockingTarget()
    {
        var facingTo = LockingTarget.actorCameraStatus.cameraTarget.position - actor.transform.position;
        facingTo.y = 0;
        facingTo.Normalize();
        if (disableTurningToLock)
        {
            actor.movement.UpdateTurn(inputMoveDirection);
        }
        else
        {
            actor.movement.UpdateTurn(facingTo);
        }
        float angle = Vector3.SignedAngle(actor.transform.forward, facingTo, Vector3.up);
        if (angle > turnBackAngle || angle < -turnBackAngle)
        {
            InputButton(InputType.IdleTurnBack);
        }
        else if (angle > maxBodyTwistAngle)
        {
            InputButton(InputType.IdleTurnRight);
        }
        else if (angle < -maxBodyTwistAngle)
        {
            InputButton(InputType.IdleTurnLeft);
        }
        else if (Mathf.Abs(angle) < stopTurnAngle)
        {
            InputButton(InputType.IdleTurnStop);
        }
    }

    void LateUpdate()
    {
        if (inputThisFrame.Count == 0)
            return;
        InputType execType = InputType.None;
        Action execAction = null;
        foreach (var (key, value) in inputThisFrame)
        {
            if (execType == InputType.None || key > execType)
            {
                execType = key;
                execAction = value;
            }
        }
        inputThisFrame.Clear();
        execAction?.Invoke();
    }
}
