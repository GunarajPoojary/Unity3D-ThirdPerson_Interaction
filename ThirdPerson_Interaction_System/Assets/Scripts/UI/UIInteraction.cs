using TMPro;
using UnityEngine;

public class UIInteraction : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _interactionTypeText;
    [SerializeField] private InteractEventChannel _interactEvent;
    [SerializeField] private GameObject _canvas;
    [SerializeField] private InteractableHoverEventChannel _interactableHoverEvent;

    private void OnEnable()
    {
        _interactableHoverEvent.OnInteractableHover += Show;
        _interactableHoverEvent.OnInteractableLost += Hide;
        _interactEvent.OnInteract += Hide;
    }

    private void OnDisable()
    {
        _interactableHoverEvent.OnInteractableHover -= Show;
        _interactableHoverEvent.OnInteractableLost -= Hide;
        _interactEvent.OnInteract -= Hide;
    }

    private void Start() => Hide();

    public void Show(Transform parent, InteractableType interactableType)
    {
        _canvas.transform.SetParent(parent);
        _canvas.transform.localPosition = Vector3.zero;
        _interactionTypeText.text = interactableType.ToString();
        _canvas.SetActive(true);
    }

    public void Hide()
    {
        _canvas.transform.SetParent(transform);

        _canvas.SetActive(false);
    }
}