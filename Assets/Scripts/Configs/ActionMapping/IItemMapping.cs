
public interface IItemMapping<Key, Value>
{
    public bool TryGetValue(Key key, out Value value);
}