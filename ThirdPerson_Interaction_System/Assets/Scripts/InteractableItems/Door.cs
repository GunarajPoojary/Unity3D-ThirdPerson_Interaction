using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform _leftDoor;
    [SerializeField] private Transform _rightDoor;
    [SerializeField] private Vector3 _doorOpenLocalOffset;
    [SerializeField] private float _openDuration = 0.5f;

    private Vector3 _leftClosedPos;
    private Vector3 _rightClosedPos;

    private void Awake()
    {
        _leftClosedPos = _leftDoor.localPosition;
        _rightClosedPos = _rightDoor.localPosition;
    }

    public void Open()
    {
        _leftDoor.DOLocalMove(_leftClosedPos + _doorOpenLocalOffset, _openDuration);
        _rightDoor.DOLocalMove(_rightClosedPos - _doorOpenLocalOffset, _openDuration);
    }

    public void Close()
    {
        _leftDoor.DOLocalMove(_leftClosedPos, _openDuration);
        _rightDoor.DOLocalMove(_rightClosedPos, _openDuration);
    }
}