using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform _leftDoor;
    [SerializeField] private Transform _rightDoor;
    [SerializeField] private Vector3 _doorOpenLocalOffset;

    public void Open()
    {
        _leftDoor.localPosition = _doorOpenLocalOffset;
        _rightDoor.localPosition = -_doorOpenLocalOffset;
    }

    public void Close()
    {

    }
}