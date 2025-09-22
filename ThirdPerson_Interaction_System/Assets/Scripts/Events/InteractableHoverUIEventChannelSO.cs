using UnityEngine;

/// <summary>
/// ScriptableObject Event Channel for broadcasting when an interactable is hovered.
/// Passes a tuple containing the Transform of the UI anchor and the InteractableType.
/// </summary>
[CreateAssetMenu(fileName = "InteractableHoverEvent", menuName = "Custom/Events/InteractableHoverEventChannel")]
public class InteractableHoverUIEventChannelSO : EventChannel<(Transform, string)> { }