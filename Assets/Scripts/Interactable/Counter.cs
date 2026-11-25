using UnityEngine;

public class Counter : Interactable
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
		if(item.CurrentStage!=BakeryItem.Stage.Ingredients)
			return;
		item.SetStage(BakeryItem.Stage.Raw);
	}
}
