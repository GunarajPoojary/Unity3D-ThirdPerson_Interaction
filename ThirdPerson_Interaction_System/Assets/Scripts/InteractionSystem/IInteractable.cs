using UnityEngine;

public interface IInteractable
{
    /// <summary>
    /// UI anchor transform used to position interaction UI near the object.
    /// </summary>
    Transform UIAnchor { get; }
    bool CanInteract { get; }
    string InteractionTypeName { get; }
    void Interact();
    void OnFocusGained();
    void OnFocusLost();
}