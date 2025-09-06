using UnityEngine;

/// <summary>
/// Manages visual highlighting of objects by changing their layer to an outline layer.
/// </summary>
public class OutlineHighlighter : MonoBehaviour
{
    [SerializeField] private int _outlineLayerIndex = 9;   
    [SerializeField] private GameObject[] _visuals;        
    private const int DEFAULTLAYERINDEX = 0;               

    /// <summary>
    /// Applies highlight by setting visuals to the outline layer.
    /// </summary>
    public void Highlight()
    {
        foreach (GameObject visual in _visuals)
            visual.layer = _outlineLayerIndex;
    }

    /// <summary>
    /// Removes highlight by resetting visuals to the default layer.
    /// </summary>
    public void UnHighlight()
    {
        foreach (GameObject visual in _visuals)
            visual.layer = DEFAULTLAYERINDEX;
    }
}