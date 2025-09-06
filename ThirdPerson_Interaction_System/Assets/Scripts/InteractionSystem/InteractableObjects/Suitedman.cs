using UnityEngine;

/// <summary>
/// Represents a Suited Man character that can be interacted with.
/// Implements IInteractable and IHighlightable for interaction and visual feedback.
/// Plays an animation and displays a chat bubble on interaction.
/// </summary>
[RequireComponent(typeof(OutlineHighlighter))]
public class Suitedman : MonoBehaviour, IInteractable, IHighlightable
{
    [SerializeField] private ChatBubble _chatBubble;          
    [SerializeField] private int _interactableLayerIndex = 6; 
    [SerializeField] private Animator _animator;              

    private OutlineHighlighter _highlighter;

    [field: SerializeField] public InteractableType InteractableType { get; private set; }
    [field: SerializeField] public Transform UIAnchor { get; private set; }

    private const int DEFAULTLAYERINDEX = 0;
    private static readonly int InteractTrigger = Animator.StringToHash("Interact");

    private void Awake()
    {
        _highlighter = GetComponent<OutlineHighlighter>();

        // Set callback for when animation ends
        GetComponentInChildren<SuitedmanAnimationEventTrigger>().OnInteractAnimationEndAction = ToggleInteractivity;
    }

    private void Start() => _highlighter.UnHighlight();
    private void OnDestroy() => _highlighter.UnHighlight();

    public void Highlight() => _highlighter.Highlight();
    public void UnHighlight() => _highlighter.UnHighlight();
    
    public void Interact()
    {
        _animator.SetTrigger(InteractTrigger);
        _highlighter.UnHighlight();
        gameObject.layer = DEFAULTLAYERINDEX;
        _chatBubble.ShowMessage();
    }

    // Resets object to interactable state and hides the chat message after animation ends.
    private void ToggleInteractivity()
    {
        gameObject.layer = _interactableLayerIndex;
        _chatBubble.HideMessage();
    }
}