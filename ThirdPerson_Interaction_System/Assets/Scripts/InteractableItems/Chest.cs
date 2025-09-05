using UnityEngine;
using DG.Tweening;

public class Chest : InteractableItem
{
    [SerializeField] private Transform _crateTop;
    [SerializeField] private Vector3 _openAngle;
    [SerializeField] private float _openDuration = 0.5f;
    [SerializeField] private Highlightable _highlightable;
    private const int DEFAULTLAYERINDEX = 0;

    private void Start() => _highlightable.UnHighlight();
    private void OnDestroy() => _highlightable.UnHighlight();

    public override void Highlight() => _highlightable.Highlight();
    public override void UnHighlight() => _highlightable.UnHighlight();

    public override void Interact()
    {
        _crateTop.DOLocalRotate(_openAngle, _openDuration);
        _highlightable.UnHighlight();
        gameObject.layer = DEFAULTLAYERINDEX;
    }
}