
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InputTypeActionMapping", menuName = "Configs/InputTypeActionMapping")]
public class InputTypeActionMapping : BaseItemMapping<EnumByName<InputType>, ActionTimelineAsset>
{
    public ActionTimelineAsset Idle;
    public StrafeMoveAnimation Strafe;

    protected override int CompareKey(EnumByName<InputType> a, EnumByName<InputType> b)
    {
        return a.Value.CompareTo(b.Value);
    }
}