using UnityEngine;
using System;
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

    private readonly Collider[] _results = new Collider[1];
    private Coroutine _checkRoutine;
    private WaitForSeconds _interval;
    private bool _enableDetection;

    private IHighlightable _currentDetected;

    // Events
    public event Action<IHighlightable> OnInteractableFound;
    public event Action<IHighlightable> OnInteractableLost;

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
        int colliders = Physics.OverlapSphereNonAlloc(
            transform.position + _positionOffset,
            _radius,
            _results,
            _interactableLayer,
            QueryTriggerInteraction.Ignore);

        if (colliders > 0 && _results[0].TryGetComponent(out IHighlightable highlightable))
        {
            if (highlightable == _currentDetected) return;

            // Found a new interactable
            OnInteractableLost?.Invoke(_currentDetected);
            _currentDetected = highlightable;
            OnInteractableFound?.Invoke(_currentDetected);
        }
        else
        {
            if (_currentDetected != null)
            {
                // Lost the previously detected interactable
                OnInteractableLost?.Invoke(_currentDetected);
                _currentDetected = null;
            }
        }
    }

    /// <summary>
    /// Draws a gizmo in the editor to visualize the detection sphere (for debugging).
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!_enableGizmo) return;

        Gizmos.color = Color.yellow;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawSphere(_positionOffset, _radius);
    }
}