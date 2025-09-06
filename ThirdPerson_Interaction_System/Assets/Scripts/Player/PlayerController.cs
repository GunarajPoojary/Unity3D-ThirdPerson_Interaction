using UnityEngine;

/// <summary>
/// Simplified player controller for walking movement only.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    [Header("Movement Settings")]
    [Tooltip("Walk speed of the character in m/s")]
    [SerializeField] private float _moveSpeed = 2.0f;

    [Range(0.0f, 0.3f)]
    [Tooltip("How fast the character turns to face movement direction")]
    [SerializeField] private float _rotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration rate")]
    [SerializeField] private float _speedChangeRate = 10.0f;

    [SerializeField] private Animator _animator;

    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;

    private CharacterController _controller;
    private GameObject _mainCamera;

    private void Awake() => _mainCamera = Camera.main.gameObject;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        AssignAnimationIDs();
    }

    private void Update()
    {
        Move();
    }

    private void AssignAnimationIDs()
    {
        _animator.SetFloat(Animator.StringToHash("Speed"), 0f);
        _animator.SetFloat(Animator.StringToHash("MotionSpeed"), 0f);
    }

    /// <summary>
    /// Handles simple walk movement based on player input.
    /// </summary>
    private void Move()
    {
        float targetSpeed = _moveSpeed;

        // Stop moving if no input
        if (_inputReader.MoveInput == Vector2.zero)
            targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
        float speedOffset = 0.1f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * _speedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * _speedChangeRate);
        if (_animationBlend < 0.01f)
            _animationBlend = 0f;

        Vector3 inputDirection = new Vector3(_inputReader.MoveInput.x, 0.0f, _inputReader.MoveInput.y).normalized;

        if (_inputReader.MoveInput != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _rotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime));

        _animator.SetFloat(Animator.StringToHash("Speed"), _animationBlend);
        _animator.SetFloat(Animator.StringToHash("MotionSpeed"), 1);
    }
}