using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCameraBase))]
public abstract class BaseCameraController : MonoBehaviour
{

    private CinemachineVirtualCameraBase _virtualCamera;
    public CinemachineVirtualCameraBase virtualCamera
    {
        get
        {
            if (_virtualCamera == null)
            {
                _virtualCamera = GetComponent<CinemachineVirtualCameraBase>();
            }
            return _virtualCamera;
        }
    }

    public abstract void TransferFrom(CinemachineVirtualCameraBase fromCamera);
}