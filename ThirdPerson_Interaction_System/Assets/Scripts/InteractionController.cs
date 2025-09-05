using UnityEngine;

/// <summary>
/// Manages highlighting and interaction with detected interactables.
/// Subscribes to events from InteractionSensor.
/// </summary>
public class InteractionController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    [SerializeField] private InteractableEventChannel _interactableEvent;
    [SerializeField] private InteractEventChannel _interactEvent;
    [SerializeField] private InteractableHoverEventChannel _interactableHoverEvent;
    private IHighlightable _currentHighlightable;
    private IInteractable _currentInteractable;

    private void OnEnable()
    {
        _interactableEvent.OnInteractableFound += HandleInteractableFound;
        _interactableEvent.OnInteractableLost += HandleInteractableLost;

        _inputReader.InteractStartedAction += Interact;
    }

    private void OnDisable()
    {
        _interactableEvent.OnInteractableFound -= HandleInteractableFound;
        _interactableEvent.OnInteractableLost -= HandleInteractableLost;

        _inputReader.InteractStartedAction -= Interact;
    }

    private void HandleInteractableFound(IInteractable interactable)
    {
        if (interactable is IHighlightable highlightable)
        {
            _currentHighlightable = highlightable;
            _currentHighlightable?.Highlight();
            _interactableHoverEvent.RaiseFound(interactable.UIAnchor, interactable.InteractableType);
        }

        _currentInteractable = interactable;
    }

    private void HandleInteractableLost(IInteractable interactable)
    {
        if (interactable is IHighlightable highlightable)
        {
            highlightable?.UnHighlight();
            _interactableHoverEvent.RaiseLost();

            if (_currentHighlightable == highlightable)
                _currentHighlightable = null;
        }

        _currentInteractable = null;
    }

    /// <summary>
    /// Example: Call this from Input system to interact with the object.
    /// </summary>
    public void Interact()
    {
        _currentInteractable?.Interact();
        _interactEvent.RaiseEvent();
    }
}