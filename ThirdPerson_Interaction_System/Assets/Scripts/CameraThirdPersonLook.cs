using UnityEngine;

public class CameraThirdPersonLook : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [SerializeField] private Transform _cinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    [SerializeField] private float _topClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    [SerializeField] private float _bottomClamp = -30.0f;

    private const float _threshold = 0.01f;

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private void Start() => _cinemachineTargetYaw = _cinemachineCameraTarget.rotation.eulerAngles.y;
    private void LateUpdate() => CameraRotation();

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_inputReader.LookInput.sqrMagnitude >= _threshold)
        {
            //Don't multiply mouse input by Time.deltaTime;
            // float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            // _cinemachineTargetYaw += _inputReader.LookInput.x * deltaTimeMultiplier;
            // _cinemachineTargetPitch += _inputReader.LookInput.y * deltaTimeMultiplier;
            _cinemachineTargetYaw += _inputReader.LookInput.x;
            _cinemachineTargetPitch += _inputReader.LookInput.y;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);

        // Cinemachine will follow this target
        _cinemachineCameraTarget.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}