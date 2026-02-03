
using System;

[Serializable]
public class ItemState
{
    public ActionTimelineAsset action;

    private IInputTypeActionMapping itemActionMapping;
    public IInputTypeActionMapping ItemActionMapping
    {
        get
        {
            if (itemActionMapping == null)
            {
                var mapping = new InputTypeActionMappingObject();
                if (action != null)
                {
                    mapping.AddMapping(InputType.UseItem, action);
                }
                itemActionMapping = mapping;
            }
            return itemActionMapping;
        }
    }
}
