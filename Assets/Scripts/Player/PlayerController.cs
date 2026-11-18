using UnityEngine;

public class PlayerController : MonoBehaviour
{
// PLAYER MOVEMENT
	const float SPEED=5.0f;
	const float SPRINT_MULTIPLIER=1.5f;
	bool isSprint=false;
	CharacterController characterController;
	Vector2 dir;

// PLAYER INTERACT
	GameObject interactableInside=null;

// INIT REFERENCES
	void Awake()
	{
		characterController=GetComponent<CharacterController>();
	}

// EVENT SUBSCRIPTION
	void OnEnable()
	{
		if(GameManager.Instance.InputManager!=null)
		{
			GameManager.Instance.InputManager.Move+=OnMove;
			GameManager.Instance.InputManager.SprintPressed+=OnSprintPressed;
			GameManager.Instance.InputManager.SprintReleased+=OnSprintReleased;
			GameManager.Instance.InputManager.InteractPressed+=Interact;
		}
	}
	void OnDisable()
	{
		if(GameManager.Instance.InputManager!=null)
		{
			GameManager.Instance.InputManager.Move-=OnMove;
			GameManager.Instance.InputManager.SprintPressed-=OnSprintPressed;
			GameManager.Instance.InputManager.SprintReleased-=OnSprintReleased;
			GameManager.Instance.InputManager.InteractPressed-=Interact;
		}
	}

// MOVEMENT & ROTATION
	void Update()
	{
		Vector3 movement=new Vector3(dir.x, 0, dir.y).normalized;
		float currentSpeed=isSprint ? SPEED*SPRINT_MULTIPLIER : SPEED;
		if(movement!=Vector3.zero)
			transform.forward=movement;
		characterController.Move(movement*currentSpeed*Time.deltaTime);
	}

// STATE MANAGEMENT
	void OnMove(Vector2 input) => dir=input;
	void OnSprintPressed() => isSprint=true;
	void OnSprintReleased() => isSprint=false;

	void Interact()
	{
		if(interactableInside==null)
			return;
	}

// SETTER
	public void SetInteractable(GameObject interactable) => interactableInside=interactable;
}
