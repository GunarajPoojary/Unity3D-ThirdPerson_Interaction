using UnityEngine;

public interface IInteractable
{
    Transform UIAnchor{ get; }
    InteractableType InteractableType { get; }
    void Interact();
}
public enum InteractableType { Talk, PickUp, OpenClose }