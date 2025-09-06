using UnityEngine;

/// <summary>
/// Keeps the object facing the main camera at all times.
/// Typically used for UI elements like floating text or indicators.
/// </summary>
public class Billboard : MonoBehaviour
{
    private Transform _maincameraTransform;

    private void Awake() => _maincameraTransform = Camera.main.transform;

    private void Update()
    {
        if (_maincameraTransform != null)
        {
            transform.rotation = Quaternion.LookRotation(_maincameraTransform.forward, _maincameraTransform.up);
        }
    }
}