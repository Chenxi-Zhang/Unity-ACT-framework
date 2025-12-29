using System;
using System.Collections.Generic;
using UnityEngine;

public class ActorLogicInput : MonoBehaviour
{
    public Actor actor;
    public StrafeMoveAnimation strafeMoveAnimation;
    private bool IsLocking => actor.cameraStatus.IsLocking;
    private Actor LockingTarget => actor.cameraStatus.LockingTarget;

    Dictionary<InputType, Action> inputActions = new();
    Dictionary<InputType, Action> inputThisFrame = new();

    public float maxBodyTwistAngle = 45f;
    public float turnBackAngle = 150f;
    public float stopTurnAngle = 5f;

    public BoolStatus disableTurningToLock = new();
    public BoolStatus canStrafe = new();
    private bool IsStrafe => canStrafe && IsLocking;

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

    public void InputMove(Vector2 rawInput, float distance)
    {
        if (IsStrafe)
        {
            DoStrafeMove(rawInput, distance);
        }
        else
        {
            DoMove(rawInput, distance);
        }
    }

    private float startMoveThreshold = 0.1f;

    private void DoStrafeMove(Vector2 rawInput, float distance)
    {
        if (distance > startMoveThreshold)
        {
            var action = strafeMoveAnimation.GetDirAnim(rawInput, out var actorDir);
            var faceWorldDir = ConvertLocalToWorldDir(new Vector3(actorDir.x, 0, actorDir.y));
            inputMoveDirection = faceWorldDir;
            if (action != actor.actionPlayableDirector.PlayingAction)
            {
                actor.actionPlayableDirector.PlayAction(action);
            }
        }
        else
        {
            inputMoveDirection = Vector3.zero;
            TryAddInput(InputType.MoveCancel);
        }
    }

    private void DoMove(Vector2 rawInput, float distance)
    {
        if (distance > startMoveThreshold)
        {
            inputMoveDirection = ConvertFromCameraLocalToWorld(rawInput);
            TryAddInput(InputType.Move);
        }
        else
        {
            inputMoveDirection = Vector3.zero;
            TryAddInput(InputType.MoveCancel);
        }
    }

    private Vector3 ConvertFromCameraLocalToWorld(Vector2 move)
    {
        var cam = Camera.main;
        var movement = move.magnitude;
        if (cam != null && movement > 0.1f)
        {
            // Convert the input vector to world space using the camera's transform
            var direction = cam.transform.TransformDirection(new Vector3(move.x, 0, move.y));
            // Flatten the worldMove vector to ignore vertical movement
            direction.y = 0;
            direction.Normalize();
            return direction;
        }
        return Vector3.zero;
    }

    public Vector3 ConvertLocalToWorldDir(Vector3 localDir)
    {
        Vector3 forward;
        if (IsLocking)
        {
            // 锁定的情况下使用锁定目标方向作为前进方向
            forward = LockingTarget.cameraStatus.cameraTarget.position - actor.transform.position;
            forward.y = 0;
            forward.Normalize();
        }
        else
        {
            forward = actor.transform.forward;
        }
        // 计算右方向
        var right = Vector3.Cross(Vector3.up, forward);
        // 将本地方向转换为世界方向
        return forward * localDir.z + right * localDir.x;
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

    public void DoUpdate(float deltaTime)
    {
        if (IsLocking)
        {
            TryTurnToLockingTarget(deltaTime);
        }
        else
        {
            actor.movement.UpdateTurn(inputMoveDirection, deltaTime);
        }
        inputMoveDirection = Vector3.zero;
    }

    private void TryTurnToLockingTarget(float deltaTime)
    {
        var facingTo = LockingTarget.cameraStatus.cameraTarget.position - actor.transform.position;
        facingTo.y = 0;
        facingTo.Normalize();
        if (IsStrafe || disableTurningToLock)
        {
            actor.movement.UpdateTurn(inputMoveDirection, deltaTime);
        }
        else
        {
            actor.movement.UpdateTurn(facingTo, deltaTime);
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
