using UnityEngine;

public abstract class InteractableItem : MonoBehaviour, IHighlightable, IInteractable
{
    [field: SerializeField] public InteractableType InteractableType { get; private set; }
    [field: SerializeField] public Transform UIAnchor { get; private set; }

    public abstract void Highlight();
    public abstract void Interact();
    public abstract void UnHighlight();
}