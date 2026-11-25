using UnityEngine;

public class DisplaySlot : Interactable
{
	PlayerController playerController;
	GameObject displayedProduct;

// GETTERS
	public bool IsOccupied => displayedProduct!=null;

// ON INTERACT METHOD OVERRIDE
	public override void Interact()
	{
		base.Interact();
		playerController=PlayerController.Instance;
		if(playerController.IsHoldingSomething)
		{
			if(IsOccupied)
				return;
			Food item=playerController.HeldItem;
			if(item==null||item.CurrentStage!=Food.Stage.Cooked)
				return;
			GameObject product=playerController.DisplayHeldRecipe();
			if(product==null)
				return;
			displayedProduct=product;
			product.transform.SetParent(transform);
			product.transform.localPosition=Vector3.zero;
			product.transform.localRotation=Quaternion.identity;
		}
		else
		{
			if(!IsOccupied)
				return;
			Food item=displayedProduct.GetComponent<Food>();
			bool hasProductBeenPicked=playerController.PickUpRecipe(item.Recipe, Food.Stage.Cooked);
			if(!hasProductBeenPicked)
				return;
			Destroy(displayedProduct);
			displayedProduct=null;
		}
	}
}
