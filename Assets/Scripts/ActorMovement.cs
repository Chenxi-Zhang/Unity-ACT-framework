using System;
using UnityEngine;

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
        actor.characterController.Move(deltaPos);
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
    }
}
