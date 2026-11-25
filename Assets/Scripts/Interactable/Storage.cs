using UnityEngine;

public class Storage : Interactable
{
	public Recipe recipe;
	PlayerController playerController;

// ON INTERACT METHOD OVERRIDE
	public override void Interact()
	{
		base.Interact();
		playerController=PlayerController.Instance;
		if(playerController.IsHoldingSomething)
			return;
		playerController.PickUpRecipe(recipe);
	}
}
