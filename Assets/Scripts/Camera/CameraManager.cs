
using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : CinemachineCameraManagerBase
{
    [Header("Controller Configuration")]
    public PlayerInputController playerInputController;
    public Actor ControllingActor => playerInputController.ControllingActor;

    public FollowCameraController followCameraController;
    public LockCameraController lockCameraController;

    public CinemachineVirtualCameraBase followCamera => followCameraController.virtualCamera;
    public CinemachineVirtualCameraBase lockCamera => lockCameraController.virtualCamera;

    private BaseCameraController activeController;

    [Header("Default Camera Settings")]
    public CameraUpdateData defaultData = new ()
    {
        blendTime = 0.5f,
        changeCameraDistance = true,
        cameraDistance = 2.5f
    };

    public CinemachineImpulseSource impulseSource;

    protected override void Start()
    {
        base.Start();
        followCameraController.gameObject.SetActive(false);
        lockCameraController.gameObject.SetActive(false);
        activeController = null;
    }

    private bool IsLockMode
    {
        get
        {
            if (ControllingActor == null)
                return false;
            return ControllingActor.cameraStatus.IsLocking;
        }
    }

    private bool IsOverrideMode
    {
        get
        {
            if (ControllingActor == null)
                return false;
            return ControllingActor.cameraStatus.isOverriding;
        }
    }

    private bool IsImpulse
    {
        get
        {
            if (ControllingActor == null)
                return false;
            return ControllingActor.cameraStatus.isImpulse;
        }
    }

    private CameraUpdateData GetCameraUpdateData()
    {
        if (ControllingActor == null)
            return defaultData;
        return ControllingActor.cameraStatus.currentData;
    }

    protected override CinemachineVirtualCameraBase ChooseCurrentCamera(Vector3 worldUp, float deltaTime)
    {
        var old = activeController;
        BaseCameraController next;
        var cameraUpdateData = GetCameraUpdateData();
        if (IsOverrideMode)
        {
            next = ControllingActor.cameraStatus.overrideCameraController;
        }
        else if (IsLockMode)
        {
            lockCameraController.SetCameraUpdateData(cameraUpdateData);
            next = lockCameraController;
        }
        else
        {
            followCameraController.SetCameraUpdateData(cameraUpdateData);
            next = followCameraController;
        }
        if (old != next)
        {
            if (old != null)
            {
                old.gameObject.SetActive(false);
                next.TransferFrom(old.virtualCamera);
            }
            next.gameObject.SetActive(true);
            activeController = next;
        }
        if (IsImpulse)
        {
            ControllingActor.cameraStatus.isImpulse = false;
            impulseSource.GenerateImpulse();
        }
        return next.virtualCamera;
    }

    public void SetFollow(Actor actor)
    {
        followCamera.Follow = actor.cameraStatus.cameraTarget;
        lockCamera.Follow = actor.cameraStatus.cameraTarget;
        actor.cameraStatus.InitCameraData(defaultData);
    }

    public void LockTo(Actor actor)
    {
        lockCameraController.SetLookingTarget(actor.cameraStatus.cameraTarget);
    }

    public void Recenter()
    {
        followCameraController.RecenterFollowCamera();
    }

}