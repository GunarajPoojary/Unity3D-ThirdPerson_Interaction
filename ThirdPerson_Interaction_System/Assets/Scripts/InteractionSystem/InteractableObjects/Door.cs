using UnityEngine;
using DG.Tweening;
using System;

/// <summary>
/// Represents a Door that can open and close by sliding two door parts apart or together.
/// </summary>
public class Door : MonoBehaviour
{
    [SerializeField] private Transform _leftDoor;
    [SerializeField] private Transform _rightDoor;
    [SerializeField] private Vector3 _doorOpenLocalOffset;
    [SerializeField] private float _openDuration = 0.5f;

    private Vector3 _leftClosedPos; 
    private Vector3 _rightClosedPos;
    private bool _isOpen = false;   

    public event Action OnDoorToggled;

    private void Awake()
    {
        _leftClosedPos = _leftDoor.localPosition;
        _rightClosedPos = _rightDoor.localPosition;
    }

    /// <summary>
    /// Toggles door state between open and closed with animation.
    /// </summary>
    public void OpenOrClose()
    {
        _isOpen = !_isOpen;

        if (_isOpen)
        {
            // Animate opening
            _leftDoor.DOLocalMove(_leftClosedPos + _doorOpenLocalOffset, _openDuration);
            _rightDoor.DOLocalMove(_rightClosedPos - _doorOpenLocalOffset, _openDuration)
                      .OnComplete(() => OnDoorToggled?.Invoke());
        }
        else
        {
            // Animate closing
            _leftDoor.DOLocalMove(_leftClosedPos, _openDuration);
            _rightDoor.DOLocalMove(_rightClosedPos, _openDuration)
                      .OnComplete(() => OnDoorToggled?.Invoke());
        }
    }
}