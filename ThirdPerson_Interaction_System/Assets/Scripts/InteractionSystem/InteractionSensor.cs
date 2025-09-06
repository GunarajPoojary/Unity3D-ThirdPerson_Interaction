using System;
using UnityEngine;

/// <summary>
/// Continuously detects nearby interactable objects using an overlap sphere.
/// Raises events when an interactable is found or lost.
/// </summary>
public class InteractionSensor : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private float _radius = 1f;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private float _checkRate = 0.1f;

    [SerializeField] private VoidEventChannelSO _interactableLostEvent;

    [Header("Debug")]
    [SerializeField] private bool _enableGizmo;

    private readonly Collider[] _results = new Collider[1];
    private float _nextCheckTime;
    private IInteractable _currentDetected;

    public event Action<IInteractable> OnInteractableFound;

    private void Update()
    {
        if (Time.time >= _nextCheckTime)
        {
            Detect();
            _nextCheckTime = Time.time + _checkRate;
        }
    }

    // Detects interactables within radius and triggers corresponding events.
    private void Detect()
    {
        Vector3 center = transform.TransformPoint(_positionOffset);

        int colliders = Physics.OverlapSphereNonAlloc(
            center,
            _radius,
            _results,
            _interactableLayer,
            QueryTriggerInteraction.Ignore);

        if (colliders > 0 && _results[0].TryGetComponent(out IInteractable interactable))
        {
            // Found a new interactable
            if (interactable == _currentDetected) return;

            _currentDetected = interactable;
            OnInteractableFound?.Invoke(_currentDetected);
        }
        else if (_currentDetected != null)
        {
            // Lost the previously detected interactable
            _interactableLostEvent.RaiseEvent(null);
            _currentDetected = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!_enableGizmo) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.TransformPoint(_positionOffset), _radius);
    }
}