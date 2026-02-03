
using System.Collections.Generic;

public class InputTypeActionMappingObject : IInputTypeActionMapping
{
    private ActionTimelineAsset idle;
    public ActionTimelineAsset Idle => idle;

    private StrafeMoveAnimation strafe;
    public StrafeMoveAnimation Strafe => strafe;

    private Dictionary<EnumByName<InputType>, ActionTimelineAsset> mapping = new();

    public InputTypeActionMappingObject(ActionTimelineAsset idle = null, StrafeMoveAnimation strafe = null)
    {
        this.idle = idle;
        this.strafe = strafe;
    }

    public void AddMapping(EnumByName<InputType> key, ActionTimelineAsset value)
    {
        mapping[key] = value;
    }

    public bool TryGetValue(EnumByName<InputType> key, out ActionTimelineAsset value)
    {
        return mapping.TryGetValue(key, out value);
    }
}