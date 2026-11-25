using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-25)]
public class InputManager : MonoBehaviour
{
// INPUT SYSTEM
	InputSystemActions Inputs;
	public enum PlayerState { UI, Player, Cinematic }
	[SerializeField] PlayerState CurrentPlayerState=PlayerState.UI;

// INPUT EVENTS
	public event Action<Vector2> Move;
	public event Action<Vector2> Look;
	public event Action JumpPressed;
	public event Action InteractPressed;
	public event Action SprintPressed;
	public event Action SprintReleased;
	public event Action pausePressed;

	// EVENT SUBSCRIPTION
	void RegisterEvent()
	{
		Inputs.Player.Move.performed+=OnMove;
		Inputs.Player.Move.canceled+=OnMove;
		Inputs.Player.Look.performed+=OnLook;
		Inputs.Player.Look.canceled+=OnLook;
		Inputs.Player.Jump.started+=OnJumpStarted;
		Inputs.Player.Interact.started+=OnInteractStarted;
		Inputs.Player.Sprint.started+=OnSprintStarted;
		Inputs.Player.Sprint.canceled+=OnSprintCanceled;
		Inputs.Default.Pause.started+=OnPauseStarted;
	}
	void UnregisterEvent()
	{
		if(Inputs==null)
			return;
		Inputs.Player.Move.performed-=OnMove;
		Inputs.Player.Move.canceled-=OnMove;
		Inputs.Player.Look.performed-=OnLook;
		Inputs.Player.Look.canceled-=OnLook;
		Inputs.Player.Jump.started-=OnJumpStarted;
		Inputs.Player.Interact.started-=OnInteractStarted;
		Inputs.Player.Sprint.started-=OnSprintStarted;
		Inputs.Player.Sprint.canceled-=OnSprintCanceled;
		Inputs.Default.Pause.started-=OnPauseStarted;
	}

// INPUT CALLBACKS
	void OnMove(InputAction.CallbackContext ctx) => Move?.Invoke(ctx.ReadValue<Vector2>());
	void OnLook(InputAction.CallbackContext ctx) => Look?.Invoke(ctx.ReadValue<Vector2>());
	void OnJumpStarted(InputAction.CallbackContext ctx) => JumpPressed?.Invoke();
	void OnInteractStarted(InputAction.CallbackContext ctx) => InteractPressed?.Invoke();
	void OnSprintStarted(InputAction.CallbackContext ctx) => SprintPressed?.Invoke();
	void OnSprintCanceled(InputAction.CallbackContext ctx) => SprintReleased?.Invoke();
	void OnPauseStarted(InputAction.CallbackContext ctx) => pausePressed?.Invoke();

// INIT & CLEANUP
	void Awake()
	{
		GameManager gameManager=GetComponent<GameManager>();
		if(gameManager==null || GameManager.Instance!=gameManager)
		{
			enabled=false;
			return;
		}
		Inputs=new InputSystemActions();
		RegisterEvent();
		UpdatePlayerState();
	}
	void OnDestroy()
	{
		if(Inputs==null)
			return;
		UnregisterEvent();
		Inputs.Disable();
		Inputs.Dispose();
		Inputs=null;
	}

//PLAYER STATE MANAGEMENT
	public PlayerState GetPlayerState => CurrentPlayerState;
	public void SetPlayerState(PlayerState playerState)
	{
		CurrentPlayerState=playerState;
		UpdatePlayerState();
	}
	void UpdatePlayerState()
	{
		Inputs.Default.Enable();
		Inputs.Player.Disable();
		Inputs.UI.Disable();
		switch(CurrentPlayerState)
		{
			case PlayerState.UI:
				SetUI();
				break;
			case PlayerState.Player:
				SetPlayer();
				break;
			case PlayerState.Cinematic:
				SetCinematic();
				break;
			default:
				break;
		}
	}

// STATE CONFIG
	void SetUI()
	{
		Inputs.UI.Enable();
	}
	void SetPlayer()
	{
		Inputs.Player.Enable();
	}
	void SetCinematic()
	{
		Cursor.visible=true;
		Cursor.lockState=CursorLockMode.None;
	}
}
