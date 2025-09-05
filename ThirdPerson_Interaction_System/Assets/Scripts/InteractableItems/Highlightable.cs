using UnityEngine;

public class Highlightable : MonoBehaviour
{
    [SerializeField] private int _outlineLayerIndex = 9;
    [SerializeField] private GameObject[] _visuals;
    private const int DEFAULTLAYERINDEX = 0;

    public void Highlight()
    {
        foreach (GameObject visual in _visuals)
            visual.layer = _outlineLayerIndex;
    }

    public void UnHighlight()
    {
        foreach (GameObject visual in _visuals)
            visual.layer = DEFAULTLAYERINDEX; 
    }
}