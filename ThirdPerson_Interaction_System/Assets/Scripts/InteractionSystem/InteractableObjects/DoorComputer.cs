using UnityEngine;

/// <summary>
/// Represents a computer that interacts with a Door, allowing opening/closing via interaction.
/// Implements IInteractable and IHighlightable for interaction and visual feedback.
/// </summary>
[RequireComponent(typeof(OutlineHighlighter))]
public class DoorComputer : MonoBehaviour, IInteractable, IHighlightable
{
    [SerializeField] private Door _door;
    [SerializeField] private int _interactableLayerIndex = 6;

    private OutlineHighlighter _highlighter;

    [field: SerializeField] public InteractableType InteractableType { get; private set; }
    [field: SerializeField] public Transform UIAnchor { get; private set; }

    private const int DEFAULTLAYERINDEX = 0;

    private void Awake() => _highlighter = GetComponent<OutlineHighlighter>();

    private void OnEnable() => _door.OnDoorToggled += ToggleInteractivity;
    private void OnDisable() => _door.OnDoorToggled -= ToggleInteractivity;

    private void Start() => _highlighter.UnHighlight();
    private void OnDestroy() => _highlighter.UnHighlight();

    public void Highlight() => _highlighter.Highlight();
    public void UnHighlight() => _highlighter.UnHighlight();

    public void Interact()
    {
        _door.OpenOrClose();
        _highlighter.UnHighlight();
        gameObject.layer = DEFAULTLAYERINDEX;
    }

    // Sets object to interactable layer to re-enable interaction after door toggles.
    private void ToggleInteractivity() => gameObject.layer = _interactableLayerIndex;
}