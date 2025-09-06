using UnityEngine;

/// <summary>
/// Manages player interaction with nearby interactable objects.
/// Subscribes to InteractionSensor events and handles interaction input.
/// </summary>
[RequireComponent(typeof(InteractionSensor))]
public class InteractionController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private VoidEventChannelSO _interactableLostEvent;
    [SerializeField] private VoidEventChannelSO _interactEvent;
    [SerializeField] private InteractableHoverUIEventChannelSO _interactableHoverEvent;

    private InteractionSensor _interactionSensor;
    private IHighlightable _currentHighlightable;
    private IInteractable _currentInteractable;

    private void Awake()
    {
        _interactionSensor = GetComponent<InteractionSensor>();
    }

    private void OnEnable()
    {
        _interactionSensor.OnInteractableFound += HandleInteractableFound;
        _interactableLostEvent.OnEventRaised += HandleInteractableLost;

        _inputReader.InteractStartedAction += Interact;
    }

    private void OnDisable()
    {
        _interactionSensor.OnInteractableFound -= HandleInteractableFound;
        _interactableLostEvent.OnEventRaised -= HandleInteractableLost;

        _inputReader.InteractStartedAction -= Interact;
    }

    // Called when an interactable is detected nearby.
    // Highlights it and raises a UI event.
    private void HandleInteractableFound(IInteractable interactable)
    {
        if (interactable is IHighlightable highlightable)
        {
            _currentHighlightable = highlightable;
            _currentHighlightable?.Highlight();
            _interactableHoverEvent.RaiseEvent((interactable.UIAnchor, interactable.InteractableType));
        }

        _currentInteractable = interactable;
    }

    /// Called when an interactable is no longer in range.
    // Removes highlight and resets current interactable.
    private void HandleInteractableLost(Empty e = null)
    {
        _currentHighlightable?.UnHighlight();
        _currentInteractable = null;
        _currentHighlightable = null;
    }

    /// <summary>
    /// Called from the input system when interaction is triggered.
    /// Calls the Interact method on the current interactable.
    /// </summary>
    public void Interact()
    {
        _currentInteractable?.Interact();
        _interactEvent.RaiseEvent(null);
    }
}