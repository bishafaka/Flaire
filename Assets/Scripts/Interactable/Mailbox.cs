using UnityEngine;

public class Mailbox : Interactable
{
// REFERENCES
	[SerializeField] UIController uiController;
	TimeManager timeManager;

// INIT & CLEANUP EVENT
	void Start()
	{
		timeManager=GameManager.Instance.TimeManager;
		timeManager.OnDayChanged+=UpdateMail;
		UpdateMail();
	}
	void OnDestroy()
	{
		timeManager.OnDayChanged-=UpdateMail;
	}

// DAY UPDATE
	void UpdateMail()
	{
		Debug.Log("new mail!");
	}

// ON INTERACT METHOD OVERRIDE
	public override void Interact()
	{
		base.Interact();
		uiController.ShowMailbox();
	}
}
