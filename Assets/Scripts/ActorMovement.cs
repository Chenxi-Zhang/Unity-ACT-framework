using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Animator))]
public class ActorMovement : MonoBehaviour
{
    public Actor actor;
    // 定义视野范围
    public float maxHorizontalAngle = 70f; // 水平视野角度（左右各70度）
    public float maxVerticalAngle = 50f;   // 垂直视野角度（上下各50度）

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

    public CharacterController controller => actor.characterController;
    public bool IsLocking => actor.cameraStatus.IsLocking;
    public Actor LockingTarget => actor.cameraStatus.LockingTarget;

    public GameObject actorRoot => actor.gameObject;
    public GameObject modelRoot => animator.gameObject;

    public Vector3 actorPosition
    {
        get => actorRoot.transform.position;
        set => actorRoot.transform.position = value;
    }

    public Quaternion actorRotation
    {
        get => actorRoot.transform.rotation;
        set => actorRoot.transform.rotation = value;
    }

    public Quaternion modelRotation
    {
        get => modelRoot.transform.localRotation;
        set => modelRoot.transform.localRotation = value;
    }

    public float modelPosY
    {
        get => modelRoot.transform.localPosition.y;
        set => modelRoot.transform.localPosition = new Vector3(0, value, 0);
    }

    Vector3 velocity = Vector3.zero;
    Quaternion rotation = Quaternion.identity;

    public BoolStatus TurnDisabled = new();

    // 由输入调用的旋转
    public void UpdateTurn(Vector3 direction, float deltaTime)
    {
        if (TurnDisabled)
            return;
        if (direction.sqrMagnitude > 0.01f)
        {
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            // Smoothly rotate towards the target direction
            rotation = Quaternion.RotateTowards(
                rotation,
                targetRotation,
                GetTurnSpeed() * deltaTime
            );
        }
    }

    float GetTurnSpeed()
    {
        return 360f * 2; //每秒旋转角度
    }

    public void DoUpdate(float deltaTime)
    {
        actorRotation = rotation;
        UpdateForcingMove();
    }

    void OnAnimatorMove()
    {
        var deltaPos = animator.deltaPosition;
        var deltaRot = animator.deltaRotation;
        var rootDeltaRot = Quaternion.identity;
        var modelDeltaRot = Quaternion.identity;
        if (TurnDisabled)
        {
            // 动画的旋转直接应用到root节点上，并且只应用y轴旋转
            Vector3 euler = deltaRot.eulerAngles;
            rootDeltaRot = Quaternion.Euler(0, euler.y, 0);
        }
        else
        {
            modelDeltaRot = deltaRot;
        }
        rotation *= rootDeltaRot;
        modelRotation *= modelDeltaRot;
        controller.Move(new Vector3(deltaPos.x, 0, deltaPos.z));
        modelPosY += deltaPos.y;
        UpdateY();
#if UNITY_EDITOR
        if (isRecording)
        {
            // 记录动画的位移和旋转
            previewDeltaPosition += deltaPos;
            previewRootDeltaRotation *= rootDeltaRot;
            previewDeltaRotation *= modelDeltaRot;
        }
#endif
    }

    private Vector3 lookingPosition = Vector3.zero;
    private FloatEaseTool globalLookAtWeight = new(0, 10f);

    void OnAnimatorIK(int layerIndex)
    {
        float lookWeightTarget = 0.0f;
        if (IsLocking)
        {
            lookingPosition = LockingTarget.cameraStatus.cameraTarget.position;
            Vector3 targetDirection = lookingPosition - actorPosition;
            targetDirection.Normalize();
            // 计算水平角度（左右方向）
            float horizontalAngle = Vector3.Angle(
                new Vector3(actorRoot.transform.forward.x, 0, actorRoot.transform.forward.z).normalized,
                new Vector3(targetDirection.x, 0, targetDirection.z).normalized);
            // 计算垂直角度（上下方向）
            float verticalAngle = Vector3.Angle(
                new Vector3(0, actorRoot.transform.forward.y, 1).normalized,
                new Vector3(0, targetDirection.y, 1).normalized);
            // 检查目标是否在前方（点积为正值）
            bool isInFront = Vector3.Dot(actorRoot.transform.forward, targetDirection) > 0;
            // 检查是否在视野范围内
            if (isInFront && horizontalAngle <= maxHorizontalAngle && verticalAngle <= maxVerticalAngle)
            {
                // 设置 Look At 权重和位置
                lookWeightTarget = 1.0f;
            }
        }
        animator.SetLookAtPosition(lookingPosition);
        var globalModifier = globalLookAtWeight.GetEaseValue(lookWeightTarget, Time.deltaTime);
        animator.SetLookAtWeight(globalModifier, 0.3f, 0.6f, 1.0f, 0.5f);
    }

    void UpdateY()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            return;
#endif
        if (controller.isGrounded)
        {
            velocity = Vector3.zero;
        }
        else
        {
            velocity += Physics.gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
    }

    internal void ResetRotation()
    {
        modelRotation = Quaternion.identity;
        modelPosY = 0;
    }

#if UNITY_EDITOR
    bool isRecording = false;
    protected Vector3 previewDeltaPosition;
    protected Quaternion previewRootDeltaRotation;
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
        previewRootDeltaRotation = Quaternion.identity;
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
        controller.Move(-previewDeltaPosition);
        actorRotation *= Quaternion.Inverse(previewRootDeltaRotation);
        modelRotation *= Quaternion.Inverse(previewDeltaRotation);
    }
#endif


    private Vector3Clamper forceMoveClamper;
    private float forceMoveLastTime;
    bool isForceMoving => forceMoveLastTime < forceMoveClamper.maxX;

    public void ForceSetPosition(Vector3 pos, float speed)
    {
        float duration = 0f;
        if (speed > 0)
        {
            duration = Vector3.Distance(actorPosition, pos) / speed;
        }
        forceMoveLastTime = Time.time;
        forceMoveClamper = new Vector3Clamper(
            forceMoveLastTime,
            forceMoveLastTime + duration,
            actorPosition,
            pos
        );
    }

    // 一般由交互调用
    public void ForceSetTurn(Vector3 direction)
    {
        rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    void UpdateForcingMove()
    {
        if (isForceMoving)
        {
            var posLast = forceMoveClamper.Clamp(forceMoveLastTime);
            var posNow = forceMoveClamper.Clamp(Time.time);
            forceMoveLastTime = Time.time;
            var delta = posNow - posLast;
            controller.Move(delta);
#if UNITY_EDITOR
            previewDeltaPosition += delta;
            // Root rotation consider later.
#endif
        }
    }

}
