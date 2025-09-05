using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;

[CreateAssetMenu(fileName = "InputReader", menuName = "Custom/Player/Input/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
	private GameInput _gameInput;

	public Vector2 MoveInput
	{
		get
		{
			if (_gameInput == null || !_gameInput.Gameplay.enabled)
				LazyInitialize();

			return _gameInput.Gameplay.Move.ReadValue<Vector2>();
		}
	}

	public Vector2 LookInput
	{
		get
		{
			if (_gameInput == null || !_gameInput.Gameplay.enabled)
				LazyInitialize();

			return _gameInput.Gameplay.Look.ReadValue<Vector2>();
		}
	}

	public string CurrentControlScheme => _gameInput.controlSchemes.FirstOrDefault().name;

	public UnityAction SprintPerformedAction = delegate { };
	public UnityAction SprintCanceledAction = delegate { };
	public UnityAction JumpPerformedAction = delegate { };
	public UnityAction JumpCanceledAction = delegate { };
	public UnityAction MovePerformedAction = delegate { };
	public UnityAction MoveCanceledAction = delegate { };
	public UnityAction InteractStartedAction = delegate { };

	private void LazyInitialize()
	{
		Initialize();
		EnableGameplayActions();
	}

	public void Initialize() => _gameInput ??= new GameInput();

	public void EnableGameplayActions()
	{
		_gameInput?.Gameplay.Enable();
		_gameInput?.Gameplay.SetCallbacks(this);
	}

	public void OnMove(CallbackContext context)
	{
		if (context.performed)
			MovePerformedAction?.Invoke();

		if (context.canceled)
			MoveCanceledAction?.Invoke();
	}

	public void OnSprint(CallbackContext context)
	{
		if (context.performed)
			SprintPerformedAction?.Invoke();

		if (context.canceled)
			SprintCanceledAction?.Invoke();
	}

	public void OnJump(CallbackContext context)
	{
		if (context.performed)
			JumpPerformedAction?.Invoke();

		if (context.canceled)
			JumpCanceledAction?.Invoke();
	}

	public void OnAttack(CallbackContext context) { }

	public void OnCrouch(CallbackContext context) { }

	public void OnInteract(CallbackContext context)
	{
		if (context.started)
			InteractStartedAction?.Invoke();
	}

	public void OnLook(CallbackContext context) { }

	public void OnNext(CallbackContext context) { }

	public void OnPrevious(CallbackContext context) { }
}