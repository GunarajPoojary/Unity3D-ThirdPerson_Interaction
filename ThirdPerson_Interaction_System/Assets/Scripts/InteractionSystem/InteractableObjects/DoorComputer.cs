using UnityEngine;

/// <summary>
/// Represents a computer that interacts with a Door, allowing opening/closing via interaction.
/// </summary>
[RequireComponent(typeof(Interactable))]
public class DoorComputer : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private int _interactableLayerIndex = 6;

    private const int DEFAULTLAYERINDEX = 0;

    private Interactable _interactable;

    private void Awake() => _interactable = GetComponent<Interactable>();

    private void OnEnable()
    {
        _interactable.OnInteract += Toggle;

        _door.OnDoorToggled += ToggleInteractivity;
    }

    private void OnDisable()
    {
        _interactable.OnInteract -= Toggle;

        _door.OnDoorToggled -= ToggleInteractivity;
    }

    private void Toggle()
    {
        _door.OpenOrClose();
        gameObject.layer = DEFAULTLAYERINDEX;
    }

    // Sets object to interactable layer to re-enable interaction after door toggles.
    private void ToggleInteractivity() => gameObject.layer = _interactableLayerIndex;
}