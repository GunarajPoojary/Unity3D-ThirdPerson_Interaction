using UnityEngine;
using System.Collections;

public class Interactor : MonoBehaviour
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

    private IHighlightable _currentHighlightable;

    private void Awake() => _interval = new WaitForSeconds(_checkRate);

    private void Start()
    {
        StartDetection();
    }

    private void OnDestroy()
    {
        StopDetection();
    }

    public void StartDetection()
    {
        _enableDetection = true;

        _checkRoutine = StartCoroutine(CheckRoutine());
    }

    public void StopDetection()
    {
        _enableDetection = false;

        if (_checkRoutine != null)
            StopCoroutine(_checkRoutine);
    }

    private IEnumerator CheckRoutine()
    {
        while (_enableDetection)
        {
            CheckForPickup();
            yield return _interval;
        }
    }

    private void CheckForPickup()
    {
        int colliders = Physics.OverlapSphereNonAlloc(
            transform.position + _positionOffset,
            _radius,
            _results,
            _interactableLayer,
            QueryTriggerInteraction.Ignore);

        if (colliders > 0)
        {
            if (_results[0].TryGetComponent(out IHighlightable highlightable))
            {
                if (highlightable == _currentHighlightable) return;

                _currentHighlightable = highlightable;
                _currentHighlightable.Highlight();
            }
            else
            {
                _currentHighlightable?.UnHighlight();
                _currentHighlightable = null;
            }
        }
        else
        {
            _currentHighlightable?.UnHighlight();
            _currentHighlightable = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!_enableGizmo) return;

        Gizmos.color = Color.yellow;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawSphere(_positionOffset, _radius);
    }
}