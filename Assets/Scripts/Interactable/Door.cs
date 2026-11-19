using UnityEngine;

public class Door : Interactable
{
// ON INTERACT METHOD OVERRIDE
	public override void Interact()
	{
		base.Interact();
		TimeManager timeManager=GameManager.Instance.TimeManager;
		if(timeManager.IsShiftFinished())
			timeManager.SetNewDay();
	}
}
