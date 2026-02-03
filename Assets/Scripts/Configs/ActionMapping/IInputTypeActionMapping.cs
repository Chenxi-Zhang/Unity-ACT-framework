
public interface IInputTypeActionMapping : IItemMapping<EnumByName<InputType>, ActionTimelineAsset>
{
    public ActionTimelineAsset Idle { get; }
    public StrafeMoveAnimation Strafe { get; }
}