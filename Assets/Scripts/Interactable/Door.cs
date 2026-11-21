using UnityEngine;

public class Door : Interactable
{
// REFERENCES
	[SerializeField] UIController uiController;
	TimeManager timeManager;

// INIT
	void Start()
	{
		timeManager=GameManager.Instance.TimeManager;
	}

// ON INTERACT METHOD OVERRIDE
	public override void Interact()
	{
		base.Interact();
		if(timeManager.IsShiftFinished())
			timeManager.SetNewDay();
		else
			uiController.ShowDoor();
	}
}
