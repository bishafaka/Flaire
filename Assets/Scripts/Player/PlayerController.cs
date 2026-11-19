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
		InputManager inputManager=GameManager.Instance.InputManager;
		if(inputManager!=null)
		{
			inputManager.Move+=OnMove;
			inputManager.SprintPressed+=OnSprintPressed;
			inputManager.SprintReleased+=OnSprintReleased;
			inputManager.InteractPressed+=Interact;
		}
	}
	void OnDisable()
	{
		InputManager inputManager=GameManager.Instance.InputManager;
		if(inputManager!=null)
		{
			inputManager.Move-=OnMove;
			inputManager.SprintPressed-=OnSprintPressed;
			inputManager.SprintReleased-=OnSprintReleased;
			inputManager.InteractPressed-=Interact;
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
		Interactable interactable=interactableInside.GetComponent<Interactable>();
		if(interactable==null)
			return;
		switch(interactable.GetCategory())
		{
			case Interactable.Category.Mailbox:
				Debug.Log("mailbox!");
				break;
			default:
				break;
		}
	}

// TRIGGER SET INTERACTABLE
	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Interactable"))
			interactableInside=other.gameObject;
	}
	void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Interactable"))
			interactableInside=null;
	}
}
