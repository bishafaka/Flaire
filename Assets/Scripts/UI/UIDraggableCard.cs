using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIDraggableCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
// VALUES
	const float SPEED=0.25f;
	const int MAX_X=30;
	const int MAX_Y=180;
	const int SPRING=100;
	const int DAMPING=5;

// VISUALS
	Sprite frontImage;
	Sprite backImage;

// REFERENCES
	RectTransform rectTransform;
	Image image;
	Vector3 currentRotation;
	Vector3 rotationVelocity;
	bool isDragging;
	Quaternion originalRotation;

// INIT
	public void InitCard(Recipe recipe)
	{
		frontImage=recipe.RecipeIconFront;
		backImage=recipe.RecipeIconBack;
		UpdateCardSide();
	}
	void Awake()
	{
		rectTransform=GetComponent<RectTransform>();
		image=GetComponent<Image>();
		originalRotation=rectTransform.localRotation;
	}

	// BEGIN DRAG
	public void OnBeginDrag(PointerEventData eventData)
	{
		isDragging=true;
		rotationVelocity=Vector3.zero;
	}

// ON DRAG
	public void OnDrag(PointerEventData eventData)
	{
		if(!isDragging)
			return;
		Vector2 delta=eventData.delta;
		currentRotation.x-=delta.y*SPEED;
		currentRotation.y+=delta.x*SPEED;
		currentRotation.x=Mathf.Clamp(currentRotation.x, -MAX_X, MAX_X);
		currentRotation.y=Mathf.Clamp(currentRotation.y, -MAX_Y, MAX_Y);
		ApplyRotation();
	}

// END DRAG
	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging=false;
	}

// SPRING
	void Update()
	{
		if(isDragging)
			return;
		Vector3 acceleration=-currentRotation*SPRING-rotationVelocity*DAMPING;
		rotationVelocity+=acceleration*Time.deltaTime;
		currentRotation+=rotationVelocity*Time.deltaTime;
		if(currentRotation.sqrMagnitude<0.001f && rotationVelocity.sqrMagnitude<0.01f)
		{
			currentRotation=Vector3.zero;
			rotationVelocity=Vector3.zero;
		}
		ApplyRotation();
	}
	void ApplyRotation()
	{
		rectTransform.localRotation=originalRotation*Quaternion.Euler(currentRotation);
		UpdateCardSide();
	}

// FRONT & BACK SPRITE
	void UpdateCardSide()
	{
		Vector3 forward=rectTransform.rotation*Vector3.forward;
		bool showingFront=Vector3.Dot(forward, Vector3.forward)>0f;
		image.sprite=showingFront ? frontImage : backImage;
	}
}
