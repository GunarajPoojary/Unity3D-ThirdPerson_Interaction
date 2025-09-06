using UnityEngine;
using DG.Tweening;

/// <summary>
/// Represents a Chest interactable object.
/// Implements IInteractable and IHighlightable for interaction and visual highlighting functionality.
/// </summary>
[RequireComponent(typeof(OutlineHighlighter))]
public class Chest : MonoBehaviour, IInteractable, IHighlightable
{
    [SerializeField] private Transform _crateTop;
    [SerializeField] private Vector3 _openAngle;
    [SerializeField] private float _openDuration = 0.5f;

    private OutlineHighlighter _highlighter;

    [field: SerializeField] public InteractableType InteractableType { get; private set; }  
    [field: SerializeField] public Transform UIAnchor { get; private set; }                 
    private const int DEFAULTLAYERINDEX = 0;

    private void Awake() => _highlighter = GetComponent<OutlineHighlighter>();
    private void Start() => _highlighter.UnHighlight();
    private void OnDestroy() => _highlighter.UnHighlight();

    public void Highlight() => _highlighter.Highlight();
    public void UnHighlight() => _highlighter.UnHighlight();

    public void Interact()
    {
        _crateTop.DOLocalRotate(_openAngle, _openDuration);
        _highlighter.UnHighlight();
        gameObject.layer = DEFAULTLAYERINDEX;
    }
}