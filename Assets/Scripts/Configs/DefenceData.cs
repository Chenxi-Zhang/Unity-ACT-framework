
using System;
using System.Collections.Generic;

[Serializable]
public class DefenceData
{
    public List<ShockResponseAction> responseActions = new();

    public ActionTimelineAsset GetActionFromShock(ShockType shockType)
    {
        foreach (var responseAction in responseActions)
        {
            if (responseAction.shockType.name == shockType.name)
            {
                return responseAction.actionTimelineAsset;
            }
        }
        return responseActions[0].actionTimelineAsset;
    }
}

[Serializable]
public class ShockResponseAction
{
    public ShockType shockType;
    public ActionTimelineAsset actionTimelineAsset;
}