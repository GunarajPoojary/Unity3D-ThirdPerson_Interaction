using System;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    [SerializeField] private int _outlineWidth = 3;
    [SerializeField] private Color _outlineColor = Color.white;
    private bool _isEnabled = true;
    [field: SerializeField] public Transform UIAnchor { get; private set; }
    [field: SerializeField] public string InteractionTypeName { get; private set; }
    
    private Outline _outline;

    public bool CanInteract => _isEnabled;

    public event Action OnInteract;

    private void Awake()
    {
        _outline = gameObject.AddComponent<Outline>();
        _outline.OutlineMode = Outline.Mode.OutlineVisible;
        _outline.OutlineColor = _outlineColor;
        _outline.OutlineWidth = _outlineWidth;
        _outline.enabled = false;
    }

    public void Interact() => OnInteract?.Invoke();
    public void OnFocusGained() => _outline.enabled = true;
    public void OnFocusLost() => _outline.enabled = false;
}