using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Animator))]
public class ActorMovement : MonoBehaviour
{
    public Actor actor;
    private Animator _animator;
    public Animator animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }

    Vector3 velocity = Vector3.zero;
    Quaternion rotation = Quaternion.identity;

    // 由输入调用的旋转
    public void UpdateTurn(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            // Smoothly rotate towards the target direction
            rotation = Quaternion.RotateTowards(
                rotation,
                targetRotation,
                GetTurnSpeed() * Time.deltaTime
            );
        }
    }

    float GetTurnSpeed()
    {
        return 360f * 2; //每秒旋转角度
    }

    void Update() {
        actor.transform.rotation = rotation;
    }

    void OnAnimatorMove()
    {
        var deltaPos = animator.deltaPosition;
        var deltaRot = animator.deltaRotation;
        transform.localRotation *= deltaRot;
        actor.characterController.Move(new Vector3(deltaPos.x, 0, deltaPos.z));
        transform.localPosition += new Vector3(0, deltaPos.y, 0);
        UpdateY();
#if UNITY_EDITOR
        if (isRecording)
        {
            // 记录动画的位移和旋转
            previewDeltaPosition += animator.deltaPosition;
            previewDeltaRotation *= animator.deltaRotation;
        }
#endif
    }

    void UpdateY()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
        if (actor.characterController.isGrounded)
        {
            velocity = Vector3.zero;
        }
        else
        {
            velocity += Physics.gravity * Time.deltaTime;
            actor.characterController.Move(velocity * Time.deltaTime);
        }
    }

    internal void ResetRotation()
    {
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
    }

#if UNITY_EDITOR
    bool isRecording = false;
    protected Vector3 previewDeltaPosition;
    protected Quaternion previewDeltaRotation;

    public void StartRecordMovement()
    {
        if (isRecording)
        {
            ResetTransform();
        }
        isRecording = false;
        if (Application.isPlaying)
            return;
        isRecording = true;
        previewDeltaPosition = Vector3.zero;
        previewDeltaRotation = Quaternion.identity;
    }

    public void ResetTransform()
    {
        if (!isRecording)
            return;
        isRecording = false;
        if (Application.isPlaying)
            return;
        // Reset the animator's position and rotation
        actor.characterController.Move(-previewDeltaPosition);
        animator.transform.localRotation *= Quaternion.Inverse(previewDeltaRotation);
    }
#endif

}
