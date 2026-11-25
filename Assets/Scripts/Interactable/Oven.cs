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
		Food item=playerController.HeldItem;
		if(item==null)
			return;
		if(item.CurrentStage!=Food.Stage.Raw)
			return;
		item.SetStage(Food.Stage.Cooked);
	}
}
