
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InputTypeActionMapping", menuName = "Configs/InputTypeActionMapping")]
public class InputTypeActionMapping : BaseItemMapping<EnumByName<InputType>, ActionTimelineAsset>, IInputTypeActionMapping
{
    [SerializeField]
    private ActionTimelineAsset idle;
    public ActionTimelineAsset Idle => idle;
    [SerializeField]
    private StrafeMoveAnimation strafe;
    public StrafeMoveAnimation Strafe => strafe;

    protected override int CompareKey(EnumByName<InputType> a, EnumByName<InputType> b)
    {
        return a.Value.CompareTo(b.Value);
    }
}