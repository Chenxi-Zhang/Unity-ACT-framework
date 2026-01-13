
using System;

[Serializable]
public class InputTypeActionMappingManager : BaseItemMappingManager<InputTypeActionMapping, EnumByName<InputType>, ActionTimelineAsset>
{

    public ActionTimelineAsset GetIdleAction()
    {
        for (int i = Mappings.Count - 1; i >= 0 ; i--)
        {
            var itemMapping = Mappings[i];
            if (itemMapping.Idle != null)
            {
                return itemMapping.Idle;
            }
        }
        return defaultMapping.Idle;
    }

    public StrafeMoveAnimation GetStrafe()
    {
        for (int i = Mappings.Count - 1; i >= 0 ; i--)
        {
            var itemMapping = Mappings[i];
            if (itemMapping.Strafe != null)
            {
                return itemMapping.Strafe;
            }
        }
        return defaultMapping.Strafe;
    }

}