
using UnityEngine;

[CreateAssetMenu(fileName = "ShockTypeActionMapping", menuName = "Configs/ShockTypeActionMapping")]
public class ShockTypeActionMapping : BaseItemMapping<ShockType, ActionTimelineAsset>
{
    protected override int CompareKey(ShockType a, ShockType b)
    {
        return ShockConfig.Instance.Compare(a, b);
    }
}
