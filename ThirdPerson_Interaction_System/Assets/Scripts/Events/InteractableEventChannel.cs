using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InteractableEvent", menuName = "Custom/Events/InteractableEventChannel")]
public class InteractableEventChannel : ScriptableObject
{
    public event Action<IInteractable> OnInteractableFound = delegate { };
    public event Action<IInteractable> OnInteractableLost = delegate { };

    public void RaiseFound(IInteractable highlightable)
        => OnInteractableFound?.Invoke(highlightable);

    public void RaiseLost(IInteractable highlightable)
        => OnInteractableLost?.Invoke(highlightable);
}