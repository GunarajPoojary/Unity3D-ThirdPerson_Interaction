using UnityEngine;
using UnityEngine.Events;

public class EventChannel<T> : ScriptableObject
{
    public event UnityAction<T> OnEventRaised = delegate { };

    public void RaiseEvent(T value) => OnEventRaised?.Invoke(value);
}

/// <summary>
/// Empty class used as a placeholder for events that don't require parameters
/// </summary>
public class Empty { }