using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _maincameraTransform;

    private void Awake()
    {
        _maincameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (_maincameraTransform != null)
        {
            transform.rotation = Quaternion.LookRotation(_maincameraTransform.forward, _maincameraTransform.up);
        }
    }
}