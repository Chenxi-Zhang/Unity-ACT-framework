
public enum InputType
{
    None,

    IdleTurnLeft,
    IdleTurnRight,
    IdleTurnBack,
    IdleTurnStop,

    Move,
    MoveCancel,
    ForceActionStrafe,

    SwitchWeapon,
    UseItem,

    Attack,
    AttackHold,
    AttackRelease,
    Interact,
    Defence,
    DefenceHold,
    DefenceRelease,
    Sprint,
    SprintHold,
    SprintRelease,

    ForceActionAI,
    ForceActionBeCounter,
    ForceBeHit,

    Death,
}