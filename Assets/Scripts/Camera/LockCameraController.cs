
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class LockCameraController : BaseCameraController, IInputAxisOwner
{

    private InputAxis xAxis = new() { Range = new Vector2(-999, 999) };
    private InputAxis yAxis = new() { Range = new Vector2(-999, 999) };
    // 超过此速度才会触发切换锁定目标
    public float changeLockMinSpeed = 80f;

    private bool isChanging = true;

    public void GetInputAxes(List<IInputAxisOwner.AxisDescriptor> axes)
    {
        axes.Add(new() { DrivenAxis = () => ref xAxis, Name = "X Axis", Hint = IInputAxisOwner.AxisDescriptor.Hints.X });
        axes.Add(new() { DrivenAxis = () => ref yAxis, Name = "Y Axis", Hint = IInputAxisOwner.AxisDescriptor.Hints.Y });
    }

    public override void TransferFrom(CinemachineVirtualCameraBase fromCamera)
    {
        virtualCamera.ForceCameraPosition(fromCamera.transform.position, fromCamera.transform.rotation);
        ResetCameraChangeLockState();
    }

    private void ResetCameraChangeLockState()
    {
        xAxis.Value = 0;
        yAxis.Value = 0;
        isChanging = false;
    }

    public void SetLookingTarget(Transform target)
    {
        virtualCamera.LookAt = target;
    }

    void LateUpdate()
    {
        var input = new Vector2(xAxis.Value, yAxis.Value);
        xAxis.Value = 0;
        yAxis.Value = 0;
        ChangeLockState(input);
    }

    private void ChangeLockState(Vector2 input)
    {
        var speed = input.magnitude / Time.deltaTime;
        if (speed >= changeLockMinSpeed)
        {
            if (!isChanging)
            {
                TryChangeLockTarget(input);
            }
            isChanging = true;
        }
        else if (speed < changeLockMinSpeed * 0.1)
        {
            isChanging = false;
        }
    }

    private void TryChangeLockTarget(Vector2 input)
    {
        var actor = manager.ControllingActor;
        if (actor == null)
            return;
        if (actor.actorCameraStatus.TryChangeLockTarget(input))
        {
            SetLookingTarget(actor.actorCameraStatus.LockingTarget.actorCameraStatus.cameraTarget);
        }
    }
}