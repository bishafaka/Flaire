using UnityEngine;

public class Oven : Interactable
{
	PlayerController playerController;

	public override void Interact()
	{
		base.Interact();
		playerController=PlayerController.Instance;
		if(!playerController.IsHoldingSomething)
			return;
		BakeryItem item=playerController.HeldItem;
		if(item==null)
			return;
		if(item.CurrentStage!=BakeryItem.Stage.Raw)
			return;
		item.SetStage(BakeryItem.Stage.Product);
	}
}
