using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InteractEvent", menuName = "Custom/Events/InteractEventChannel")]
public class InteractEventChannel : ScriptableObject
{
    public event Action OnInteract = delegate { };

    public void RaiseEvent() => OnInteract?.Invoke();
}