using UnityEngine;

public class Storage : Interactable
{
	[SerializeField] UIController uiController;
	
// ON INTERACT METHOD OVERRIDE
	public override void Interact()
	{
		base.Interact();
		if(PlayerController.Instance.IsHoldingSomething)
			return;
		uiController.ShowRecipes();
	}
}
