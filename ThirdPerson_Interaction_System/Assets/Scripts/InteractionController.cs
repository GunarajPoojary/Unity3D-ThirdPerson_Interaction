using StarterAssets;
using UnityEngine;

/// <summary>
/// Manages highlighting and interaction with detected interactables.
/// Subscribes to events from InteractionSensor.
/// </summary>
[RequireComponent(typeof(InteractionSensor))]
public class InteractionController : MonoBehaviour
{
    private InteractionSensor _sensor;
    private IHighlightable _currentHighlightable;
    private StarterAssetsInputs _input;

    private void Awake()
    {
        _sensor = GetComponent<InteractionSensor>();
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void OnEnable()
    {
        _sensor.OnInteractableFound += HandleInteractableFound;
        _sensor.OnInteractableLost += HandleInteractableLost;
    }

    private void OnDisable()
    {
        _sensor.OnInteractableFound -= HandleInteractableFound;
        _sensor.OnInteractableLost -= HandleInteractableLost;
    }

    private void Update()
    {
        if (_input.interact)
        {
            Interact();
            _input.interact = false;
        }
    }

    private void HandleInteractableFound(IHighlightable highlightable)
    {
        _currentHighlightable = highlightable;
        _currentHighlightable.Highlight();
    }

    private void HandleInteractableLost(IHighlightable highlightable)
    {
        highlightable?.UnHighlight();
        if (_currentHighlightable == highlightable)
            _currentHighlightable = null;
    }

    /// <summary>
    /// Example: Call this from Input system to interact with the object.
    /// </summary>
    public void Interact()
    {
        if (_currentHighlightable is IInteractable interactable)
        {
            interactable.Interact();
        }
    }
}