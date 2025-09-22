using System;
using UnityEngine;

/// <summary>
/// Continuously detects nearby interactable objects using an overlap sphere.
/// Raises events when an interactable is found or lost.
/// </summary>
public class Interactor : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    [Header("Detection Settings")]
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private float _radius = 1f;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private float _checkRate = 0.1f;

    [SerializeField] private VoidEventChannelSO _interactableLostEvent;
    [SerializeField] private InteractableHoverUIEventChannelSO _interactableHoverEvent;

    [Header("Debug")]
    [SerializeField] private bool _enableGizmo;

    private readonly Collider[] _results = new Collider[1];
    private float _nextCheckTime;
    private IInteractable _focused;

    public IInteractable CurrentFocused => _focused;

    private void OnEnable()
    {
        _inputReader.InteractStartedAction += Interact;
    }

    private void OnDisable()
    {
        _inputReader.InteractStartedAction -= Interact;

        // Clear focus when disabled
        if (_focused != null)
        {
            SetFocusedInteractable(null);
        }
    }

    private void Update()
    {
        if (Time.time >= _nextCheckTime)
        {
            Detect();
            _nextCheckTime = Time.time + _checkRate;
        }
    }

    /// <summary>
    /// Detects interactables within radius and triggers corresponding events.
    /// </summary>
    private void Detect()
    {
        Vector3 center = transform.TransformPoint(_positionOffset);

        int colliders = Physics.OverlapSphereNonAlloc(
            center,
            _radius,
            _results,
            _interactableLayer,
            QueryTriggerInteraction.Ignore);

        IInteractable newInteractable = null;

        // Check if we found a valid interactable
        if (colliders > 0 && _results[0].TryGetComponent(out IInteractable interactable))
        {
            // Only consider interactables that can be interacted with
            if (interactable.CanInteract)
                newInteractable = interactable;
        }

        // Update focused interactable if it changed
        if (!ReferenceEquals(_focused, newInteractable))
            SetFocusedInteractable(newInteractable);
    }

    /// <summary>
    /// Sets the currently focused interactable, handling focus transitions properly.
    /// </summary>
    /// <param name="newInteractable">The new interactable to focus, or null to clear focus</param>
    private void SetFocusedInteractable(IInteractable newInteractable)
    {
        // Handle losing focus on previous interactable
        if (_focused != null)
        {
            _focused.OnFocusLost();
            _interactableLostEvent.RaiseEvent(null);
        }

        // Update focused reference
        _focused = newInteractable;

        // Handle gaining focus on new interactable
        if (_focused != null)
        {
            _focused.OnFocusGained();
            _interactableHoverEvent.RaiseEvent((_focused.UIAnchor, _focused.InteractableType));
        }
    }

    /// <summary>
    /// Called from the input system when interaction is triggered.
    /// Calls the Interact method on the current interactable if it can be interacted with.
    /// </summary>
    private void Interact()
    {
        if (_focused != null && _focused.CanInteract)
            _focused.Interact();
    }

    private void OnDrawGizmos()
    {
        if (!_enableGizmo) return;

        Gizmos.color = _focused != null ? Color.green : Color.yellow;
        Gizmos.DrawWireSphere(transform.TransformPoint(_positionOffset), _radius);

        // Draw a line to the focused interactable
        if (_focused != null && _focused is MonoBehaviour focusedMB)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.TransformPoint(_positionOffset), focusedMB.transform.position);
        }
    }
}