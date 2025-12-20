
using System;
using Unity.Cinemachine;
using UnityEngine;

public class FollowCameraController : BaseCameraController
{
    private CinemachineOrbitalFollow positionControl;

    private ref InputAxis VerticalAxis => ref positionControl.VerticalAxis;
    private ref InputAxis HorizontalAxis => ref positionControl.HorizontalAxis;

    void Awake()
    {
        positionControl = virtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineOrbitalFollow;
        Debug.Assert(positionControl != null, "FollowCameraController requires a CinemachineOrbitalFollow component in the Body stage.");
    }

    public override void TransferFrom(CinemachineVirtualCameraBase fromCamera)
    {
        virtualCamera.ForceCameraPosition(fromCamera.transform.position, fromCamera.transform.rotation);
        var angleX = fromCamera.transform.eulerAngles.x;
        if (angleX > 180f)
        {
            angleX -= 360f;
        }
        VerticalAxis.Value = angleX;
        VerticalAxis.Validate();
        var angleY = fromCamera.transform.eulerAngles.y;
        var facingAngleY = manager.ControllingActor.transform.eulerAngles.y;
        angleY = Mathf.DeltaAngle(facingAngleY, angleY);
        HorizontalAxis.Value = angleY;
        HorizontalAxis.Validate();
    }

    public void RecenterFollowCamera()
    {
        VerticalAxis.TriggerRecentering();
        HorizontalAxis.TriggerRecentering();
    }
}