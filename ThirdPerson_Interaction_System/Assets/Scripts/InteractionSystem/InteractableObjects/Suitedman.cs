using UnityEngine;

/// <summary>
/// Represents a Suited Man character that can be interacted with.
/// Implements IInteractable and IHighlightable for interaction and visual feedback.
/// Plays an animation and displays a chat bubble on interaction.
/// </summary>
public class Suitedman : MonoBehaviour
{
    [SerializeField] private ChatBubble _chatBubble;
    [SerializeField] private int _interactableLayerIndex = 6;
    [SerializeField] private Animator _animator;

    [field: SerializeField] public InteractableType InteractableType { get; private set; }
    [field: SerializeField] public Transform UIAnchor { get; private set; }

    public bool CanInteract => throw new System.NotImplementedException();

    private const int DEFAULTLAYERINDEX = 0;
    private Interactable _interactable;

    private static readonly int InteractTrigger = Animator.StringToHash("Interact");

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();

        // Set callback for when animation ends
        GetComponentInChildren<SuitedmanAnimationEventTrigger>().OnInteractAnimationEndAction = ToggleInteractivity;
    }

    private void OnEnable()
    {
        _interactable.OnInteract += Talk;
    }

    private void OnDisable()
    {
        _interactable.OnInteract -= Talk;
    }

    private void Talk()
    {
        _animator.SetTrigger(InteractTrigger);
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