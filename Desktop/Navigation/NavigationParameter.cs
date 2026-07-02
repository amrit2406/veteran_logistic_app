using System.Collections.Generic;

namespace veteran_logistic.Navigation;

/// <summary>
/// Parameter bag passed during navigation.
/// </summary>
public sealed class NavigationParameter
{
    private readonly Dictionary<string, object?> _values = new();

    public object? this[string key]
    {
        get => _values.TryGetValue(key, out var v) ? v : null;
        set => _values[key] = value;
    }

    public bool TryGetValue<T>(string key, out T? value)
    {
        if (_values.TryGetValue(key, out var obj) && obj is T t)
        {
            value = t;
            return true;
        }

        value = default;
        return false;
    }
}
