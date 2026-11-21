using System.Collections.Generic;
using UnityEngine;

public class Mailbox : Interactable
{
// REFERENCES
	[SerializeField] UIController uiController;
	[SerializeField] List<Letter> allLetters=new List<Letter>();
	TimeManager timeManager;

// INIT & CLEANUP EVENT
	void Start()
	{
		timeManager=GameManager.Instance.TimeManager;
		timeManager.OnDayChanged+=UpdateMail;

	// REMOVES LETTERS ALREADY SEEN FROM SAVE DATA
		SaveManager save=GameManager.Instance.SaveManager;
		for (int i=allLetters.Count-1; i>=0; i--)
		{
			if(save.HasRecievedLetter(allLetters[i]))
				allLetters.RemoveAt(i);
		}
	}
	void OnDestroy()
	{
		timeManager.OnDayChanged-=UpdateMail;
	}

// DAY UPDATE NEW MAIL RECIEVE
	void UpdateMail()
	{
		if(allLetters.Count<=0)
			return;

	// 50% CHANCE OF RECIEVING NEW MAIL
		int random=Random.Range(0, 2);
		if(random==0)
			return;
		GameManager.Instance.SaveManager.RecieveLetter(allLetters[0]);
		allLetters.RemoveAt(0);
		Debug.Log("new mail!");
	}

// ON INTERACT METHOD OVERRIDE
	public override void Interact()
	{
		base.Interact();
		uiController.ShowMailbox();
	}
}
