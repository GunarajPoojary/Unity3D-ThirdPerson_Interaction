using UnityEngine;

public class DoorComputer : InteractableObject
{
    [SerializeField] private Door _door;
    [SerializeField] private LayerMask _interactableLayer;
    [SerializeField] private Highlightable _highlightable;
    private const int DEFAULTLAYERINDEX = 0;

    private void Start() => _highlightable.UnHighlight();
    private void OnDestroy() => _highlightable.UnHighlight();

    public override void Highlight() => _highlightable.Highlight();
    public override void UnHighlight() => _highlightable.UnHighlight();

    public override void Interact()
    {
        _door.Open();
        _highlightable.UnHighlight();

        gameObject.layer = DEFAULTLAYERINDEX;
    }
}