using TMPro;
using UnityEngine;

/// <summary>
/// Handles displaying and hiding the interaction UI prompt when the player's within the interaction range of an interactable object.
/// </summary>
public class UIInteraction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _interactionTypeText;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private VoidEventChannelSO _interactEvent;
    [SerializeField] private InteractableHoverUIEventChannelSO _interactableHoverEvent;
    [SerializeField] private VoidEventChannelSO _interactableLostEvent;

    private void OnEnable()
    {
        // Subscribe to hover and lost events
        _interactableHoverEvent.OnEventRaised += Show;
        _interactableLostEvent.OnEventRaised += Hide;
        _interactEvent.OnEventRaised += Hide;
    }

    private void OnDisable()
    {
        _interactableHoverEvent.OnEventRaised -= Show;
        _interactableLostEvent.OnEventRaised -= Hide;
        _interactEvent.OnEventRaised -= Hide;
    }

    private void Start() => Hide();

    /// <summary>
    /// Shows the interaction UI at the given transform anchor, with appropriate text.
    /// </summary>
    /// <param name="value">Tuple containing UI anchor and interaction type</param>
    public void Show((Transform parent, string InteractionTypeName) value)
    {
        _canvas.transform.SetParent(value.parent);
        _canvas.transform.localPosition = Vector3.zero;

        // Display appropriate interaction type text
        _interactionTypeText.text = value.InteractionTypeName;

        _canvas.SetActive(true);
    }

    /// <summary>
    /// Hides the interaction UI and resets its parent.
    /// </summary>
    public void Hide(Empty e = null)
    {
        _canvas.transform.SetParent(transform);
        _canvas.SetActive(false);
    }
}