using UnityEngine;
using System;

[CreateAssetMenu(fileName = "InteractableHoverEvent", menuName = "Custom/Events/InteractableHoverEventChannel")]
public class InteractableHoverEventChannel : ScriptableObject
{
    public event Action<Transform, InteractableType> OnInteractableHover = delegate { };
    public event Action OnInteractableLost = delegate { };

    public void RaiseFound(Transform interactableUIAnchor, InteractableType interactableType)
        => OnInteractableHover?.Invoke(interactableUIAnchor, interactableType);

    public void RaiseLost()
        => OnInteractableLost?.Invoke();
}