using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static PlayerController Instance { get; private set; }

// PLAYER MOVEMENT
	const float SPEED=5.0f;
	const float SPRINT_MULTIPLIER=1.5f;
	bool isSprint=false;
	CharacterController characterController;
	Vector2 dir;

// PLAYER INTERACT
	[SerializeField] Transform handPosition;
	GameObject currentInteractable=null;
	Interactable currentInteractableScript=null;
	GameObject heldObject;
	BakeryItem heldItem;

// GETTERS
	public bool IsHoldingSomething => heldObject!=null;
	public BakeryItem HeldItem => heldItem;

// INIT REFERENCES & CLEANUP
	void Awake()
	{
		Instance=this;
		characterController=GetComponent<CharacterController>();
	}
	void OnDestroy()
	{
		if(Instance==this)
			Instance=null;
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

// PLAYER INTERACTION
	void Interact()
	{
		if(currentInteractable==null)
			return;
		if(currentInteractableScript==null)
			return;
		currentInteractableScript.Interact();
	}

// PICK UP INGREDIENTS FROM STORAGE
	public bool PickUpRecipe(Recipe recipe, BakeryItem.Stage stage=BakeryItem.Stage.Ingredients)
	{
		if(IsHoldingSomething || recipe==null)
			return false;
		if(recipe.RecipePrefab==null || handPosition==null)
			return false;

	// INIT RECIPE PREFAB
		heldObject=Instantiate(recipe.RecipePrefab, handPosition.position, handPosition.rotation, handPosition);
		heldItem=heldObject.GetComponent<BakeryItem>();
		if(heldItem==null)
		{
			Destroy(heldObject);
			heldObject=null;
			return false;
		}
		heldItem.Init(recipe, stage);
		return true;
	}

// DISPLAYS RECIPE AND CLEANUP
	public GameObject DisplayHeldRecipe()
	{
		if(!IsHoldingSomething)
			return null;
		GameObject item=heldObject;
		heldObject=null;
		heldItem=null;
		item.transform.SetParent(null);
		return item;
	}

// CLEAR HANDS
	public void ClearHands()
	{
		if(heldObject!=null)
			Destroy(heldObject);
		heldObject=null;
		heldItem=null;
	}

// TRIGGER SET INTERACTABLE
	void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Interactable"))
		{
			currentInteractable=other.gameObject;
			currentInteractableScript=other.GetComponent<Interactable>();
		}
	}
	void OnTriggerExit(Collider other)
	{
		if(other.CompareTag("Interactable"))
		{
			currentInteractable=null;
			currentInteractableScript=null;
		}
	}
}
