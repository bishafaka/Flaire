using UnityEngine;

public class Mailbox : Interactable
{
// REFERENCES
	[SerializeField] UIController uiController;

// ON INTERACT METHOD OVERRIDE
	public override void Interact()
	{
		base.Interact();
		uiController.ShowMailbox();
	}
}
