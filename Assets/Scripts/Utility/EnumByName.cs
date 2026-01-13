using System;
using UnityEngine;

[Serializable]
public class EnumByName<T> where T : struct, Enum
{
    [SerializeField] private string value;

    public T Value
    {
        get
        {
            if (Enum.TryParse(value, out T result))
                return result;
            return default;
        }
        set => this.value = value.ToString();
    }

    // Implicit conversion from wrapper → enum
    public static implicit operator T(EnumByName<T> wrapper)
    {
        return wrapper != null ? wrapper.Value : default;
    }

    // Implicit conversion from enum → wrapper
    public static implicit operator EnumByName<T>(T enumValue)
    {
        return new EnumByName<T> { Value = enumValue };
    }

    public override bool Equals(object obj)
    {
        if (obj is EnumByName<T> enumByName)
        {
            return value.Equals(enumByName.value);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    public override string ToString()
    {
        return value;
    }
}
