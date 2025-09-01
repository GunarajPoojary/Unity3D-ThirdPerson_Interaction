using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldItem : MonoBehaviour, IHighlightable, IInteractable
{
    [SerializeField] private ScriptableRendererFeature _outilineFeature;

    private void Awake()
    {
        UnHighlight();
    }

    public void Highlight() => _outilineFeature.SetActive(true);

    public void UnHighlight() => _outilineFeature.SetActive(false);

    public void Interact()
    {
        Debug.Log("Interact");
    }
}