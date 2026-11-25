using UnityEngine;

public class Door : Interactable
{
// SCENE INDEX
	[SerializeField] int sceneToTP=0;

// REFERENCES
	[SerializeField] bool isHouseDoor=false;
	[SerializeField] UIController uiController;
	TimeManager timeManager;
// FLAGS
	bool hasBeenUsed=false;

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
			if(hasBeenUsed)
				return;
			hasBeenUsed=true;
			timeManager.SetNewDay();
			uiController.ChangeScene(sceneToTP);
			return;
		}
		if(timeManager.IsShiftFinished())
			uiController.ChangeScene(sceneToTP);
		else
			uiController.ShowSkipPanel();
	}
}
