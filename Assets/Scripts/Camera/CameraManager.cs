
using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : CinemachineCameraManagerBase
{
    public PlayerInputController playerInputController;
    public Actor ControllingActor => playerInputController.ControllingActor;

    public FollowCameraController followCameraController;
    public LockCameraController lockCameraController;

    public CinemachineVirtualCameraBase followCamera => followCameraController.virtualCamera;
    public CinemachineVirtualCameraBase lockCamera => lockCameraController.virtualCamera;

    private BaseCameraController activeController;

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
            return ControllingActor.actorCameraStatus.IsLocking;
        }
    }

    protected override CinemachineVirtualCameraBase ChooseCurrentCamera(Vector3 worldUp, float deltaTime)
    {
        var old = activeController;
        BaseCameraController next = IsLockMode ? lockCameraController : followCameraController;
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
        return next.virtualCamera;
    }

    public void SetFollow(Actor actor)
    {
        followCamera.Follow = actor.actorCameraStatus.cameraTarget;
        lockCamera.Follow = actor.actorCameraStatus.cameraTarget;
    }

    public void LockTo(Actor actor)
    {
        lockCameraController.SetLookingTarget(actor.actorCameraStatus.cameraTarget);
    }

    public void Recenter()
    {
        followCameraController.RecenterFollowCamera();
    }

}