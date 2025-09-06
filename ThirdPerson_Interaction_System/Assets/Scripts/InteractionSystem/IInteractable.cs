using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// UI anchor transform used to position interaction UI near the object.
    /// </summary>
    Transform UIAnchor{ get; }
    InteractableType InteractableType { get; }
    void Interact();
}