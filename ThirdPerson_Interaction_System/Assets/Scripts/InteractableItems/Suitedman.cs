using UnityEngine;

public class Suitedman : InteractableObject
{
    [SerializeField] private Highlightable _highlightable;
    [SerializeField] private int _interactableLayerIndex = 6;

    [SerializeField] private Animator _animator;

    private const int DEFAULTLAYERINDEX = 0;
    private static readonly int InteractTrigger = Animator.StringToHash("Interact");

    private void Awake()
    {
        GetComponentInChildren<SuitedmanAnimationEventTrigger>().OnInteractAnimationEndAction = ToggleInteractivity;
    }

    private void Start() => _highlightable.UnHighlight();
    private void OnDestroy() => _highlightable.UnHighlight();

    public override void Highlight() => _highlightable.Highlight();
    public override void UnHighlight() => _highlightable.UnHighlight();

    public override void Interact()
    {
        _animator.SetTrigger(InteractTrigger);
        _highlightable.UnHighlight();
        gameObject.layer = DEFAULTLAYERINDEX;
    }

    private void ToggleInteractivity() => gameObject.layer = _interactableLayerIndex;
}