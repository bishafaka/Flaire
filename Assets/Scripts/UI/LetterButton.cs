using TMPro;
using UnityEngine;

public class LetterButton : MonoBehaviour
{
// LETTER DATA
	Letter letter;
	UIController uiController;

// REFERENCES
	[SerializeField] TextMeshProUGUI letterSender;

// INIT
	public void Init(Letter newLetter, UIController newUIController)
	{
		letter=newLetter;
		uiController=newUIController;
		letter.LetterSender.StringChanged+=InitText;
	}

// EVENTS
	void OnDisable()
	{
		letter.LetterSender.StringChanged-=InitText;
	}
	void InitText(string value)
	{
		letterSender.text=value;
	}

// SHOW LETTER
	public void ShowLetter()
	{
		uiController.ShowLetter(letter);
	}
}
