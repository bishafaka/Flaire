using UnityEngine;

public class Door : Interactable
{
// SCENE INDEX
	[SerializeField] int sceneToTP=0;

// REFERENCES
	[SerializeField] bool isHouseDoor=false;
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
		if(isHouseDoor)
		{
			timeManager.SetNewDay();
			return;
		}
		if(timeManager.IsShiftFinished())
			uiController.ChangeScene(sceneToTP);
		else
			uiController.ShowSkipPanel();
	}
}
