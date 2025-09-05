using UnityEngine;
using System.Collections;

/// <summary>
/// Detects nearby interactable objects using an overlap sphere.
/// Raises events when an interactable is found or lost.
/// </summary>
public class InteractionSensor : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private Vector3 _positionOffset;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private float _checkRate = 0.1f;

    [Header("Debug")]
    [SerializeField] private bool _enableGizmo;
    [SerializeField] private InteractableEventChannel _interactableEvent;

    private readonly Collider[] _results = new Collider[1];
    private Coroutine _checkRoutine;
    private WaitForSeconds _interval;
    private bool _enableDetection;

    private IInteractable _currentDetected;

    private void Awake() => _interval = new WaitForSeconds(_checkRate);
    private void Start() => StartDetection();
    private void OnDestroy() => StopDetection();

    /// <summary>
    /// Starts the detection loop.
    /// </summary>
    public void StartDetection()
    {
        _enableDetection = true;
        _checkRoutine = StartCoroutine(CheckRoutine());
    }

    /// <summary>
    /// Stops the detection loop.
    /// </summary>
    public void StopDetection()
    {
        _enableDetection = false;

        if (_checkRoutine != null)
            StopCoroutine(_checkRoutine);
    }

    /// <summary>
    /// Continuously checks for interactables at a fixed interval while detection is enabled.
    /// </summary>
    private IEnumerator CheckRoutine()
    {
        while (_enableDetection)
        {
            Detect();
            yield return _interval;
        }
    }

    /// <summary>
    /// Detects nearby interactables and triggers events when they change.
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

        if (colliders > 0 && _results[0].TryGetComponent(out IInteractable interactable))
        {
            if (interactable == _currentDetected) return;

            // Found a new interactable
            _interactableEvent.RaiseLost(_currentDetected);
            _currentDetected = interactable;
            _interactableEvent.RaiseFound(_currentDetected);
        }
        else
        {
            if (_currentDetected != null)
            {
                // Lost the previously detected interactable
                _interactableEvent.RaiseLost(_currentDetected);
                _currentDetected = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!_enableGizmo) return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.TransformPoint(_positionOffset), _radius);
    }
}