
public struct BoolStatus
{
    private int status;

    public void AddStatus()
    {
        status++;
    }

    public void RemoveStatus()
    {
        status--;
    }

    public static implicit operator bool(BoolStatus boolStatus)
    {
        return boolStatus.status > 0;
    }

}