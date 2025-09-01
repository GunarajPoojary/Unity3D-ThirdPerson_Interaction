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

    private IHighlightable _highlightable;

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
        _checkRoutine = StartCoroutine(CheckRoutine());
    }

    public void StopDetection()
    {
        if (_checkRoutine != null)
            StopCoroutine(_checkRoutine);
    }

    private IEnumerator CheckRoutine()
    {
        while (true)
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
            if (_results[0].TryGetComponent(out IHighlightable highlightable) && _highlightable != highlightable)
                highlightable.Highlight();
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