using UnityEngine;

/// <summary>
/// Handles third-person camera rotation based on player input.
/// </summary>
public class CameraThirdPersonLook : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    [Header("Cinemachine")]
    [Tooltip("Follow target set in Cinemachine Virtual Camera.")]
    [SerializeField] private Transform _cinemachineCameraTarget;

    [Tooltip("Max upward angle the camera can move.")]
    [SerializeField] private float _topClamp = 70.0f;

    [Tooltip("Max downward angle the camera can move.")]
    [SerializeField] private float _bottomClamp = -30.0f;

    private const float _threshold = 0.01f;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private void Start() => _cinemachineTargetYaw = _cinemachineCameraTarget.rotation.eulerAngles.y;
    private void LateUpdate() => CameraRotation();

    private void CameraRotation()
    {
        // Check for input magnitude above threshold
        if (_inputReader.LookInput.sqrMagnitude >= _threshold)
        {
            _cinemachineTargetYaw += _inputReader.LookInput.x;
            _cinemachineTargetPitch += _inputReader.LookInput.y;
        }

        // Clamp pitch to prevent over-rotation
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);

        // Apply rotation to camera target
        _cinemachineCameraTarget.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}